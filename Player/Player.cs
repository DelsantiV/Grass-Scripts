using System.Collections;
using System.Collections.Generic;
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

    public int money;
    private IInteractable currentInteraction;
    private LayerMask notInteractable;
    public InteractableObject currentObject { get; private set; }
    public CanvasManager CanvasManager { get => canvasManager; }
    private void Awake()
    {
        controller = GetComponent<FirstPersonController>();
        controller.cameraCanMove = false;
        controller.playerCanMove = false;
        notInteractable = LayerMask.GetMask("Ground", "Player");
    }
    private void Start()
    {
        StartCoroutine(InitializePlayer());
    }
    private void Update()
    {
        HandleInteractions();
        HandleInputs();
    }
    private IEnumerator InitializePlayer()
    {
        yield return new WaitForSeconds(1);
        canvasManager.InitializeCanvas();
        Vector3 startPos = PlayerCamera.transform.localPosition;
        float startTime = Time.time;
        float lerpSpeed = 0.2f;
        float baseFov = PlayerCamera.fieldOfView;
        float targetFov = controller.fov;
        while(PlayerCamera.transform.localPosition.magnitude > 0.01f)
        {
            float distanceCovered = (Time.time - startTime) * lerpSpeed;
            PlayerCamera.transform.localPosition = Vector3.Lerp(startPos, Vector3.zero, distanceCovered / startPos.magnitude);
            PlayerCamera.fieldOfView = Mathf.Lerp(baseFov, targetFov, distanceCovered /startPos.magnitude);
            yield return null;
        }
        PlayerCamera.transform.localPosition = Vector3.zero;
        PlayerCamera.fieldOfView = targetFov;
        controller.cameraCanMove = true;
        controller.playerCanMove = true;
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
            if (interactable is ObjectReceiver)
            {
                canvasManager.SetReticle(CanvasManager.ReticleType.Place);
                return;
            }
        }
        canvasManager.SetReticle(CanvasManager.ReticleType.Base);
    }
}
