using Infrastructure.AssetManagement;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Factory
{
    public class UIFactory : IUIFactory
    {
        private Canvas _mainCanvas;
        private IGameFactory _gameFactory;
        private GameObject _playerGameObject;
        private readonly IContainerBuilder _builder;
        private MainMenuPopup _mainMenuPopup;
        private readonly SceneLoader _sceneLoader;

        [Inject]
        public UIFactory(IContainerBuilder builder, SceneLoader sceneLoader, IGameFactory gameFactory)
        {
            _builder = builder;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
        }
        
        public MainMenuPopup CreateMainMenuPopup()
        {
            if (_mainMenuPopup == null)
            {
                _mainMenuPopup = InitializePopup<MainMenuPopup>(AssetPath.MainMenuPopupPath);
                _mainMenuPopup.Construct(_sceneLoader, _gameFactory);
            }
            return _mainMenuPopup;
        }
        
        private TPopup InitializePopup<TPopup>(string path) where TPopup : BasePopup
        {
            var popupComponent = Object.Instantiate((GameObject) Resources.Load(path));
            var popup = popupComponent.GetComponent<TPopup>();
            _builder.RegisterComponent<TPopup>(popup);

            return popup;
        }
    }
}