using UnityEngine;

public class Openable : InteractableObject
{
    public bool isLocked;
    [SerializeField] protected bool uninteractableAfterOpen = false;
    public override bool ShouldDisplayNameOnMouseOver => isLocked;
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
    public void Unlock()
    {
        isLocked = false;
        Open();
        outline.OutlineColor = Color.aliceBlue;
        if (uninteractableAfterOpen) SetUnInteractable();
    }

    protected override void Interact(Player player)
    {
        if (isLocked) return;
        base.Interact(player);
        Debug.Log(transform.rotation.eulerAngles.y);
        isOpen = !isOpen;
        animator.SetTrigger(isOpen ? "Open" : "Close") ;
    }
    public void Open()
    {
        isOpen = true;
        animator.SetTrigger("Open");   
    }
    protected void Close()
    {
        isOpen = false;
        animator.SetTrigger("Close");
    }
}
