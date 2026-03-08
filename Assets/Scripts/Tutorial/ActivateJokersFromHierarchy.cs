using UnityEngine;

public class ActivateJokersFromHierarchy : MonoBehaviour
{
    public void ActivateJoker1()
    {
        ActivateByName("Joker1");
    }

    public void ActivateJoker2()
    {
        ActivateByName("Joker2");
    }

    private void ActivateByName(string namePrefix)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Hierarchy에 있는 오브젝트만
            if (obj.hideFlags != HideFlags.None)
                continue;

            if (obj.name.StartsWith(namePrefix))
            {
                obj.SetActive(true);
                Debug.Log(namePrefix + " 활성화");
                return;
            }
        }

        Debug.LogWarning(namePrefix + "(Clone) 찾지 못함");
    }
}