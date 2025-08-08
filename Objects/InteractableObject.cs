using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public ObjectSO objectSO;
    [SerializeField] protected bool outlineOnLookAt = true;
    public virtual string ObjectName => objectSO.objectName;
    public Rigidbody rb { get; private set; }
    protected Outline outline;
    public bool shouldHaveRigidBody = true;
    public virtual bool ShouldDisplayNameOnMouseOver => objectSO.showNameOnMouseOver;
    protected virtual void Awake()
    {
        outline = gameObject.GetOrAddComponent<Outline>();
    }
    protected virtual void Start()
    {
        if (objectSO.isCollectible && shouldHaveRigidBody) rb = gameObject.GetOrAddComponent<Rigidbody>();
    }
    public virtual void OnInteract(Player player)
    {
        if (objectSO.neededKeyID != 0)
        {
            if (player.currentObject == null) return;
            if (!player.IsCurrentObjectKey(objectSO.neededKeyID)) return;
        }
        Interact(player);
    }
    protected virtual void Interact(Player player)
    {
        if (objectSO.isCollectible)
        {
            TryCollectObject(player);
            return;
        }
    }
    public virtual void OnStopInteract(Player player) { }
    protected virtual void TryCollectObject(Player player)
    {
        if (player.currentObject == null)
        {
            player.GiveObject(this);
            transform.parent = player.RightHand;
            transform.SetLocalPositionAndRotation(objectSO.inHandPosition, Quaternion.Euler(objectSO.inHandRotation));
            rb.isKinematic = true;
        }
    }
    public virtual void OnObjectUsed() { }

    public void OnLookAt(Player player)
    {
        if (outlineOnLookAt) outline.enabled = true;
    }

    public void OnStopLookAt(Player player)
    {
        outline.enabled = false;
    }
    public void SetUnInteractable()
    {
        outline.enabled = false;
        Destroy(this);
    }
}
