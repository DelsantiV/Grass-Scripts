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
    private void Awake()
    {
        controller = GetComponent<FirstPersonController>();
        notInteractable = LayerMask.GetMask("Ground", "Player");
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
                    currentInteraction.OnStopLookAt();
                    if (isInteracting) currentInteraction.OnStopInteract();
                }
                currentInteraction = newInteraction;
                currentInteraction.OnLookAt(this);
                if (currentInteraction.ShouldDisplayNameOnMouseOver) canvasManager.SetInteractionText(currentInteraction.ObjectName);
                return;
            }
        }
        if (isLooking)
        {
            if (isInteracting)
            {
                currentInteraction?.OnStopInteract();
                isInteracting = false;
            }
            canvasManager.CloseInteractionText();
            canvasManager.CloseInteractionText();
            isLooking = false;
            currentInteraction?.OnStopLookAt();
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
            currentInteraction?.OnStopInteract();
            currentInteraction = null;
        }
    }
    private void DropCurrentObject()
    {
        if (currentObject != null)
        {
            currentObject.transform.parent = null;
            currentObject.rb.isKinematic = false;
            currentObject.gameObject.SetLayerAllChildren(LayerMask.NameToLayer("Default"));
            currentObject = null;
        }
    }
    public InteractableObject TakeObject()
    {
        InteractableObject obj = currentObject;
        currentObject.gameObject.SetLayerAllChildren(LayerMask.NameToLayer("Default"));
        currentObject = null;
        return obj;
    }
    public void GiveObject(InteractableObject obj)
    {
        currentObject = obj;
        currentObject.gameObject.SetLayerAllChildren(gameObject.layer);
    }
}
