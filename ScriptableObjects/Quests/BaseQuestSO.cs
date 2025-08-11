using UnityEngine;

[CreateAssetMenu(fileName = "BaseQuestSO", menuName = "SO/Quest")]
public class BaseQuestSO : ScriptableObject
{
    public string questName;
    public string questID;
    public InteractableObject questObject;
    public bool spawnQuestObject;
    public Vector3 questObjectSpawnPosition;
    public Quaternion questObjectSpawnRotation;
}
