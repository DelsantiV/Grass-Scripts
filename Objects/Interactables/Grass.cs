using UnityEngine;

public class Grass : InteractableObject
{
    [SerializeField] GameObject borders;

    protected override void Interact(Player player)
    {
        base.Interact(player);
        player.SetWorldMessage("It's good to feel some grass ! You had even forgotten what the green color was like...");
        Destroy(borders);
        Destroy(gameObject);
    }
}
