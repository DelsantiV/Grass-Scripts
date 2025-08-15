using UnityEngine;

public class Grass : InteractableObject
{
    [SerializeField] GameObject borders;

    protected override void Interact(Player player)
    {
        base.Interact(player);
        Destroy(borders);
        Destroy(gameObject);
    }
}
