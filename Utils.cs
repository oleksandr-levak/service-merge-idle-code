using System.Collections.Generic;
using System.Linq;
using MergeIdle.Scripts.Configs.Audio.Data;
using MergeIdle.Scripts.Configs.Audio.Enums;
using MergeIdle.Scripts.Configs.Audio.Scriptables;
using MergeIdle.Scripts.Configs.Game.Scriptables;
using MergeIdle.Scripts.Configs.Merge.Data;
using MergeIdle.Scripts.Configs.Merge.Enum;
using MergeIdle.Scripts.Configs.Merge.Scriptables;
using MergeIdle.Scripts.Configs.Purchase.Data;
using MergeIdle.Scripts.Configs.Purchase.Enum;
using MergeIdle.Scripts.Configs.Purchase.Scriptables;
using MergeIdle.Scripts.Configs.Salon.Data;
using MergeIdle.Scripts.Configs.Salon.Enum;
using MergeIdle.Scripts.Configs.Salon.Scriptables;
using MergeIdle.Scripts.Configs.Store.Data;
using MergeIdle.Scripts.Configs.Store.Scriptables;
using MergeIdle.Scripts.Databases.MapItems.Enum;
using MergeIdle.Scripts.Databases.MergeItems.Enum;
using MergeIdle.Scripts.Managers.Salon.Data;
using UnityEngine;

namespace MergeIdle.Scripts
{
    public static class Utils
    {
        private static GameDatabase _gameDatabase;
        private static MergeItemsDatabase _mergeItemsDatabase;
        private static PurchaseItemsDatabase _purchaseItemsDatabase;
        private static PurchaseFactorsDatabase _purchaseFactorsDatabase;
        private static OrderClientsDatabase _orderClientsDatabase;
        private static MapItemsDatabase _mapItemsDatabase;
        private static SalonItemsDatabase _salonItemsDatabase;
        private static StoreItemsDatabase _storeItemsDatabase;
        private static AudioDatabase _audioDatabase;
        
        public static GameDatabase GameDatabase => _gameDatabase;

        public static void InitGameDatabase(GameDatabase gameDatabase)
        {
            _gameDatabase = gameDatabase;
        }
    
        public static void InitMergeItemsDatabase(MergeItemsDatabase mergeItemsDatabase)
        {
            _mergeItemsDatabase = mergeItemsDatabase;
        }
        
        public static void InitPurchaseItemsDatabase(PurchaseItemsDatabase purchaseItemsDatabase)
        {
            _purchaseItemsDatabase = purchaseItemsDatabase;
        }
        
        public static void InitOrderItemsDatabase(OrderClientsDatabase orderClientsDatabase)
        {
            _orderClientsDatabase = orderClientsDatabase;
        }
        
        public static void InitMapItemsDatabase(MapItemsDatabase mapItemsDatabase)
        {
            _mapItemsDatabase = mapItemsDatabase;
        }
        
        public static void InitSalonItemsDatabase(SalonItemsDatabase salonItemsDatabase)
        {
            _salonItemsDatabase = salonItemsDatabase;
        }
        
        public static void InitPurchaseFactorsDatabase(PurchaseFactorsDatabase purchaseFactorsDatabase)
        {
            _purchaseFactorsDatabase = purchaseFactorsDatabase;
        }
        
        public static void InitStoreItemsDatabase(StoreItemsDatabase storeItemsDatabase)
        {
            _storeItemsDatabase = storeItemsDatabase;
        }
        
        public static void InitAudioDatabase(AudioDatabase audioDatabase)
        {
            _audioDatabase = audioDatabase;
        }
        
        public static AudioItem GetAudio(EAudio audioType)
        {
            return _audioDatabase.GetByType(audioType);
        }

        
        public static List<StoreItem> GetStoreItems() => _storeItemsDatabase.Items;

        public static float GetLevelFactor(int lvl)
        {
            var levelFactors = _purchaseFactorsDatabase.Items;

            foreach (var levelFactor in levelFactors)
            {
                var level = levelFactor.level;

                if (lvl == level) return levelFactor.factor;

                if (lvl < level)
                {
                    var upperFactorIndex = levelFactors.IndexOf(levelFactor);
                    var currentFactorIndex = upperFactorIndex - 1;
                    return levelFactors[currentFactorIndex].factor;
                }
            }

            return 1;
        }
        
        public static List<Sprite> GetMergeItemsSprites(EMergeType type)
        {
            var items = _mergeItemsDatabase.Items;
            var itemsByCategory = GetItemsByCategory(items, EMergeCategory.NO_GENERATOR);
            var itemsByType = GetMergeItemByType(itemsByCategory.mergeSprites, type);
            return itemsByType?.mergeSprites;
        }
        
        public static List<Sprite> GetPurchasesSprites(EPurchaseType purchaseType)
        {
            List<Sprite> sprites = new List<Sprite>();
            var items = _mergeItemsDatabase.Items;
            var itemsByCategory = GetItemsByCategory(items, EMergeCategory.GENERATOR);
            
            var mergeTypes = _purchaseItemsDatabase.Items
                .FindAll(x => x.type == purchaseType)
                .ConvertAll(x => x.mergeType);

            foreach (var mergeType in mergeTypes)
            {
                var itemByType = GetMergeItemByType(itemsByCategory.mergeSprites, mergeType);
                var itemSprite = itemByType.mergeSprites[0];
                sprites.Add(itemSprite);
            }
            return sprites;
        }

        public static Sprite GetItemVisualById(EMergeCategory category, EMergeType type, int itemId)
        {
            var items = _mergeItemsDatabase.Items;
            var itemsByCategory = GetItemsByCategory(items, category);
            var mergeItemByType = GetMergeItemByType(itemsByCategory.mergeSprites, type);
            return mergeItemByType?.mergeSprites[itemId];
        }

        public static int GetItemMaxLevelById(EMergeCategory category, EMergeType type)
        {
            var items = _mergeItemsDatabase.Items;
            var itemsByCategory = GetItemsByCategory(items, category);
            var mergeItemByType = GetMergeItemByType(itemsByCategory.mergeSprites, type);
            return mergeItemByType.mergeSprites.Count;
        }
        
        public static MergeItem GetGeneratorByType(EMergeType mergeType)
        {
            return _mergeItemsDatabase.Items
                .FindAll(x => x.mergeCategory == EMergeCategory.GENERATOR)[0].mergeSprites
                .FirstOrDefault(x => x.mergeType == mergeType);
        }
        
        public static List<EMergeType> GetMergeTypes()
        {
            return _mergeItemsDatabase.Items
                .FirstOrDefault(x => x.mergeCategory == EMergeCategory.NO_GENERATOR).mergeSprites
                .ConvertAll(x => x.mergeType);
        }
        
        public static List<EMergeType> GetGeneratorsTypes()
        {
            return _mergeItemsDatabase.Items
                .FirstOrDefault(x => x.mergeCategory == EMergeCategory.GENERATOR).mergeSprites
                .ConvertAll(x => x.mergeType);
        }

        public static MergeItemToGenerate GetItemTypeByGenerator(EMergeType mergeType)
        {
            var mergeItems = _mergeItemsDatabase.Items
                .FindAll(x => x.mergeCategory == EMergeCategory.GENERATOR)[0].mergeSprites
                .FirstOrDefault(x => x.mergeType == mergeType)
                ?.mergeItemToGenerates;
            
            int purchaseItemToGeneratesLength = mergeItems.Count;
            if (purchaseItemToGeneratesLength == 1) return mergeItems[0];
            int i = Random.Range(1, 101);


            var bottomLimit = 0;
            for (int j = 0; j < purchaseItemToGeneratesLength; j++)
            {
                if (i >= bottomLimit && i <= mergeItems[j].probability + bottomLimit)
                {
                    return mergeItems[j];
                }
                bottomLimit += mergeItems[j].probability;
            }

            return mergeItems[0];
        }

        public static List<EPurchaseType> GetPurchaseTypes()
        {
            return _purchaseItemsDatabase.Items.ConvertAll(x => x.type).Distinct().ToList();
        }
        
        public static List<PurchaseItem> GetPurchaseItems(EPurchaseType type)
        {
            return _purchaseItemsDatabase.Items.FindAll(x => x.type == type);
        }

        public static List<PurchaseItem> GetPurchasesByType(EPurchaseType type)
        {
            return _purchaseItemsDatabase.Items.FindAll(x => x.type == type);
        }

        public static List<SalonItem> GetSalonsByMapId(EMapId id)
        {
            List<SalonItem> salonItems = new List<SalonItem>();

            var salons = _mapItemsDatabase.Items.FirstOrDefault(x => x.id == id)?.salonIds;

            if (salons != null)
            {
                for (int i = 0; i < salons.Count; i++)
                {
                    var salon = GetSalonById(salons[i]);
                    salonItems.Add(salon);
                }
            }
            return salonItems;
        }

        public static Sprite GetSalonManagerImage(SalonData salonData)
        {
            var salonLevels = _salonItemsDatabase.Items.FirstOrDefault(x => x.id == salonData.id)?.salonLevels;
            var salonLevel = salonLevels.FirstOrDefault(x => x.level == salonData.level);
            return salonLevel.salonSprite;
        }

        public static SalonItem GetSalonById(ESalonId id)
        {
            return _salonItemsDatabase.Items.FirstOrDefault(x => x.id == id);
        }

        private static MergeCategory GetItemsByCategory(List<MergeCategory> mergeCategories, EMergeCategory category)
        {
            return mergeCategories.FirstOrDefault(x => x.mergeCategory == category);
        }
    
        private static MergeItem GetMergeItemByType(List<MergeItem> mergeItems, EMergeType type)
        {
            return mergeItems.FirstOrDefault(x => x.mergeType == type);
        }
    }
}