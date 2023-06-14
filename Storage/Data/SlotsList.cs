using System;
using System.Collections.Generic;

namespace MergeIdle.Scripts.Storage.Data
{
    [Serializable]
    public class SlotsList
    {
        public List<SlotInStorage> slots;
        public SlotsList(List<SlotInStorage> slots)
        {
            this.slots = slots;
        }
    }
}