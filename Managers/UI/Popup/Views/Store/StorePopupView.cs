using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views.Store
{
    public class StorePopupView: BasePopupView
    {
        public event Action<Product> OnClickBuyComplete;
        public event Action<Product,PurchaseFailureReason> OnClickBuyFailed;
        public event Action OnClickClose;
        
        [SerializeField] private Button _close;
        [SerializeField] private List<StoreItemView> _storeItems;

        public void SetupPurchases()
        {
            var storeItems = Utils.GetStoreItems();
            for (int i = 0; i < _storeItems.Count; i++)
            {
                var storeItemView = _storeItems[i];
                var storeItemData = storeItems[i];
                storeItemView.Setup(storeItemData);
                
                storeItemView.OnClickBuyComplete += (p) => OnClickBuyComplete?.Invoke(p);
                storeItemView.OnClickBuyFailed += (p, r) => OnClickBuyFailed?.Invoke(p, r);
            }
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            _close.onClick.AddListener(() => OnClickClose?.Invoke());
         
            OnClickClose += Hide;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            OnClickBuyFailed = null;
            OnClickBuyComplete = null;
            OnClickClose = null;
            _close.onClick.RemoveAllListeners();
        }
    }
}