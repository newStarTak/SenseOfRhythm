using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterCtrl : MonoBehaviour
{
    public GameObject Bullet_1;
    public GameObject Bullet_2;
    public GameObject Long_1;
    public GameObject Long_2;
    public GameObject Arrow_1to2;
    public GameObject Arrow_2to1;
    public GameObject Repeat_1;
    public GameObject Repeat_2;

    private Transform Check_1;
    private Transform Check_2;

    private GameObject GameMgr;

    // Find 2 Checks
    void Start()
    {
        Check_1 = GameObject.FindGameObjectWithTag("CHECK1").transform;
        Check_2 = GameObject.FindGameObjectWithTag("CHECK2").transform;

        GameMgr = GameObject.FindGameObjectWithTag("GAMEMGR");
    }

    public void OnTargetFound()
    {
        if(!GameMgr.GetComponent<GameMgr>().isInGameStart)
        {
            GameMgr.GetComponent<GameMgr>().isShooterReady = true;
        }
    }

    public void OnTargetLost()
    {
        if (!GameMgr.GetComponent<GameMgr>().isInGameStart)
        {
            GameMgr.GetComponent<GameMgr>().isShooterReady = false;
        }
    }

    // Shoot Bullets and LookAt Proper Check
    public void ShootBullet_1()
    {
        Debug.Log("ShootBullet_1 Called");
        Instantiate(Bullet_1, gameObject.transform.position, gameObject.transform.rotation);
        transform.LookAt(Check_1);
    }

    public void ShootBullet_2()
    {
        Debug.Log("ShootBullet_2 Called");
        Instantiate(Bullet_2, gameObject.transform.position, gameObject.transform.rotation);
        transform.LookAt(Check_2);
    }

    public void ShootLong_1(float extHowLong)
    {
        Debug.Log("ShootLong_1 Called");
        GameObject clone = Instantiate(Long_1, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        clone.GetComponent<BulletCtrl>().howLong = extHowLong;
        transform.LookAt(Check_1);
    }

    public void ShootLong_2(float extHowLong)
    {
        Debug.Log("ShootLong_2 Called");
        GameObject clone = Instantiate(Long_2, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        clone.GetComponent<BulletCtrl>().howLong = extHowLong;
        transform.LookAt(Check_2);
    }

    public void ShootArrow_1to2()
    {
        Debug.Log("ShootArrow_1to2 Called");
        Instantiate(Arrow_1to2, gameObject.transform.position, gameObject.transform.rotation);
        transform.LookAt(Check_1.position);
    }

    public void ShootArrow_2to1()
    {
        Debug.Log("ShootArrow_2to1 Called");
        Instantiate(Arrow_2to1, gameObject.transform.position, gameObject.transform.rotation);
        transform.LookAt(Check_2.position);
    }

    public void ShootRepeat_1(int extTimes)
    {
        Debug.Log("ShootRepeat_1 Called");
        GameObject clone = Instantiate(Repeat_1, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        clone.GetComponent<BulletCtrl>().times = extTimes;
        transform.LookAt(Check_1.position);
    }

    public void ShootRepeat_2(int extTimes)
    {
        Debug.Log("ShootRepeat_2 Called");
        GameObject clone = Instantiate(Repeat_2, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        clone.GetComponent<BulletCtrl>().times = extTimes;
        transform.LookAt(Check_2.position);
    }
}
