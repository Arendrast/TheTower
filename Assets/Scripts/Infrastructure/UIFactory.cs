using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.StaticData;
using UnityEngine;
using VContainer;

namespace Infrastructure
{
    public class UIFactory : IUIFactory
    {
        private Canvas _mainCanvas;
        private IGameFactory _gameFactory;
        private GameObject _playerGameObject;
        private MonoBehaviour _customCoroutineRunner;
        private readonly IContainerBuilder _builder;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;

        [Inject]
        public UIFactory(IAssetProvider assetProvider, IContainerBuilder builder, IStaticDataService staticData, 
            IPersistentProgressService progressService, MonoBehaviour customCoroutineRunner)
        {
            _assetProvider = assetProvider;
            _builder = builder;
            _staticData = staticData;
            _progressService = progressService;
            _customCoroutineRunner = customCoroutineRunner;
        }
    }
}