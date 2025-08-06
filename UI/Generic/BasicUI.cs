using UnityEngine;

public class BasicUI : MonoBehaviour
{
    public virtual void Open()
    {
        gameObject.SetActive(true);
        OnOpen();
    }
    protected virtual void OnOpen(){ }
    public virtual void Close()
    {
        gameObject.SetActive(false);
        OnClose();
    }
    public virtual void Open(bool closeChildren)
    {
        if (closeChildren) OpenAllChildren();
        Open();
    }
    public virtual void Close(bool closeChildren)
    {
        if (closeChildren) CloseAllChildren();
        Close();
    }
    protected virtual void OnClose() { }
    protected virtual void CloseAllChildren()
    {
        BasicUI[] children = transform.GetComponentsInChildren<BasicUI>();
        foreach (BasicUI child in children) child.Close();
    }
    protected virtual void OpenAllChildren()
    {
        BasicUI[] children = transform.GetComponentsInChildren<BasicUI>(includeInactive: true);
        foreach (BasicUI child in children) child.Open();
    }
}
