using DG.Tweening;
using MergeIdle.Scripts.Managers.Merge.Data;
using MergeIdle.Scripts.Managers.Merge.Enums;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Merge.Views
{
    public class Slot : MonoBehaviour
    {
        public int id;
        public Item currentItem;
        public ESlotState state = ESlotState.Empty;
        public GameObject selectedFrameSprite;
        public Item itemPrefab;
        
        private Sequence _sequence;

        public void CreateItem(CreateItemData createItemData) 
        {
            var itemGO = Instantiate(itemPrefab, transform, true);

            var transform1 = itemGO.transform;
            transform1.localPosition = Vector3.zero;
            transform1.localScale = Vector3.one;

            currentItem = itemGO.GetComponent<Item>();
            currentItem.Init(createItemData, this);

            ChangeStateTo(ESlotState.Full);
        }

        private void ChangeStateTo(ESlotState targetState)
        {
            state = targetState;
        }

        public void ItemGrabbed()
        {
            Destroy(currentItem.gameObject);
            ChangeStateTo(ESlotState.Empty);
        }
    
        public void SetFrameState(bool state)
        {
            selectedFrameSprite.SetActive(state);
            
            if (state)
            {
                _sequence = DOTween.Sequence()
                    .SetRecyclable(true).SetAutoKill(true)
                    .Append(selectedFrameSprite.transform.DOScale(0.185f, 0.8f))
                    .Append(selectedFrameSprite.transform.DOScale(0.16f, 0.8f))
                    .SetEase(Ease.Linear)
                    .SetLoops(-1);

            }
            else if (_sequence != null)
            {
                _sequence.Kill();
                _sequence = null;
            }
        }

        private void OnDisable()
        {
            _sequence.Kill();
            _sequence = null;
        }
    }
}