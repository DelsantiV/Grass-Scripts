using System.Linq;
using DialogueEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class ShopNPC : BaseNPC
{

    [SerializeField] private NPCConversation NoCheckOutConversation;

    [SerializeField] private NPCConversation CheckOutConversation;

    [SerializeField] private ShopCheckout checkout;

    [SerializeField] private BagObject bagPrefab;

    private bool saidHello;

    private Player shopPlayer;

   

    public override void OnInteract(Player player)
    {

        shopPlayer = player;

        conversation = SetShopConversation();

        base.OnInteract(player);

    }

    public override void OnStopLookAt(Player player)
    {
        CloseOptionalText();

        base.OnStopLookAt(player);
    }

    public void SaidHello()
    {

        conversation = NoCheckOutConversation;

        saidHello = true;


    }

    public void CloseOptionalText()
    {

        canvasManager.CloseOptionalText();

    }

    public NPCConversation SetShopConversation()
    {


        if (saidHello)
        {

            if (checkout.prize == 0)
            {

                return NoCheckOutConversation;
            }
            else
            {

                canvasManager.SetOptionalText("That will be " + checkout.prize.ToString() + " $");

                return CheckOutConversation;
            }

        }

        else
        {

            return conversation;
        }


    }
    public void HasEnoughMoney()
    {


        if (shopPlayer.money >= checkout.prize)
        {
            ConversationManager.Instance.SetBool("hasEnoughMoney", true);

            BagObject bag = Instantiate(bagPrefab, transform.position, transform.rotation);


            bag.keyIDs = checkout.boughtInteractableObjects.ConvertAll(obj => obj.keyID);


            shopPlayer.ChangeMoney(-checkout.prize);


        }

        else
        {
            ConversationManager.Instance.SetBool("hasEnoughMoney", false);
        }



    }

}




