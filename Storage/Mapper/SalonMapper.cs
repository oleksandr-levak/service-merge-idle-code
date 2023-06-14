using MergeIdle.Scripts.Configs.Salon.Data;
using MergeIdle.Scripts.Managers.Salon.Data;
using MergeIdle.Scripts.Managers.Salon.Views;

namespace MergeIdle.Scripts.Storage.Mapper
{
    public static class SalonMapper
    {
        public static SalonItem MapToItem(this SalonView salonView)
        {
            SalonItem salonItem = new SalonItem
            {
                id = salonView.SalonId,
                level = salonView.Level,
                exp = salonView.EXP,
                salonLevels = salonView.SalonLevels,
                isOpen = salonView.IsOpen,
                currentLevel = salonView.CurrentLevel
            };
            return salonItem;
        }
        
        public static SalonData MapToData(this SalonView salonView)
        {
            SalonData salonData = new SalonData
            {
                id = salonView.SalonId,
                level = salonView.Level,
            };
            return salonData;
        }
    }
}