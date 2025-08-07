using UnityEngine;
public interface IInteractable
{
    public string ObjectName {get;}

    public bool ShouldDisplayNameOnMouseOver { get; }

    public void OnInteract(Player player);
    public void OnStopInteract();
}

