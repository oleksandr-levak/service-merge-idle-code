using System;
using MergeIdle.Scripts.Managers.UI.Popup.Enums;
using MergeIdle.Scripts.Managers.UI.Popup.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views
{
    public class BasePopupView: MonoBehaviour, IBasePopupView
    {
        public event Action<EPopup> OnShow;
        public event Action<EPopup> OnHide;
        public event Action OnClickBackground;
        
        [SerializeField] private EPopup _popupType;
        [SerializeField] private GameObject _view;
        [SerializeField] private Button _background;

        public EPopup PopupType => _popupType;
        

        public void Show()
        {
            OnShow?.Invoke(_popupType);
            _view.SetActive(true);
        }

        public void Hide()
        {
            OnHide?.Invoke(_popupType);
            _view.SetActive(false);
        }

        protected virtual void OnEnable()
        {
            if (_background != null)
            {
                _background.onClick.AddListener(() => OnClickBackground?.Invoke());
                OnClickBackground += Hide; 
            }
        }
        
        protected virtual void OnDestroy()
        {
            if (_background != null)
            {
                _background.onClick.RemoveListener(() => OnClickBackground?.Invoke());
                OnClickBackground = null; 
            }

            Delegate[] onShowList = OnShow?.GetInvocationList();
            if (onShowList != null)
                foreach (var d in onShowList)
                    OnShow -= (d as Action<EPopup>);
            
            Delegate[] onHideList = OnHide?.GetInvocationList();
            if (onHideList != null)
                foreach (var d in onHideList)
                    OnHide -= (d as Action<EPopup>);
        }
    }
}