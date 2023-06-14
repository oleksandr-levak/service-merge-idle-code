using System;
using System.Collections.Generic;
using MergeIdle.Scripts.Databases.MergeItems.Enum;

namespace MergeIdle.Scripts.Configs.Merge.Data
{
    [Serializable]
    public class MergeCategory
    {
        public EMergeCategory mergeCategory;
        public List<MergeItem> mergeSprites; 
    }
}