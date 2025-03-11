using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckCtrl : MonoBehaviour
{
    public bool isCheck1;
    public bool isCheck2;
    [Space()]
    public List<GameObject> Bullets = new List<GameObject>();
    public List<GameObject> RemoveBullets = new List<GameObject>();
    [Space()]
    public bool isTimeGoesAfterLost = false;
    [Space()]
    public Text debugText1;
    public Text debugText2;
    public Text debugBullet;

    private GameObject GameMgr;

    void Start()
    {
        GameMgr = GameObject.FindGameObjectWithTag("GAMEMGR");
    }

    void Update()
    {
        if(isTimeGoesAfterLost)
        {
            foreach (GameObject Bullet in Bullets)
            {
                Bullet.GetComponent<BulletCtrl>().passedTimeAfterLost += Time.deltaTime;
            }
        }
    }

    public void OnTargetFound()
    {
        if (!GameMgr.GetComponent<GameMgr>().isInGameStart)
        {
            if(isCheck1)
            {
                GameMgr.GetComponent<GameMgr>().isCheck1Ready = true;
            }
            else if(isCheck2)
            {
                GameMgr.GetComponent<GameMgr>().isCheck2Ready = true;
            }
        }

        // if bullet is <LONG>, stop measuring Time. Call "OrderScore" and add bullet to <RemoveBullets> List
        foreach (GameObject Bullet in Bullets)
        {
            if (Bullet.GetComponent<BulletCtrl>().isLong_1 || Bullet.GetComponent<BulletCtrl>().isLong_2)
            {
                isTimeGoesAfterLost = false;

                if (debugBullet)
                {
                    debugBullet.text = Bullet.GetComponent<BulletCtrl>().passedTimeAfterLost.ToString();
                }

                Bullet.GetComponent<BulletCtrl>().OrderScore();
                RemoveBullets.Add(Bullet);
            }
        }

        foreach (GameObject Bullet in RemoveBullets)
        {
            Bullets.Remove(Bullet);
        }

        if (debugText1)
        {
            if (isCheck1)
            {
                debugText1.text = "Check1 Found!";
            }
            else if (isCheck2)
            {
                debugText1.text = "Check2 Found!";
            }
        }
    }

    public void OnTargetLost()
    {
        if (!GameMgr.GetComponent<GameMgr>().isInGameStart)
        {
            if(isCheck1)
            {
                GameMgr.GetComponent<GameMgr>().isCheck1Ready = false;
            }
            else if (isCheck2)
            {
                GameMgr.GetComponent<GameMgr>().isCheck2Ready = false;
            }
        }

        // Remove Bullet, Call "OrderScore" or Do Something Special
        foreach (GameObject Bullet in Bullets)
        {
            if (Bullet.GetComponent<BulletCtrl>().isBullet_1 || Bullet.GetComponent<BulletCtrl>().isBullet_2)
            {
                Bullet.GetComponent<BulletCtrl>().OrderScore();

                RemoveBullets.Add(Bullet);
            }
            if (Bullet.GetComponent<BulletCtrl>().isLong_1 || Bullet.GetComponent<BulletCtrl>().isLong_2)
            {
                isTimeGoesAfterLost = true;
                Bullet.transform.GetChild(0).gameObject.SetActive(true);
                Bullet.GetComponentInChildren<UICtrl>().isFillGoes = true;
            }
            else if (Bullet.GetComponent<BulletCtrl>().isArrow_1to2 || Bullet.GetComponent<BulletCtrl>().isArrow_2to1)
            {
                Bullet.GetComponent<BulletCtrl>().OrderScore();

                Bullet.GetComponent<BulletCtrl>().isArrowTurn = true;

                RemoveBullets.Add(Bullet);
            }
            else if(Bullet.GetComponent<BulletCtrl>().isRepeat_1 || Bullet.GetComponent<BulletCtrl>().isRepeat_2)
            {
                Bullet.GetComponent<BulletCtrl>().OrderScore();

                Bullet.GetComponent<BulletCtrl>().times--;

                if(Bullet.GetComponent<BulletCtrl>().targetCheckNum == 1)
                {
                    Bullet.GetComponent<BulletCtrl>().targetCheckNum = 2;
                    Bullet.transform.LookAt(GameObject.FindGameObjectWithTag("CHECK2").transform);
                }
                else if (Bullet.GetComponent<BulletCtrl>().targetCheckNum == 2)
                {
                    Bullet.GetComponent<BulletCtrl>().targetCheckNum = 1;
                    Bullet.transform.LookAt(GameObject.FindGameObjectWithTag("CHECK1").transform);
                }
                else
                {
                    if(Bullet.GetComponent<BulletCtrl>().isRepeat_1)
                    {
                        Bullet.GetComponent<BulletCtrl>().targetCheckNum = 2;
                        Bullet.transform.LookAt(GameObject.FindGameObjectWithTag("CHECK2").transform);
                    }
                    else if (Bullet.GetComponent<BulletCtrl>().isRepeat_2)
                    {
                        Bullet.GetComponent<BulletCtrl>().targetCheckNum = 1;
                        Bullet.transform.LookAt(GameObject.FindGameObjectWithTag("CHECK1").transform);
                    }
                }

                RemoveBullets.Add(Bullet);
            }

            if (debugBullet)
            {
                //debugBullet.text = Bullet.GetComponent<BulletCtrl>().times.ToString();
                debugBullet.text = Bullet.GetComponent<BulletCtrl>().passedTimeBeforeLost.ToString();
                Bullet.GetComponent<BulletCtrl>().passedTimeBeforeLost = 0f;
            }
        }

        foreach (GameObject Bullet in RemoveBullets)
        {
            Bullets.Remove(Bullet);
        }

        if (debugText1)
        {
            if(isCheck1)
            {
                debugText2.text = "Check1 Lost!";
            }
            else if(isCheck2)
            {
                debugText2.text = "Check2 Lost!";
            }
        }
    }

    public void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "BULLET")
        {
            if (debugText2)
            {
                //debugText2.text = Bullets.Count + " bullet(s) Ready!";
            }

            if(!Bullets.Contains(coll.gameObject))
            {
                Bullets.Add(coll.gameObject);
            }
        }
    }

    public void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "BULLET")
        {
            if (debugText2)
            {
                //debugText2.text = coll.gameObject.name + "Miss!";
            }

            Bullets.Remove(coll.gameObject);
        }
    }
}
