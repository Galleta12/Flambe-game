using System.Collections.Generic;
using UnityEngine;

public class FrostAttackState : FrostBaseState
{
    private readonly Rigidbody2D _rb;
    private readonly SpriteRenderer _sr;
    
    private readonly int _punchAnimationHash = Animator.StringToHash("1_atk");

    private readonly List<Collider2D> _colliderList;
    private bool _hasDoneDamage;

    public FrostAttackState(FrostStateMachine stateMachine) : base(stateMachine)
    {
        _rb = stateMachine.GetComponent<Rigidbody2D>();
        _sr = stateMachine.GetComponent<SpriteRenderer>();

        _colliderList = new List<Collider2D>();
    }
    
    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_punchAnimationHash, 0.1f);
        _hasDoneDamage = false;
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
        var normalizedTime = GetNormalizedTime(StateMachine.Animator);
        if (normalizedTime >= 1f)
        {
            if ((_rb.transform.position - StateMachine.PlayerRb.transform.position).magnitude > 5)
            {
                StateMachine.SwitchState(new FrostIdleState(StateMachine, 2.5f));
            }
            else
            {
                StateMachine.SwitchState(new FrostChaseState(StateMachine));
            }
        }
        else
        {
            if (!_hasDoneDamage && normalizedTime >= 0.5f) DoDamage(_sr.flipX ? StateMachine.PunchTriggerRight : StateMachine.PunchTriggerLeft);
        }
    }

    public override void Exit()
    {
    }

    private void DoDamage(Collider2D collider)
    {
        collider.OverlapCollider(new ContactFilter2D {useLayerMask = true, layerMask = LayerMask.GetMask("Character")}, _colliderList);
        foreach (var target in _colliderList)
        {
           if(target.CompareTag("Player")){

                var health = target.GetComponent<Health>();
                //the other variable is to check if the collision was from behind or in the front
                
                Vector2 direction = (collider.transform.position - StateMachine.transform.position).normalized;
                //knockback
                health.DealDamage(150, direction * 10, direction);
                //health.DealDamage(10, Vector2.zero, direction);
                _hasDoneDamage = true;
           }
        }
    }
}