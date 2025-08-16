using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ShopCheckout : ObjectReceiver
{
    [SerializeField] private Openable barrier;
    public List<ShopObject> boughtObjects => currentObjects.ConvertAll(obj => obj.iobj.gameObject.GetComponent<ShopObject>());
    public List<InteractableObject> boughtInteractableObjects => currentObjects.ConvertAll(obj => obj.iobj.gameObject.GetComponent<InteractableObject>());
    public int prize => boughtObjects.Sum(obj => obj.prize);
    public override bool IsObjectValid(InteractableObject obj)
    {
        if (obj == null) return false;
        return obj.TryGetComponent<ShopObject>(out _);
    }
    public void CheckOut()
    {
        container.DestroyAllObjects();
        barrier.Open();
    }
}
