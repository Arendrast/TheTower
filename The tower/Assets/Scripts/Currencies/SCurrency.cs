using UnityEngine;

namespace Currencies
{
    public class SCurrency : Currency
    {
        [HideInInspector] public int GettingMoneyForLvl;
        private const string NameMoneyPlayerPref = "SCurrency";

        private new void Awake()
        {
            nameVariableInPlayerPref = NameMoneyPlayerPref;
            base.Awake();
        }
    }
}
