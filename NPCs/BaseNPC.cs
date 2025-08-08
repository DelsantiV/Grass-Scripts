using UnityEngine;
using UnityEngine.Animations.Rigging;
using DialogueEditor;


public class BaseNPC : MonoBehaviour, IInteractable
{ 
    [SerializeField] protected NPCSO NPCSO;
    [SerializeField] private Rig rig;
    [SerializeField] private Transform headLookAtTransform;
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private NPCConversation Conversation;
    public float speed;
    protected Animator animator;
    private Outline outline;
    public string ObjectName => NPCSO.NPCname;


    private bool isLookAtPosition;

    public bool ShouldDisplayNameOnMouseOver => true;






    protected virtual void Awake()
    {
        outline = gameObject.GetOrAddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
        outline.OutlineColor = Color.whiteSmoke;
    }



    #region Dialog

     protected virtual void StartDialog(Player player)
    {
        player.SetCursorLockMode(false);
        ConversationManager.Instance.StartConversation(Conversation);

    }
    
    protected virtual void EndDialog(Player player)
    {
        player.SetCursorLockMode(true);
        ConversationManager.Instance.EndConversation();

    }

   


    #endregion

    #region Interact
    public virtual void OnInteract(Player player)
    {
        StartDialog(player);

        isLookAtPosition = true;
        headLookAtTransform.position = new Vector3(player.transform.position.x, headLookAtTransform.position.y, player.transform.position.z);

    }

    public virtual void OnStopInteract(Player player)
    {


    }

    #endregion

    #region LookAt
    public void OnLookAt(Player player)
    {
        outline.enabled = true;
    }

    public void OnStopLookAt(Player player)
    {
        outline.enabled = false;

        isLookAtPosition = false;

        EndDialog(player);

    }

    #endregion


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
  


   
}
