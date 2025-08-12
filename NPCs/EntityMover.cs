using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class EntityMover : MonoBehaviour
{
    [SerializeField] private DestinationMode destinationMode = DestinationMode.BackAndForth;
    [SerializeField] private float walkingSpeed = 1f;
    [SerializeField] private float runningSpeed = 4f;
    [SerializeField] private bool isRunning;
    [SerializeField] private float minWaitTimeOnDestination = 3f;
    [SerializeField] private float maxWaitTimeOnDestination = 5f;
    [SerializeField] private float destinationRange = 15f;
    [SerializeField] private Vector3 destination;
    [SerializeField] private bool shouldFleePlayer;
    [SerializeField] private float fleeDistance = 10f;
    [SerializeField] private float fleeUpdateTimeInterval = 0.2f;
    [SerializeField] private PlayerDetecter playerDetecter;
    private NavMeshAgent agent;
    private Vector3 origin;
    private bool isOnReturn;
    private bool hasArrived;
    private float currentSpeed;
    private bool isStopped { get => agent.isStopped; set => agent.isStopped = value; }
    private bool isPaused;
    public UnityEvent OnDestinationReached;
    private AnimatorHandler animatorHandler;
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        SetRunning(isRunning);
        animatorHandler = new(GetComponent<Animator>());
        origin = transform.position;
        OnDestinationReached = new();
        InitializeBehaviour();
    }
    protected virtual void Start()
    {
        if (shouldFleePlayer)
        {
            playerDetecter.EnableDetecter(true);
            if (playerDetecter.isPlayerInRange) FleePlayer();
            playerDetecter.OnPlayerDetected.AddListener(OnPlayerDetected);
        }

    }
    private void Update()
    {   
        if (isStopped || isPaused)
        {
            return;
        }
        if (currentSpeed != agent.velocity.magnitude)
        {
            currentSpeed = agent.velocity.magnitude;
            float relativeSpeed = Mathf.Clamp01((currentSpeed - walkingSpeed)/(runningSpeed - walkingSpeed));
            bool isMoving = agent.velocity.magnitude > 0.01f;
            float animatorSpeed = 1f;
            if (isMoving && currentSpeed < walkingSpeed)
            {
                animatorSpeed = currentSpeed / walkingSpeed;
            }
            animatorHandler.SetAnimationSpeed(relativeSpeed, isMoving, animatorSpeed);
        }
    }
    private void LateUpdate()
    {
        if (destinationMode != DestinationMode.None)
        {
            if (agent.remainingDistance < 0.001f)
            {
                if (!hasArrived) OnDestinationReached.Invoke();
                hasArrived = true;
            }
            else
            {
                hasArrived = false;
            }
        }
    }
    public void InitializeBehaviour()
    {
        agent.enabled = true;
        isStopped = false;
        enabled = true;
        
        if (destinationMode == DestinationMode.BackAndForth || destinationMode == DestinationMode.Random) OnDestinationReached.AddListener(WaitAndSetDestination);
        if (destinationMode == DestinationMode.Destination) OnDestinationReached.AddListener(StopMovement);
        WaitAndSetDestination();
    }
    private void SetDestination()
    {
        if (isStopped || isPaused) return;
        switch (destinationMode)
        {
            case DestinationMode.None: return;
            case DestinationMode.Destination: agent.SetDestination(destination); return;
            case DestinationMode.BackAndForth:
            {
                agent.SetDestination(isOnReturn ? origin : destination);
                isOnReturn = !isOnReturn;
                return;
            }
            case DestinationMode.Random:
            {
                if (RandomPoint(transform.position, destinationRange,out destination)) agent.SetDestination(destination);
                return;
            }
        }
    }
    private IEnumerator WaitAndSetDestination(float seconds)
    {
        isPaused = true;
        animatorHandler.SetAnimationSpeed(0f, false);
        yield return new WaitForSeconds(seconds);
        if (isPaused)
        {
            isPaused = false;
            SetDestination();
        }
        
    }
    private void WaitAndSetDestination()
    {
        if (shouldFleePlayer)
        {
            if (playerDetecter.isPlayerInRange)
            {
                FleePlayer();
                return;
            }
        }
        agent.autoBraking = true;
        SetRunning(false);
        if (maxWaitTimeOnDestination == 0f) SetDestination();
        float seconds = Random.Range(minWaitTimeOnDestination, maxWaitTimeOnDestination);
        StartCoroutine(WaitAndSetDestination(seconds));
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, agent.areaMask))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
    public void SetRunning(bool running)
    {
        isRunning = running;
        agent.speed = isRunning ? runningSpeed : walkingSpeed;
    }
    public void StopMovement()
    {
        OnDestinationReached.RemoveAllListeners();
        isStopped = true;
        agent.enabled = false;
        enabled = false;
        playerDetecter.EnableDetecter(false);
        animatorHandler.SetAnimationSpeed(0f, false);
    }
    private void OnPlayerDetected()
    {
        FleePlayer();
    }

    private void FleePlayer()
    {
        SetRunning(true);
        agent.autoBraking = false;
        isPaused = false;
        Vector3 correctedPlayerPos = new Vector3(playerDetecter.player.transform.position.x, transform.position.y, playerDetecter.player.transform.position.z);
        Vector3 fleeDirection = (transform.position - correctedPlayerPos).normalized;
        if (fleeDirection == Vector3.zero) fleeDirection = transform.forward;
        Vector3 fleePoint = fleeDirection * fleeDistance + transform.position;
        if (NavMesh.SamplePosition(fleePoint, out NavMeshHit hit, 5.0f, agent.areaMask))
        {
            agent.SetDestination(hit.position);
            return;
        }
        SetDestination();
        StartCoroutine(WaitAndUpdateFleeDestination(fleeUpdateTimeInterval));
    }
    private IEnumerator WaitAndUpdateFleeDestination(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        if (shouldFleePlayer)
        {
            if (playerDetecter.isPlayerInRange) FleePlayer();
        }
    }
    public enum DestinationMode
    {
        Destination,
        BackAndForth,
        Random,
        None
    }
    private class AnimatorHandler
    {
        private Animator animator;
        public AnimatorHandler(Animator animator) { this.animator = animator; }
        private bool isMoving;

        public void SetAnimationSpeed(float speed, bool moving, float animationSpeed = 1f)
        {
            if (moving != isMoving) 
            { 
                isMoving = moving;
                animator.SetBool("isMoving", isMoving);
            }
            animator.SetFloat("Speed", speed);
            animator.speed = animationSpeed;
        }
    }
}
