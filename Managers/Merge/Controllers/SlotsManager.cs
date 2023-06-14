using System.Collections.Generic;
using DG.Tweening;
using MergeIdle.Scripts.Configs.Merge.Enum;
using MergeIdle.Scripts.Databases.MergeItems.Enum;
using MergeIdle.Scripts.Managers.Merge.Data;
using MergeIdle.Scripts.Managers.Merge.Enums;
using MergeIdle.Scripts.Managers.Merge.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MergeIdle.Scripts.Managers.Merge.Controllers
{
    public class SlotsManager : MonoBehaviour
    {
        private const float SPAWN_MOVING_SPEED = 0.5f;
        private const int ROW_LENGHT = 5;
        private const int COL_LENGHT = 7;

        [SerializeField] private GameObject _slotsPack;
        [SerializeField] private RectTransform _slotsRectTransform;

        [SerializeField] private List<Slot> _slots;

        private Dictionary<int, Slot> _slotDictionary;
        private List<int> _nearSlots;

        public List<Slot> Slots => _slots;
        public RectTransform SlotsRectTransform => _slotsRectTransform;
        public List<int> NearSlots => _nearSlots;

        private void Start()
        {
            _nearSlots = new List<int>();
        }

        private Slot GetRandomSlot()
        {
            var rand = Random.Range(0, _slots.Count);
            Slot slot = GetSlotById(rand);

            while (slot.state == ESlotState.Full)
            {
                if (IsAllSlotsOccupied()) break;
                rand = Random.Range(0, _slots.Count);
                slot = GetSlotById(rand);
            }

            return slot;
        }

        private Slot GetSlotInTurn()
        {
            foreach (var slot in _slots)
            {
                if (slot.state != ESlotState.Full)
                {
                    return slot;
                }
            }

            return null;
        }

        public Slot GetSlot(int slotId = -1)
        {
            Slot slot;
            if (slotId == -2)
            {
                slot = GetSlotInTurn();
            }
            else if (slotId == -1)
            {
                slot = GetRandomSlot();
            }
            else
            {
                slot = GetSlotById(slotId);
            }

            if (IsAllSlotsOccupied())
            {
                return null;
            }

            return slot;
        }


        public void InitSlots()
        {
            foreach (Transform child in _slotsPack.transform)
            {
                if (child.GetComponent<Slot>() != null)
                {
                    _slots.Add(child.GetComponent<Slot>());
                }
            }

            _slotDictionary = new Dictionary<int, Slot>();

            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].id = i;
                _slotDictionary.Add(i, _slots[i]);
            }
        }

        public void InitItems(List<int> slotsIds, EItemState state)
        {
            for (int i = 0; i < slotsIds.Count; i++)
            {
                var mergeTypes = Utils.GetMergeTypes();
                var index = Random.Range(0, mergeTypes.Count);
                EMergeType mergeType = mergeTypes[index];
                InitItem(state, mergeType, slotsIds[i]);
            }
        }

        public void InitItem(EItemState state, EMergeType mergeType, int slotId)
        {
            var spawnItem = CreateMergeItem(state, mergeType, slotId);
            PlaceItem(spawnItem);
        }

        private SpawnItemData CreateMergeItem(EItemState state, EMergeType mergeType, int slotId)
        {
            SpawnItemData spawnItemData =
                new SpawnItemData(state, EMergeCategory.NO_GENERATOR, mergeType, true, 0, 0, slotId);
            return spawnItemData;
        }

        public void PlaceItem(SpawnItemData spawnItemData)
        {
            Slot slot = GetSlot(spawnItemData.SlotStartId);
            
            if (slot != null)
            {
                slot.CreateItem(spawnItemData.CreateItemData);
            }
        }

        public void PlaceItemByMoving(SpawnItemData spawnItemData)
        {
            Slot slotStart = GetSlot(spawnItemData.SlotStartId);
            Slot slotEnd = GetSlot(spawnItemData.SlotEndId);

            if (slotEnd != null)
            {
                Vector3 endPosition = slotEnd.transform.position;

                slotEnd.CreateItem(spawnItemData.CreateItemData);
                slotEnd.currentItem.transform.position = slotStart.currentItem.transform.position;
        
                slotEnd.currentItem.gameObject.transform.DOMove(endPosition, SPAWN_MOVING_SPEED);
            }
        }

        private bool IsAllSlotsOccupied()
        {
            foreach (var slot in _slots)
            {
                if (slot.state == ESlotState.Empty)
                {
                    //empty slot found
                    return false;
                }
            }
            //no slot empty 
            return true;
        }

        public List<Slot> GetDuplicateItemsSlots(List<Slot> slots)
        {
            List<Slot> duplicateSlots = new List<Slot>();
            var mergeableSlotsWithItems = GetMergeableSlots(slots, EMergeCategory.GENERATOR, EItemState.OPEN);

            for (int i = 0; i < mergeableSlotsWithItems.Count; i++)
            {
                duplicateSlots.Add(mergeableSlotsWithItems[i]);
                for (int j = 0; j < mergeableSlotsWithItems.Count; j++)
                {
                    if (i == j) continue;
                    var pointerSlotItem = duplicateSlots[0].currentItem;
                    var slotItem = mergeableSlotsWithItems[j].currentItem;
                    if (pointerSlotItem.Data.Type == slotItem.Data.Type && pointerSlotItem.Data.Id == slotItem.Data.Id)
                    {
                        duplicateSlots.Add(mergeableSlotsWithItems[j]);
                        return duplicateSlots;
                    }
                }
                duplicateSlots.Clear();
            }

            return duplicateSlots;
        }

        public List<Slot> GetOpenSlots(EMergeType mergeType, int level)
        {
            List<Slot> slots = new List<Slot>();
            foreach (var dictItem in _slotDictionary)
            {
                var item = dictItem.Value;
                if (item.currentItem != null && item.currentItem.Data.State == EItemState.OPEN 
                && item.currentItem.Data.Type == mergeType && item.currentItem.Data.Id == level - 1)
                {
                    slots.Add(item);
                }
            } 
            return slots;
        }

        public void DeleteSlotsItems(EMergeType mergeType, int level, int amount)
        {
            var slots = GetOpenSlots(mergeType, level);

            if (amount <= slots.Count)
            {
                for (int i = 0; i < amount; i++)
                {
                    Destroy(slots[i].currentItem.gameObject);
                    slots[i].state = ESlotState.Empty;
                }
            }
        }

        public Slot GetSlotById(int id)
        {
            return _slotDictionary[id];
        }
        
        public int GetNearFreeSlot(int currSlotId)
        {
            int id = currSlotId;
            int counter = 1;
            for (int i = 0; i < counter; i++)
            {
                // Right
                if (isRightSlot(ROW_LENGHT, COL_LENGHT, id))
                {
                    if (ESlotState.Empty == GetSlotById(id + 1).state)
                    {
                        _nearSlots.Add(id + 1);
                        return id + 1;
                    }
                }
                
                // Right Top
                if (isRightTopSlot(ROW_LENGHT, COL_LENGHT, id))
                {
                    if (ESlotState.Empty == GetSlotById(id + 1 - ROW_LENGHT).state)
                    {
                        _nearSlots.Add(id + 1 - ROW_LENGHT);
                        return id + 1 - ROW_LENGHT;
                    }
                }
                
                // Top
                if (isTopSlot(ROW_LENGHT, COL_LENGHT, id))
                {
                    if (ESlotState.Empty == GetSlotById(id - ROW_LENGHT).state)
                    {
                        _nearSlots.Add(id - ROW_LENGHT);
                        return id - ROW_LENGHT;
                    }
                }
                
                // Left Top
                if (isLeftTopSlot(ROW_LENGHT, COL_LENGHT, id))
                {
                    if (ESlotState.Empty == GetSlotById(id - 1 - ROW_LENGHT).state)
                    {
                        _nearSlots.Add(id - 1 - ROW_LENGHT);
                        return id - 1 - ROW_LENGHT;
                    }
                }
                
                // Left
                if (isLeftSlot(ROW_LENGHT, COL_LENGHT, id))
                {
                    if (ESlotState.Empty == GetSlotById(id - 1).state)
                    {
                        _nearSlots.Add(id - 1);
                        return id - 1;
                    }
                }
                
                // Left Bottom
                if (isLeftBottomSlot(ROW_LENGHT, COL_LENGHT, id))
                {
                    if (ESlotState.Empty == GetSlotById(id - 1 + ROW_LENGHT).state)
                    {
                        _nearSlots.Add(id - 1 + ROW_LENGHT);
                        return id - 1 + ROW_LENGHT;
                    }
                }

                // Bottom
                if (isBottomSlot(ROW_LENGHT, COL_LENGHT, id))
                {
                    if (ESlotState.Empty == GetSlotById(id + ROW_LENGHT).state)
                    {
                        _nearSlots.Add(id + ROW_LENGHT);
                        return id + ROW_LENGHT;
                    }
                }
                
                // Right Bottom
                if (isRightBottomSlot(ROW_LENGHT, COL_LENGHT, id))
                {
                    if (ESlotState.Empty == GetSlotById(id + 1 + ROW_LENGHT).state)
                    {
                        _nearSlots.Add(id + 1 + ROW_LENGHT);
                        return id + 1 + ROW_LENGHT;
                    }
                }

                if (counter == ROW_LENGHT * COL_LENGHT && IsAllSlotsOccupied()) return -1;
                if (counter == ROW_LENGHT * COL_LENGHT && !IsAllSlotsOccupied()) return GetRandomSlot().id;
                
                if (_nearSlots.Count > counter)
                {
                    id = _nearSlots[counter];
                }
                counter++;
            }

            return -1;
        }
        
        private List<Slot> GetMergeableSlots(List<Slot> slots, EMergeCategory category, EItemState state) => 
            slots
            .FindAll(x =>
            {
                if (x.currentItem == null) return false;
                var itemData = x.currentItem.Data;
                var isCategory = itemData.State == state;
                var isState = itemData.Category == category;
                if (isCategory && isState)
                {
                    var maxMergeItemLevel = Utils.GetItemMaxLevelById(itemData.Category, itemData.Type);
                    var maxMergeItemIndex = maxMergeItemLevel - 1;
                    return x.currentItem.Data.Id != maxMergeItemIndex;
                }
                return false;
            });
        
        private bool isLeftSlot(int row, int col, int currSlotId) => currSlotId != 0 && currSlotId % row != 0;
        private bool isRightSlot(int row, int col, int currSlotId) => currSlotId != 0 && (currSlotId + 1) % row != 0;
        private bool isTopSlot(int row, int col, int currSlotId) => currSlotId >= row;
        private bool isBottomSlot(int row, int col, int currSlotId) => currSlotId <= row * (col - 1) - 1;
        
        private bool isLeftTopSlot(int row, int col, int currSlotId) => 
            isLeftSlot(row, col, currSlotId) && isTopSlot(row, col, currSlotId);
        
        private bool isRightTopSlot(int row, int col, int currSlotId) => 
            isRightSlot(row, col, currSlotId) && isTopSlot(row, col, currSlotId);
        
        private bool isRightBottomSlot(int row, int col, int currSlotId) => 
            isRightSlot(row, col, currSlotId) && isBottomSlot(row, col, currSlotId);
        
        private bool isLeftBottomSlot(int row, int col, int currSlotId) => 
            isLeftSlot(row, col, currSlotId) && isBottomSlot(row, col, currSlotId);

        public void CheckCloseSlots(Slot openSlot)
        {
            List<int> slotIdsToCheck = new List<int>();
            int openSlotId = openSlot.id;
            
            // Left
            if (isLeftSlot(ROW_LENGHT, COL_LENGHT, openSlotId))
            {
                int leftSlotId = openSlotId - 1;
                slotIdsToCheck.Add(leftSlotId);
            }
            
            // Right
            if (isRightSlot(ROW_LENGHT, COL_LENGHT, openSlotId))
            {
                int rightSlotId = openSlotId + 1;
                slotIdsToCheck.Add(rightSlotId);
            }
            
            // Top
            if (isTopSlot(ROW_LENGHT, COL_LENGHT, openSlotId))
            {
                int topSlotId = openSlotId - ROW_LENGHT;
                slotIdsToCheck.Add(topSlotId);
            }
            
            // Bottom
            if (isBottomSlot(ROW_LENGHT, COL_LENGHT, openSlotId))
            {
                int bottomSlotId = openSlotId + ROW_LENGHT;
                slotIdsToCheck.Add(bottomSlotId);
            }

            if (slotIdsToCheck.Count > 0)
            {
                foreach (var id in slotIdsToCheck)
                {
                    Slot slotById = GetSlotById(id);
                    if (slotById.currentItem != null && slotById.currentItem.Data.State == EItemState.CLOSE)
                    {
                        slotById.currentItem.Open();
                    }
                }
            }
        }
    }
}