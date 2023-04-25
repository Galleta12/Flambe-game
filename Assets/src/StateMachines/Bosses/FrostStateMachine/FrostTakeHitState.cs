using UnityEngine;

public class FrostTakeHitState : FrostBaseState
{
    private readonly int _takeHitAnimationHash = Animator.StringToHash("take_hit");
    private readonly FrostBaseState _nextState;

    private readonly Rigidbody2D _rb;

    public FrostTakeHitState(FrostStateMachine stateMachine, FrostBaseState nextState) : base(stateMachine)
    {
        _nextState = nextState;
        _rb = stateMachine.GetComponent<Rigidbody2D>();
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_takeHitAnimationHash, 0.1f);
        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void FixedTick(float fixedDeltaTime)
    {
        //Debug.Log(GetNormalizedTime(StateMachine.Animator));
        if (GetNormalizedTime(StateMachine.Animator) >= 1f) StateMachine.SwitchState(_nextState);
    }

    public override void Exit()
    {
        
    }
}