using UnityEngine;

public class ObjectContainer : MonoBehaviour
{
    [SerializeField] protected GameObject containedObject;
    [SerializeField] protected Vector3 containedObjectPosition;
    [SerializeField] protected Vector3 containedObjectRotation;
    [SerializeField] protected bool instantiateObjectOnlyOnOpen;
    protected virtual void Awake()
    {
        if (!instantiateObjectOnlyOnOpen)
        {
            SetContainedObject();
        }
    }
    public virtual void Open()
    {
        if (instantiateObjectOnlyOnOpen) SetContainedObject();
    }
    protected virtual void SetContainedObject()
    {
        containedObject.transform.parent = transform;
        containedObject.transform.SetLocalPositionAndRotation(containedObjectPosition, Quaternion.Euler(containedObjectRotation));
        if (containedObject.TryGetComponent<InteractableObject>(out InteractableObject iobj))
        {
            if (iobj.shouldHaveRigidBody)
            {
                iobj.OnCollected.AddListener(() => iobj.shouldHaveRigidBody = true);
                iobj.shouldHaveRigidBody = false;
            }
        }
    }
}
