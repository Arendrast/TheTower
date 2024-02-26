using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Infrastructure.Services.Input;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.Randomizer;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.StaticData;
using Infrastructure.States;
using Logic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Scopes
{
    public class ServicesScope : LifetimeScope
    {
        [SerializeField] private LoadingCurtain _loadingCurtainPrefab;
        private CustomCoroutineRunner _coroutineRunnerInstance;
        private IContainerBuilder _builder;

        protected override void Configure(IContainerBuilder containerBuilder)
        {
            DontDestroyOnLoad(gameObject);
            _builder = containerBuilder;
            
            RegisterBuilder();
            RegisterStaticDataService();
            RegisterInputService();
            RegisterRandomService();
            RegisterAssetProvider();
            RegisterPersistentProgressService();
            RegisterSaveLoadService();
            RegisterGameFactory();
            RegisterUIFactory();

            RegisterCoroutineRunner();
            RegisterSceneLoader();
            RegisterLoadingCurtain();

            RegisterGameStateMachine();
        }

        private void RegisterCoroutineRunner()
        {
            CreateCoroutineRunner();
            
            DontDestroyOnLoad(_coroutineRunnerInstance);
            
            _builder.RegisterComponent<ICoroutineRunner>(_coroutineRunnerInstance);
            _builder.RegisterComponent<MonoBehaviour>(_coroutineRunnerInstance);
            
        }

        private void CreateCoroutineRunner()
        {
            _coroutineRunnerInstance = new GameObject("CoroutineRunner", new[] {typeof(CustomCoroutineRunner)})
                .GetComponent<CustomCoroutineRunner>();
        }

        private void RegisterLoadingCurtain()
        {
            LoadingCurtain loadingCurtainInstance = Instantiate(_loadingCurtainPrefab);
            DontDestroyOnLoad(loadingCurtainInstance);
            _builder.RegisterComponent<LoadingCurtain>(loadingCurtainInstance);
        }

        private void RegisterBuilder() =>
            _builder.RegisterInstance<IContainerBuilder>(_builder);

        private void RegisterUIFactory()
        {
             _builder.Register<UIFactory>(Lifetime.Singleton)
                .As<IUIFactory>();
        }

        private void RegisterSceneLoader()
        {
            SceneLoader sceneLoader = new SceneLoader(_coroutineRunnerInstance);

            _builder.RegisterInstance<SceneLoader>(sceneLoader);
        }

        private void RegisterGameFactory()
        {
            _builder.Register<GameFactory>(Lifetime.Singleton)
                .As<IGameFactory>();
        }
        

        private void RegisterPersistentProgressService()
        {
            _builder.Register<PersistentProgressService>(Lifetime.Singleton).
                As<IPersistentProgressService>();
        }

        private void RegisterRandomService()
        {
            _builder.Register<RandomService>(Lifetime.Singleton)
                .As<IRandomService>();
        }

        private void RegisterInputService()
        {
            if (Application.isEditor)
            {
                _builder.Register<EditorInputService>(Lifetime.Singleton)
                    .As<IInputService>();
            }
            else
            {
                _builder.Register<MobileInputService>(Lifetime.Singleton)
                    .As<IInputService>();
            }
        }

        private void RegisterStaticDataService()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();

            _builder.RegisterInstance<IStaticDataService>(staticData);
        }

        private void RegisterSaveLoadService()
        {
            _builder.Register<SaveLoadService>(Lifetime.Singleton)
                .As<ISaveLoadService>();
        }

        private void RegisterAssetProvider()
        {
            _builder.Register<AssetProvider>(Lifetime.Singleton)
                .As<IAssetProvider>();
        }

        private void RegisterGameStateMachine() => _builder.RegisterEntryPoint<GameStateMachine>();
    }
}