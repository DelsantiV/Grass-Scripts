using UnityEngine;

[CreateAssetMenu(fileName = "NPCSO", menuName = "SO/NPC")]
public class NPCSO : ScriptableObject
{
    public string NPCname;
    public string NPCIntroDialog;
}


[CreateAssetMenu(fileName = "NPCQuestSO", menuName = "SO/NPC")]
public class NPCQuestSO : NPCSO
{
    public BaseQuest quest;
    public string QuestDialog;
    public string QuestAcceptedDialog;
}
