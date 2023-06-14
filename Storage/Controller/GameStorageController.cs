using System;
using System.Collections.Generic;
using System.Linq;
using MergeIdle.Scripts.Configs.Order.Data;
using MergeIdle.Scripts.Configs.Order.Enum;
using MergeIdle.Scripts.Configs.Purchase.Data;
using MergeIdle.Scripts.Configs.Purchase.Enum;
using MergeIdle.Scripts.Configs.Salon.Data;
using MergeIdle.Scripts.Configs.Salon.Enum;
using MergeIdle.Scripts.Managers.Merge.Views;
using MergeIdle.Scripts.Managers.Order.Views;
using MergeIdle.Scripts.Managers.Purchase.Views;
using MergeIdle.Scripts.Managers.Salon.Views;
using MergeIdle.Scripts.Storage.Const;
using MergeIdle.Scripts.Storage.Data;
using MergeIdle.Scripts.Storage.Mapper;
using UnityEngine;

namespace MergeIdle.Scripts.Storage.Controller
{
    public class GameStorageController:StorageController
    {
        public event Action OnChangeTutorialStage;

        [Header("Max")]
        [SerializeField] private float _maxMoney;
        [SerializeField] private float _maxDiamonds;
        [SerializeField] private int _maxEnergy;
        
        private float _money;
        private float _diamonds;
        private int _energy;

        private void Awake()
        {
            SetMoney(GetFloatValue(StorageType.MONEY));
            SetEnergy(GetIntValue(StorageType.ENERGY));
            SetDiamonds(GetFloatValue(StorageType.DIAMONDS));
        }
        
        public int CompletedOrdersAmount
        {
            get => GetIntValue(StorageType.COMPLETED_ORDERS_AMOUNT);
            set => SetIntValue(StorageType.COMPLETED_ORDERS_AMOUNT, value);
        }
        
        public int GameTimeSeconds
        {
            get => GetIntValue(StorageType.GAME_TIME_SECONDS);
            set => SetIntValue(StorageType.GAME_TIME_SECONDS, value);
        }
        
        public int GameTimeMinutes => (int) Math.Round(GameTimeSeconds / 60f);

        public void IncCompletedOrdersAmount()
        {
            var prevVal = CompletedOrdersAmount;
            CompletedOrdersAmount = prevVal + 1;
        }

        public float GetCurrencyValue(ECurrency currency)
        {
            switch (currency)
            {
                case ECurrency.MONEY: return _money;
                default: return _diamonds;
            }
        }

        public void SetCurrencyValue(ECurrency currency, float value)
        {
            switch (currency)
            {
               case ECurrency.MONEY:
                   SetMoney(value);
                   break;
               default:
                   SetDiamonds(value);
                   break;
            }
        }

        public float Money => _money;

        public void SetMoney(float value)
        {
            if (value >= _maxMoney)
            {
                SetFloatValue(StorageType.MONEY, _maxMoney);
                _money = _maxMoney; 
            }
            else
            {
                SetFloatValue(StorageType.MONEY, value);
                _money = value;  
            }
        }
        
        public int Energy => _energy;

        public void SetEnergy(int value)
        {
            if (value >= _maxEnergy)
            {
                SetIntValue(StorageType.ENERGY, _maxEnergy);
                _energy = _maxEnergy;  
            }
            else
            {
                SetIntValue(StorageType.ENERGY, value);
                _energy = value;  
            }
        }
        
        public float Diamonds => _diamonds;

        public void SetDiamonds(float value)
        {
            if (value >= _maxDiamonds)
            {
                SetFloatValue(StorageType.DIAMONDS, _maxDiamonds);
                _diamonds = _maxDiamonds; 
            }
            else
            {
                SetFloatValue(StorageType.DIAMONDS, value);
                _diamonds = value;  
            }
        }
        
        public bool IsFirstVisit => GetValue(StorageType.IS_FIRST_VISIT) != "No";
        public void SetIsFirstVisit(string val) => SetValue(StorageType.IS_FIRST_VISIT, val);
        
        public bool IsFirstPurchase => GetValue(StorageType.IS_FIRST_PURCHASE) != "No";
        public void SetIsFirstPurchase(string val) => SetValue(StorageType.IS_FIRST_PURCHASE, val);
        
        public int TutorialTimer
        {
            get => GetIntValue(StorageType.TUTORIAL_TIMER);
            set => SetIntValue(StorageType.TUTORIAL_TIMER, value);
        }

        public int TutorialStage
        {
            get => GetIntValue(StorageType.TUTORIAL_STAGE);
            set
            {
                SetIntValue(StorageType.TUTORIAL_STAGE, value);
                OnChangeTutorialStage?.Invoke();
            }
        }
        
        public int Music
        {
            get => GetIntValue(StorageType.MUSIC);
            set => SetIntValue(StorageType.MUSIC, value);
        }
        
        public int Sound
        {
            get => GetIntValue(StorageType.SOUND);
            set => SetIntValue(StorageType.SOUND, value);
        }

        public List<SlotInStorage> GetSlotsValue()
        {
            string json = PlayerPrefs.GetString(StorageType.SLOTS, null);
            SlotsList slotsList = JsonUtility.FromJson<SlotsList>(json);
            return slotsList.slots;
        }

        public void SetSlotsValue(List<Slot> list)
        {
            List<SlotInStorage> slotInStorages = list
                .FindAll(el => el.currentItem != null)
                .ConvertAll(_ => _.MapToStorage());
            SlotsList slotsList = new SlotsList(slotInStorages);
            string jsonData = JsonUtility.ToJson(slotsList);
            SetValue(StorageType.SLOTS, jsonData);
        }
        
        public List<PurchaseItem> GetPurchasesValue()
        {
            string json = PlayerPrefs.GetString(StorageType.PURCHASES, null);
            PurchasesList purchasesList = JsonUtility.FromJson<PurchasesList>(json);
            return purchasesList.purchaseItems;
        }
        
        public void SetPurchasesValue(Dictionary<EPurchaseId, PurchaseView> views)
        {
            List<PurchaseItem> purchaseItems = views.Values.ToList()
                .ConvertAll(el => el.MapToItem());
            PurchasesList purchasesList = new PurchasesList(purchaseItems);
            string jsonData = JsonUtility.ToJson(purchasesList);
            SetValue(StorageType.PURCHASES, jsonData);
        }
        
        public List<SalonItem> GetSalonsValue()
        {
            string json = PlayerPrefs.GetString(StorageType.SALONS, null);
            SalonsList salonsList = JsonUtility.FromJson<SalonsList>(json);
            return salonsList.salonItems;
        }
        
        public void SetSalonsValue(Dictionary<ESalonId, SalonView> views)
        {
            List<SalonItem> salonItems = views.Values.ToList()
                .ConvertAll(el => el.MapToItem());
            SalonsList salonsList = new SalonsList(salonItems);
            string jsonData = JsonUtility.ToJson(salonsList);
            SetValue(StorageType.SALONS, jsonData);
        }
        
        public List<OrderClient> GetOrderClientsValue()
        {
            string json = PlayerPrefs.GetString(StorageType.ORDERS, null);
            OrderClientsList orderClientsList = JsonUtility.FromJson<OrderClientsList>(json);
            return orderClientsList.orderClients;
        }
        
        public void SetOrderClientsValue(Dictionary<EOrderId, OrderView> views)
        {
            List<OrderClient> orderClients = views.Values.ToList()
                .ConvertAll(el => el.MapToItem());
            OrderClientsList OrderClientsList = new OrderClientsList(orderClients);
            string jsonData = JsonUtility.ToJson(OrderClientsList);
            SetValue(StorageType.ORDERS, jsonData);
        }
    }
}