using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Openable : InteractableObject
{
    public bool isLocked;
    [SerializeField] protected bool uninteractableAfterOpen = false;
    [SerializeField] protected bool disableColliderWhenOpen = false;
    public override bool ShouldDisplayNameOnMouseOver => isLocked;
    private Animator animator;
    private bool isOpen;
    private ObjectContainer container;
    private bool isContainer;
    private Collider _collider;
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        isContainer = TryGetComponent<ObjectContainer>(out container);
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        TryGetComponent<Collider>(out _collider);
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
        if (isOpen) Open();
        else Close();
    }
    public void Open()
    {
        isOpen = true;
        if (disableColliderWhenOpen && _collider != null) _collider.enabled = false;
        animator.SetTrigger("Open");   
    }
    protected void Close()
    {
        if (disableColliderWhenOpen && _collider != null) _collider.enabled = true;
        isOpen = false;
        animator.SetTrigger("Close");
    }
}
