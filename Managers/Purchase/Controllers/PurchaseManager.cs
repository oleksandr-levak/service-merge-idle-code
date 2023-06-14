using System;
using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Merge.Enum;
using MergeIdle.Scripts.Configs.Purchase.Data;
using MergeIdle.Scripts.Configs.Purchase.Enum;
using MergeIdle.Scripts.Managers.Purchase.Views;
using MergeIdle.Scripts.Managers.Salon.Controllers;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Purchase.Controllers
{
    public class PurchaseManager : MonoBehaviour
    {
        public event Action<PurchaseView> OnClickItem;
        public event Action<EPurchaseType> OnPurchaseTimerEnd;
        public Action<PurchasesView> OnSpawnFinished;

        private Action<int> OnTimerEnd;
        
        [SerializeField] private PurchasesView _smallPurchasesView;
        [SerializeField] private PurchaseView _smallPurchaseView;
        
        [Header("Managers")]
        [SerializeField] private SalonManager _salonManager;
        [SerializeField] private PurchaseSpawnerManager _purchaseSpawnerManager;
        [SerializeField] private PurchasePriceManager _purchasePriceManager;

        private Transform _parent;
        private List<EPurchaseType> _purchaseTypes;
        private List<PurchaseItem> _purchaseItems;
        private Dictionary<EPurchaseId, PurchaseView> _purchaseViews;
        
        public Dictionary<EPurchaseId, PurchaseView> PurchaseViews => _purchaseViews;
        public PurchasesView PurchasesPanelView => _smallPurchasesView;

        private void Start()
        {
            _purchaseTypes = Utils.GetPurchaseTypes();
            _purchaseViews = new Dictionary<EPurchaseId, PurchaseView>();
        }

        public void InitPurchases(Transform parent)
        {
            var purchaseType = _purchaseTypes[0];
            var items = Utils.GetPurchaseItems(purchaseType);
            SetupPurchases(parent, items);
        }
        
        public void SetupPurchases(Transform parent, List<PurchaseItem> purchaseItems)
        {
            _parent = parent;
            foreach (var item in purchaseItems)
            {
                SetupPurchase(parent, item, _smallPurchaseView);
            }
            
            _smallPurchasesView.PurchaseType = purchaseItems[0].type;
            OnSpawnFinished?.Invoke(_smallPurchasesView);
        }
        
        public void ReplaceByNextItems(EPurchaseType prevType)
        {
            var nextType = GetNextType(prevType);
            var prevItems = Utils.GetPurchasesByType(prevType);
            var nextItems = Utils.GetPurchasesByType(nextType);

            foreach (var item in prevItems)
            {
                DeleteById(item.id);
                _smallPurchasesView.TimerManager.OnTimerEnd -= OnTimerEnd;
                string storageKey = $"{_smallPurchasesView.PurchaseType}";
                _smallPurchasesView.TimerManager.ClearTime(storageKey);
            }
            
            foreach (var item in nextItems)
            {
                SetupPurchase(_parent, item, _smallPurchaseView);
            }

            _smallPurchasesView.PurchaseType = nextType;
            SetupPurchasesTimer(_smallPurchasesView);
        }

        public void UpdatePurchasePrice(EPurchaseId key)
        {
            var purchase = GetById(key);
            var salonsPassiveIncomes = _salonManager.GetOpenSalonsPassiveIncomes();
            var openSalonLevel = _salonManager.GetOpenSalons()[0].Level;
            var timerValue = _smallPurchasesView.SecondsToChange;
            purchase.Price = _purchasePriceManager.GetPurchasePrice(timerValue, openSalonLevel, salonsPassiveIncomes);
        }
        
        public EMergeType GetRandomPurchaseType()
        {
            var generatorsTypes = Utils.GetGeneratorsTypes();
            int i = UnityEngine.Random.Range(0, generatorsTypes.Count);
            return generatorsTypes[i];
        }
        
        public void SetupPurchasesTimer()
        {
            SetupPurchasesTimer(_smallPurchasesView);
        }
        
        private void SetupPurchasesTimer(PurchasesView purchasesView)
        {
            purchasesView.Timer.SetActive(true);
            
            OnTimerEnd = _ => OnPurchaseTimerEnd?.Invoke(purchasesView.PurchaseType);
            purchasesView.TimerManager.OnTimerEnd += OnTimerEnd;
            string key = $"{purchasesView.PurchaseType}";
            purchasesView.TimerManager.StartTimer(key, purchasesView.SecondsToChange);
        }

        private void SetupPurchase(Transform parent, PurchaseItem purchase, PurchaseView purchaseView)
        {
            var spawnedPurchaseView = _purchaseSpawnerManager.SpawnItem(parent, purchase, purchaseView);
            spawnedPurchaseView.Item.onClick.AddListener(() => OnClickItem?.Invoke(spawnedPurchaseView));
            
            if (purchase.category == EPurchaseCategory.SOFT)
            {
                var salonsPassiveIncomes = _salonManager.GetOpenSalonsPassiveIncomes();
                var openSalonLevel = _salonManager.GetOpenSalons()[0].Level;
                var timerValue = _smallPurchasesView.SecondsToChange;
                var purchasePrice = _purchasePriceManager.GetPurchasePrice(timerValue, openSalonLevel, salonsPassiveIncomes);
                spawnedPurchaseView.Price = purchasePrice;
            }
            else
            {
                spawnedPurchaseView.Price = purchase.price;
            }
            
            UpdatePurchaseViews(purchase.id, spawnedPurchaseView);
        }

        private void UpdatePurchaseViews(EPurchaseId purchaseId, PurchaseView spawnedPurchaseView)
        {
            if (_purchaseViews.ContainsKey(purchaseId))
            {
                _purchaseViews.Remove(purchaseId);
            }
            _purchaseViews.Add(purchaseId, spawnedPurchaseView);
        }
        
        private PurchaseView GetById(EPurchaseId key)
        {
            return _purchaseViews[key];
        }
        
        private void DeleteById(EPurchaseId key)
        {
            var item = GetById(key);
            item.Item.onClick.RemoveAllListeners();
            Destroy(item.gameObject);
            _purchaseViews.Remove(key);
        }

        private EPurchaseType GetNextType(EPurchaseType prev)
        {
            var prevItemIndex = GetPurchaseTypeIndex(prev);

            if (prevItemIndex + 1 <= _purchaseTypes.Count - 1)
            {
                return _purchaseTypes[prevItemIndex + 1];
            }

            return _purchaseTypes[0];
        }

        private int GetPurchaseTypeIndex(EPurchaseType prev) => _purchaseTypes.IndexOf(prev);
        
        private void RemoveListeners()
        {
            foreach (Transform child in _parent)
            {
                if (child.GetComponent<PurchaseView>() != null)
                {
                    PurchaseView purchaseView = child.GetComponent<PurchaseView>();
                    purchaseView.Item.onClick.RemoveAllListeners();
                }
            }
        }

        public void OnDestroy()
        {
            OnClickItem = null;
            OnPurchaseTimerEnd = null;
            OnTimerEnd = null;
            OnSpawnFinished = null;
            
            _smallPurchasesView.TimerManager.OnTimerEnd -= OnTimerEnd;
            
            OnSpawnFinished -= SetupPurchasesTimer;

            if (_parent != null)
            {
                RemoveListeners();
            }
        }
    }
}