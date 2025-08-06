using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSO", menuName = "SO/Object")]
public class ObjectSO : ScriptableObject
{
    public string objectName;
    public bool showNameOnMouseOver;
    public Vector3 inHandPosition;
    public Vector3 inHandRotation;
}
