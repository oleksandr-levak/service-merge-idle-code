using MergeIdle.Scripts.Managers.Merge.Data;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Merge.Views
{
    public class ItemInfo : MonoBehaviour
    {
        private int _slotId;
        private CreateItemData _createItemData;

        public SpriteRenderer visualRenderer;
        
        public int SlotId => _slotId;
        public CreateItemData Data => _createItemData;
        
        public void InitDummy(CreateItemData createItemData, int slotId)
        {
            _slotId = slotId;
            _createItemData = createItemData;
            visualRenderer.sprite = GetItemSprite(createItemData);
        }

        public void UpItemId()
        {
            _createItemData.Id += 1;
        }

        private Sprite GetItemSprite(CreateItemData createItemData)
        {
            return Utils.GetItemVisualById(createItemData.Category, createItemData.Type, createItemData.Id);
        }
    }
}