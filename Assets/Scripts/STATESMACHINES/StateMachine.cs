using FishNet;

namespace onlinetutorial.interfaces
{
    public abstract class StateMachine
    {
        protected IState currentState;


        public void ChangeState(IState newState)
        {
            currentState?.Exit();

            currentState = newState;

            currentState.Enter();
        }

        public void Input(InputData md)
        {
            currentState?.Input(md);
        }

        public IState GetCurrentState() => currentState;

        public void Update(float tickdelta)
        {
            currentState?.Update(tickdelta);
        }

        public void PhysicsUpdate(float tick)
        {
            currentState?.PhysicsUpdate(tick);
        }


    }
}