using UnityEngine;

namespace UI
{
    public abstract class BaseAnimationProvider : MonoBehaviour
    {
        public abstract void Play(string animationName);
    }
}