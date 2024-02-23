namespace Currencies
{
    public class Crystal : Currency
    {
        private const string NameCrystalPlayerPref = "Crystal";

        private new void Awake()
        {
            nameVariableInPlayerPref = NameCrystalPlayerPref;
            base.Awake();
        }
    }
}
