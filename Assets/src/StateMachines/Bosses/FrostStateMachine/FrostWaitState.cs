using UnityEngine;

public class FrostWaitState : FrostBaseState
{
    private readonly Rigidbody2D _rb;
    private readonly int _idleAnimationHash = Animator.StringToHash("idle");
    
    public FrostWaitState(FrostStateMachine stateMachine) : base(stateMachine)
    {
        _rb = stateMachine.GetComponent<Rigidbody2D>();
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_idleAnimationHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        if ((StateMachine.PlayerRb.transform.position - StateMachine.transform.position).magnitude < 20)
        {
            StateMachine.SwitchState(new FrostChaseState(StateMachine));
        }
    }

    public override void FixedTick(float fixedDeltaTime)
    {
        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }

    public override void Exit()
    {
    }
}
