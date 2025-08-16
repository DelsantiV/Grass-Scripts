using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseQuestSO", menuName = "SO/Quest")]
public class BaseQuestSO : ScriptableObject
{
    public string questName;
    public string questID;
    public List<InteractableObject>  questObjects;
    public bool spawnQuestObject;
    public Vector3 questObjectSpawnPosition;
    public Quaternion questObjectSpawnRotation;
}
