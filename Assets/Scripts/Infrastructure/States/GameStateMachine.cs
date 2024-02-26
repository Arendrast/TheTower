using System;
using System.Collections.Generic;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using Logic;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.States
{
    public class GameStateMachine : IStartable
    {
        private IExitableState _activeState;

        private Dictionary<Type, IExitableState> _states;
        
        [Inject]
        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory,
            IPersistentProgressService progressService, ISaveLoadService saveLoadService, IUIFactory uiFactory)
        {
            RegisterState<BootstrapState>(new BootstrapState(this));
            
            RegisterState<LoadProgressState>(new LoadProgressState(this, progressService, 
            saveLoadService));
            
            RegisterState<LoadLevelState>(new LoadLevelState(this, sceneLoader, loadingCurtain, 
                gameFactory, uiFactory));
            
            RegisterState<MainMenuState>(new MainMenuState(this));
            
            Enter<BootstrapState>();
        }

        private void RegisterState<TState>(TState implementation) where TState : class, IExitableState
        {
            _states ??= new Dictionary<Type, IExitableState>();
            _states[typeof(TState)] = implementation;
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            var state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }

        public void Start()
        {
            
        }
    }
}