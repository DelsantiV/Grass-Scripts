using NUnit.Framework;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MusicSO", menuName = "SO/Music")]
public class MusicSO : ScriptableObject
{
    public AudioClip[] baseMusics;
    public QuestMusic[] questMusics;
}
[System.Serializable]
public class QuestMusic 
{ 
    public string title;
    public AudioClip audioClip;
}

