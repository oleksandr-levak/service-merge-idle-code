using System;
using MergeIdle.Scripts.Configs.Merge.Enum;

namespace MergeIdle.Scripts.Configs.Merge.Data
{
    [Serializable]
    public class MergeItemToGenerate
    {
        public EMergeType mergeType;
        public int level;
        public int probability;
    }
    
}