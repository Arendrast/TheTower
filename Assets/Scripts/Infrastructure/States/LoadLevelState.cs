using Infrastructure.Factory;
using Logic;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IGameFactory _gameFactory;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly SceneLoader _sceneLoader;
        private readonly GameStateMachine _stateMachine;
        private readonly IUIFactory _uiFactory;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            IGameFactory gameFactory, IUIFactory uiFactory)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _gameFactory.Cleanup();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() => _loadingCurtain.Hide();

        private void OnLoaded()
        {
            Exit();
            _uiFactory.CreateMainMenuPopup();
            _stateMachine.Enter<MainMenuState>();
        }
    }
}