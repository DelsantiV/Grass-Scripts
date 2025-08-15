
using System.Collections.Generic;

public class ShopCheckout : ObjectReceiver
{
    public List<ShopObject> boughtObjects => currentObjects.ConvertAll(obj => obj.iobj.gameObject.GetComponent<ShopObject>());
    public override bool IsObjectValid(InteractableObject obj)
    {
        if (obj == null) return false;
        return obj.TryGetComponent<ShopObject>(out _);
    }

    public List<ShopObject> CheckOut(out int prize)
    {
        prize = 0;
        foreach (ShopObject obj in boughtObjects) prize += obj.prize;
        return boughtObjects;
    }
}
