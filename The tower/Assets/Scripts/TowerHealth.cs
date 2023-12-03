using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : Health
{
    public float unitOfHealthRegeneration;
    [SerializeField] private float _regenerationFrequencyInSec = 1;

    private new void Start()
    {
        base.Start();
        StartCoroutine(Regenerate());
    }

    private IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(_regenerationFrequencyInSec);
        RestoreHealth(unitOfHealthRegeneration);

        StartCoroutine(Regenerate());
    }
}
