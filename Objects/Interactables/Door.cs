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
    public void Open()
    {
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
        SetUnInteractable();        
    }
}
