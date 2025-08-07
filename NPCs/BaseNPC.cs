using UnityEngine;
using UnityEngine.Animations.Rigging;


public class BaseNPC : MonoBehaviour, IInteractable
{ 
    [SerializeField] protected NPCSO NPCSO;
    [SerializeField] private Rig rig;
    [SerializeField] private Transform headLookAtTransform;
    public float speed;
    protected Animator animator;
    public string ObjectName => NPCSO.NPCname;
    private bool isNewToPlayer;


    private bool isLookAtPosition;


    public bool ShouldDisplayNameOnMouseOver => true;

    public virtual void OnInteract(Player player)
    {
        OpenNextDialogBox();

        isLookAtPosition = true;
        headLookAtTransform.position = new Vector3(player.transform.position.x, headLookAtTransform.position.y, player.transform.position.z);

        Debug.Log("Coucou");

    }

    public virtual void OnStopInteract()
    {
        isLookAtPosition = false;


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
            OpenDialogBox(NPCSO.NPCIntroDialog);
        }
    }
    protected virtual void OpenDialogBox(string text)
    {

    }
}
