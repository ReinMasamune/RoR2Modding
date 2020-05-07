namespace MainModule

open BepInEx

[<BepInPlugin("com.Rein.Test", "Testing", "1.0.0")>]
type MainModule() as this =
    class
    inherit BaseUnityPlugin()
    do
        this.Logger.LogWarning( "I Exist" )

    member this.Awake() =
        this.Logger.LogWarning( "Awake" )

    end