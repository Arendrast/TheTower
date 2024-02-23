using UnityEngine;

namespace Assets.TBR.Scripts.UI
{
    public abstract class BaseAnimationProvider : MonoBehaviour
    {
        public abstract void Play(string name);
    }
}