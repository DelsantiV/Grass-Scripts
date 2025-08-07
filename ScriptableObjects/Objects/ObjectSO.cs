using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSO", menuName = "SO/Object")]
public class ObjectSO : ScriptableObject
{
    public string objectName;
    /// <summary>
    /// ID to open other objects
    /// </summary>
    public int keyID;
    public bool showNameOnMouseOver;
    public bool isCollectible;
    public Vector3 inHandPosition;
    public Vector3 inHandRotation;
    public int neededKeyID;
}

