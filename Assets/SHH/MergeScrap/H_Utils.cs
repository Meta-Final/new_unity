using UnityEngine;

public static class H_Utils
{
    public static GameResources gameResources;

    public static GameResources InitResources()
    {
        Debug.Log("Resources initialized.");
        return gameResources = Resources.Load<GameResources>("GameResources");
    }

    public static Sprite GetItemVisualById(int itemId)
    {
        return gameResources.items[itemId];
    }
}
