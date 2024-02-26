using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class BasePopup : MonoBehaviour
    {
        public Action Opened, Closed;
        [Header("Open/Close Settings")] 
        [SerializeField] private GameObject _body;

        [SerializeField] private PopupAnimator[] _popupAnimators;
        [SerializeField] protected Button closeButton;
        [SerializeField] protected Button openButton;
        [SerializeField] private bool _isOpen;

        public bool IsOpen => _isOpen;
        private bool IsHaveAnimator() => _popupAnimators.Length > 0; 
        
        protected void Awake()
        {
            _body.SetActive(_isOpen);
            
            openButton?.onClick.AddListener(OpenPopup);
            closeButton?.onClick.AddListener(ClosePopup);
            
            OnInitialization();
        }

        private void OnSetState()
        {
            if (IsHaveAnimator())
            {
                PlayAnimation(_isOpen);   
            }
            else
            {
                _body.gameObject.SetActive(_isOpen); 
            }
        }

        public void OpenPopup()
        {
            if (_isOpen) return;

            _isOpen = true;
            OnSetState();
            Opened?.Invoke();
            OnOpenPopup();
            PlayAnimation(_isOpen);
        }

        public void ClosePopup()
        {
            if (!_isOpen) return;

            _isOpen = false;
            OnSetState();
            Closed?.Invoke();
            OnClosePopup();
        }

        #region Callbacks

        protected virtual void OnInitialization()
        {
        }

        protected virtual void OnOpenPopup()
        {
        }

        protected virtual void OnClosePopup()
        {
        }

        #endregion Callbacks

        private void PlayAnimation(bool isOpen)
        {
            foreach (var animator in _popupAnimators)
                animator.SetOpenFlag(isOpen);
        }
    }
}