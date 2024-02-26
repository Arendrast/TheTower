namespace Currencies
{
    public class CrystalCurrency : Currency
    {
        private const string NameCrystalPlayerPref = "Crystal";

        private new void Awake()
        {
            nameVariableInPlayerPref = NameCrystalPlayerPref;
            base.Awake();
        }
    }
}
