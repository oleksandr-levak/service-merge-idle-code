using System;
using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Salon.Enum;
using UnityEngine;

namespace MergeIdle.Scripts.Configs.Salon.Data
{
    [Serializable]
    public class SalonItem
    {
        public ESalonId id;
        public int level = 1;
        public int currentLevel;
        public int exp;
        public List<SalonLevel> salonLevels;
        public bool isOpen;
    }
    
}