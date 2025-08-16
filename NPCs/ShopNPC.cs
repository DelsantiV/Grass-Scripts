using DialogueEditor;
using UnityEngine;

public class ShopNPC : BaseNPC
{

    [SerializeField] private NPCConversation NoCheckOutConversation;

    [SerializeField] private NPCConversation CheckOutConversation;

    [SerializeField] private ShopCheckout checkout;

    [SerializeField] private BagObject bagPrefab;


    [SerializeField] private Transform bagSpawnPosition;

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

        ConversationManager.Instance.SetBool("hasEnoughMoney", shopPlayer.money >= checkout.prize);
    }

    public void ProceedCheckout()
    {
        BagObject bag = Instantiate(bagPrefab, bagSpawnPosition.position, transform.rotation);
        bag.keyIDs = checkout.boughtInteractableObjects.ConvertAll(obj => obj.keyID);
        shopPlayer.ChangeMoney(-checkout.prize);
        checkout.CheckOut();
        saidHello = false;
        conversation = secondMeetingConversation;
    }

}




