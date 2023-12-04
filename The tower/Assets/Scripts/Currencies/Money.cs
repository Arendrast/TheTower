using UnityEngine;

namespace Currencies
{
    public class Money : Currency
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
