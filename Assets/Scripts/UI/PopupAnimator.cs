using UnityEngine;

namespace UI
{
    public class PopupAnimator : MonoBehaviour
    {
        [SerializeField] private string _openAnimationName = "Open";
        [SerializeField] private string _closeAnimationName = "Close";

        private BaseAnimationProvider _animationProvider;

        private void Awake()
        {
            _animationProvider = GetComponent<BaseAnimationProvider>();
            
            if (!_animationProvider)
                Debug.LogError($"AnimationProvider not exists on {gameObject.name}");
        }

        public void SetOpenFlag(bool flag)
        {
            if (!_animationProvider) 
                _animationProvider = GetComponent<BaseAnimationProvider>();

            _animationProvider.Play(flag 
                ? _openAnimationName 
                : _closeAnimationName);
        }
    }
}