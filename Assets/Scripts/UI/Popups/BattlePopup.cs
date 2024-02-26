using Infrastructure;
using Infrastructure.Factory;
using Infrastructure.Services.SaveLoad;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class BattlePopup : BasePopup
    {
        [SerializeField] private Button _openLevelButton;
        private SceneLoader _sceneLoader;
        private IGameFactory _gameFactory;

        public void Construct(SceneLoader sceneLoader, 
            IGameFactory gameFactory)
        {
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
        }

        protected override void OnInitialization()
        {
            base.OnInitialization();
            _openLevelButton.onClick.AddListener(OpenLevel);
        }

        private void OnOpenLevel()
        {
            Debug.Log(_gameFactory);
            var player = _gameFactory.CreatePlayer(Vector3.zero);
            var tower = player.GetComponent<Tower>();
            var towerHealth = player.GetComponent<TowerHealth>();
            var moneyCurrency = _gameFactory.CreateMoneyCurrency();
            
            _gameFactory.CreateMusicPlayer();
            _gameFactory.CreateUpgrades(tower, towerHealth, moneyCurrency);
            tower.Initialize();
            towerHealth.Initialize();
        }
        

        private void OpenLevel() => _sceneLoader.Load(SceneNames.FirstLevel, OnOpenLevel);
    }
}