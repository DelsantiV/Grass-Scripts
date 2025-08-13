using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Openable : InteractableObject
{
    public bool isLocked;
    [SerializeField] protected bool uninteractableAfterOpen = false;
    [SerializeField] protected bool disableColliderWhenOpen = false;
    [SerializeField] protected bool autoClose = false;
    [SerializeField] protected float autoCloseTime = 2f;
    [SerializeField] Color outlineColor = Color.aliceBlue;
    [SerializeField] Color lockedOutlineColor = Color.mediumVioletRed;
    public override bool ShouldDisplayNameOnMouseOver => isLocked;
    private Animator animator;
    private bool isOpen;
    private ObjectContainer container;
    private bool isContainer;
    private Collider _collider;
    protected override void Awake()
    {
        base.Awake();
        isOpen = false;
        animator = GetComponent<Animator>();
        isContainer = TryGetComponent<ObjectContainer>(out container);
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        TryGetComponent<Collider>(out _collider);
    }
    protected override void Start()
    {
        base.Start();
        if (isLocked) outline.OutlineColor = lockedOutlineColor;
        else outline.OutlineColor = outlineColor;
    }
    public void Unlock(bool openOnUnlock = true)
    {
        isLocked = false;
        if (openOnUnlock) Open();
        if (isContainer) container.Open();
        outline.OutlineColor = outlineColor;
    }
    public void Lock(bool closeOnLock = true)
    {
        isLocked = true;
        if (closeOnLock) Close();
        outline.OutlineColor = lockedOutlineColor;
    }

    protected override void Interact(Player player)
    {
        if (isLocked) return;
        base.Interact(player);
        if (!isOpen) Open();
        else Close();
    }
    public void Open()
    {
        if (isOpen) return;
        if (uninteractableAfterOpen) SetUnInteractable();
        isOpen = true;
        if (disableColliderWhenOpen && _collider != null) _collider.isTrigger = true;
        animator.SetTrigger("Open");   
        if (autoClose)
        {
            StartCoroutine(CloseInSeconds(autoCloseTime));
        }
    }
    public void Close()
    {
        if (!isOpen) return;
        if (disableColliderWhenOpen && _collider != null) _collider.isTrigger = false;
        isOpen = false;
        animator.SetTrigger("Close");
    }
    protected IEnumerator CloseInSeconds(float seconds)
    {
        isLocked = true;
        yield return new WaitForSeconds(seconds);
        Close();
        isLocked = false;
    }
}
