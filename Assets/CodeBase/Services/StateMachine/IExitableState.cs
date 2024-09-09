namespace Services.StateMachine
{
    public interface IExitableState : IState
    {
        void Exit();
    }
}