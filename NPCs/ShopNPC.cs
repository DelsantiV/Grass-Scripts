using DialogueEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class ShopNPC : BaseNPC
{

    [SerializeField] private NPCConversation NoCheckOutConversation;

    [SerializeField] private NPCConversation CheckOutConversation;

    [SerializeField] private ShopCheckout checkout;

    private bool saidHello;

    private int playerMoney;

   

    public override void OnInteract(Player player)
    {

        conversation = SetShopConversation();

        playerMoney = player.money;

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
        checkout.CheckOut(out int prize);

        if (saidHello)
        {

            if (prize == 0)
            {

                return NoCheckOutConversation;
            }
            else
            {

                canvasManager.SetOptionalText("That will be " + prize.ToString() + " $");

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
        checkout.CheckOut(out int prize);

        if (playerMoney >= prize)
        {
            ConversationManager.Instance.SetBool("hasEnoughMoney", true);


            foreach (ContainedObject obj  in checkout.currentObjects)
            {


            }



        }

        else
        {
            ConversationManager.Instance.SetBool("hasEnoughMoney", false);
        }



    }

}




