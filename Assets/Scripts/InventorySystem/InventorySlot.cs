﻿using System.Collections.Generic;
using System.Linq;
using General;
using MyCustomEditor;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class InventorySlot : MonoBehaviour, IObjectBeindInitialized
    {
        public bool IsAvailable => IsEmpty ? false : Item.IsAvailable;

        [field: SerializeField]
        public Image ItemImage
        {
            get => _itemImage;
            private set => _itemImage = value;
        }

        public InventoryItem Item
        {
            get => _item;
            private set => _item = value;
        }
        
        [field: SerializeField] public bool IsActive { get; private set;  }
        
        public bool IsEmpty => Item == null;
        public Vector3 DefaultLocalPositionOfItemImage { get; private set; }
        
        [SerializeField] private Image _itemImage;

        [SerializeField] private InventoryItem _defaultItem;

        [SerializeField] private Image _lockImage;
        
        [ReadOnlyInspector] [SerializeField] private InventoryItem _item;

        public void Initialize()
        {
            if (_defaultItem)
                Item = _defaultItem;

            var image = GetComponent<Image>();
            var list = GetComponentsInChildren<Image>().ToList();
            
            if (list.Contains(image))
                list.Remove(image);
            
            _itemImage = AddImage(_itemImage, ref list);
            _lockImage = AddImage(_lockImage, ref list);

            if (!IsEmpty && Item.Sprite && _itemImage)
            {
                _itemImage.sprite = Item.Sprite;
                SetTransparencyOfItemImage(255);
            }

            if (!IsActive)
                _lockImage.gameObject.SetActive(IsEmpty ? true : !Item.IsAvailable);
            else
            {
                _lockImage.gameObject.SetActive(false);   
                SetTransparencyOfItemImage(0);
            }

            DefaultLocalPositionOfItemImage = ItemImage.transform.localPosition;
        }

        private Image AddImage(Image image, ref List<Image> imageList)
        {
            if (image)
                return image;

            for (var i = 0; i < imageList.Count; i++)
            {
                if (!imageList[i]) continue;
                    
                image = imageList[i];
                imageList.Remove(imageList[i]);
                break;
            }
            
            if (!image)
                Debug.LogError($"У {name} нету одного из изображений. Добавь!");

            return image;
        }

        public void RemoveItem()
        {
            Item = null;
            ItemImage.sprite = null;
            SetTransparencyOfItemImage(0);
        }

        public void AddItem(InventoryItem item)
        {
            if (IsEmpty)
                Item = item;

            ItemImage.sprite = item.Sprite;
            SetTransparencyOfItemImage(255);
        }

        private void SetTransparencyOfItemImage(int value)
        {
            var color = ItemImage.color;
            ItemImage.color = new Color(color.r, color.g, color.b, value);
        }
    }
}