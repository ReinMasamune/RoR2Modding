namespace CollectOnButton
open System
open BepInEx
open UnityEngine

[<BepInPlugin("com.Rein.CollectOnbutton", "CollectOnButton", "1.0.0.0")>]
type internal Plugin() =
    inherit BaseUnityPlugin()

    let boundKey = KeyCode.F10
    let GetKey () : Boolean =
        Input.GetKeyDown(boundKey)

    with
        member this.FixedUpdate() =
            if GetKey () then
                base.Logger.LogMessage( "Collecting garbage" )
                GC.Collect()
    end