using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletCtrl : MonoBehaviour
{
    public bool isBullet_1;
    public bool isBullet_2;
    [Space()]
    public bool isLong_1;
    public bool isLong_2;
    public float howLong = 0f;
    public float passedTimeBeforeLost = 0f;
    public float passedTimeAfterLost = 0f;
    [Space()]
    public bool isArrow_1to2;
    public bool isArrow_2to1;
    public bool isArrowTurn = false;
    [Space()]
    public bool isRepeat_1;
    public bool isRepeat_2;
    public int targetCheckNum = 0;
    public int times = 0;

    private Transform Shooter;
    private Transform Check_1;
    private Transform Check_2;

    private Vector3 TargetInit;
    private Vector3 TargetCheck_1;
    private Vector3 TargetCheck_2;

    public GameObject particle;

    // Find Shooter N 2 Checks and Target Position Setting
    void Start()
    {
        Shooter = GameObject.FindGameObjectWithTag("SHOOTER").transform;
        Check_1 = GameObject.FindGameObjectWithTag("CHECK1").transform;
        Check_2 = GameObject.FindGameObjectWithTag("CHECK2").transform;
        
        if (isBullet_1 || isLong_1)
        {
            TargetInit = Check_1.position + ((Check_1.position - Shooter.position) / 2);
        }
        else if(isBullet_2 || isLong_2)
        {
            TargetInit = Check_2.position + ((Check_2.position - Shooter.position) / 2);
        }
        else if(isArrow_1to2)
        {
            TargetInit = Check_1.position + ((Check_1.position - Shooter.position) / 2);
            TargetCheck_2 = Check_2.position + (Check_2.position - Check_1.position);
        }
        else if(isArrow_2to1)
        {
            TargetInit = Check_2.position + ((Check_2.position - Shooter.position) / 2);
            TargetCheck_1 = Check_1.position + (Check_1.position - Check_2.position);
        }
        else if(isRepeat_1)
        {
            TargetInit = Check_1.position + ((Check_1.position - Shooter.position) / 2);
            TargetCheck_1 = Check_1.position + (Check_1.position - Check_2.position);
            TargetCheck_2 = Check_2.position + (Check_2.position - Check_1.position);
        }
        else if(isRepeat_2)
        {
            TargetInit = Check_2.position + ((Check_2.position - Shooter.position) / 2);
            TargetCheck_1 = Check_1.position + (Check_1.position - Check_2.position);
            TargetCheck_2 = Check_2.position + (Check_2.position - Check_1.position);
        }
    }

    void Update()
    {
        // For Debug - Bullets moving Time from Shooter to Check
        if (!GameObject.FindGameObjectWithTag("CHECK1").GetComponent<CheckCtrl>().isTimeGoesAfterLost)
        {
            passedTimeBeforeLost += Time.deltaTime;
        }

        if (isBullet_1 || isLong_1)
        {
            if (!GameObject.FindGameObjectWithTag("CHECK1").GetComponent<CheckCtrl>().isTimeGoesAfterLost)
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetInit,
                                        Vector3.Distance(Shooter.position, Check_1.position) / 2 * Time.deltaTime);
            }
        }
        else if (isBullet_2 || isLong_2)
        {
            if (!GameObject.FindGameObjectWithTag("CHECK2").GetComponent<CheckCtrl>().isTimeGoesAfterLost)
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetInit,
                                        Vector3.Distance(Shooter.position, Check_1.position) / 2 * Time.deltaTime);
            }
        }
        else if (isArrow_1to2)
        {
            if (isArrowTurn)
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetCheck_2,
                                Vector3.Distance(Check_1.position, Check_2.position) * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetInit,
                                Vector3.Distance(Shooter.position, Check_1.position) / 2 * Time.deltaTime);
            }
            transform.LookAt(Check_2);
        }
        else if (isArrow_2to1)
        {
            if (isArrowTurn)
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetCheck_1,
                                Vector3.Distance(Check_2.position, Check_1.position) * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetInit,
                                Vector3.Distance(Shooter.position, Check_2.position) / 2 * Time.deltaTime);
            }
            transform.LookAt(Check_1);
        }
        else if (isRepeat_1)
        {
            if (targetCheckNum == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetCheck_1,
                                Vector3.Distance(Check_2.position, Check_1.position) * Time.deltaTime);
            }
            else if (targetCheckNum == 2)
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetCheck_2,
                                Vector3.Distance(Check_1.position, Check_2.position) * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetInit,
                                Vector3.Distance(Shooter.position, Check_1.position) / 2 * Time.deltaTime);
                transform.LookAt(Check_2);
            }
        }
        else if (isRepeat_2)
        {
            if (targetCheckNum == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetCheck_1,
                                Vector3.Distance(Check_2.position, Check_1.position) * Time.deltaTime);
            }
            else if (targetCheckNum == 2)
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetCheck_2,
                                Vector3.Distance(Check_1.position, Check_2.position) * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetInit,
                                Vector3.Distance(Shooter.position, Check_2.position) / 2 * Time.deltaTime);
                transform.LookAt(Check_1);
            }
        }

        if (Vector3.Distance(transform.position, TargetInit) < 0.1f
            || Vector3.Distance(transform.position, TargetCheck_1) < 0.1f
            || Vector3.Distance(transform.position, TargetCheck_2) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    public void OrderScore()
    {
        {
            if (isBullet_1 || isBullet_2)
            {
                GameObject.FindGameObjectWithTag("GAMEMGR").GetComponent<GameMgr>().GetScore(100);

                Instantiate(particle, transform.position, transform.rotation);

                Destroy(gameObject);
            }
            else if (isLong_1 || isLong_2)
            {
                if(howLong - 0.2f < passedTimeAfterLost && passedTimeAfterLost < howLong + 0.2f)
                {
                    GameObject.FindGameObjectWithTag("GAMEMGR").GetComponent<GameMgr>().GetScore(100 * (int)howLong);

                    Instantiate(particle, transform.position, transform.rotation);

                    Destroy(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else if (isArrowTurn && (isArrow_1to2 || isArrow_2to1))
            {
                GameObject.FindGameObjectWithTag("GAMEMGR").GetComponent<GameMgr>().GetScore(200);

                Instantiate(particle, transform.position, transform.rotation);

                Destroy(gameObject);
            }
            else if (isRepeat_1 || isRepeat_2)
            {
                GameObject.FindGameObjectWithTag("GAMEMGR").GetComponent<GameMgr>().GetScore(50);

                Instantiate(particle, transform.position, transform.rotation);

                if (times <= 1)
                {
                    Destroy(gameObject);
                }
                
            }
        }
    }
}