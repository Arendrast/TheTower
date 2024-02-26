using System.Collections.Generic;
using Currencies;
using General;
using Infrastructure.AssetManagement;
using Infrastructure.Services.Input;
using Infrastructure.Services.PersistentProgress;
using Player;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IInputService _inputService;
        private GameObject _playerGameObject;

        public GameFactory(IAssetProvider assets, IInputService inputService)
        {
            _assets = assets;
            _inputService = inputService;
        }

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        public GameObject CreatePlayer(Vector3 at)
        {
            _playerGameObject = InstantiateRegistered(AssetPath.PlayerPrefabPath);

            return _playerGameObject;
        }

        public Upgrades CreateUpgrades(Tower tower, TowerHealth towerHealth, MoneyCurrency moneyCurrency)
        {
            var upgrades = _assets.Instantiate<Upgrades>();
            upgrades.Construct(tower, towerHealth, moneyCurrency);
            
            return upgrades;
        }

        public MoneyCurrency CreateMoneyCurrency()
        {
            var moneyCurrency = _assets.Instantiate<MoneyCurrency>(AssetPath.Currencies);

            return moneyCurrency;
        }

        public MusicPlayer CreateMusicPlayer()
        {
            var musicPlayer = _assets.Instantiate<MusicPlayer>();
            var songConfig = _assets.Instantiate<SongConfig>(AssetPath.StaticData);
            musicPlayer.Construct(songConfig.Songs);

            return musicPlayer;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public GameObject GetPlayer() => _playerGameObject;

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            var gameObject = _assets.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            var gameObject = _assets.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }
    }
}