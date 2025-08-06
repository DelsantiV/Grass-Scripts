using UnityEngine;

public class CollectibleObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ObjectSO objectSO;
    public virtual string ObjectName { get => objectSO.objectName; }

    public bool ShouldDisplayNameOnMouseOver { get => objectSO.showNameOnMouseOver; }

    public virtual void OnInteract(Player player)
    {
        player.CollectObject(this);
    }
    public virtual void SetToHand(Transform hand)
    {
        transform.parent = hand;
        transform.position = objectSO.inHandPosition;
        transform.rotation = Quaternion.Euler(objectSO.inHandRotation);
    }
}
