using System;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views.Mistake
{
    public class MistakePopupView: BasePopupView
    {
        public event Action OnClickBuyMore;

        [SerializeField] private Button _buyMore;
        protected override void OnEnable()
        {
            base.OnEnable();
            _buyMore.onClick.AddListener(() => OnClickBuyMore?.Invoke());
            OnClickBuyMore += Hide;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnClickBuyMore = null;
            _buyMore.onClick.RemoveAllListeners();
        }
    }
}