namespace VectorRendering.Constructs

open System
open System.Numerics
open VectorRendering.General

type public IDrawable =
    interface
        abstract member Draw : Vector2 -> RGBASingle -> RGBASingle
        abstract member bounds : Vector4 with get
end


