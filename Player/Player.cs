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
    public InteractableObject currentObject;
    private void Start()
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
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out RaycastHit hit, 10, ~notInteractable))
        {
            var selectionTransform = hit.transform;

            if (selectionTransform.TryGetComponent<IInteractable>(out currentInteraction))
            {
                if (currentInteraction.ShouldDisplayNameOnMouseOver) canvasManager.SetInteractionText(currentInteraction.ObjectName);
                isInteracting = true;

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
                if (currentInteraction != null) currentInteraction.OnStopInteract();
                isInteracting = false;
            }
        }
    }
    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.P)) DropCurrentObject();
    }
    private void DropCurrentObject()
    {
        if (currentObject != null)
        {
            currentObject.transform.parent = null;
            currentObject.AddComponent<Rigidbody>();
            currentObject = null;
        }
    }
}
