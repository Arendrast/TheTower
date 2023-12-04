using System.Collections;
using General;
using UnityEngine;

namespace Player
{
    public class TowerHealth : Health, IObjectBeindInitialized
    {
        public float UnitOfHealthRegeneration;
        [SerializeField] private float _regenerationFrequencyInSec = 1;

        public void Initialize()
        {
            Start();
            StartCoroutine(Regenerate());
        }

        private IEnumerator Regenerate()
        {
            yield return new WaitForSeconds(_regenerationFrequencyInSec);
            RestoreHealth(UnitOfHealthRegeneration);

            StartCoroutine(Regenerate());
        }
    }
}
