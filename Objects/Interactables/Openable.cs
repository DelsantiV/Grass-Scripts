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
    [SerializeField] Color unlockOutlineColor = Color.aliceBlue;
    [SerializeField] Color lockedOutlineColor = Color.mediumVioletRed;
    [SerializeField] AudioClip openAudio;
    [SerializeField] AudioClip closeAudio;
    private AudioSource audioSource;
    public override bool ShouldDisplayNameOnMouseOver => isLocked;
    private Animator animator;
    private bool isOpen;
    public bool canInteract { get; private set; }
    private ObjectContainer container;
    private bool isContainer;
    protected override void Awake()
    {
        base.Awake();
        isOpen = false;
        animator = GetComponent<Animator>();
        audioSource = gameObject.GetOrAddComponent<AudioSource>();
        isContainer = TryGetComponent<ObjectContainer>(out container);
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        canInteract = true;
    }
    protected override void Start()
    {
        base.Start();
        if (isLocked) outline.OutlineColor = lockedOutlineColor;
        else outline.OutlineColor = unlockOutlineColor;
    }
    public void Unlock(bool openOnUnlock = true)
    {
        isLocked = false;
        if (openOnUnlock) Open();
        if (isContainer) container.Open();
        outline.OutlineColor = unlockOutlineColor;
    }
    public void Lock(bool closeOnLock = true)
    {
        isLocked = true;
        if (closeOnLock) Close();
        outline.OutlineColor = lockedOutlineColor;
    }

    protected override void Interact(Player player)
    {
        if (isLocked || !canInteract) return;
        base.Interact(player);
        if (!isOpen) Open();
        else Close();
    }
    public void Open()
    {
        if (isOpen) return;
        audioSource.PlayOneShot(openAudio);
        if (uninteractableAfterOpen) SetUnInteractable();
        isOpen = true;
        if (disableColliderWhenOpen) collider.isTrigger = true;
        animator.SetTrigger("Open");   
        if (autoClose)
        {
            canInteract = false;
            NeedRefresh = true;
            StartCoroutine(CloseInSeconds(autoCloseTime));
        }
    }
    public void Close()
    {
        if (!isOpen) return;
        audioSource.PlayOneShot(closeAudio);
        if (disableColliderWhenOpen) collider.isTrigger = false;
        isOpen = false;
        animator.SetTrigger("Close");
    }
    protected IEnumerator CloseInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Close();
        canInteract = true;
    }
}
