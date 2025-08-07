using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected ObjectSO objectSO;
    public virtual string ObjectName => objectSO.objectName;
    public Rigidbody rb { get; private set; }
    private Outline outline;
    public virtual bool ShouldDisplayNameOnMouseOver => objectSO.showNameOnMouseOver;
    protected virtual void Awake()
    {
        if (objectSO.isCollectible) rb = gameObject.GetOrAddComponent<Rigidbody>();
        outline = gameObject.GetOrAddComponent<Outline>();
    }

    public virtual void OnInteract(Player player)
    {
        if (objectSO.neededKeyID != 0)
        {
            if (player.currentObject == null) return;
            if (player.currentObject.objectSO.keyID != objectSO.neededKeyID) return;
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
    public virtual void OnStopInteract() { }
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
        outline.enabled = true;
    }

    public void OnStopLookAt()
    {
        outline.enabled = false;
    }
}
