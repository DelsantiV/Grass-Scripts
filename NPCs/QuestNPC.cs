using DialogueEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class QuestNPC : BaseNPC
{

    [SerializeField] private BaseQuestSO quest;

    [SerializeField] private NPCConversation questRefusedConversation;

    [SerializeField] private NPCConversation questAcceptedConversation;

    [SerializeField] private NPCConversation questCompletedConversation;



    #region Quest

    public void QuestRefused()
    {

        conversation = questRefusedConversation;

    }



    public void QuestAccepted()
    {


        conversation = questAcceptedConversation;

        if (quest.spawnQuestObject)
        {
            Instantiate(quest.questObject.gameObject, quest.questObjectSpawnPosition, quest.questObjectSpawnRotation);

        }


    }



    public void QuestCompleted()
    {

        conversation = questCompletedConversation;

    }

    public void VerifyObject(Player player)
    {

        if (player.currentObject != null)
        {
            if (player.IsCurrentObjectKey(quest.questObject.objectSO.keyID))
            {


                QuestCompleted();


            }

        }

    }




    #endregion


    #region Interact


    public override void OnInteract(Player player)
    {
        VerifyObject(player);

        base.OnInteract(player);
        


    }

    #endregion




}
