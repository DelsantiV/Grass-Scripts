
public class ShopCheckout : ObjectReceiver
{
    public override bool IsObjectValid(InteractableObject obj)
    {
        if (obj == null) return false;
        return obj.TryGetComponent<ShopObject>(out _);
    }

    public void CheckOut(out int prize)
    {
        prize = 0;
        foreach (ShopObject obj in currentObjects.ConvertAll(obj => obj.iobj.gameObject.GetComponent<ShopObject>())) prize += obj.prize;
    }
}
