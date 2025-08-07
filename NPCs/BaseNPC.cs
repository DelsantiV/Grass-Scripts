using UnityEngine;
using UnityEngine.Animations.Rigging;


public class BaseNPC : MonoBehaviour, IInteractable
{ 
    [SerializeField] protected NPCSO NPCSO;
    [SerializeField] private Rig rig;
    [SerializeField] private Transform headLookAtTransform;
    [SerializeField] private CanvasManager canvasManager;
    public float speed;
    protected Animator animator;
    public string ObjectName => NPCSO.NPCname;
    private bool isNewToPlayer = true;


    private bool isLookAtPosition;


    public bool ShouldDisplayNameOnMouseOver => true;

    public virtual void OnInteract(Player player)
    {
        OpenNextDialogBox();

        isLookAtPosition = true;
        headLookAtTransform.position = new Vector3(player.transform.position.x, headLookAtTransform.position.y, player.transform.position.z);

    }

    public virtual void OnStopInteract()
    {
        isLookAtPosition = false;
        CloseDialogBox();


    }
    protected virtual void Awake()
    {

    }
    protected virtual void Update()
    {

        float targetWeight = isLookAtPosition ? 1f : 0f;
        float lerpSpeed = 2f;
        rig.weight = Mathf.Lerp(rig.weight, targetWeight, Time.deltaTime * lerpSpeed);


        if (speed != 0)
        {
            MoveForward();
        }



    }

    private void MoveForward()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    protected virtual void OpenNextDialogBox()
    {
        if (isNewToPlayer)
        {
            isNewToPlayer = false;
            canvasManager.SetDialogText(NPCSO.NPCIntroDialog);
        }
    }

    protected virtual void CloseDialogBox()
    {
        canvasManager.CloseDialogText();
    }
}
