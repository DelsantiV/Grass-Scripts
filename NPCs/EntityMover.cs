using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class EntityMover : MonoBehaviour
{
    [SerializeField] private DestinationMode destinationMode;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private bool isRunning;
    [SerializeField] private float minWaitTimeOnDestination;
    [SerializeField] private float maxWaitTimeOnDestination;
    [SerializeField] private float destinationRange;
    [SerializeField] private Vector3 destination;
    private NavMeshAgent agent;
    private Vector3 origin;
    private bool isOnReturn;
    private float currentSpeed;
    private bool isStopped;
    public UnityEvent OnDestinationReached;
    private AnimatorHandler animatorHandler;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = isRunning ? runningSpeed : walkingSpeed;
        animatorHandler = new(GetComponent<Animator>());
        origin = transform.position;
        OnDestinationReached = new();
        if (destinationMode == DestinationMode.BackAndForth || destinationMode == DestinationMode.Random) OnDestinationReached.AddListener(WaitAndSetDestination);
        if (destinationMode == DestinationMode.Destination) OnDestinationReached.AddListener(StopMovement);
    }
    private void Start()
    {
        SetDestination();
    }
    private void Update()
    {   if (isStopped) return;     
        if (destinationMode != DestinationMode.None)
        {
            if (agent.remainingDistance < 0.001f)
            {
                OnDestinationReached.Invoke();
            }
        }
        if (currentSpeed != agent.velocity.magnitude)
        {
            currentSpeed = agent.velocity.magnitude;
            float relativeSpeed = Mathf.Clamp01((currentSpeed - walkingSpeed)/(runningSpeed - walkingSpeed) + 0.05f);
            animatorHandler.SetAnimationSpeed(relativeSpeed);
        }
    }
    private void SetDestination()
    {
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
        isStopped = true;
        animatorHandler.SetAnimationSpeed(0f);
        yield return new WaitForSeconds(seconds);
        isStopped = false;
        SetDestination();
    }
    private void WaitAndSetDestination()
    {
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
        agent.isStopped = true;
        agent.enabled = false;
        isStopped = true;
        animatorHandler.SetAnimationSpeed(0f);
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

        public void SetAnimationSpeed(float speed)
        {
            animator.SetFloat("Speed", speed);
        }
    }
}
