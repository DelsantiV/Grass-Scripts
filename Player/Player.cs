using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private Transform rightHand;
    public Transform RightHand { get { return rightHand; } }
    public FirstPersonController controller;
    private Camera PlayerCamera { get => controller.playerCamera; }
    private bool isLooking;
    private bool isInteracting;
    private IInteractable currentInteraction;
    private LayerMask notInteractable;
    public InteractableObject currentObject { get; private set; }
    public CanvasManager CanvasManager { get; private set; }
    private void Awake()
    {
        controller = GetComponent<FirstPersonController>();
        notInteractable = LayerMask.GetMask("Ground", "Player");
        CanvasManager = FindFirstObjectByType<CanvasManager>();
    }

    private void Update()
    {
        HandleInteractions();
        HandleInputs();
    }

    private void HandleInteractions()
    {
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out RaycastHit hit, 4, ~notInteractable))
        {
            var selectionTransform = hit.transform;

            if (selectionTransform.TryGetComponent<IInteractable>(out IInteractable newInteraction))
            {
                isLooking = true;
                if (newInteraction == currentInteraction) return;
                // new interaction is different from current
                if (currentInteraction != null)
                {
                    currentInteraction.OnStopLookAt(this);
                    if (isInteracting) currentInteraction.OnStopInteract(this);
                }
                currentInteraction = newInteraction;
                currentInteraction.OnLookAt(this);
                SetReticle(currentInteraction);
                return;
            }
        }
        if (isLooking)
        {
            if (isInteracting)
            {
                currentInteraction?.OnStopInteract(this);
                isInteracting = false;
            }
            canvasManager.SetReticle(CanvasManager.ReticleType.Base);
            canvasManager.CloseInteractionText();
            canvasManager.CloseInteractionText();
            isLooking = false;
            currentInteraction?.OnStopLookAt(this);
            currentInteraction = null;
        }
        
    }
    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.P)) DropCurrentObject();
        if (currentObject != null && Input.GetMouseButtonDown(0)) currentObject.OnObjectUsed();
        if (Input.GetKeyDown(KeyCode.E) && isLooking)
        {
            isInteracting = true;
            currentInteraction?.OnInteract(this);
        }
        if (Input.GetKeyUp(KeyCode.E) && isInteracting)
        {
            isInteracting = false;
            currentInteraction?.OnStopInteract(this);
            currentInteraction = null;
        }
    }
    private void DropCurrentObject()
    {
        if (currentObject != null)
        {
            currentObject.OnObjectDropped();
            currentObject = null;
        }
    }
    /// <summary>
    /// Takes the current object from the player hand
    /// </summary>
    /// <returns></returns>
    public InteractableObject TakeObject()
    {
        if (currentObject == null) return null;
        InteractableObject obj = currentObject;
        currentObject.gameObject.SetLayerAllChildren(LayerMask.NameToLayer("Default"));
        currentObject = null;
        return obj;
    }
    public bool IsCurrentObjectKey(int keyID)
    {
        if (currentObject == null) return false;
        if (currentObject.objectSO.keyID == keyID) return true;
        return false;
    }
    /// <summary>
    /// Give the object to the player.
    /// </summary>
    /// <param name="obj"></param>
    public void GiveObject(InteractableObject obj)
    {
        currentObject = obj;
        currentObject.gameObject.SetLayerAllChildren(gameObject.layer);
    }
    public void SetCursorLockMode(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        controller.cameraCanMove = locked;
    }
    private void SetReticle(IInteractable interactable)
    {
        if (interactable != null)
        {
            if (interactable is BaseNPC) 
            { 
                canvasManager.SetReticle(CanvasManager.ReticleType.Talk);
                return;
            }
            if (interactable is InteractableObject)
            {
                if ((interactable as InteractableObject).isCollectible)
                {
                    canvasManager.SetReticle(CanvasManager.ReticleType.Grab);
                    return;
                }
            }
            if (interactable is WashableDecal)
            {
                canvasManager.SetReticle(CanvasManager.ReticleType.Wash);
                return;
            }
        }
        canvasManager.SetReticle(CanvasManager.ReticleType.Base);
    }
}
