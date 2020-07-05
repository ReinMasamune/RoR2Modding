namespace VectorRendering.Drawables

open System
open VectorRendering.Constructs
open VectorRendering.General
open VectorRendering.Vectors

type public Circle =
    struct
        val position : Vector2
        val startRadSq : Single
        val endRadSq : Single
        val mainColor : Vector4
        val borderColor : Vector4
        val borderOutSq : Single
        val borderInSq : Single
        val bounds : Vector4

        private new( borderInSq:Single, borderOutSq:Single, borderColor:Vector4, mainColor:Vector4, endRadSq:Single, startRadSq:Single, position:Vector2, bounds:Vector4 ) =
            { borderInSq = borderInSq; borderOutSq = borderOutSq; borderColor = borderColor; mainColor = mainColor; 
            endRadSq = endRadSq; startRadSq = startRadSq; position = position; bounds = bounds; } 


        public new( borderWidth:Single, borderColor:Vector4, mainColor:Vector4, endRadius:Single, startRadius:Single, position:Vector2 ) =
            let GetInnerBorder (borderWidth:Single) (startRadius:Single) : Single =
                if startRadius = 0.0f then
                    -1.0f;
                else
                    let temp = startRadius + borderWidth
                    temp * temp

            let borderInSq = GetInnerBorder borderWidth startRadius
            let tempOuter = endRadius - borderWidth
            let borderOutSq = tempOuter * tempOuter
            let endRadSq = endRadius * endRadius
            let startRadSq = startRadius * startRadius

            let xMin = position.X - endRadius
            let xMax = position.X + endRadius
            let yMin = position.Y - endRadius
            let yMax = position.Y + endRadius

            let bounds = new Vector4( xMin, yMin, xMax, yMax )

            Circle( borderInSq, borderOutSq, borderColor, mainColor, endRadSq, startRadSq, position, bounds )

        public new( mainColor:Vector4, startRadius:Single, endRadius:Single, position:Vector2 ) =
            Circle( 0.0f, Vector4.Zero, mainColor, endRadius, startRadius, position )

        public new( borderWidth:Single, borderColor:Vector4, mainColor:Vector4, radius:Single, position:Vector2 ) =
            Circle( borderWidth, borderColor, mainColor, radius, 0.0f, position )

        public new( mainColor:Vector4, radius:Single, position:Vector2 ) =
            Circle( 0.0f, Vector4.Zero, mainColor, radius, 0.0f, position )


        member internal this.DrawInternal (coords:Vector2) (currentColor:Vector4) : Vector4 =

            let diff = coords - this.position
            let distSq = diff.LengthSquared()
            if distSq > this.endRadSq then
                currentColor
            else if distSq < this.startRadSq then
                currentColor
            else
                if distSq > this.borderOutSq then
                    this.borderColor
                else if distSq < this.borderInSq then
                    this.borderColor
                else
                    this.mainColor

        member internal this.boundsInternal with get () = this.bounds


    interface IDrawable with
        member this.Draw (coords:Vector2) (currentColor:Vector4) : Vector4 =
            this.DrawInternal coords currentColor

        member this.bounds with get () = this.boundsInternal
end