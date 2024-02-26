using UnityEngine;

namespace Currencies
{
    public class MoneyCurrency : Currency
    {
        [HideInInspector] public int GettingMoneyForLvl;
        private const string NameMoneyPlayerPref = "Money";

        private new void Awake()
        {
            nameVariableInPlayerPref = NameMoneyPlayerPref;
            base.Awake();
        }
    }
}
