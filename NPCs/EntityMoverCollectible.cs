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
        interactableObject.OnCollected.AddListener(StopMovement);
        interactableObject.OnDropped.AddListener(ResumeMovement);
    }
}
