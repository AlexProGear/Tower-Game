using UnityEngine;

public static class Extensions
{
    public static GameObject FindChildWithTag(this GameObject gameObject, string tag)
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.CompareTag(tag))
                return child.gameObject;

            GameObject recursiveSearch = FindChildWithTag(child.gameObject, tag);
            if (recursiveSearch != null)
                return recursiveSearch;
        }

        return null;
    }
}