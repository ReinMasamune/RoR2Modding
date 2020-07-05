namespace rec VectorRendering.Drawables

open System
open VectorRendering.Constructs
open VectorRendering.General
open VectorRendering.InternalHelpers
open VectorRendering.Vectors

[<Struct>]
type internal RenderResult =
    | Interior
    | Border
    | Exterior
    | Debug

[<Struct>]
type internal PolygonVertex =
    val internal position: Vector2
    val internal index: Int32
    val internal vertType: VertexType
    val internal angle: Single
    val internal circlePosition: Vector2
    val internal circleRadius: Single
    val internal cornerDistance: Single
    val internal prevEdgeFrac: Single
    val internal nextEdgeFrac: Single
    val internal next: Int32
    val internal prev: Int32
    val internal otherPoints: PolygonVertex[]

    member internal this.nextVertex with get () : PolygonVertex =
        this.otherPoints.[this.next]

    member internal this.prevVertex with get () : PolygonVertex =
        this.otherPoints.[this.prev]


    private new(position: Vector2, index: Int32, vertType: VertexType, angle: Single, circlePosition: Vector2, circleRadius: Single, cornerDistance: Single, prevEdgeFrac: Single, nextEdgeFrac: Single, prev: Int32, next: Int32, otherPoints: PolygonVertex[] ) =
        { position = position; index = index; vertType = vertType; angle = angle; circlePosition = circlePosition; circleRadius = circleRadius; cornerDistance = cornerDistance; nextEdgeFrac = nextEdgeFrac; prevEdgeFrac = prevEdgeFrac; prev = prev; next = next; otherPoints = otherPoints; }

    internal new( pointData: Vector3, index: Int32, totalVerts: Int32 ) =
        let pos = Vector2( pointData.X, pointData.Y )
        let roundingRadius = pointData.Z
        let (prevIndex, nextIndex) =
            match index with
            | 0 -> (totalVerts - 1, 1)
            | value when value = totalVerts - 1 -> (index - 1, 0)
            | _ -> (index - 1, index + 1)

        let circleRad = roundingRadius

        PolygonVertex( pos, index, VertexType.Flat, 0.0f, pos, circleRad, 0.0f, 0.0f, 0.0f, prevIndex, nextIndex, Unchecked.defaultof<PolygonVertex[]> )

    internal new(existing: PolygonVertex, otherPoints: PolygonVertex[] ) =
        PolygonVertex(existing.position, existing.index, existing.vertType, existing.angle, existing.circlePosition, existing.circleRadius, 0.0f, existing.nextEdgeFrac, existing.prevEdgeFrac, existing.prev, existing.next, otherPoints )

    internal new(existing: PolygonVertex, vertType: VertexType ) =
        let diff1 = existing.prevVertex.position - existing.position
        let diff2 = existing.nextVertex.position - existing.position

        let normal1 = Vector2.Normalize diff1
        let normal2 = Vector2.Normalize diff2

        let directionToCircle = Vector2.Normalize( normal1 + normal2 )

        let dist1 = diff1.Length()
        let dist2 = diff2.Length()

        let radiusSquare = existing.circleRadius * existing.circleRadius

        let angle =
            0.5f * match vertType with
                   | Convex -> Geometry.AngleBetweenTwoVectors normal1 normal2
                   | Concave -> -(Geometry.AngleBetweenTwoVectors normal1 normal2)
                   | Flat -> 0.0f
        //if angle = 0.0f then
        PolygonVertex( existing.position, existing.index, VertexType.Flat, 0.0f, existing.position, 0.0f, 0.0f, 0.0f, 0.0f, existing.prev, existing.next, existing.otherPoints )
        //else if angle < 0.0f then
        //    let dist = existing.circleRadius / (single <| Math.Sin( float <| -angle ) )

        //    let distSquare = dist * dist
        //    let hyp = single <| Math.Sqrt( float <| (distSquare - radiusSquare) )
        //    let frac1 = hyp / dist1
        //    let frac2 = hyp / dist2
        //    let circlePos = existing.position + ( directionToCircle * dist)
        //    let cornerDistSq = distSquare - radiusSquare
        //    PolygonVertex( existing.position, existing.index, vertType, angle, circlePos, radiusSquare, cornerDistSq, frac1, frac2, existing.prev, existing.next, existing.otherPoints )
        //else
        //    let dist = existing.circleRadius / (single <| Math.Sin( float <| angle ) )

        //    let distSquare = dist * dist
        //    let hyp = single <| Math.Sqrt( float <| (distSquare - radiusSquare) )
        //    let frac1 = hyp / dist1
        //    let frac2 = hyp / dist2
        //    let circlePos = existing.position + ( directionToCircle * dist)
        //    let cornerDistSq = distSquare - radiusSquare
        //    PolygonVertex( existing.position, existing.index, vertType, angle, circlePos, radiusSquare, cornerDistSq, frac1, frac2, existing.prev, existing.next, existing.otherPoints )


[<Struct>]
[<NoComparison>]
[<NoEquality>]
type public Polygon =
    val internal points: PolygonVertex[]
    val internal bounds: Vector4
    val internal borderWidth: Single
    val internal mainColor: RGBASingle
    val internal borderColor: RGBASingle

    private new( points: PolygonVertex[], bounds: Vector4, borderWidth: Single, mainColor: RGBASingle, borderColor: RGBASingle ) =
        { points = points; bounds = bounds; borderWidth = borderWidth; mainColor = mainColor; borderColor = borderColor; }

    private new( points: PolygonVertex[] ) =
        let FindMinMaxBounds (bounds:Vector4) (vert: PolygonVertex) : Vector4 =
            let point = vert.position
            let mutable bounds = bounds

            if point.X <= bounds.X then
                bounds.X <- point.X
            if point.X >= bounds.Z then
                bounds.Z <- point.X
            if point.Y <= bounds.Y then
                bounds.Y <- point.Y
            if point.Y >= bounds.W then
                bounds.W <- point.Y
            bounds

        let initialBounds = new Vector4( Single.PositiveInfinity, Single.PositiveInfinity, Single.NegativeInfinity, Single.NegativeInfinity )

        let bounds = 
            points
            |> Seq.fold FindMinMaxBounds initialBounds

        Polygon( points, bounds, 0.0f, Vector4.Zero, Vector4.Zero )

    public new( mainColor: RGBASingle, borderColor: RGBASingle, borderWidth: Single, points: Vector3[] ) =
        let vertCount = points.Length
        let vertArray =
            points
            |> Array.mapi( fun index vec -> PolygonVertex(vec, index, vertCount ))
        let poly = Polygon( vertArray )

        let OtherPointsMapper (index: Int32) : Int32 =
            poly.points.[index] <- PolygonVertex( poly.points.[index], poly.points )
            index

        let MapFoldDet (current: Int32) (index: Int32) : Sign*Int32 =
            let vert = poly.points.[index]
            let prev = vert.prevVertex
            let next = vert.nextVertex

            let v1 = vert.position - prev.position
            let v2 = next.position - vert.position

            let ad = v1.X * v2.Y
            let bc = v1.Y * v2.X
            let det = ad - bc
            match det with
            | value when value > 0.0f -> (Positive, current + 1)
            | value when value < 0.0f -> (Negative, current - 1)
            | _ -> (Zero, current)

        let inds = [0..vertCount - 1]

        inds |> List.iter ( fun index -> poly.points.[index] <- PolygonVertex( poly.points.[index], poly.points ) )
        let (signs, total) =
            inds |> List.mapFold MapFoldDet 0

        let totalSign =
            match total with
            | i when i < 0 -> Negative
            | i when i > 0 -> Positive
            | _ -> Zero

        let UpdateTypes ((index,sign): Int32*Sign) : Unit =
            let vertType =
                match (totalSign,sign) with
                | (a,b) when a = b -> VertexType.Convex
                | (a,Zero) -> VertexType.Flat
                | (Zero,b) -> VertexType.Flat
                | _ -> VertexType.Concave
            poly.points.[index] <- PolygonVertex( poly.points.[index], vertType )

        List.zip inds signs
        |> List.iter UpdateTypes

        Polygon( poly.points, poly.bounds, borderWidth * borderWidth, mainColor, borderColor )

    member internal this.DrawInternal (coords: Vector2) (currentColor: RGBASingle) : RGBASingle =
        let self = this
        if Geometry.TestPointInBox this.bounds coords then

                let edges = this.points |> Array.map (fun point -> Vector4( point.position.X, point.position.Y, point.nextVertex.position.X, point.nextVertex.position.Y ) ) 

                let windingNumber = Geometry.WindingNumber edges coords

                let isInsidePoly = windingNumber <> 0


                let mutable closestEdgeDist = Single.PositiveInfinity

                let GetRelevantCorner (point: PolygonVertex) : Option<struct(PolygonVertex*Single*Single)> =
                        let struct(edge1Dist,edge1Frac) = Geometry.PointLineSegmentDistanceSquaredAndFrac coords struct(point.position, point.prevVertex.position) 
                        let struct(edge2Dist,edge2Frac) = Geometry.PointLineSegmentDistanceSquaredAndFrac coords struct(point.position, point.nextVertex.position)

                        match Math.Min( edge1Dist, edge2Dist ) with
                        | value when value < closestEdgeDist -> closestEdgeDist <- value
                        | _ -> ()

                        if (edge1Frac < point.prevEdgeFrac && edge2Frac < point.nextEdgeFrac) then
                            Some struct( point, edge1Dist, edge2Dist )
                        else
                            None


                let relevantCorners =
                    this.points |> Array.choose GetRelevantCorner

                let baseResult = 
                    if isInsidePoly then 
                        if closestEdgeDist < this.borderWidth then 
                            Border 
                        else 
                            Interior 
                    else Exterior

                if isInsidePoly then
                    let ResultFolder (current: RenderResult) ((corner,prevDist,nextDist): struct(PolygonVertex*Single*Single) ) : RenderResult =
                        let cornerDist = Vector2.DistanceSquared( coords, corner.position )
                        let circleDist = Vector2.DistanceSquared( coords, corner.circlePosition )
                        if corner.vertType = Convex then
                            if circleDist > corner.circleRadius then
                                Exterior
                            else
                                if Math.Sqrt( float <| circleDist ) > (Math.Sqrt( float <| corner.circleRadius ) - Math.Sqrt( float <| self.borderWidth ) ) then
                                    Border
                                else
                                    current
                        else if corner.vertType = Concave then
                            if cornerDist < corner.cornerDistance && circleDist > (corner.circleRadius) then
                                if Math.Sqrt(float <| circleDist) > (Math.Sqrt(float <| corner.circleRadius) + Math.Sqrt(float <| self.borderWidth) ) then
                                    Interior
                                else
                                    Border
                            else
                                current
                        else
                            current
                            


                    let result = relevantCorners |> Array.fold ResultFolder baseResult

                    match result with
                    | Debug -> RGBASingle( 0.0f, 1.0f, 1.0f, 1.0f )                        
                    | Interior -> this.mainColor
                    | Border -> this.borderColor
                    | Exterior -> currentColor

                else
                    let ResultFolder (current: RenderResult) ((corner,prevDist,nextDist): struct(PolygonVertex*Single*Single) ) : RenderResult =
                        if corner.vertType = Convex then
                            current
                        else if corner.vertType = Concave then
                            let cornerDist = Vector2.DistanceSquared( coords, corner.position )
                            let circleDist = Vector2.DistanceSquared( coords, corner.circlePosition )
                            if cornerDist < corner.cornerDistance && circleDist > (corner.circleRadius) then
                                if Math.Sqrt(float <| circleDist) > (Math.Sqrt(float <| corner.circleRadius) + Math.Sqrt(float <| self.borderWidth) ) then
                                    Interior
                                else
                                    Border
                            else
                                current
                        else
                            current

                    let result = relevantCorners |> Array.fold ResultFolder baseResult

                    match result with
                    | Debug -> RGBASingle( 0.0f, 1.0f, 1.0f, 1.0f )                        
                    | Interior -> this.mainColor
                    | Border -> this.borderColor
                    | Exterior -> currentColor
            else
                currentColor

    member internal this.boundsInternal with get () = this.bounds


    interface IDrawable with
        member this.Draw (coords: Vector2) (currentColor: RGBASingle) : RGBASingle = this.DrawInternal coords currentColor
           
        member this.bounds with get() = this.boundsInternal