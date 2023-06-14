using System;
using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Salon.Enum;
using MergeIdle.Scripts.Databases.MapItems.Enum;
using MergeIdle.Scripts.Databases.MergeItems.Enum;

namespace MergeIdle.Scripts.Databases.MapItems.Data
{
    [Serializable]
    public class MapItem
    {
        public EMapId id;
        public List<ESalonId> salonIds;
    }
    
}