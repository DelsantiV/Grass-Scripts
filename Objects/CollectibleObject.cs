using UnityEngine;

public class CollectibleObject : InteractableObject
{
    public override bool ShouldDisplayNameOnMouseOver { get => objectSO.showNameOnMouseOver; }
    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnInteract(Player player)
    {
        player.CollectObject(this);
    }
    public virtual void SetToHand(Transform hand)
    {
        transform.parent = hand;
        transform.SetLocalPositionAndRotation(objectSO.inHandPosition, Quaternion.Euler(objectSO.inHandRotation));
        Destroy(rb);
    }
}
