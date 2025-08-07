using UnityEngine;

public class BaseNPC : MonoBehaviour, IInteractable
{ 
    [SerializeField] protected NPCSO NPCSO;
    public float speed;
    protected Animator animator;
    public string ObjectName => NPCSO.NPCname;
    private bool isNewToPlayer;

    public bool ShouldDisplayNameOnMouseOver => true;

    public virtual void OnInteract(Player player)
    {
        OpenNextDialogBox();
    }
    protected virtual void Awake()
    {

    }
    protected virtual void Update()
    {
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
