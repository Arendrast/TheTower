using System;
using Assets.TBR.Scripts.UI;
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
        [SerializeField] private Button _openButton;
        [SerializeField] private bool _isOpen;

        public bool IsOpen => _isOpen;
        
        protected void Awake()
        {
            _body.SetActive(_isOpen);
            
            _openButton?.onClick.AddListener(OpenPopup);
            closeButton?.onClick.AddListener(ClosePopup);

            //if (_popupAnimators.Length == 0)
                //Debug.LogWarning("Animation list is empty!");

            OnInitialization();
        }

        public void OpenPopup()
        {
            if (_isOpen) return;

            _isOpen = true;
            _body.SetActive(true);
            
            Opened?.Invoke();
            OnOpenPopup();
            PlayAnimation(_isOpen);
        }

        protected void ClosePopup()
        {
            if (!_isOpen) return;

            _isOpen = false;
            _body.SetActive(false);

            Closed?.Invoke();
            OnClosePopup();
            PlayAnimation(_isOpen);
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
            for (int i = 0; i < _popupAnimators.Length; i++)
                _popupAnimators[i].SetOpenFlag(isOpen);
        }
    }
}