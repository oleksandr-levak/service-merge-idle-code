using System;
using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Salon.Data;

namespace MergeIdle.Scripts.Storage.Data
{
    [Serializable]
    public class SalonsList
    {
        public List<SalonItem> salonItems;
        public SalonsList(List<SalonItem> salonItems)
        {
            this.salonItems = salonItems;
        }
    }
}