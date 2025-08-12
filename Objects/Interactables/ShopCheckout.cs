using System.Collections.Generic;
using UnityEngine;

public class ShopCheckout : MonoBehaviour
{
    public List<InteractableObject> boughtObjects;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StoreObject") && other.TryGetComponent<InteractableObject>(out InteractableObject iobj))
        {
            boughtObjects.Add(iobj);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("StoreObject") && other.TryGetComponent<InteractableObject>(out InteractableObject iobj))
        {
            if (boughtObjects.Contains(iobj)) boughtObjects.Remove(iobj);
        }
    }
}
