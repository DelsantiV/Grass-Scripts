using UnityEngine;

public class Grass : InteractableObject
{
    [SerializeField] GameObject borders;

    protected override void Interact(Player player)
    {
        base.Interact(player);
        AudioManager.Instance.GoToNextMusic();
        player.SetWorldMessage("It's good to feel some grass ! You had even forgotten what the green color was like... Your soul was lost in the virtual world. Touching grass makes you want to help people around");
        Destroy(borders);
        Destroy(gameObject);
    }
}
