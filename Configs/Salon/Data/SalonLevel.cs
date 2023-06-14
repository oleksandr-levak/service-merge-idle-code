using System;
using MergeIdle.Scripts.Configs.Purchase.Enum;
using UnityEngine;

namespace MergeIdle.Scripts.Configs.Salon.Data
{
    [Serializable]
    public class SalonLevel
    {
        public int level;
        public int exp;
        
        public float amountPerSecond;
        public ECurrency currency;
        public Sprite levelSprite;
        public Sprite salonSprite;
    }
    
}