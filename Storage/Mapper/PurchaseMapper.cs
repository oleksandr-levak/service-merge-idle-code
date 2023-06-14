using MergeIdle.Scripts.Configs.Purchase.Data;
using MergeIdle.Scripts.Managers.Purchase.Views;

namespace MergeIdle.Scripts.Storage.Mapper
{
    public static class PurchaseMapper
    {
        public static PurchaseItem MapToItem(this PurchaseView purchaseView)
        {
            PurchaseItem purchaseItem = new PurchaseItem
            {
                id = purchaseView.PurchaseId,
                type = purchaseView.PurchaseType,
                currency = purchaseView.Currency,
                level = purchaseView.Level,
                price = purchaseView.Price,
                mergeType = purchaseView.MergeType,
                category = purchaseView.PurchaseCategory
            };
            return purchaseItem;
        }
    }
}