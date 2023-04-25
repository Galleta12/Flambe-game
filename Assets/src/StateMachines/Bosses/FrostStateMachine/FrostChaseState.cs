
using UnityEngine;

public class FrostChaseState : FrostBaseState
{
    private readonly Rigidbody2D _rb;
    private readonly SpriteRenderer _sr;
    private readonly int _walkAnimationHash = Animator.StringToHash("walk");
    private readonly float _walkSpeedMultiplier = 2f;
    
    public FrostChaseState(FrostStateMachine stateMachine) : base(stateMachine)
    {
        _rb = stateMachine.GetComponent<Rigidbody2D>();
        _sr = stateMachine.GetComponent<SpriteRenderer>();
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_walkAnimationHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        if ((StateMachine.PlayerRb.transform.position - StateMachine.transform.position).magnitude > 25)
        {
            StateMachine.SwitchState(new FrostWaitState(StateMachine));
        }
    }

    public override void FixedTick(float fixedDeltaTime)
    {
        var horizontalVectorToPlayer = StateMachine.PlayerRb.position.x - _rb.position.x;
        var distanceToPlayer = Mathf.Abs(horizontalVectorToPlayer);
        
        var direction = Mathf.Clamp(horizontalVectorToPlayer, -1f, 1f);
        _sr.flipX = direction > 0;
        if (distanceToPlayer > 2)
        {
            direction *= _walkSpeedMultiplier;
            _rb.velocity = new Vector2(direction, _rb.velocity.y);
        }
        else
        {
            StateMachine.SwitchState(new FrostAttackState(StateMachine));
        }
    }

    public override void Exit()
    {
        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }
}