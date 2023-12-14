using System.Collections.Generic;
using System.Linq;
using General;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour, IObjectBeindInitialized
    {
        public InventorySlot TargetSlot { get; private set; }
        [field: SerializeField] public int Capacity { get; set; }
        public bool IsFull => _slotsList.All(slot => !slot.IsEmpty);
        
        [SerializeField] private List<InventorySlot> _slotsList = new List<InventorySlot>();
        [SerializeField] private Image _imageTargetSlot;
        private readonly Vector3 _bufferPosition = new Vector3(1000, 1000, 1000);
        

        public void SetTargetSlot(InventorySlot slot)
        {
            if (slot == TargetSlot && slot != null)
                return;
            else if (slot == null)
            {
                _imageTargetSlot.transform.localPosition = _bufferPosition;
                TargetSlot = null;
                return;
            }

            TargetSlot = slot;
            _imageTargetSlot.transform.SetParent(slot.transform);
            _imageTargetSlot.transform.localPosition = Vector3.zero;
        }

        public void Initialize()
        {
            if (_slotsList.Count == 0)
                _slotsList = GetComponentsInChildren<InventorySlot>().ToList();
            RemoveExcessSlots(_slotsList);
        }

        private void RemoveExcessSlots<T>(IList<T> list) where T : InventorySlot 
        {
            for (var slotIndex = 0; slotIndex < list.Count; slotIndex++)
            {
                var slot = list[slotIndex];

                if (!slot)
                    list.Remove(slot);

                slot.Initialize();
            }
        }

        public void RemoveItem()
        {
            if (!TargetSlot)
                return;
            
            TargetSlot.RemoveItem();
            TargetSlot = null;
        }
        
        
        InventoryItem GetItem(InventoryItem itemType)
        {
            return _slotsList.Find(slot => slot.Item == itemType).Item;
        }

        public InventoryItem[] GetAllItems()
        {
            var itemList = new List<InventoryItem>(Capacity);
            foreach (var slot in _slotsList)
            {
                if (!slot.IsEmpty)
                    itemList.Add(slot.Item);
            }

            return itemList.ToArray();
        }

        public InventoryItem[] GetAllItems(InventoryItem item)
        {
            var itemList = new List<InventoryItem>(Capacity);
            foreach (var slot in _slotsList)
            {
                if (!slot.IsEmpty && slot.Item == item)
                    itemList.Add(slot.Item);
            }

            return itemList.ToArray();
        }

        public InventoryItem[] GetEquippedItems(InventoryItem item)
        {
            var itemList = new List<InventoryItem>(Capacity);
            foreach (var slot in _slotsList)
            {
                if (!slot.IsEmpty && slot.Item.IsEquipped)
                    itemList.Add(slot.Item);
            }

            return itemList.ToArray();
        }

        public void Clear()
        {
            foreach (var slot in _slotsList)
            {
                if (!slot.IsEmpty)
                    slot.RemoveItem();
            }
        }
    }
}
