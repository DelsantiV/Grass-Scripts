using System.Collections.Generic;
using UnityEngine;

public class ShopCheckout : MonoBehaviour
{
    public List<ShopObject> boughtObjects;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ShopObject>(out ShopObject obj))
        {
            boughtObjects.Add(obj);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("StoreObject") && other.TryGetComponent<ShopObject>(out ShopObject obj))
        {
            if (boughtObjects.Contains(obj)) boughtObjects.Remove(obj);
        }
    }

    public void CheckOut(out int prize)
    {
        prize = 0;
        foreach (ShopObject obj in boughtObjects) prize += obj.prize;
    }
}
