using System;
using MergeIdle.Scripts.Configs.Merge.Enum;
using MergeIdle.Scripts.Databases.MergeItems.Enum;

namespace MergeIdle.Scripts.Managers.Merge.Data
{
    [Serializable]
    public class CreateItemData
    {
        private EItemState _state;
        private EMergeCategory _category;
        private EMergeType _type;
        private bool _isMergeable; 
        private int _amountOfItems;
        private int _id;

        public EItemState State { get => _state; set => _state = value; }

        public EMergeCategory Category => _category;
        public EMergeType Type => _type;
        public int AmountOfItems { get => _amountOfItems; set => _amountOfItems = value; }

        public bool IsMergeable => _isMergeable;
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public CreateItemData(EItemState state, EMergeCategory category, EMergeType type, bool isMergeable, int amountOfItems, int id)
        {
            _state = state;
            _category = category;
            _type = type;
            _isMergeable = isMergeable;
            _amountOfItems = amountOfItems;
            _id = id;
        }
    }
}