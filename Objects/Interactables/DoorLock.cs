using UnityEngine;

public class DoorLock : InteractableObject
{
    [SerializeField] private Openable door;
    [SerializeField] Vector3 keyPosition;
    [SerializeField] Vector3 keyRotation;

    protected override void Awake()
    {

        base.Awake();
        if (door == null) door = GetComponentInParent<Openable>();
        door.isLocked = true;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
    }
    protected override void Interact(Player player)
    {
        base.Interact(player);
        PutKeyInLock(player.TakeObject());
        door.Unlock();
        SetUnInteractable();
    }
    protected virtual void PutKeyInLock(InteractableObject key)
    {
        key.transform.parent = transform;
        key.transform.localScale = Vector3.one;
        key.transform.SetLocalPositionAndRotation(keyPosition, Quaternion.Euler(keyRotation));
        key.SetUnInteractable();
    }
}
