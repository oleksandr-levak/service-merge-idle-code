using System;
using MergeIdle.Scripts.Configs.Salon.Enum;
using MergeIdle.Scripts.Managers.Salon.Views;

namespace MergeIdle.Scripts.Managers.Salon.Data
{
    [Serializable]
    public class SalonData
    {
        public ESalonId id;
        public int level;
        
        public static SalonData MapFrom(SalonView source)
        {
            return new SalonData
            {
                id = source.SalonId,
                level = source.Level
            };
        }
    }
}