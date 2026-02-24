using Unity.VisualScripting;
using UnityEngine;

public class StateAgent : AIAgent
{
    enum State
    {
        Idle,
        Move,
        Patrol,
        Attack,
        Death,
        Flee,
        Chase
    }

    public Movement movement;
    public Perception perception;
    public Animator animator;
    public Transform attackPoint;

    [SerializeField] State state;

    [Header("Parameters")]
    public float timer;
    public float health;
    public float maxHealth = 100.0f;
    public float distanceToDestination;
    public float distanceToEnemy;
    public AIAgent enemy;

    //public AIStateMachine StateMachine { get; private set; } = new AIStateMachine();
    public AIPushDownStateMachine PushDownStateMachine { get; private set; } = new AIPushDownStateMachine();

    public Vector3 Destination
    {
        get { return movement.Destination; }
        set { movement.Destination = value; }
    }

    void Start()
    {
        state = State.Idle;
        timer = 2.0f;
        health = maxHealth;

        PushDownStateMachine.AddState(new AIIdleState(this));
        PushDownStateMachine.AddState(new AIPatrolState(this));
        PushDownStateMachine.AddState(new AIDamagedState(this));
        PushDownStateMachine.AddState(new AIChaseState(this));
        PushDownStateMachine.AddState(new AIAttackState(this));
        PushDownStateMachine.AddState(new AIDeathState(this));

        PushDownStateMachine.PushState<AIIdleState>();
    }

    private void Update()
    {
        UpdateParameters();
        PushDownStateMachine.Update();


       
    }

    private void UpdateParameters()
    {
        // update params
        timer -= Time.deltaTime;
        distanceToDestination = Vector3.Distance(transform.position, Destination);
        animator.SetFloat("Blend", movement.Velocity.magnitude);

        //look for enemies
        enemy = null;
        var gameObjects = perception.GetGameObjects();
        if (gameObjects.Length > 0)
        {
            StateAgent stateAgent = null;
            if(gameObjects[0].TryGetComponent<StateAgent>(out stateAgent))
            {
                //makes sure the enemy is not already damaged or dead
                if (stateAgent.PushDownStateMachine.CurrentState != null &&
                    stateAgent.PushDownStateMachine.CurrentState.GetType() != typeof(AIDamagedState) &&
                    stateAgent.PushDownStateMachine.CurrentState.GetType() != typeof(AIDeathState))
                {
                    enemy = stateAgent;
                }
            }
        }
        distanceToEnemy = (enemy != null) ? Vector3.Distance(transform.position, enemy.transform.position) : float.MaxValue;
    }

    public void OnDamage(float damage)
    {
        health -= damage;
        if (health <= 0.0f)
        {
            PushDownStateMachine.SetState<AIDeathState>();
        }
        else
        {
            PushDownStateMachine.SetState<AIDamagedState>();
        }
    }
    //private void OnGUI()
    //{
    //    GUI.skin.label.alignment = TextAnchor.MiddleCenter;
    //    Rect rect = new Rect(0, 0, 100, 60);
    //    // transform world position of agent to screen position
    //    Vector3 point = Camera.main.WorldToScreenPoint(transform.position);
    //    rect.x = point.x;// - (rect.width / 2);
    //    rect.y = Screen.height - point.y - rect.height - 40;

    //    // get current state
    //    string str = PushDownStateMachine.GetString();

    //    // set box and label (text)
    //    GUI.backgroundColor = Color.black;
    //    GUI.Box(rect, GUIContent.none);
    //    GUI.Label(rect, str);
    //}
}
