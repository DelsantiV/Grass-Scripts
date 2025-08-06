using Grass;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CanvasManager canvasManager;
    public FirstPersonController controller;
    private Camera PlayerCamera { get => controller.playerCamera; }
    private bool isInteracting;
    private LayerMask playerLayer;
    private void Start()
    {
        controller = GetComponent<FirstPersonController>();
        playerLayer = LayerMask.GetMask("Player");
    }

    private void Update()
    {

    }

    private void HandleInteractions()
    {
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out RaycastHit hit, 10, ~playerLayer))
        {
            var selectionTransform = hit.transform;

            if (selectionTransform.TryGetComponent<IInteractable>(out IInteractable currentInteraction))
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
                isInteracting = false;
            }
        }
    }
}
