namespace VectorRendering.Constructs

open System
open VectorRendering.General
open VectorRendering.Vectors

type public IDrawable =
    interface
        abstract member Draw : Vector2 -> RGBASingle -> RGBASingle
        abstract member bounds : Vector4 with get
end


