using System;
using MergeIdle.Scripts.Configs.Merge.Enum;
using MergeIdle.Scripts.Databases.MergeItems.Enum;

namespace MergeIdle.Scripts.Managers.Merge.Data
{
    [Serializable]
    public class SpawnItemData
    {
        private EItemState _state;
        private EMergeCategory _category;
        private EMergeType _type;
        private bool _isMergeable; 
        private int _amountOfItems;
        private int _id;
        private int _slotStartId;
        private int _slotEndId = -1;

        public int SlotStartId
        {
            get => _slotStartId;
            set => _slotStartId = value;
        }

        public int SlotEndId => _slotEndId;
        public CreateItemData CreateItemData { get; }
        
        public SpawnItemData(int slotId)
        {
            _slotStartId = slotId;
        }

        public SpawnItemData(EItemState state, EMergeCategory category, EMergeType type, bool isMergeable, int amountOfItems, int id, int slotStartId, int slotEndId)
        {
            _state = state;
            _category = category;
            _type = type;
            _isMergeable = isMergeable;
            _amountOfItems = amountOfItems;
            _id = id;
            _slotStartId = slotStartId;
            _slotEndId = slotEndId;

             CreateItemData = new CreateItemData(state, category, type, isMergeable, amountOfItems, id);
        }
        
        public SpawnItemData(EItemState state, EMergeCategory category, EMergeType type, bool isMergeable, int amountOfItems, int id, int slotStartId = -1)
        {
            _state = state;
            _category = category;
            _type = type;
            _isMergeable = isMergeable;
            _amountOfItems = amountOfItems;
            _id = id;
            _slotStartId = slotStartId;
            
            CreateItemData = new CreateItemData(state, category, type, isMergeable, amountOfItems, id);
        }
        
        public SpawnItemData(CreateItemData createItemData, int slotStartId = -1)
        {
            _state = createItemData.State;
            _category = createItemData.Category;
            _type = createItemData.Type;
            _isMergeable = createItemData.IsMergeable;
            _amountOfItems = createItemData.AmountOfItems;
            _id = createItemData.Id;
            _slotStartId = slotStartId;
            
            CreateItemData = createItemData;
        }
    }
}