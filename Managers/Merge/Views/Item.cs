using MergeIdle.Scripts.Configs.Merge.Enum;
using MergeIdle.Scripts.Databases.MergeItems.Enum;
using MergeIdle.Scripts.Managers.Merge.Data;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Merge.Views
{
    public class Item : MonoBehaviour
    {
        private Color CLOSE_COLOR = new Color(0.5f, 0.5f, 0.5f, 1);

        private CreateItemData _createItemData;

        public SpriteRenderer visualRenderer;
        public GameObject immovableSprite;
        public GameObject whiteCircleSprite;
        public GameObject generatorSprite;
        
        public void DecreaseAmountOfItems() => _createItemData.AmountOfItems -= 1;
        public CreateItemData Data => _createItemData;

        public void Init(CreateItemData createItemData, Slot slot)
        {
            _createItemData = createItemData;

            if (createItemData.State == EItemState.CLOSE)
            {
                visualRenderer.sprite = Utils.GameDatabase.CloseSprite;
                visualRenderer.color = CLOSE_COLOR;
            } 
            else
            {
                if (createItemData.State == EItemState.IMMOVABLE)
                {
                    immovableSprite.SetActive(true);
                    visualRenderer.color = CLOSE_COLOR;
                }

                if (createItemData.Category == EMergeCategory.GENERATOR)
                {
                    generatorSprite.SetActive(true);
                }

                visualRenderer.sprite = GetItemSprite(createItemData);
            }
        }

        public void Open()
        {
            _createItemData.State = EItemState.IMMOVABLE;
            immovableSprite.SetActive(true);
            visualRenderer.sprite = GetItemSprite(_createItemData);
        }

        public void SetHoverState(bool state)
        {
            whiteCircleSprite.SetActive(state);
        }

        private Sprite GetItemSprite(CreateItemData createItemData)
        {
            return Utils.GetItemVisualById(createItemData.Category, createItemData.Type, createItemData.Id);
        }
    }
}
