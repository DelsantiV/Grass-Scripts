public interface IInteractable
{
    public string ObjectName {get;}
    public bool ShouldDisplayNameOnMouseOver { get; }
    public bool NeedRefresh { get; set; }
    public void OnLookAt(Player player);
    public void OnInteract(Player player);
    public void OnStopLookAt(Player player);
    public void OnStopInteract(Player player);
}

