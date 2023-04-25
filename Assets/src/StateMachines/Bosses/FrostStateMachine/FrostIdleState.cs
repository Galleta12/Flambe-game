using System.Collections;
using UnityEngine;

public class FrostIdleState : FrostBaseState
{
    private readonly Rigidbody2D _rb;
    private readonly int _idleAnimationHash = Animator.StringToHash("idle");
    private readonly float _idleTime;

    private Coroutine _waitCoroutine;
    
    public FrostIdleState(FrostStateMachine stateMachine, float idleTime) : base(stateMachine)
    {
        _rb = stateMachine.GetComponent<Rigidbody2D>();
        _idleTime = idleTime;
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_idleAnimationHash, 0.1f);
        _waitCoroutine = StateMachine.StartCoroutine(WaitIdleTime());
    }

    public override void Tick(float deltaTime)
    {
        if (!((StateMachine.PlayerRb.transform.position - StateMachine.transform.position).magnitude > 25)) return;
        StateMachine.StopCoroutine(_waitCoroutine);
        StateMachine.SwitchState(new FrostWaitState(StateMachine));
    }

    public override void FixedTick(float fixedDeltaTime)
    {
        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }

    public override void Exit()
    {
    }

    private IEnumerator WaitIdleTime()
    {
        yield return new WaitForSeconds(_idleTime);
        StateMachine.SwitchState(new FrostChaseState(StateMachine));
    }
}
