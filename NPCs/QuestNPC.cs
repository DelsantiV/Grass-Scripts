using System;
using DialogueEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class QuestNPC : BaseNPC
{

    [SerializeField] private BaseQuestSO quest;

    [SerializeField] private NPCConversation questRefusedConversation;

    [SerializeField] private NPCConversation questAcceptedConversation;

    [SerializeField] private NPCConversation questCompletedConversation;


    [SerializeField] private Transform spawnQuestObjectPosition;



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
            foreach (InteractableObject questobj in quest.questObjects)
            {
                Instantiate(questobj.gameObject, quest.questObjectSpawnPosition, quest.questObjectSpawnRotation);
            }
        }


    }



    public void QuestCompleted()
    {

        conversation = questCompletedConversation;

    }

    private bool CheckQuestObjects(Player player)
    {
        foreach(InteractableObject questobj in quest.questObjects)
        {
            if (!player.IsCurrentObjectKey(questobj.objectSO.keyID)) return false;

        }

        return true;

    }

    public void VerifyObject(Player player)
    {

        if (player.currentObject != null)
        {

            if (CheckQuestObjects(player))
            {


                InteractableObject questobj = player.TakeObject();

                questobj.transform.parent = spawnQuestObjectPosition;


                questobj.transform.SetLocalPositionAndRotation(Vector3.zero,Quaternion.identity);

                questobj.isCollectible = false;

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
