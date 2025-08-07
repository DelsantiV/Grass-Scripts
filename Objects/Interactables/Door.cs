using UnityEngine;

public class Door : InteractableObject
{
    private Animator animator;
    private bool isOpen;
    protected override void Awake()
    {
        base.Awake(); 
        animator = GetComponent<Animator>();
    }
    protected override void Interact(Player player)
    {
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
    }
}
