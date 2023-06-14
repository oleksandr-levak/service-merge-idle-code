using MergeIdle.Scripts.Configs.Purchase.Data;
using MergeIdle.Scripts.Databases.MergeItems.Enum;
using MergeIdle.Scripts.Managers.Purchase.Views;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Purchase.Controllers
{
    public class PurchaseSpawnerManager : MonoBehaviour
    {
        public PurchaseView SpawnItem(Transform parent, PurchaseItem purchase, PurchaseView purchaseView)
        {  
            var spawnedPurchaseView = Instantiate(purchaseView, parent, false);
            var purchaseViewImage = Utils.GetItemVisualById(EMergeCategory.GENERATOR, purchase.mergeType, purchase.level - 1);
            var purchaseViewCurrencyImage = Utils.GameDatabase.GetCurrencySpriteByType(purchase.currency);
            var mergeItem = Utils.GetGeneratorByType(purchase.mergeType);
            
            spawnedPurchaseView.Image = purchaseViewImage;
            spawnedPurchaseView.CurrencyImage = purchaseViewCurrencyImage;
            
            spawnedPurchaseView.Currency = purchase.currency;

            spawnedPurchaseView.PurchaseId = purchase.id;
            spawnedPurchaseView.PurchaseType = purchase.type;
            spawnedPurchaseView.PurchaseCategory = purchase.category;
            
            spawnedPurchaseView.MergeType = purchase.mergeType;
            spawnedPurchaseView.Level = purchase.level;
            spawnedPurchaseView.IsMergeable = mergeItem.isMergeable;
            spawnedPurchaseView.AmountOfItems = mergeItem.amountOfItems;
            
            spawnedPurchaseView.SetupCurrencyLabel(purchase.category);
            return spawnedPurchaseView;
        }
    }
}