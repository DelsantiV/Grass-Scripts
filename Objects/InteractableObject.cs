using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected ObjectSO objectSO;
    public virtual string ObjectName => objectSO.objectName;
    private Rigidbody rb;
    public virtual bool ShouldDisplayNameOnMouseOver => objectSO.showNameOnMouseOver;
    protected virtual void Awake()
    {
        if (objectSO.isCollectible) rb = gameObject.GetOrAddComponent<Rigidbody>();
    }

    public virtual void OnInteract(Player player)
    {
        if (objectSO.keyObject != null)
        {
            if (player.currentObject == null) return;
            if (player.currentObject.objectSO != objectSO) return;
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
            transform.parent = player.RightHand;
            transform.SetLocalPositionAndRotation(objectSO.inHandPosition, Quaternion.Euler(objectSO.inHandRotation));
            Destroy(rb);
        }
    }
}
