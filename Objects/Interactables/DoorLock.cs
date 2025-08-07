using UnityEngine;
using UnityEngine.Events;

public class DoorLock : InteractableObject
{
    [SerializeField] private Door door;
    [SerializeField] Vector3 keyPosition;
    [SerializeField] Vector3 keyRotation;

    protected override void Awake()
    {
        if (door == null) door = GetComponentInParent<Door>();
        
    }
    protected override void Interact(Player player)
    {
        base.Interact(player);
        InteractableObject key = player.currentObject;
        key.transform.parent = transform;
        key.transform.SetLocalPositionAndRotation(keyPosition, Quaternion.Euler(keyRotation));
        door.Open();
    }
}
