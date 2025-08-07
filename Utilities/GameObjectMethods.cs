using UnityEngine;
 public static class GameObjectMethods
{
    public static void SetLayerAllChildren(this GameObject GO, int layer, bool includeInactive = true)
    {
        var children = GO.GetComponentsInChildren<Transform>(includeInactive : includeInactive);
        GO.layer = layer;
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (!gameObject.TryGetComponent<T>(out T component)) component = gameObject.AddComponent<T>();
        return component;
    }
}

