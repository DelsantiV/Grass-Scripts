using UnityEngine;

public class ObjectContainer : MonoBehaviour
{
    [SerializeField] protected GameObject containedObject;
    [SerializeField] protected Vector3 containedObjectPosition;
    [SerializeField] protected Vector3 containedObjectRotation;

    protected virtual void Awake()
    {
        containedObject.transform.parent = transform;
        containedObject.transform.SetLocalPositionAndRotation(containedObjectPosition, Quaternion.Euler(containedObjectRotation));
        if (containedObject.TryGetComponent<Rigidbody>(out Rigidbody rb)) rb.isKinematic = true;
    }
}
