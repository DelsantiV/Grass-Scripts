using System.Collections.Generic;
using UnityEngine;

public class BagObject : InteractableObject
{

    public List<int> keyIDs;

    public override void OnInteract(Player player)
    {
        if (player.currentObject != null) 
        { 
            if (player.currentObject is BagObject bag)
            {
                keyIDs.AddRange(bag.keyIDs);
                Destroy(bag.gameObject);
            }
        }
        return;
    }
}
