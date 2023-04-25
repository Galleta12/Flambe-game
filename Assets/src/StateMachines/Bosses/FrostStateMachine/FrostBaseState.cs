

public abstract class FrostBaseState : State
{
    protected readonly FrostStateMachine StateMachine;
    
    protected FrostBaseState(FrostStateMachine stateMachine){
        StateMachine = stateMachine;
    }
}