using UnityEngine;

public class Openable : InteractableObject
{
    public bool isLocked;
    [SerializeField] protected bool uninteractableAfterOpen = false;
    public override bool ShouldDisplayNameOnMouseOver => isLocked;
    private Animator animator;
    private bool isOpen;
    private ObjectContainer container;
    private bool isContainer;
    protected override void Awake()
    {
        base.Awake(); 
        animator = GetComponent<Animator>();
        isContainer = TryGetComponent<ObjectContainer>(out container);
        outline.OutlineMode = Outline.Mode.OutlineVisible;
    }
    protected override void Start()
    {
        base.Start();
        if (isLocked) outline.OutlineColor = Color.mediumVioletRed;
        else outline.OutlineColor = Color.aliceBlue;
    }
    public void Unlock()
    {
        isLocked = false;
        Open();
        if (isContainer) container.Open();
        outline.OutlineColor = Color.aliceBlue;
        if (uninteractableAfterOpen) SetUnInteractable();
    }

    protected override void Interact(Player player)
    {
        if (isLocked) return;
        base.Interact(player);
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
