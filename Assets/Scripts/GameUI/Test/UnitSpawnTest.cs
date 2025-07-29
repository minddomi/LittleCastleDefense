using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitSpawnTest : MonoBehaviour
{
    public TMP_InputField inputField;
    public UnitSpawner spawner;

    public void OnClickSpawn()
    {
        string unitID = inputField.text.Trim();
        Debug.Log($"[��ư Ŭ����] �Էµ� ID: '{unitID}'");

        if (!string.IsNullOrEmpty(unitID))
        {
            spawner.SpawnUnit(unitID);
            inputField.text = "";
            inputField.ActivateInputField();
        }
        else
        {
            Debug.LogWarning("[�Է� ����] ���� ID�� �Է��ϼ���.");
        }
    }
}
