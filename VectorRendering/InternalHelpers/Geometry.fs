namespace VectorRendering.InternalHelpers

open System
open System.Numerics
open VectorRendering.General
open VectorRendering.Constructs

module internal Geometry =
    let internal CalculateRelationship (line:Vector4) (point:Vector2) : PointLineRelationship =
        let startP = new Vector2( line.X, line.Y )
        let endP = new Vector2( line.Z, line.W )
        let diffEndStart = endP - startP
        let diffPointStart = point - startP

        let temp1 = diffEndStart.X * diffPointStart.Y
        let temp2 = diffPointStart.X * diffEndStart.Y

        match temp1 - temp2 with
        | value when value < 0.0f -> PointLineRelationship.Left
        | value when value > 0.0f -> PointLineRelationship.Right
        | _ -> PointLineRelationship.On


    let internal WindingNumber (poly:Seq<Vector4>) (point:Vector2) : Int32 =

        let Folder (windingNumber:Int32) (edge:Vector4) : Int32 =
            match edge with
            | edge when edge.Y <= point.Y && edge.W > point.Y && CalculateRelationship edge point = PointLineRelationship.Left -> windingNumber + 1
            | edge when edge.Y > point.Y && edge.W <= point.Y && CalculateRelationship edge point = PointLineRelationship.Right -> windingNumber - 1
            | _ -> windingNumber

        poly
        |> Seq.fold Folder 0 

    let internal TestBoxOverlap (box1:Vector4) (box2:Vector4) : Boolean =
        let AxisOverlap (min1:Single) (max1:Single) (min2:Single) (max2:Single) : Boolean =
            match struct(min1, max1, min2, max2) with
            | (min1, max1, min2, max2) when max1 >= min2 && max2 >= min1 -> true
            | _ -> false

        AxisOverlap box1.X box1.Z box2.X box2.Z
        &&
        AxisOverlap box2.Y box2.W box1.Y box1.W

    let internal TestPointInBox (box:Vector4) (point:Vector2) : Boolean =
        let inline InAxis (min:Single) (max:Single) (value:Single) : Boolean =
            match value with
            | v when v <= max && v >= min -> true
            | _ -> false

        InAxis box.X box.Z point.X
        &&
        InAxis box.Y box.W point.Y

    let internal AngleBetweenTwoVectors (v1: Vector2) (v2: Vector2) : Single =
        Math.Acos( float( Vector2.Dot( v1, v2 ) ) )
        |> single

    let internal PointLineSegmentDistanceSquaredAndFrac (point: Vector2) ((lineP1,lineP2): struct(Vector2*Vector2)) : struct(Single*Single) =
        let lineLengthSq = Vector2.DistanceSquared( lineP1, lineP2 )
        if lineLengthSq = 0.0f then
            let dist = Vector2.DistanceSquared( lineP1, point )
            (dist, 0.5f )
        else
            let frac = Math.Max( 0.0f, Math.Min( 1.0f, Vector2.Dot( point - lineP1, lineP2 - lineP1 ) / lineLengthSq ) )
            let projected = lineP1 + frac * (lineP2 - lineP1)
            struct(Vector2.DistanceSquared( point, projected ), frac)