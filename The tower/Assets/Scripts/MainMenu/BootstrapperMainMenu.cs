using System.Collections.Generic;
using General;
using InventorySystem;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace MainMenu
{
    public class BootstrapperMainMenu : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private List<Switchable> _switchableMenuList;
        [SerializeField] private List<VolumeСontrol> _volumeControlList;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private UnityEvent _onAwake;
        private void Awake()
        {
            _sceneLoader.Initialize();
            _inventory.Initialize();
            InitializeList(_switchableMenuList);
            InitializeList(_volumeControlList);
            
            _onAwake?.Invoke();
        }
        
        private void InitializeList(IEnumerable<IObjectBeindInitialized> list)
        {
            foreach (var obj in list)
                obj.Initialize();
        }
    }
}
