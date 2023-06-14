using System;
using MergeIdle.Scripts.Configs.Purchase.Enum;
using MergeIdle.Scripts.Configs.Store.Enum;
using UnityEngine;

namespace MergeIdle.Scripts.Configs.Store.Data
{
    [Serializable]
    public class StoreItem
    {
        public EStoreItemId id;
        public ECurrency currency;
        public int amount;
        public float price;
        public Sprite image;
        public string productId;
    }
    
}