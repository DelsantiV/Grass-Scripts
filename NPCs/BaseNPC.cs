using UnityEngine;

public class BaseNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private NPCSO NPCSO;
    public string ObjectName { get => NPCSO.NPCname; }
    private bool isNewToPlayer;

    public bool ShouldDisplayNameOnMouseOver => true;

    public virtual void OnInteract(Player player)
    {
        OpenNextDialogBox();
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
