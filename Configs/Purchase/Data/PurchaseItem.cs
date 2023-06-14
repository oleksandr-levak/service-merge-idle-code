using System;
using MergeIdle.Scripts.Configs.Merge.Enum;
using MergeIdle.Scripts.Configs.Purchase.Enum;

namespace MergeIdle.Scripts.Configs.Purchase.Data
{
    [Serializable]
    public class PurchaseItem
    {
        public EPurchaseType type;
        public EPurchaseId id;
        public float price;
        public ECurrency currency;
        public EPurchaseCategory category;
        public int level;
        public EMergeType mergeType;
    }
    
}