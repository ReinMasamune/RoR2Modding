namespace AlternativeArtificer
{
    using R2API.AssetPlus;

    public partial class Main
    {
        private void DoText()
        {
            Languages.AddToken( "REIN_ALTARTI_PASSIVE_NAME", "Elemental Intensity" );
            Languages.AddToken( "REIN_ALTARTI_PASSIVE_DESC", "Selecting <style=cIsUtility>Ice</style> skills <style=cIsUtility>freezes</style> nearby enemies on execute." + "\n" +
                "Selecting <style=cIsUtility>Fire</style> skills increases <style=cIsDamage>Burning Damage</style> temporarily on casting a skill." + "\n" +
                "Selecting <style=cIsUtility>Lightning</style> skills fires <style=cIsDamage>Homing Projectiles</style> on casting a skill." );

            Languages.AddToken( "REIN_ALTARTI_LIGHTNING_SPECIAL_NAME", "Ion Surge" );
            Languages.AddToken( "REIN_ALTARTI_LIGHTNING_SPECIAL_DESC", "Repeatedly dash in any direction. Float in the air for a short time between each dash. 3 dashes in total." );
            Languages.AddToken( "REIN_ALTARTI_ALTMASTERYSKIN_NAME", "[Insert cool name here]" );
        }
    }
}
