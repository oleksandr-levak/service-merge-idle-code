using MergeIdle.Scripts.Configs.Audio.Scriptables;
using MergeIdle.Scripts.Configs.Game.Scriptables;
using MergeIdle.Scripts.Configs.Merge.Scriptables;
using MergeIdle.Scripts.Configs.Purchase.Scriptables;
using MergeIdle.Scripts.Configs.Salon.Scriptables;
using MergeIdle.Scripts.Configs.Store.Scriptables;
using UnityEngine;

namespace MergeIdle.Scripts.Configs
{
    public class ConfigsController:MonoBehaviour
    {
        [SerializeField] private GameDatabase _gameDatabase;
        [SerializeField] private MergeItemsDatabase _mergeItemsDatabase;
        [SerializeField] private PurchaseItemsDatabase _purchaseItemsDatabase;
        [SerializeField] private PurchaseFactorsDatabase _purchaseFactorsDatabase;
        [SerializeField] private OrderClientsDatabase _orderClientsDatabase;
        [SerializeField] private MapItemsDatabase _mapItemsDatabase;
        [SerializeField] private SalonItemsDatabase _salonItemsDatabase;
        [SerializeField] private StoreItemsDatabase _storeItemsDatabase;
        [SerializeField] private AudioDatabase _audioDatabase;
        
        public void InitConfigs()
        {
            Utils.InitGameDatabase(_gameDatabase);
            Utils.InitMergeItemsDatabase(_mergeItemsDatabase);
            Utils.InitPurchaseItemsDatabase(_purchaseItemsDatabase);
            Utils.InitOrderItemsDatabase(_orderClientsDatabase);
            Utils.InitMapItemsDatabase(_mapItemsDatabase);
            Utils.InitSalonItemsDatabase(_salonItemsDatabase);
            Utils.InitPurchaseFactorsDatabase(_purchaseFactorsDatabase);
            Utils.InitStoreItemsDatabase(_storeItemsDatabase);
            Utils.InitAudioDatabase(_audioDatabase);
        }
    }
}