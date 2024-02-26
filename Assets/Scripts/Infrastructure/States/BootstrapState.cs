namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        
        public BootstrapState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            EnterLoadLevel();
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }
    }
}