using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected ObjectSO objectSO;
    public virtual string ObjectName => objectSO.objectName;

    public virtual bool ShouldDisplayNameOnMouseOver => objectSO.showNameOnMouseOver;

    public virtual void OnInteract(Player player)
    {
        Debug.Log("Interact");
    }
}
