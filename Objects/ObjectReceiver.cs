using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjectReceiver : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Vector3> objectsPositions;
    [SerializeField] ObjectContainer container;
    private List<Vector3> freePositions;
    public string ObjectName => string.Empty;

    public bool ShouldDisplayNameOnMouseOver => false;
    private void Awake()
    {
        freePositions = objectsPositions;
        if (container == null) container = gameObject.GetOrAddComponent<ObjectContainer>();
    }
    private void Start()
    {
        foreach (ContainedObject obj in container.containedObjects)
        {
            if (objectsPositions.Contains(obj.position)) objectsPositions.Remove(obj.position);
            obj.iobj.OnCollected.AddListener(() => freePositions.Add(obj.position));
        }
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
            Vector3 position = Vector3.zero;
            if (freePositions.Count > 0) 
            {
                position = freePositions[0];
                freePositions.RemoveAt(0);
                newObj.OnCollected.AddListener(() => freePositions.Add(position));
            }
            ContainedObject containedObj = new(newObj, position, Quaternion.identity);
            container.AddContainedObject(containedObj);
            objectsPositions.Add(containedObj.position);
        }
    }

    public void OnStopLookAt(Player player)
    {
        
    }

    public void OnStopInteract(Player player)
    {

    }
}
