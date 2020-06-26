namespace VectorRendering.Drawables

open VectorRendering.Constructs
open System.Numerics

type public Renderable =
    | Circle of Circle
    | Polygon of Polygon
    | Custom of IDrawable

    member inline this.Draw (coord: Vector2) (color: Vector4) : Vector4 =
        match this with
        | Circle value -> value.DrawInternal coord color
        | Polygon value -> value.DrawInternal coord color
        | Custom value -> value.Draw coord color

    member inline this.bounds with get () =
        match this with
        | Circle value -> value.boundsInternal
        | Polygon value -> value.boundsInternal
        | Custom value -> value.bounds

    static member op_Implicit (value: Circle) : Renderable = Circle value
    static member op_Implicit (value: Polygon) : Renderable = Polygon value
    static member op_Implicit (value: IDrawable) : Renderable = Custom value
    