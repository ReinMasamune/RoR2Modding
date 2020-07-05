namespace VectorRendering.Vectors

open System

[<Struct>]
[<StructuralComparison>]
[<StructuralEquality>]
type public Vector2 =
    val mutable X : Single
    val mutable Y : Single

    new( X: Single, Y: Single ) =
        { X = X; Y = Y; }

    static member inline SquareMagnitude( v1: Vector2 ) : Single =
        v1.X * v1.X + v1.Y * v1.Y

    static member inline Magnitude( v1: Vector2 ) : Single =
        single <| Math.Sqrt( float <| Vector2.SquareMagnitude( v1 ) )

    member inline this.Length () : Single =
        Vector2.Magnitude( this )

    member inline this.LengthSquared () : Single =
        Vector2.SquareMagnitude( this )

    static member inline (~-) (v: Vector2) : Vector2 =
        Vector2( -v.X, -v.Y )

    static member inline (-) (v1: Vector2, v2: Vector2) : Vector2 =
        Vector2( v1.X - v2.X, v1.Y - v2.Y )

    static member inline (+) (v1: Vector2, v2: Vector2) : Vector2 =
        Vector2( v1.X + v2.X, v1.Y + v2.Y )

    static member inline (*) (v1: Vector2, value: Single ) : Vector2 =
        Vector2( v1.X * value, v1.Y * value )

    static member inline (*) ( value: Single, v1: Vector2) : Vector2 =
        Vector2( v1.X * value, v1.Y * value )

    static member inline (/) (v1: Vector2, value: Single ) : Vector2 =
        Vector2( v1.X / value, v1.Y / value )

    static member Zero with inline get() = Vector2( 0.0f, 0.0f )
    
    static member inline Dot( v1: Vector2, v2: Vector2 ) : Single =
        v1.X * v2.X + v1.Y * v2.Y

    static member inline DistanceSquared( v1: Vector2, v2: Vector2 ) : Single =
        Vector2.SquareMagnitude( v1 - v2 )
        
    static member inline Distance( v1: Vector2, v2: Vector2 ) : Single =
        Vector2.Magnitude( v1 - v2 )

    static member inline Normalize( v1: Vector2 ) : Vector2 =
        if v1 = Vector2.Zero then
            Vector2.Zero
        else
            v1 / Vector2.Magnitude( v1 )




[<Struct>]
[<StructuralComparison>]
[<StructuralEquality>]
type public Vector3 =
    val mutable X : Single
    val mutable Y : Single
    val mutable Z : Single

    new( X: Single, Y: Single, Z: Single ) =
        { X = X; Y = Y; Z = Z;}

    new( v: Vector2, Z: Single ) =
        Vector3( v.X, v.Y, Z )

    new( X: Single, v: Vector2 ) =
        Vector3( X, v.X, v.Y )

    static member inline SquareMagnitude( v1: Vector3 ) : Single =
        v1.X * v1.X + v1.Y * v1.Y + v1.Z * v1.Z

    static member inline Magnitude( v1: Vector3 ) : Single =
        single <| Math.Sqrt( float <| Vector3.SquareMagnitude( v1 ) )

    member inline this.Length () : Single =
        Vector3.Magnitude( this )

    member inline this.LengthSquared () : Single =
        Vector3.SquareMagnitude( this )

    static member inline (~-) (v: Vector3) : Vector3 =
        Vector3( -v.X, -v.Y, -v.Z )

    static member inline (-) (v1: Vector3, v2: Vector3) : Vector3 =
        Vector3( v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z )

    static member inline (+) (v1: Vector3, v2: Vector3) : Vector3 =
        Vector3( v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z )

    static member inline (*) (v1: Vector3, value: Single ) : Vector3 =
        Vector3( v1.X * value, v1.Y * value, v1.Z * value )

    static member inline (*) (value: Single, v1: Vector3 ) : Vector3 =
        Vector3( v1.X * value, v1.Y * value, v1.Z * value )

    static member inline (/) (v1: Vector3, value: Single ) : Vector3 =
        Vector3( v1.X / value, v1.Y / value, v1.Z / value )

    static member Zero with inline get() = Vector3( 0.0f, 0.0f, 0.0f )

    static member inline Normalize( v1: Vector3 ) : Vector3 =
        if v1 = Vector3.Zero then
            Vector3.Zero
        else
            v1 / Vector3.Magnitude( v1 )

    static member inline Dot( v1: Vector3, v2: Vector3 ) : Single =
        v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z



    static member inline DistanceSquared( v1: Vector3, v2: Vector3 ) : Single =
        Vector3.SquareMagnitude( v1 - v2 )
        
    static member inline Distance( v1: Vector3, v2: Vector3 ) : Single =
        Vector3.Magnitude( v1 - v2 )

[<Struct>]
[<StructuralComparison>]
[<StructuralEquality>]
type public Vector4 =
    val mutable X : Single
    val mutable Y : Single
    val mutable Z : Single
    val mutable W : Single

    new( X: Single, Y: Single, Z: Single, W: Single ) =
        { X = X; Y = Y; Z = Z; W = W }

    new( v: Vector3, w: Single ) =
        Vector4( v.X, v.Y, v.Z, w )

    new( x: Single, v: Vector3 ) =
        Vector4( x, v.X, v.Y, v.Z )

    new( v1: Vector2, v2: Vector2 ) =
        Vector4( v1.X, v1.Y, v2.X, v2.Y )

    new( v1: Vector2, Z: Single, W: Single ) =
        Vector4( v1.X, v1.Y, Z, W )

    new( x: Single, Y: Single, v: Vector2 ) =
        Vector4( x, Y, v.X, v.Y )

    new( x: Single, v: Vector2, w: Single ) =
        Vector4( x, v.X, v.Y, w )

    static member inline SquareMagnitude( v1: Vector4 ) : Single =
        v1.X * v1.X + v1.Y * v1.Y + v1.Z * v1.Z + v1.W * v1.W

    static member inline Magnitude( v1: Vector4 ) : Single =
        single <| Math.Sqrt( float <| Vector4.SquareMagnitude( v1 ) )

    member inline this.Length () : Single =
        Vector4.Magnitude( this )

    member inline this.LengthSquared () : Single =
        Vector4.SquareMagnitude( this )

    static member inline (~-) (v: Vector4) : Vector4 =
        Vector4( -v.X, -v.Y, -v.Z, -v.W )

    static member inline (-) (v1: Vector4, v2: Vector4) : Vector4 =
        Vector4( v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.W - v2.W )

    static member inline (+) (v1: Vector4, v2: Vector4) : Vector4 =
        Vector4( v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.W + v2.W )

    static member inline (*) (v1: Vector4, value: Single ) : Vector4 =
        Vector4( v1.X * value, v1.Y * value, v1.Z * value, v1.W * value )

    static member inline (*) (value: Single, v1: Vector4 ) : Vector4 =
        Vector4( v1.X * value, v1.Y * value, v1.Z * value, v1.W * value )

    static member inline (/) (v1: Vector4, value: Single ) : Vector4 =
        Vector4( v1.X / value, v1.Y / value, v1.Z / value, v1.W / value )

    static member Zero with inline get() = Vector4( 0.0f, 0.0f, 0.0f, 0.0f )

    static member inline Normalize( v1: Vector4 ) : Vector4 =
        if v1 = Vector4.Zero then
            Vector4.Zero
        else
            v1 / Vector4.Magnitude( v1 )

    static member inline Dot( v1: Vector4, v2: Vector4 ) : Single =
        v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z * v1.W * v2.W



    static member inline DistanceSquared( v1: Vector4, v2: Vector4 ) : Single =
        Vector4.SquareMagnitude( v1 - v2 )
        
    static member inline Distance( v1: Vector4, v2: Vector4 ) : Single =
        Vector4.Magnitude( v1 - v2 )