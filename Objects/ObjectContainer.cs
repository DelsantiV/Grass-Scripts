using UnityEngine;
using System.Collections.Generic;

public class ObjectContainer : MonoBehaviour
{
    [SerializeField] protected List<ContainedObject> initialObjects;
    public int maxNumberOfObjects;
    [SerializeField] protected bool instantiateObjectOnlyOnOpen;
    public List<ContainedObject> containedObjects { get ; private set; }
    public bool isFull { get => containedObjects.Count >= maxNumberOfObjects; }
    public int currentNumberOfObjects { get => containedObjects.Count; }
    protected virtual void Awake()
    {
        containedObjects ??= new List<ContainedObject>();
        if (maxNumberOfObjects == 0) maxNumberOfObjects = containedObjects.Count;
        if (!instantiateObjectOnlyOnOpen)
        {
            SetAllObjects();
        }
    }
    public virtual void Open()
    {
        if (instantiateObjectOnlyOnOpen) SetAllObjects();
    }
    public virtual void SetAllObjects()
    {
        foreach (ContainedObject containedObject in initialObjects) AddContainedObject(containedObject, true);
    }
    public virtual void AddContainedObject(ContainedObject containedObject, bool force = false)
    {
        if (isFull && !force) return;
        containedObjects.Add(containedObject);
        containedObject.iobj.transform.parent = transform;
        if (containedObject.position != Vector3.zero) containedObject.iobj.transform.SetLocalPositionAndRotation(containedObject.position, containedObject.rotation);
        bool addRb = containedObject.iobj.shouldHaveRigidBody;
        containedObject.iobj.OnCollected.AddListener(() => OnContainedObjectCollected(containedObject, addRb));
        if (containedObject.iobj.shouldHaveRigidBody)
        {
            containedObject.iobj.shouldHaveRigidBody = false;
            if (containedObject.iobj.TryGetComponent<Rigidbody>(out Rigidbody rb)) Destroy(rb); 
        }
    }
    private void OnContainedObjectCollected(ContainedObject obj, bool addRigidbody)
    {
        if (addRigidbody) obj.iobj.shouldHaveRigidBody = true;
        containedObjects.Remove(obj);
        obj.iobj.OnCollected.RemoveAllListeners();
    }
}
[System.Serializable]
public struct ContainedObject
{
    public InteractableObject iobj;
    public Vector3 position;
    public Quaternion rotation;
    public ContainedObject(InteractableObject iobj, Vector3 position, Vector3 rotation)
    {
        this.iobj = iobj;
        this.position = position;
        this.rotation = Quaternion.Euler(rotation);
    }
    public ContainedObject(InteractableObject iobj, Vector3 position, Quaternion rotation)
    {
        this.iobj = iobj;
        this.position = position;
        this.rotation = rotation;
    }
}
