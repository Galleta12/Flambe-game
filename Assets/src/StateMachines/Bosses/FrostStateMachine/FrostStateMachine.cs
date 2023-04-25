using TMPro;
using UnityEngine;

public class FrostStateMachine : StateMachine
{
    [SerializeField] protected internal Collider2D PunchTriggerLeft;
    [SerializeField] protected internal Collider2D PunchTriggerRight;

    [field:SerializeField] public TMP_Text EnemyGuardStamina {get; private set;}
    
    [field:SerializeField] public TMP_Text EnemyLife {get; private set;}
    
    protected internal Rigidbody2D PlayerRb;
    protected internal Animator Animator;
    protected internal Health Health;
    protected internal GuardStamina Guard;

    private void Awake()
    {
        var playerGameObject = GameObject.FindWithTag("Player");
        if (playerGameObject == null) Debug.LogError("FrostBoss could not find player by tag!");
        PlayerRb = playerGameObject.GetComponent<Rigidbody2D>();
        
        Animator = GetComponent<Animator>();
        Health = GetComponent<Health>();
        Guard = GetComponent<GuardStamina>();

        Health.OnTakeDamage += OnTakeHit;
        Health.OnDeath += OnDie;
    }

    private void Start()
    {
        var initialState = new FrostChaseState(this);
        SwitchState(initialState);
    }

      public override void CustomUpdate(float deltaTime)
    {
        var currentHealth = Health.GetHealth();
        EnemyLife.text = "Enemy: " + currentHealth; 
        var currentGuardStamina =  Guard.GetGuardStamina();
        EnemyGuardStamina.text = "Enemy Stamina: " + currentGuardStamina;
    }

    private void OnTakeHit(Vector2 knockBack)
    {
        SwitchState(new FrostTakeHitState(this, new FrostChaseState(this)));
    }

    private void OnDie()
    {
        SwitchState(new FrostDeathState(this));
    }
}