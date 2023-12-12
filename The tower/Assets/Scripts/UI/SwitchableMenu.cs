using System.Collections;
using General;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine.UI;

namespace UI
{
    public class SwitchableMenu : MonoBehaviour, IObjectBeindInitialized
    {
        public bool IsOpen { get; private set; }
        [SerializeField] private Vector3 _fullScale = new Vector3(1, 1, 1);
        [SerializeField] private float _durationOfSetScale = 1f;
        [SerializeField] private Image _window, _background;
        [SerializeField] private bool _isOffOnStart = true;
        private TweenerCore<Vector3, Vector3, VectorOptions> _currentSetScaleTweenerCore;

        public void Initialize()
        {
            IsOpen = !_isOffOnStart;
            if (_isOffOnStart)
            {
                gameObject.SetActive(false);
                _window.transform.localScale = Vector3.zero;
                _background.transform.localScale = Vector3.zero;
            }
        }

        public virtual void SetState()
        {
            IsOpen = !IsOpen;

            if (IsOpen)
                gameObject.SetActive(true);

            _currentSetScaleTweenerCore?.Kill();

            if (!IsOpen)
            {
                SetScale(_window.transform).OnComplete(() => 
                    SetScale(_background.transform).OnComplete(() => 
                        gameObject.SetActive(false)));
            }
            else
            {
                SetScale(_window.transform).OnComplete(() =>
                    SetScale(_background.transform));
            }
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> SetScale(Transform localTransform)
        {
            return localTransform.transform.DOScale(IsOpen ? _fullScale : Vector3.zero, _durationOfSetScale).SetUpdate(true);   
        }

        public void SetStateAfterWhile(float time) => Invoke(nameof(SetState), time);
    }
}
