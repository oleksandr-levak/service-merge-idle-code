using System;
using MergeIdle.Scripts.Configs.Merge.Enum;

namespace MergeIdle.Scripts.Configs.Order.Data
{
    [Serializable]
    public class OrderItem
    {
        public int level;
        public int amount;
        public EMergeType mergeType;
    }
    
}