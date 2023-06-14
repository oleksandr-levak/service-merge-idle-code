using System;
using MergeIdle.Scripts.Configs.Purchase.Enum;
using UnityEngine;

namespace MergeIdle.Scripts.Databases.Game.Data
{
    [Serializable]
    public class Currency
    {
        public ECurrency currency;
        public Sprite sprite;
    }
}