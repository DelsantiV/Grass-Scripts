using UnityEngine;
using System;

public class QuestNPC : BaseNPC
{
    protected NPCQuestSO NPCQuestSO
    {
        get => (NPCQuestSO) NPCSO;
        set
        {
            if (value is NPCQuestSO) NPCSO = value;
            else throw new Exception("Cannot assign non Quest specific NPCSO");
        }
    }
}
