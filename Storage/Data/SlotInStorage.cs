using System;
using MergeIdle.Scripts.Configs.Merge.Enum;
using MergeIdle.Scripts.Databases.MergeItems.Enum;
using MergeIdle.Scripts.Managers.Merge.Data;

namespace MergeIdle.Scripts.Storage.Data
{
    [Serializable]
    public class SlotInStorage
    {
        public int id;
        public EItemState state;
        public EMergeCategory category;
        public EMergeType type;
        public bool isMergeable;
        public int amountOfItems;
        public int slotId;
        
        public SlotInStorage(CreateItemData spawnItemData, int slotId)
        {
            id = spawnItemData.Id;
            state = spawnItemData.State;
            category = spawnItemData.Category;
            type = spawnItemData.Type;
            isMergeable = spawnItemData.IsMergeable;
            amountOfItems = spawnItemData.AmountOfItems;
            this.slotId = slotId;
        }

        public SlotInStorage(int slotId)
        {
            this.slotId = slotId;
        }
    }
}