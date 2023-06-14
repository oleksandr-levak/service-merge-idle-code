using System;
using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Merge.Enum;
using UnityEngine;

namespace MergeIdle.Scripts.Configs.Merge.Data
{
    [Serializable]
    public class MergeItem
    {
        public EMergeType mergeType;
        public int amountOfItems = 15;
        public bool isMergeable = true;
        public List<Sprite> mergeSprites;
        public List<MergeItemToGenerate> mergeItemToGenerates;
    }
}