using UnityEngine;
using UnityEngine.Animations.Rigging;
using DialogueEditor;
using Unity.VisualScripting;


public class BaseNPC : MonoBehaviour, IInteractable
{ 
    [SerializeField] protected NPCSO NPCSO;
    [SerializeField] private Rig headRig;
    [SerializeField] private Rig rootRig;
    [SerializeField] private Transform headLookAtTransform;
    [SerializeField] private Transform rootLookAtTransform;
    [SerializeField] protected CanvasManager canvasManager;


    [SerializeField] private NPCConversation baseConversation;

    [SerializeField] private NPCConversation secondMeetingConversation;


    [SerializeField] protected NPCConversation conversation;

    public float speed;
    protected Animator animator;
    private Outline outline;
    public string ObjectName => NPCSO.NPCname;


    private bool isLookAtPosition;

    private bool isRootLookAtPosition;



    public bool ShouldDisplayNameOnMouseOver => true;






    protected virtual void Awake()
    {
        outline = gameObject.GetOrAddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = Color.whiteSmoke;
        conversation = baseConversation;
    }



    #region Dialog

     protected virtual void StartDialog(Player player)
    {


        player.SetCursorLockMode(false);

        
        ConversationManager.Instance.StartConversation(conversation);
        
    }

    public void ChangeConversation()
    {

        conversation = secondMeetingConversation;

    }



    protected virtual void EndDialog(Player player)
    {

        player.SetCursorLockMode(true);

        EndDialog();

    }

    protected virtual void EndDialog()
    {

        ConversationManager.Instance.EndConversation();
        
    }

    #endregion






    #region Interact
    public virtual void OnInteract(Player player)
    {

        canvasManager.CloseInteractionText();
        

        StartDialog(player);


        

        headLookAtTransform.position = new Vector3(player.transform.position.x, headLookAtTransform.position.y, player.transform.position.z);


        Debug.Log(Vector3.Angle(transform.forward, new Vector3(player.transform.position.x - transform.position.x, headLookAtTransform.position.y, player.transform.position.z - transform.position.z)));



        if (Vector3.Angle(transform.forward, new Vector3(player.transform.position.x - transform.position.x, headLookAtTransform.position.y, player.transform.position.z - transform.position.z)) < 90)
        {


            isLookAtPosition = true;
        }
        else
        {

            isRootLookAtPosition = true;

        }


        rootLookAtTransform.position = new Vector3(player.transform.position.x, rootLookAtTransform.position.y, player.transform.position.z);

    }

    public virtual void OnStopInteract(Player player)
    {


    }

    #endregion

    #region LookedAt
    public virtual void OnLookAt(Player player)
    {
        outline.enabled = true;
        canvasManager.SetInteractionText(NPCSO.NPCname);
    }

    public virtual void OnStopLookAt(Player player)
    {
        outline.enabled = false;

        isLookAtPosition = false;

        isRootLookAtPosition = false;

        EndDialog(player);

        canvasManager.CloseInteractionText();

    }

    #endregion


    protected virtual void Update()
    {

        float headTargetWeight = isLookAtPosition ? 1f : 0f;

        float rootTargetWeight = isRootLookAtPosition ? 1f : 0f;

        float lerpHeadSpeed = 2f;


        float lerpRootSpeed = 1f;


        headRig.weight = Mathf.Lerp(headRig.weight, headTargetWeight, Time.deltaTime * lerpHeadSpeed);

        rootRig.weight = Mathf.Lerp(rootRig.weight, rootTargetWeight, Time.deltaTime * lerpRootSpeed);


        if (speed != 0)
        {
            MoveForward();
        }



    }

    private void MoveForward()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
  


   
}
