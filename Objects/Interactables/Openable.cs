using UnityEngine;

public class Openable : InteractableObject
{
    public bool isLocked;
    [SerializeField] protected bool uninteractableAfterOpen = false;
    private Animator animator;
    private bool isOpen;
    protected override void Awake()
    {
        base.Awake(); 
        animator = GetComponent<Animator>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
    }
    protected void Start()
    {
        if (isLocked) outline.OutlineColor = Color.mediumVioletRed;
    }

    protected override void Interact(Player player)
    {
        if (isLocked) return;
        base.Interact(player);
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
    }
    public void Open()
    {
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
        if (uninteractableAfterOpen) SetUnInteractable();        
    }
    protected void Close()
    {
        isOpen = false;
        animator.SetBool("isOpen", isOpen );
    }
}
