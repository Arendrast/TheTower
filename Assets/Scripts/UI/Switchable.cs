using System.Threading;
using General;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine.UI;
using Timer = General.Timer;

namespace UI
{
    public class Switchable : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        [SerializeField] private float _setScaleDuration = 0.5f, _fadeDuration = 0.4f;
        [SerializeField] private Image _window, _background;
        [SerializeField] private bool _isOffOnStart = true;
        private TweenerCore<Vector3, Vector3, VectorOptions> _currentSetScaleTweenerCore;
        private Vector3 _initializeScale;
        private float _initialTransparencyOfBackground;
        private Timer _timer;

        public void Start()
        {
            IsOpen = !_isOffOnStart;
            _initializeScale = transform.localScale;
            _initialTransparencyOfBackground = _background.color.a;
            _timer = new Timer(this);
            _timer.OnTimeIsOver += SetState;
            if (_isOffOnStart)
            {
                gameObject.SetActive(false);
                _window.transform.localScale = Vector3.zero;
                _background.DOFade(0, 0);
            }
        }

        public virtual void SetState()
        {
            IsOpen = !IsOpen;

            if (IsOpen)
            {
                SetScale(_window.transform);
                gameObject.SetActive(true);
            }
            else
                SetScale(_window.transform).OnComplete(() => gameObject.SetActive(false));

            _currentSetScaleTweenerCore?.Kill();

            _background.DOFade(IsOpen ? _initialTransparencyOfBackground : 0, _fadeDuration).SetUpdate(true);
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> SetScale(Transform localTransform)
        {
            return localTransform.transform.DOScale(IsOpen ? _initializeScale : Vector3.zero, _setScaleDuration)
                .SetUpdate(true);
        }

        public virtual void SetStateAfterWhile(float time) => Invoke(nameof(SetState), time);
    }
}
