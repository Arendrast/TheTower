using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventorySystem
{
    public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private ScrollRect _scrollRect;
        private Image CurrentImageItem => _currentInventorySlot.ItemImage;
        private InventorySlot _currentInventorySlot;

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_currentInventorySlot)
            {
                if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent<InventorySlot>(out var slot)
                    && slot != _currentInventorySlot && slot.IsActive)
                {
                    slot.RemoveItem();
                    slot.AddItem(_currentInventorySlot.Item);
                }

                ReturnImageToSlot();
                _scrollRect.enabled = true;
                _currentInventorySlot = null;
            }
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent<InventorySlot>(out var slot) && !slot.IsActive)
            {
                Debug.Log(1);
                if (slot.IsAvailable && !slot.IsEmpty)
                {
                    _currentInventorySlot = slot;
                    CurrentImageItem.transform.SetParent(_inventory.transform);
                    CurrentImageItem.rectTransform.SetAsLastSibling();
                    _inventory.SetTargetSlot(slot);
                    _scrollRect.enabled = false;

                    _inventory.SetTargetSlot(slot);
                }
                
                else
                    _inventory.SetTargetSlot(null);
            }
        }

        private void ReturnImageToSlot()
        {
            CurrentImageItem.transform.SetParent(_currentInventorySlot.transform);
            CurrentImageItem.transform.localPosition = _currentInventorySlot.DefaultLocalPositionOfItemImage;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_currentInventorySlot)
            {
                CurrentImageItem.transform.position += new Vector3(eventData.delta.x, eventData.delta.y);
            }
        }
    }
}