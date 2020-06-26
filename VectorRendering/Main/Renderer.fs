namespace VectorRendering

open System
open VectorRendering.Constructs
open System.Collections.Generic
open VectorRendering.General
open VectorRendering.InternalHelpers
open System.Numerics
open VectorRendering.Drawables


module Renderer = 
    [<Struct>]
    type internal PixelResult =
        val ind: Int32
        val color: RGBASingle
        val coords: Vector2
        val localObjects: Renderable[]
        val skip: Boolean

        internal new( ind: Int32, color: RGBASingle, coords: Vector2, localObjects: Renderable[], skip: Boolean ) =
            { ind = ind; color = color; coords = coords; localObjects = localObjects; skip = skip; }

    let private FoldRender (coords:Vector2) (currentColor:RGBASingle) (object: Renderable) : RGBASingle =
        object.Draw coords currentColor
        
    let private GetCoords (width:Int32) (height:Int32) (index:Int32) : Vector2 =

        let x = single( index % width )
        let y = single( index / width )
        let xFrac = ((x / single (width-1)) * 2.0f) - 1.0f
        let yFrac = ((y / single (height-1)) * 2.0f ) - 1.0f

        new Vector2( xFrac, yFrac )

    let private ParallelRender (objects: Seq<Renderable>) (texture:IList<RGBASingle>) ((index,coords,box):struct(Int32*Vector2*Vector4)) : Async<PixelResult> =        
        let FilterPredicate (box:Vector4) (object: Renderable) : Boolean =
            Geometry.TestBoxOverlap box object.bounds
        async {
            let localObjects =
                objects
                |> Seq.where( FilterPredicate box )
                |> Seq.toArray

            let folder = FoldRender coords
            let initColor = new RGBASingle( 0.0f, 0.0f, 0.0f, 0.0f )
            
            let res =
                localObjects
                |> Array.fold folder initColor

            return PixelResult( index, res, coords, localObjects, false )
        }

    let Render (horizBlocks:Int32) (vertBlocks:Int32) (aaFactor:Int32) (aaPasses: Int32) (scene: SVGScene<'TTexture>) : Unit =
        let diffThreshold = 0.00000000001f
        let equalThreshold = 0.000000000005f
        let adjacentWeight = 1.0f

        let pixelWidth = 1.0f /  single scene.texWidth
        let pixelHeight = 1.0f / single scene.texHeight
        let maxPixDist = single ( pixelHeight * pixelHeight + pixelWidth * pixelWidth )

        let blockSizeX:Single = 2.0f / single horizBlocks
        let blockSizeY:Single = 2.0f / single vertBlocks

        let XGenerator (index:Int32) : Single =
            blockSizeX * single index - 1.0f

        let YGenerator (index:Int32) : Single =
            blockSizeY * single index - 1.0f

        let hSet: Int32[] = [0..horizBlocks-1] |> List.toArray
        let vSet: Int32[] = [0..vertBlocks-1] |> List.toArray
        let CreatePairs (xInd:Int32) : Vector4[] =
            let CreatePair (yInd:Int32) : Vector4 =
                new Vector4( XGenerator xInd, YGenerator yInd, XGenerator (xInd+1), YGenerator (yInd+1) )

            hSet |> Array.map CreatePair

        let blockBoxes =
            vSet
            |> Array.collect CreatePairs

        let PixelMapper (ind: Int32) (index:Int32) : struct(Int32*Vector2*Vector4) =
            let coords = GetCoords scene.texWidth scene.texHeight index
            let pixelBox = new Vector4( coords.X - pixelWidth, coords.Y - pixelHeight, coords.X + pixelWidth, coords.Y + pixelHeight )
            struct( index, coords, pixelBox )

        let pixelCoords =
            [0..scene.tex.Count-1]
            |> List.mapi PixelMapper
            |> List.toArray

        let CheckObject (bounds:Vector4) (object: Renderable) : Boolean =
            Geometry.TestBoxOverlap bounds object.bounds

        let InitBlockInfo (ind:Int32) (bounds:Vector4) : struct(ResizeArray<struct(Int32*Vector2*Vector4)>*Renderable[]*Vector4) =
            let objects = 
                scene.objects
                |> Seq.where (CheckObject bounds)
                |> Seq.rev
                |> Seq.toArray

            let pixels = new ResizeArray<struct(Int32*Vector2*Vector4)>()

            struct(pixels,objects,bounds)
        
        let blockInfos : struct(ResizeArray<struct(Int32*Vector2*Vector4)>*Renderable[]*Vector4)[] =
            blockBoxes
            |> Array.mapi InitBlockInfo

        let FindBlock ((ind,coord,box): struct(Int32*Vector2*Vector4)) : Unit =
            let struct(pixels,_,_) =
                blockInfos
                |> Array.find( fun struct(_,_,bounds) -> Geometry.TestPointInBox bounds coord )

            pixels.Add( struct(ind,coord,box) )

        pixelCoords
        |> Array.iter FindBlock

        let RenderBlock ((pixels,objects,_):struct(ResizeArray<struct(Int32*Vector2*Vector4)>*Renderable[]*Vector4)) : Async<PixelResult>[] =
            pixels
            |> Seq.map( fun pix -> ParallelRender objects scene.tex pix )
            |> Seq.toArray

        let pixelObjects =
            blockInfos
            |> Array.collect RenderBlock
            |> Async.Parallel
            |> Async.RunSynchronously


        let pixelMap =
            pixelObjects
            |> Array.mapi( fun ind pix -> struct(ind, pix.ind) )
            |> Array.sortBy( fun struct(ind, pixInd) -> pixInd )
            |> Array.map( fun struct(ind,_) -> ind )

        let ConvertIndex (x:Int32) (y:Int32) : Option<Int32> =
            pixelMap.TryGetIndex( Some (y * scene.texWidth + x) )

        let maxIndex = scene.texWidth * scene.texHeight - 1

        let IsDifferentColor (color1:RGBASingle) (color2:RGBASingle) : Boolean = 
            let value = Vector4.Distance( color1, color2 )
            let result = value > diffThreshold
            result

        let IsSameColor (color1: RGBASingle) (color2: RGBASingle) : Boolean =
            let value = Vector4.Distance( color1, color2 )
            let result = value < equalThreshold
            result

        let NeedsSampling (pix: PixelResult) : Boolean =
            if pix.skip then
                false
            else
                let x = pix.ind % scene.texWidth
                let y = pix.ind / scene.texWidth
            
                let left = pixelObjects.TryGetIndex( ConvertIndex (x-1) y )
                let right = pixelObjects.TryGetIndex( ConvertIndex (x+1) y )
                let bottom = pixelObjects.TryGetIndex( ConvertIndex x (y+1) )
                let top = pixelObjects.TryGetIndex( ConvertIndex x (y-1) )
                let tl = pixelObjects.TryGetIndex( ConvertIndex (x-1) (y-1) )
                let tr = pixelObjects.TryGetIndex( ConvertIndex (x+1) (y-1) )
                let bl = pixelObjects.TryGetIndex( ConvertIndex (x-1) (y+1) )
                let br = pixelObjects.TryGetIndex( ConvertIndex (x+1) (y+1) )

                (
                    (
                        match tl with
                        | Some value -> IsSameColor pix.color value.color
                        | None -> false
                        &&
                        (
                            match top with
                            | Some value -> IsDifferentColor pix.color value.color
                            | None -> false
                            ||
                            match left with
                            | Some value -> IsDifferentColor pix.color value.color
                            | None -> false
                        )
                    )
                    ||
                    (
                        match tr with
                        | Some value -> IsSameColor pix.color value.color
                        | None -> false
                        &&
                        (
                            match top with
                            | Some value -> IsDifferentColor pix.color value.color
                            | None -> false
                            ||
                            match right with
                            | Some value -> IsDifferentColor pix.color value.color
                            | None -> false
                        )
                    )
                    ||
                    (
                        match bl with
                        | Some value -> IsSameColor pix.color value.color
                        | None -> false
                        &&
                        (
                            match bottom with
                            | Some value -> IsDifferentColor pix.color value.color
                            | None -> false
                            ||
                            match left with
                            | Some value -> IsDifferentColor pix.color value.color
                            | None -> false
                        )    
                    )
                    ||
                    (
                        match br with
                        | Some value -> IsSameColor pix.color value.color
                        | None -> false
                        &&
                        (
                            match bottom with
                            | Some value -> IsDifferentColor pix.color value.color
                            | None -> false
                            ||
                            match right with
                            | Some value -> IsDifferentColor pix.color value.color
                            | None -> false
                        )    
                    )
                )
                ||
                (
                    match left with
                    | Some value -> IsDifferentColor pix.color value.color
                    | None -> false
                    ||
                    match right with
                    | Some value -> IsDifferentColor pix.color value.color
                    | None -> false
                    ||
                    match top with
                    | Some value -> IsDifferentColor pix.color value.color
                    | None -> false
                    ||
                    match bottom with
                    | Some value -> IsDifferentColor pix.color value.color
                    | None -> false
                )

        let CalcWeight (dist:Single) : Single =
            maxPixDist / (maxPixDist + dist )

        let startWeight = CalcWeight 0.0f

        let rec CalcSampleCount (value:Int32) (current:Single) =
            match value with
            | 0 -> current
            | _ -> CalcSampleCount (value-1) (current + 4.0f**(single (value-1)))

        let rec GetSampleCoords (center:Vector2) (coords:Vector3[]) (box:Vector4) (remainingDepth:Int32) (indexStart:Int32) : Int32 =
            let coord = new Vector2( (box.X + box.Z) / 2.0f, (box.Y + box.W) / 2.0f )
            coords.[indexStart] <- new Vector3( coord, CalcWeight( Vector2.DistanceSquared( coord, center ) ) )

            let b1 = new Vector4( box.X, box.Y, coord.X, coord.Y )
            let b2 = new Vector4( coord.X, box.Y, box.Z, coord.Y )
            let b3 = new Vector4( box.X, coord.Y, coord.X, box.W )
            let b4 = new Vector4( coord.X, coord.Y, box.Z, box.W )

            if remainingDepth = 0 then
                indexStart + 1
            else
                indexStart + 1
                |> GetSampleCoords center coords b1 (remainingDepth - 1)
                |> GetSampleCoords center coords b2 (remainingDepth - 1)
                |> GetSampleCoords center coords b3 (remainingDepth - 1)
                |> GetSampleCoords center coords b4 (remainingDepth - 1)

        let sampleCount = int32( CalcSampleCount (aaFactor + 1) 0.0f )
        
        let sampleOffsets: Vector3[] = Array.zeroCreate sampleCount

        let zeroBox = new Vector4( -pixelWidth, -pixelHeight, pixelWidth, pixelHeight )
        let zeroCenter = Vector2.Zero

        GetSampleCoords zeroCenter sampleOffsets zeroBox aaFactor 0
        |> ignore

        let totalWeight = ( sampleOffsets |> Seq.sumBy( fun vec -> vec.Z ) )
            

        let Sample ((index,pixIndex): struct(Int32*Int32))  : Async<struct(Int32*PixelResult)> =
            let pix = pixelObjects.[index]
            let x = pixIndex % scene.texWidth
            let y = pixIndex / scene.texWidth
            let left = pixelObjects.TryGetIndex(ConvertIndex (x-1) y)
            let right = pixelObjects.TryGetIndex(ConvertIndex (x+1) y)
            let bottom = pixelObjects.TryGetIndex(ConvertIndex x (y+1))
            let top = pixelObjects.TryGetIndex(ConvertIndex x (y-1))

            let TryAddSample (pix: Option<PixelResult>) ((color,total): struct(RGBASingle*Single))  : struct(RGBASingle*Single) =
                match pix with
                | Some value -> struct( color + value.color * adjacentWeight, total + adjacentWeight )
                | None -> struct(color, total)

            let AddStart ((color,total): struct(RGBASingle*Single)) : struct(RGBASingle*Single) =
                struct( color + pix.color, total + startWeight )

            let initColor = new RGBASingle( 0.0f, 0.0f, 0.0f, 0.0f )

            let SampleCoord (point:Vector3) : RGBASingle =
                let point2 = new Vector2( point.X, point.Y )
                let resColor = 
                    pix.localObjects
                    |> Array.fold (fun (current: RGBASingle) object -> object.Draw point2 current ) initColor
                resColor * point.Z
            
            async {
                let InitArray (ind:Int32) : Vector3 =
                    let offset = sampleOffsets.[ind]
                    Vector3(pix.coords.X + offset.X, pix.coords.Y + offset.Y, offset.Z)
               
                let coords : Vector3[] = Array.init sampleCount InitArray

                let resColor =
                    let struct(temp, total) = 
                        struct (coords |> Array.sumBy (SampleCoord), totalWeight)
                        |> TryAddSample( left )
                        |> TryAddSample( right )
                        |> TryAddSample( top )
                        |> TryAddSample( bottom )
                        |> AddStart
                    temp / total

                let cDif = Vector4.DistanceSquared( pix.color, resColor )
                return struct( index, PixelResult(pix.ind,resColor,pix.coords,pix.localObjects, cDif <= equalThreshold ) )
            }
          
        let tokSource = new System.Threading.CancellationTokenSource()

        let rec AAPass (curPass: Int32) (inds: struct(Int32*Int32)[]) : Unit =
            let filteredInds =
                inds
                |> Array.where( fun struct(ind,_) -> NeedsSampling pixelObjects.[ind] )

            if aaPasses >= curPass && filteredInds.Length > 0 then
                Async.RunSynchronously( filteredInds |> Seq.map Sample |> Async.Parallel, -1, tokSource.Token )
                |> Array.iter ( fun struct(ind, res) -> pixelObjects.[ind] <- res )

                filteredInds |> Seq.toArray |> AAPass (curPass + 1)

        if aaFactor > 0 then
            pixelObjects
            |> Array.mapi( fun ind pix -> struct(ind,pix.ind) )
            |> AAPass( 0 )

        pixelObjects
        |> Array.iter( fun pix -> scene.tex.[pix.ind] <- pix.color )
        