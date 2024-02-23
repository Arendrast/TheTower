using UnityEngine;

namespace General
{
    public class Particles : MonoBehaviour
    {
        private void OnParticleSystemStopped() => Destroy(transform.parent.gameObject);
    }
}
