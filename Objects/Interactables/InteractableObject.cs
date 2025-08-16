using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour, IInteractable
{
    public ObjectSO objectSO;
    [SerializeField] protected bool outlineOnLookAt = true;
    [SerializeField] private Color outlinedColor = Color.orange;
    public UnityEvent OnCollected;
    public UnityEvent OnDropped;
    public UnityEvent OnInteracted;
    protected new Collider collider;
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
    public int keyID
    {
        get
        {
            if (objectSO == null) return 0;
            return objectSO.keyID;
        }
    }
    protected bool canCollect;
    public virtual bool isCollectible
    {
        get => canCollect;
        set => canCollect = value;
    }
    public bool NeedRefresh { get; set; }
    protected virtual void Awake()
    {
        outline = gameObject.GetOrAddComponent<Outline>();
        if (objectSO != null) canCollect = objectSO.isCollectible;
        outline.OutlineColor = outlinedColor;
        OnCollected = new();
        OnDropped = new();
        OnInteracted = new();
        collider = gameObject.GetComponent<Collider>();
        NeedRefresh = false;
    }
    protected virtual void Start()
    {
        if (isCollectible && shouldHaveRigidBody) rb = gameObject.GetOrAddComponent<Rigidbody>();
    }
    protected virtual void Update()
    {
        if (transform.position.y < -5f)
        {
            transform.position += new Vector3(0, 7, 0);
            rb.linearVelocity = Vector3.zero;
        }
    }
    public virtual void OnInteract(Player player)
    {
        if (neededKeyID != 0)
        {
            if (!player.IsCurrentObjectKey(neededKeyID)) return;
        }
        Interact(player);
    }
    protected virtual void Interact(Player player)
    {
        OnInteracted.Invoke();
        if (isCollectible)
        {
            TryCollectObject(player);
            return;
        }
    }
    public virtual void OnStopInteract(Player player) { }
    public virtual void TryCollectObject(Player player)
    {
        if (player.currentObject == null)
        {
            player.GiveObject(this);
            transform.parent = player.RightHand;
            transform.SetLocalPositionAndRotation(objectSO.inHandPosition, Quaternion.Euler(objectSO.inHandRotation));
            if (shouldHaveRigidBody)
            {
                rb = gameObject.GetOrAddComponent<Rigidbody>();
                rb.isKinematic = true;
            }
            collider.isTrigger = true;
            OnCollected.Invoke();
        }
    }
    public virtual void OnObjectDropped()
    {
        transform.parent = null;
        if (shouldHaveRigidBody) 
        {
            if (rb == null) rb = gameObject.GetOrAddComponent<Rigidbody>();
            rb.isKinematic = false; 
        }
        collider.isTrigger = false;
        gameObject.SetLayerAllChildren(LayerMask.NameToLayer("Default"));
        OnDropped.Invoke();
    }
    public virtual void OnObjectUsed() { }

    public void OnLookAt(Player player)
    {
        if (outlineOnLookAt && outline != null) outline.enabled = true;
        if (ShouldDisplayNameOnMouseOver) player.CanvasManager.SetInteractionText(ObjectName);
    }

    public void OnStopLookAt(Player player)
    {
        if (outlineOnLookAt && outline != null) outline.enabled = false;
        player.CanvasManager.CloseInteractionText();
    }
    public void SetUnInteractable()
    {
        outline.enabled = false;
        enabled = false;
    }
}
