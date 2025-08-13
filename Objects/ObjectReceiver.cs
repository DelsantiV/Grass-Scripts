using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjectReceiver : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Vector3> objectsPositions;
    [SerializeField] ObjectContainer container;
    public string ObjectName => string.Empty;

    public bool ShouldDisplayNameOnMouseOver => false;
    private void Awake()
    {
        if (container == null) container = gameObject.GetOrAddComponent<ObjectContainer>();
    }

    public void OnLookAt(Player player)
    {

    }

    public void OnInteract(Player player)
    {
        if (container.isFull) return;
        InteractableObject newObj = player.TakeObject();
        if (newObj != null)
        {
            ContainedObject containedObj = new(newObj, objectsPositions[container.currentNumberOfObjects], Quaternion.identity);
            container.AddContainedObject(containedObj);
        }
    }

    public void OnStopLookAt(Player player)
    {
        
    }

    public void OnStopInteract(Player player)
    {

    }
}
