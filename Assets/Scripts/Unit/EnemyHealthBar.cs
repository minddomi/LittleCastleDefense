using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Transform enemyTransform;
    public Vector3 offset = new Vector3(0, 1f, 0);
    public Image healthFill;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyTransform != null)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(enemyTransform.position + offset);
            transform.position = screenPos;
        }
    }
    public void UpdateHealth(float current, float max)
    {
        healthFill.fillAmount = current / max;
    }
}
