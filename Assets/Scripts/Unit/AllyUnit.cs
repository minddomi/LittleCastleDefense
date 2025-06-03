using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AllyUnit : MonoBehaviour
{
    public enum UnitType
    {
        Archer = 0,
        Wizard = 1,
        Siege = 2
    }

    public UnitType unitType;
    public float attackRange = 5f; // ���� ����
    public float attackInterval = 1.5f; // ���� ��ٿ�
    public float attackPower = 50f; // ���ݷ�
    public float upgradeLevel = 0.0f; // ���׷��̵� ����
    public float upgradePower = 0.0f; // ���׷��̵� ���ݷ�
    public float attackIntervalFinal = 0; // ���� ��ٿ� ���̳�
    public float archerbuff = 0.8f; // ���� ���� ����
    public bool isHero = false; // ���� ���� �Ǵ�
    
    public GameObject projectilePrefab;
    public Transform firePoint;

    private float attackTimer = 0f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        updateburf();

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackIntervalFinal)
        {
            GameObject target = FindClosestEnemyInRange();
            if (target != null)
            {
                Shoot(target.transform);
                attackTimer = 0f;
            }
        }
    }



    GameObject FindClosestEnemyInRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < attackRange && dist < minDist)
            {
                minDist = dist;
                closest = enemy;
            }
        }

        return closest;
    }

    void Shoot(Transform target)
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectile = proj.GetComponent<Projectile>();

        float totalPower = attackPower + upgradePower; // �� ������ ���
        projectile.SetTarget(target, unitType, totalPower);     
    }

    // ������Ʈ ����
    void updateburf() {
        attackIntervalFinal = attackInterval;
        GameObject[] units = GameObject.FindGameObjectsWithTag("unit");
        float totalburfmultiplier = 1.0f;
        foreach (GameObject unit in units) {

            AllyUnit ally = unit.GetComponent<AllyUnit>();
            if (ally != null && ally.isHero && ally.unitType == UnitType.Archer)
            {
                totalburfmultiplier *= ally.archerbuff;
            }
        }
        attackIntervalFinal *= totalburfmultiplier;
    }
    
}
