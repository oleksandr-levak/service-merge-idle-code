using MergeIdle.Scripts.Managers.Merge.Views;
using MergeIdle.Scripts.Storage.Data;

namespace MergeIdle.Scripts.Storage.Mapper
{
    public static class SlotMapper
    {
        public static SlotInStorage MapToStorage(this Slot slot)
        {
            SlotInStorage slotInStorage = new SlotInStorage(slot.id);
            if (slot.currentItem != null)
            {
                slotInStorage = new SlotInStorage(slot.currentItem.Data, slot.id);;
            }
            return slotInStorage;
        }
    }
}