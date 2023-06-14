using System;
using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Purchase.Data;

namespace MergeIdle.Scripts.Storage.Data
{
    [Serializable]
    public class PurchasesList
    {
        public List<PurchaseItem> purchaseItems;
        public PurchasesList(List<PurchaseItem> purchaseItems)
        {
            this.purchaseItems = purchaseItems;
        }
    }
}