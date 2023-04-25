using UnityEngine;

public class FrostDeathState : FrostBaseState
{
    private readonly int _deathAnimationHash = Animator.StringToHash("death");
    private readonly Rigidbody2D _rb;
    private readonly CapsuleCollider2D _collider;
    
    public FrostDeathState(FrostStateMachine stateMachine) : base(stateMachine)
    {
        _rb = stateMachine.GetComponent<Rigidbody2D>();
        _collider = stateMachine.GetComponent<CapsuleCollider2D>();
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_deathAnimationHash, 0.1f);
        // _rb.velocity = new Vector2(0, _rb.velocity.y);
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        _collider.enabled = false;
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void FixedTick(float fixedDeltaTime)
    {
        
    }

    public override void Exit()
    {
        
    }
}