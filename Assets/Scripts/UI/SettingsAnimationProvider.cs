using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsAnimationProvider : BaseAnimationProvider
    {
        [SerializeField] private Image _window;
        [SerializeField] private Image _background;
        [SerializeField, Range(0, 1)] private float _initialTransparencyOfBackground;
        [SerializeField] private float _fadeDuration, _setScaleDuration;
        [SerializeField] private Vector3 _startScale;
        private TweenerCore<Vector3, Vector3, VectorOptions> _currentSetScaleTweenerCore;
        public override void Play(string animationName)
        {
            var isOpen = animationName == "Open";

            if (isOpen)
            {
                gameObject.SetActive(true);
                SetScale(_window.transform, true);
            }
            else
            {
                SetScale(_window.transform, false).OnComplete(() => gameObject.SetActive(false));   
            }

            _currentSetScaleTweenerCore?.Kill();

            _background.DOFade(isOpen 
                ? _initialTransparencyOfBackground 
                : 0, _fadeDuration).SetUpdate(true);
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> SetScale(Transform localTransform, bool isOpen)
        {
            return localTransform.transform.DOScale(isOpen ? _startScale : Vector3.zero, _setScaleDuration)
                .SetUpdate(true);
        }
    }
}