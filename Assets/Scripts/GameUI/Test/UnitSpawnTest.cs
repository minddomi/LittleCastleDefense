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
        //Debug.Log($"[버튼 클릭됨] 입력된 ID: '{unitID}'");

        if (!string.IsNullOrEmpty(unitID))
        {
            spawner.SpawnUnit(unitID);
            inputField.text = "";
            inputField.ActivateInputField();
        }
        else
        {
            Debug.LogWarning("[입력 없음] 유닛 ID를 입력하세요.");
        }
    }
}
