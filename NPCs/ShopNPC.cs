using DialogueEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ShopNPC : BaseNPC
{

    [SerializeField] private NPCConversation NoCheckOutConversation;

    [SerializeField] private NPCConversation CheckOutConversation;

    [SerializeField] private ShopCheckout checkout;

    Conversation AccessibleCheckOutConversation;

    private bool saidHello;


    protected override void Awake()
    {
        base.Awake();
        AccessibleCheckOutConversation = CheckOutConversation.Deserialize();

    }

    public override void OnInteract(Player player)
    {

        conversation = SetShopConversation();

        base.OnInteract(player);

    }

    public void SaidHello()
    {

        conversation = NoCheckOutConversation;

        saidHello = true;


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

                AccessibleCheckOutConversation.Root.Text = "It will be " + prize.ToString() + " $";


                return CheckOutConversation;
            }

        }

        else
        {
            return conversation;
        }
        

    }


}
