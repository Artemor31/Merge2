namespace Services.StateMachine
{
    public interface IState<in T> : IState
    {
        void Enter(T param);
    }
    
    public interface IState
    {
        void Enter();
    }
}