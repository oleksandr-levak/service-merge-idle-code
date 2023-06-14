using System;
using MergeIdle.Scripts.Configs.Purchase.Enum;
using MergeIdle.Scripts.Configs.Store.Data;
using MergeIdle.Scripts.Configs.Store.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views.Store
{
    public class StoreItemView: MonoBehaviour
    {
        public event Action<Product,PurchaseFailureReason> OnClickBuyFailed;
        public event Action<Product> OnClickBuyComplete;
        
        [SerializeField] private Image _currencyImage;
        [SerializeField] private TMP_Text _currencyAmount;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private IAPButton _button;
        
        private EStoreItemId _id;
        private ECurrency _currency;

        public void Setup(StoreItem storeItem)
        {
            _button.productId = storeItem.productId;
            _currencyImage.sprite = storeItem.image;
            _currencyAmount.text = $"{storeItem.amount:# ###}";;
            _price.text = $"{storeItem.price} $";
            
            _id = storeItem.id;
            _currency = storeItem.currency;
            
            _button.onPurchaseComplete.AddListener((product) => OnClickBuyComplete?.Invoke(product));
            _button.onPurchaseFailed.AddListener((product, reason) => OnClickBuyFailed?.Invoke(product, reason));
        }
        
        private void OnDestroy()
        {
            OnClickBuyComplete = null;
            OnClickBuyFailed = null;
            
            _button.onPurchaseComplete.RemoveAllListeners();
            _button.onPurchaseFailed.RemoveAllListeners();
        }
    }
}