using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
public class EntityMoverCollectible : EntityMover
{
    
    private InteractableObject interactableObject;
    protected override void Awake()
    {
        base.Awake();
        interactableObject = GetComponent<InteractableObject>();
    }
    protected override void Start()
    {
        base.Start();
        interactableObject.OnCollected.AddListener(OnCollected);
        interactableObject.OnDropped.AddListener(ResumeMovement);
    }
    private void OnCollected()
    {
        StopMovement();
        AudioManager.Instance.StartGameMusic();
    }
    protected override void OnPlayerDetected()
    {
        base.OnPlayerDetected();
        AudioManager.Instance.SwitchToQuest("Pursuit", time: 60);
    }
}
