using UnityEngine;

public class Money : InteractableObject
{
    [SerializeField] private int moneyAmount;
    public override bool isCollectible => true;
    public override void TryCollectObject(Player player)
    {
        OnCollected.Invoke();
        player.ChangeMoney(moneyAmount);
        Destroy(gameObject);
    }
}
