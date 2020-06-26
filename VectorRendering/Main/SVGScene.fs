namespace VectorRendering.Constructs

open System
open System.Collections.Generic
open VectorRendering.General
open VectorRendering.Drawables

type public SVGScene<'TTexture when 'TTexture :> IList<RGBASingle>> =
    struct
        val public texWidth : Int32
        val public texHeight : Int32
        val public tex : 'TTexture
        val public objects : Seq<Renderable>
        val public background : RGBASingle

        new( texWidth:Int32, texHeight:Int32, background:RGBASingle, tex: 'TTexture, objects: Seq<Renderable>) =
            { texWidth = texWidth; texHeight = texHeight; tex = tex; objects = objects; background = background }
end