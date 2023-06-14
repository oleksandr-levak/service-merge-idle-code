using System;
using MergeIdle.Scripts.Configs.Merge.Enum;
using MergeIdle.Scripts.Configs.Purchase.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.Purchase.Views
{
    public class PurchaseView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _item;
        
        [Header("Money")]
        [SerializeField] private ECurrency _currency;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private Image _currencyImage;
        [SerializeField] private GameObject _money;
        
        [Header("Ads")]
        [SerializeField] private GameObject _ads;

        private EPurchaseId _purchaseId;
        private EPurchaseType _purchaseType;
        private EPurchaseCategory _purchaseCategory;
        
        private EMergeType _type;
        private int _level;
        private bool _isMergeable;
        private int _amountOfItems;
        private int _secondsToChange;
        public Button Item => _item;
        public EPurchaseId PurchaseId  { get => _purchaseId; set => _purchaseId = value; }
        public EPurchaseType PurchaseType  { get => _purchaseType; set => _purchaseType = value; }
        public EPurchaseCategory PurchaseCategory  { get => _purchaseCategory; set => _purchaseCategory = value; }
        public int AmountOfItems  { get => _amountOfItems; set => _amountOfItems = value; }
        public bool IsMergeable  { get => _isMergeable; set => _isMergeable = value; }
        public EMergeType MergeType  { get => _type; set => _type = value; }
        public int Level  { get => _level; set => _level = value; }
        public float Price  { get => float.Parse(_price.text); set => _price.text = $"{Math.Ceiling(value)}"; }
        public ECurrency Currency  { get => _currency; set => _currency = value; }
        public Sprite Image  { set => _image.sprite = value; }
        public Sprite CurrencyImage
        {
            set
            {
                _currencyImage.sprite = value;
                _currencyImage.SetNativeSize();
            }
        }
        public void SetupCurrencyLabel(EPurchaseCategory category)
        {
            if (category == EPurchaseCategory.REWARD) _ads.SetActive(true);
            else _money.SetActive(true);
        }
    }
}