namespace VectorRendering.General

open System
open System.Runtime.CompilerServices

[<Extension>]
type internal ArrayExtensions =
    [<Extension>]
    static member inline TryGetIndex<'T>( this: 'T[], index: Option<Int32> ) : Option<'T> = 
        match index with
        | Some ind when ind >= 0 && ind < this.Length -> Some this.[ind]
        | _ -> None