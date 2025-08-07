public interface IInteractable
{
    public string ObjectName {get;}

    public bool ShouldDisplayNameOnMouseOver { get; }
    public void OnLookAt(Player player);
    public void OnInteract(Player player);
    public void OnStopLookAt();
    public void OnStopInteract();
}

