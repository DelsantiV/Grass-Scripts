using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour, IInteractable
{
    public ObjectSO objectSO;
    [SerializeField] protected bool outlineOnLookAt = true;
    public virtual string ObjectName
    {
        get
        {
            if (objectSO == null) return string.Empty;
            return objectSO.objectName;
        }
    }
    public Rigidbody rb { get; private set; }
    protected Outline outline;
    public bool shouldHaveRigidBody = true;
    public virtual bool ShouldDisplayNameOnMouseOver
    {
        get
        {
            if (objectSO == null) return false;
            return objectSO.showNameOnMouseOver;
        }
    }
    private int neededKeyID
    {
        get
        {
            if (objectSO == null) return 0;
            return objectSO.neededKeyID;
        }
    }
    private bool isCollectible
    {
        get
        {
            if (objectSO == null) return false; 
            return objectSO.isCollectible;
        }
    }
    protected virtual void Awake()
    {
        outline = gameObject.GetOrAddComponent<Outline>();
    }
    protected virtual void Start()
    {
        if (isCollectible && shouldHaveRigidBody) rb = gameObject.GetOrAddComponent<Rigidbody>();
    }
    public virtual void OnInteract(Player player)
    {
        if (neededKeyID != 0)
        {
            if (player.currentObject == null) return;
            if (!player.IsCurrentObjectKey(neededKeyID)) return;
        }
        Interact(player);
    }
    protected virtual void Interact(Player player)
    {
        if (isCollectible)
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
            rb = gameObject.GetOrAddComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }
    public virtual void OnObjectUsed() { }

    public void OnLookAt(Player player)
    {
        if (outlineOnLookAt) outline.enabled = true;
        if (ShouldDisplayNameOnMouseOver) player.CanvasManager.SetInteractionText(ObjectName);
    }

    public void OnStopLookAt(Player player)
    {
        outline.enabled = false;
        player.CanvasManager.CloseInteractionText();
    }
    public void SetUnInteractable()
    {
        outline.enabled = false;
        Destroy(this);
    }
}
