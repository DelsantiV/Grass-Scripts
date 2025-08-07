using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private Transform rightHand;
    public Transform RightHand { get { return rightHand; } }
    public FirstPersonController controller;
    private Camera PlayerCamera { get => controller.playerCamera; }
    private bool isInteracting;
    private IInteractable currentInteraction;
    private LayerMask notInteractable;
    public InteractableObject currentObject { get; private set; }
    private void Awake()
    {
        controller = GetComponent<FirstPersonController>();
        notInteractable = LayerMask.GetMask("Player", "Ground");
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
                isInteracting = true;
                if (currentInteraction == null) currentInteraction = newInteraction;
                else if (newInteraction != currentInteraction)
                {
                    currentInteraction.OnStopInteract();
                    currentInteraction = newInteraction;
                }
                if (currentInteraction.ShouldDisplayNameOnMouseOver) canvasManager.SetInteractionText(currentInteraction.ObjectName);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentInteraction.OnInteract(this);
                }
            }
        }
        else
        {
            if (isInteracting)
            {
                canvasManager.CloseInteractionText();
                currentInteraction?.OnStopInteract();
                isInteracting = false;
            }
        }
    }
    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.P)) DropCurrentObject();
        if (currentObject != null && Input.GetMouseButtonDown(0)) currentObject.OnObjectUsed(); 
    }
    private void DropCurrentObject()
    {
        if (currentObject != null)
        {
            currentObject.transform.parent = null;
            currentObject.rb.isKinematic = false;
            currentObject = null;
        }
    }
    public InteractableObject TakeObject()
    {
        InteractableObject obj = currentObject;
        currentObject = null;
        return obj;
    }
    public void GiveObject(InteractableObject obj)
    {
        currentObject = obj;
    }
}
