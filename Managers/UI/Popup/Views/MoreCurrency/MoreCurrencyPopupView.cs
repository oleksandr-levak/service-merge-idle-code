using System;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views.MoreCurrency
{
    public class MoreCurrencyPopupView: BasePopupView
    {
        public event Action OnClickWatchNow;
        public event Action OnClickClose;
        
        [SerializeField] private Button _close;
        [SerializeField] private Button _watchNow;
        protected override void OnEnable()
        {
            base.OnEnable();
            _watchNow.onClick.AddListener(() => OnClickWatchNow?.Invoke());
            _close.onClick.AddListener(() => OnClickClose?.Invoke());
            OnClickWatchNow += Hide;
            OnClickClose += Hide;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnClickWatchNow = null;
            OnClickClose = null;
            _watchNow.onClick.RemoveAllListeners();
            _close.onClick.RemoveAllListeners();
        }
    }
}