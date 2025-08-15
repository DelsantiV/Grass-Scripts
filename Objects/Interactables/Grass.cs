using UnityEngine;

public class Grass : InteractableObject
{
    [SerializeField] GameObject borders;

    protected override void Interact(Player player)
    {
        base.Interact(player);
        CanvasManager.Instance.CloseWorldMessage();
        Destroy(borders);
        Destroy(gameObject);
    }
}
