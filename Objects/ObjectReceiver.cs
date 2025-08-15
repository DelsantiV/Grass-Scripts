using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjectReceiver : MonoBehaviour, IInteractable
{
    [SerializeField] protected bool outlineOnLookAt = true;
    [SerializeField] private Color outlinedColor = Color.yellowGreen;
    [SerializeField] private List<Vector3> objectsPositions;
    [SerializeField] protected ObjectContainer container;
    private List<Vector3> freePositions;
    public List<ContainedObject> currentObjects => container.containedObjects;
    public string ObjectName => string.Empty;
    private Outline outline;
    public bool ShouldDisplayNameOnMouseOver => false;

    public bool NeedRefresh { get; set; }

    protected virtual void Awake()
    {
        if (outlineOnLookAt) outline = gameObject.GetOrAddComponent<Outline>();
        outline.OutlineColor = outlinedColor;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.enabled = false;
        freePositions = objectsPositions;
        if (container == null) container = gameObject.GetOrAddComponent<ObjectContainer>();
        NeedRefresh = false;
    }
    protected virtual void Start()
    {
        foreach (ContainedObject obj in container.containedObjects)
        {
            if (freePositions.Contains(obj.position)) freePositions.Remove(obj.position);
            obj.iobj.OnCollected.AddListener(() => freePositions.Add(obj.position));
        }
    }

    public void OnLookAt(Player player)
    {
        if (outlineOnLookAt) outline.enabled = true;
    }

    public virtual void OnInteract(Player player)
    {
        if (container.isFull) return;
        InteractableObject newObj = player.TakeObject();
        if (IsObjectValid(newObj))
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
        }
    }
    public virtual bool IsObjectValid(InteractableObject obj)
    {
        if (obj == null) return false;
        return true;
    }

    public void OnStopLookAt(Player player)
    {
        if (outlineOnLookAt) outline.enabled = false;
    }

    public void OnStopInteract(Player player)
    {

    }
}
