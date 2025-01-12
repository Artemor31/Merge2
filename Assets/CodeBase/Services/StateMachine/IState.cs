namespace Services.StateMachine
{
    public interface IState
    {
        void Enter();
    }

    public interface IState<in T> : IState
    {
        void Enter(T param);
    }
}