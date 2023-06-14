using System;
using DG.Tweening;
using MergeIdle.Scripts.Configs.Merge.Enum;
using MergeIdle.Scripts.Databases.MergeItems.Enum;
using MergeIdle.Scripts.Managers.Merge.Enums;
using MergeIdle.Scripts.Managers.Merge.Views;
using MergeIdle.Scripts.Storage.Controller;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Merge.Controllers
{
    public class MergeManager : MonoBehaviour
    {
        private const float SWIPE_SPEED = 0.5f;
        public event Action OnUpdateSlots;
        public event Action<EMergeType> OnGenerateItem;
        public event Action<EMergeType> OnClickGenerator;
        public event Action OnMergeItem;
        public event Action OnMergeItemSelect;
        
        [SerializeField] private ItemInfo _itemInfoPrefab;
        [Header("Storage")]
        [SerializeField] private GameStorageController _gameStorage;
        [Header("Managers")]
        [SerializeField] private CoroutineManager _coroutineManager;
        [SerializeField] private SlotsManager _slotsManager;
        [SerializeField] private GeneratorManager _generatorManager;
        
        private Camera _mainCamera;
        private Slot _hitSlot;
        private RaycastHit2D _prevHit;
        private Vector3 _target;
        private ItemInfo _carryingItem;
        private EMergeType _currGen;
        private float _dist = 1; 
        private int _genClicks;
        private bool _isFreezed;
        
        private ItemInfo _prevCarryingItem;
        private int _prevSlotId;

        public bool IsFreezed
        {
            get => _isFreezed;
            set => _isFreezed = value;
        }

        public void SetCamera(Camera camera)
        {
            _mainCamera = camera;
        }
        
        //handle user input
        private void Update()
        {
            if (_isFreezed) return;

            if (Input.GetMouseButtonDown(0))
            {
                SendRayCast();
            }

            if (Input.GetMouseButton(0) && _carryingItem)
            {
                OnMergeItemSelect?.Invoke();
                OnItemSelected();
                DetectHitSlot(_carryingItem);
            }
            else if (_hitSlot != null && _hitSlot.currentItem != null)
            {
                _hitSlot.currentItem.SetHoverState(false);
            }

            if (Input.GetMouseButtonUp(0))
            {
                //Drop item
                SendRayCast();
            }
        }
 
        private void SendRayCast()
        {
            RaycastHit2D hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

   
            //we hit something
            if(hit.collider != null)
            {
                if (_prevHit.transform != null)
                {
                    _dist = Vector3.Distance(_prevHit.transform.position, hit.transform.position);
                }
                
                if (_prevHit.transform != null && _prevHit.transform != hit.transform)
                {
                    var prevSlot = _prevHit.transform.GetComponent<Slot>();
                    prevSlot.SetFrameState(false);
                }

                _prevHit = hit;

                var slot = hit.transform.GetComponent<Slot>();
                slot.SetFrameState(true);

                if (slot.state == ESlotState.Full && _carryingItem == null && slot.currentItem != null && slot.currentItem.Data.State != EItemState.OPEN)
                {
                    slot.SetFrameState(false);
                } 
                else if (slot.state == ESlotState.Empty && _carryingItem == null)
                {
                    slot.SetFrameState(false);
                }

                //we are grabbing the item in a full slot
                if (slot.state == ESlotState.Full && _carryingItem == null)
                {
                    GrabbingItemInFullSlot(slot);
                }
                //we are dropping an item to empty slot
                else if (slot.state == ESlotState.Empty && _carryingItem != null)
                {
                    DroppingItemInEmptySlot(slot);
                }
                //we are dropping to full
                else if (slot.state == ESlotState.Full && _carryingItem != null)
                {
                    DroppingItemToFullSlot(slot);
                }

                OnUpdateSlots?.Invoke();
            }
            else
            {
                if (!_carryingItem)
                {
                    return;
                }
                OnItemCarryFail();
            }
        }

        private void GrabbingItemInFullSlot(Slot slot)
        {
            if (slot.currentItem.Data.State == EItemState.CLOSE || slot.currentItem.Data.State == EItemState.IMMOVABLE)
            {
                return;
            }
                    
            var itemGO = Instantiate(_itemInfoPrefab);
            var transform1 = itemGO.transform;
            transform1.position = slot.transform.position;
            transform1.localScale = Vector3.one;

            _carryingItem = itemGO.GetComponent<ItemInfo>();
            
            //TODO Check
            _carryingItem.InitDummy(slot.currentItem.Data, slot.id);

            slot.ItemGrabbed();
            slot.SetFrameState(false);
        }
        
        private void DroppingItemInEmptySlot(Slot slot)
        {
            slot.CreateItem(_carryingItem.Data);
            Destroy(_carryingItem.gameObject);
            
            if (_carryingItem.Data.Category == EMergeCategory.GENERATOR && _dist < 0.1f)
            {
                _genClicks++;
            }
            else
            {
                _genClicks = 0;
            }
            
            if (_gameStorage.Energy > 0 && _carryingItem.Data.Category == EMergeCategory.GENERATOR && _dist < 0.1f && slot.currentItem.Data.AmountOfItems > 0 && _genClicks > 1)
            {
                DropGeneratorItem(slot);
            }
            else if (_carryingItem.Data.Category == EMergeCategory.GENERATOR && _carryingItem.Data.Type == _currGen && _dist > 0.1f)
            {
                _slotsManager.NearSlots?.Clear();
            }

            if (_carryingItem.Data.Category == EMergeCategory.GENERATOR)
            {
                OnClickGenerator?.Invoke(_carryingItem.Data.Type);
            }
        }

        private void DropGeneratorItem(Slot slot)
        {
            var item = _generatorManager.GetMergeItem(_carryingItem.Data.Type);
            var id = item.level - 1;
            var slotId = _slotsManager.GetNearFreeSlot(_carryingItem.SlotId);
            if (slotId != -1)
            {
                var generatorItem = _generatorManager.GetGeneratorSpawnItem(id, item.mergeType, _carryingItem.SlotId, slotId);
                _slotsManager.PlaceItemByMoving(generatorItem);
                _currGen = _carryingItem.Data.Type;
                slot.currentItem.DecreaseAmountOfItems();

                if (slot.currentItem.Data.AmountOfItems == 0)
                {
                    _currGen = EMergeType.NONE;
                
                    Destroy(_carryingItem.gameObject);
                    Destroy(slot.currentItem.gameObject);
                
                    slot.currentItem = null;
                    slot.state = ESlotState.Empty;
                    slot.SetFrameState(false);
                }

                OnGenerateItem?.Invoke(item.mergeType);
            }
        }

        private void DroppingItemToFullSlot(Slot slot)
        {
            var maxMergeItemLevel = Utils.GetItemMaxLevelById(slot.currentItem.Data.Category, slot.currentItem.Data.Type);
            var maxMergeItemIndex = maxMergeItemLevel - 1;
            //check item in the slot
            if (slot.currentItem.Data.State != EItemState.CLOSE && slot.currentItem.Data.Id == _carryingItem.Data.Id && slot.currentItem.Data.Type == _carryingItem.Data.Type
                && maxMergeItemIndex != slot.currentItem.Data.Id && _carryingItem.Data.IsMergeable && slot.currentItem.Data.IsMergeable)
            {
                OnItemMergedWithTarget(slot.id);
            }
            else
            {
                OnItemCarryFail();
                if (slot.currentItem.Data.State == EItemState.OPEN)
                {
                    OnItemSwipe(_carryingItem.SlotId, slot.id);
                }
            }
        }

        private void OnItemSwipe(int slotStartId, int slotEndId)
        {
            Slot slotStart = _slotsManager.GetSlot(slotStartId);
            Slot slotEnd = _slotsManager.GetSlot(slotEndId);
            Vector3 startPosition = slotStart.transform.position;
            Vector3 endPosition = slotEnd.transform.position;

            slotEnd.currentItem.gameObject.transform.DOMove(startPosition, SWIPE_SPEED);
            slotStart.currentItem.gameObject.transform.DOMove(endPosition, SWIPE_SPEED);
            
            (slotStart.currentItem, slotEnd.currentItem) = (slotEnd.currentItem, slotStart.currentItem);
        }
        
        public void ShowMergeHint(Slot firstSlot, Slot secondSlot)
        {
            PulsationAnimation(firstSlot);
            PulsationAnimation(secondSlot);
        }
        
        private void PulsationAnimation(Slot slot)
        {
            var slotTransform = slot.currentItem.gameObject.transform;
            DOTween.Sequence()
                .SetRecyclable(true).SetAutoKill(true)
                .Append(slotTransform.DOScale(0.7f, 0.2f))
                .Append(slotTransform.DOScale(1.2f, 0.2f))
                .Append(slotTransform.DOScale(1f, 0.2f))
                .SetLoops(2)
                .SetEase(Ease.Linear);
        }
        
        private void OnItemSelected()
        {
            bool canMove = _prevCarryingItem != _carryingItem || _prevSlotId != _carryingItem.SlotId;

            _target = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _target.z = 0;
            float delta = 10 * Time.deltaTime;

            float distance = Vector3.Distance(transform.position, _target);
            float newDistance = Vector3.Distance(_carryingItem.transform.position, _target);

            if (newDistance > 0.28f || !canMove)
            {
                delta *= distance;
                _carryingItem.transform.position = Vector3.MoveTowards(_carryingItem.transform.position, _target, delta);
                _prevCarryingItem = _carryingItem;
                _prevSlotId = _carryingItem.SlotId;
            }
        }

        private void OnItemMergedWithTarget(int targetSlotId)
        {
            var slot = _slotsManager.GetSlotById(targetSlotId);
            _slotsManager.CheckCloseSlots(slot);

            Destroy(_carryingItem.gameObject);
            slot.currentItem.transform.DOScale(0.1f, 0.2f);
            _coroutineManager.SetTimeout(0.2f, () =>
            {
                Destroy(slot.currentItem.gameObject);
                // TODO Check
                _carryingItem.UpItemId();
                slot.CreateItem(_carryingItem.Data);
                
                OnMergeItem?.Invoke();
            });
        }

        private void OnItemCarryFail()
        {
            var slot = _slotsManager.GetSlotById(_carryingItem.SlotId);
            slot.CreateItem(_carryingItem.Data);
            Destroy(_carryingItem.gameObject);
        }

        private void DetectHitSlot(ItemInfo carryingItem)
        {
            RaycastHit2D hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            //we hit something 
            if (hit.collider != null)
            {
                var slot = hit.transform.GetComponent<Slot>();
                
                if (_hitSlot != null && _hitSlot.currentItem != null)
                {
                    _hitSlot.currentItem.SetHoverState(false);
                }
                
                if (slot.currentItem != null && slot.currentItem.Data.State != EItemState.CLOSE)
                {
                    if (slot != _hitSlot)
                    {
                        _hitSlot = slot;
                    }

                    if (slot.currentItem.Data.Type == carryingItem.Data.Type && slot.currentItem.Data.Id == carryingItem.Data.Id)
                    {
                        slot.currentItem.SetHoverState(true); 
                    }
                }
            }
        }

    }
}