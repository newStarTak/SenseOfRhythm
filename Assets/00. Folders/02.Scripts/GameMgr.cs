using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    private AudioSource auSrc;
    public AudioClip forest;
    public AudioClip edm;
    public AudioClip rain;

    public bool isInGame;
    public bool isInGameStart;
    public bool isShooterReady;
    public bool isCheck1Ready;
    public bool isCheck2Ready;
    public GameObject scorePanel;
    public int score = 0;
    public Text scoreText;

    public GameObject selectedSheet;
    public float passedTimeInPlay;

    public int bulletIndex = 0;

    public GameObject ResultPanel;
    public GameObject BackPanel;
    public Text ResultScoreText;
    public Text ResultPercentText;
    private int successTimes = 0;
    [Space()]
    public bool isInTutorial;
    public bool isStopError;
    public GameObject SsenDuckTextPanel;
    public Text SsenDuckText;
    public GameObject Board;
    public int failTimes = 0;
    public int curText = 0;
    [Space()]
    public bool isInSelect;

    public Image Song_prev;
    public Image Song_cur;
    public Image Song_next;
    public Image curSongIllust;
    public Image selectBG;
    public Text curSongInfo;

    public Sprite s1pre;
    public Sprite s2pre;
    public Sprite s3pre;
    public Sprite s1Illust;
    public Sprite s2Illust;
    public Sprite s3Illust;
    public Text s1name;
    public Text s2name;
    public Text s3name;

    // init State -> 0
    private int curSongIndex = 0;
    [Space()]
    public GameObject SetActiveTargetUI;

    void Start()
    {
        auSrc = GetComponent<AudioSource>();

        if (isInGame)
        {
            ResultPanel.SetActive(false);

            if (scoreText)
            {
                scoreText.text = score.ToString() + "��";
            }

            selectedSheet = GameObject.FindGameObjectWithTag("SHEET");

            if(selectedSheet.name == "Song1")
            {
                auSrc.clip = forest;
            }
            else if (selectedSheet.name == "Song2")
            {
                auSrc.clip = edm;
            }
            else if (selectedSheet.name == "Song3")
            {
                auSrc.clip = rain;
            }
        }
        else if (isInTutorial)
        {
            TutorialRepeat();
        }
        else if (isInSelect)
        {
            Song_prev.sprite = s3pre;
            Song_cur.sprite = s1pre;
            Song_next.sprite = s2pre;

            curSongIllust.sprite = s1Illust;
            selectBG.sprite = s1Illust;

            curSongInfo.text = "Forest for Rest\n\n���̵�: ��";
            auSrc.clip = forest;
            auSrc.Play();
            s1name.text = "Deep Rain";
            s2name.text = "Forest for Rest";
            s3name.text = "Electronic Dominated Moment";
        }
    }

    void Update()
    {
        // For Checking Rhythm Timing (Debug)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(passedTimeInPlay - 2f + " sec Clicked - bullet");
        }
        else if(Input.GetKeyDown(KeyCode.RightShift))
        {
            Debug.Log(passedTimeInPlay - 2f + " sec Down - Long");
        }
        else if (Input.GetKeyUp(KeyCode.RightShift))
        {
            Debug.Log(passedTimeInPlay - 2f + " sec Up - Long");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log(passedTimeInPlay - 2f + " sec Clicked - Arrow");
        }

        if (isInGame && isInGameStart)
        {
            passedTimeInPlay += Time.deltaTime;
        }
        else if (isInGame && !isInGameStart && isShooterReady && isCheck1Ready && isCheck2Ready)
        {
            Invoke("AutoShoot", selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].timing);
            auSrc.Play();
            isInGameStart = true;
        }

        if (isInTutorial && score >= 250)
        {
            CancelInvoke("TutorialRepeat");
            score = 0;
            SsenDuckTextPanel.SetActive(true);
            curText++;
            TutorialRepeat();

            GameObject[] temps = GameObject.FindGameObjectsWithTag("BULLET");
            foreach (GameObject temp in temps)
            {
                Destroy(temp);
            }
        }
        else if (isInTutorial && isShooterReady && isCheck1Ready && isCheck2Ready && !isStopError && curText == 3)
        {
            SsenDuckTextPanel.SetActive(true);
            Board.SetActive(false);
            isStopError = true;
            curText++;
            TutorialRepeat();
        }
    }

    public void GetScore(int extScore)
    {
        score += extScore;

        if (scoreText)
        {
            scoreText.text = score.ToString() + "��";
        }

        successTimes++;
    }

    public void OrderBullet_1()
    {
        GameObject.FindGameObjectWithTag("SHOOTER").GetComponent<ShooterCtrl>().ShootBullet_1();
    }

    public void OrderBullet_2()
    {
        GameObject.FindGameObjectWithTag("SHOOTER").GetComponent<ShooterCtrl>().ShootBullet_2();
    }

    public void OrderLong_1(float extHowLong)
    {
        GameObject.FindGameObjectWithTag("SHOOTER").GetComponent<ShooterCtrl>().ShootLong_1(extHowLong);
    }

    public void OrderLong_2(float extHowLong)
    {
        GameObject.FindGameObjectWithTag("SHOOTER").GetComponent<ShooterCtrl>().ShootLong_2(extHowLong);
    }

    public void OrderArrow_1to2()
    {
        GameObject.FindGameObjectWithTag("SHOOTER").GetComponent<ShooterCtrl>().ShootArrow_1to2();
    }

    public void OrderArrow_2to1()
    {
        GameObject.FindGameObjectWithTag("SHOOTER").GetComponent<ShooterCtrl>().ShootArrow_2to1();
    }

    public void OrderRepeat_1(int extTimes)
    {
        GameObject.FindGameObjectWithTag("SHOOTER").GetComponent<ShooterCtrl>().ShootRepeat_1(extTimes);
    }

    public void OrderRepeat_2(int extTimes)
    {
        GameObject.FindGameObjectWithTag("SHOOTER").GetComponent<ShooterCtrl>().ShootRepeat_2(extTimes);
    }

    public void AutoShoot()
    {
        if (selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].type == 1)
        {
            OrderBullet_1();
            Debug.Log(passedTimeInPlay + " timing: " + "OrderBullet_1");
        }
        else if (selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].type == 2)
        {
            OrderBullet_2();
            Debug.Log(passedTimeInPlay + " timing: " + "OrderBullet_2");
        }
        else if (selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].type == 11)
        {
            OrderLong_1(selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].var);
            Debug.Log(passedTimeInPlay + " timing: " + "OrderLong_1 "
                + selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].var + " sec");
        }
        else if (selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].type == 22)
        {
            OrderLong_2(selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].var);
            Debug.Log(passedTimeInPlay + " timing: " + "OrderLong_2 "
                + selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].var + " sec");
        }
        else if (selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].type == 12)
        {
            OrderArrow_1to2();
            Debug.Log(passedTimeInPlay + " timing: " + "OrderArrow_1to2");
        }
        else if (selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].type == 21)
        {
            OrderArrow_2to1();
            Debug.Log(passedTimeInPlay + " timing: " + "OrderArrow_2to1");
        }
        else if (selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].type == 121)
        {
            OrderRepeat_1((int)selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].var);
            Debug.Log(passedTimeInPlay + " timing: " + "OrderRepeat_1 "
                + selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].var + " times");
        }
        else if (selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].type == 212)
        {
            OrderRepeat_2((int)selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].var);
            Debug.Log(passedTimeInPlay + " timing: " + "OrderRepeat_2 "
                + selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].var + " times");
        }
        else if (selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex].type == 0)
        {
            isInGame = false;
            Debug.Log(passedTimeInPlay + " timing: Game End");

            ResultPanel.SetActive(true);
            ResultScoreText.text = scoreText.text;
            ResultPercentText.text = ((float)successTimes / (float)selectedSheet.GetComponent<SheetCtrl>().Sheet.Length * 100).ToString("F1") + "%";

            scorePanel.SetActive(false);
            BackPanel.SetActive(false);
        }

        if (isInGame)
        {
            Invoke("AutoShoot", selectedSheet.GetComponent<SheetCtrl>().Sheet[++bulletIndex].timing -
                                selectedSheet.GetComponent<SheetCtrl>().Sheet[bulletIndex - 1].timing);
        }
    }

    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Select"))
        {
            GameObject curSheet = GameObject.Find("Song1");

            if (curSongIndex == 0)
            {
                curSheet = GameObject.Find("Song1");
            }
            else if (curSongIndex == 1)
            {
                curSheet = GameObject.Find("Song2");
            }
            else if (curSongIndex == 2)
            {
                curSheet = GameObject.Find("Song3");
            }
            DontDestroyOnLoad(curSheet);
        }
        else
        {
            Destroy(GameObject.Find("Song1"));
            Destroy(GameObject.Find("Song2"));
            Destroy(GameObject.Find("Song3"));
        }
    }

    public void PrevBtn()
    {
        if (curSongIndex > 0)
        {
            curSongIndex--;
        }
        else
        {
            curSongIndex = 2;
        }

        if (curSongIndex == 0)
        {
            Song_prev.sprite = s3pre;
            Song_cur.sprite = s1pre;
            Song_next.sprite = s2pre;
            curSongIllust.sprite = s1Illust;
            selectBG.sprite = s1Illust;
            curSongInfo.text = "Forest for Rest\n\n���̵�: ��";
            auSrc.clip = forest;
            auSrc.Play();
            s1name.text = "Deep Rain";
            s2name.text = "Forest for Rest";
            s3name.text = "Electronic Dominated Moment";
        }
        else if (curSongIndex == 1)
        {
            Song_prev.sprite = s1pre;
            Song_cur.sprite = s2pre;
            Song_next.sprite = s3pre;
            curSongIllust.sprite = s2Illust;
            selectBG.sprite = s2Illust;
            curSongInfo.text = "Electronic Dominated Moment\n\n���̵�: ��";
            auSrc.clip = edm;
            auSrc.Play();
            s1name.text = "Forest for Rest";
            s2name.text = "Electronic Dominated Moment";
            s3name.text = "Deep Rain";
        }
        else if (curSongIndex == 2)
        {
            Song_prev.sprite = s2pre;
            Song_cur.sprite = s3pre;
            Song_next.sprite = s1pre;
            curSongIllust.sprite = s3Illust;
            selectBG.sprite = s3Illust;
            curSongInfo.text = "Deep Rain\n\n���̵�: ��";
            auSrc.clip = rain;
            auSrc.Play();
            s1name.text = "Electronic Dominated Moment";
            s2name.text = "Deep Rain";
            s3name.text = "Forest for Rest";
        }
    }

    public void NextBtn()
    {
        if (curSongIndex < 2)
        {
            curSongIndex++;
        }
        else
        {
            curSongIndex = 0;
        }

        if (curSongIndex == 0)
        {
            Song_prev.sprite = s3pre;
            Song_cur.sprite = s1pre;
            Song_next.sprite = s2pre;
            curSongIllust.sprite = s1Illust;
            selectBG.sprite = s1Illust;
            curSongInfo.text = "Forest for Rest\n\n���̵�: ��";
            auSrc.clip = forest;
            auSrc.Play();
            s1name.text = "Deep Rain";
            s2name.text = "Forest for Rest";
            s3name.text = "Electronic Dominated Moment";
        }
        else if (curSongIndex == 1)
        {
            Song_prev.sprite = s1pre;
            Song_cur.sprite = s2pre;
            Song_next.sprite = s3pre;
            curSongIllust.sprite = s2Illust;
            selectBG.sprite = s2Illust;
            curSongInfo.text = "Electronic Dominated Moment\n\n���̵�: ��";
            auSrc.clip = edm;
            auSrc.Play();
            s1name.text = "Forest for Rest";
            s2name.text = "Electronic Dominated Moment";
            s3name.text = "Deep Rain";
        }
        else if (curSongIndex == 2)
        {
            Song_prev.sprite = s2pre;
            Song_cur.sprite = s3pre;
            Song_next.sprite = s1pre;
            curSongIllust.sprite = s3Illust;
            selectBG.sprite = s3Illust;
            curSongInfo.text = "Deep Rain\n\n���̵�: ��";
            auSrc.clip = rain;
            auSrc.Play();
            s1name.text = "Electronic Dominated Moment";
            s2name.text = "Deep Rain";
            s3name.text = "Forest for Rest";
        }
    }

    public void UISetActive()
    {
        if (SetActiveTargetUI.activeSelf == false)
        {
            SetActiveTargetUI.SetActive(true);
        }
        else if (SetActiveTargetUI.activeSelf == true)
        {
            SetActiveTargetUI.SetActive(false);
        }
    }

    public void TutorialRepeat()
    {
        if (curText == 0)
        {
            SsenDuckText.text = "�ȳ�, ���� �� ������ ������Ʈ �������� ��!";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 1)
        {
            SsenDuckText.text = "���ݺ��� ���� �ϳ��ϳ� �� �˷��ٰ�. ���� ������ �غ� �غ���?";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 2)
        {
            SsenDuckText.text = "AR ��Ŀ�� �׷��� �������� �νĽ��Ѻ���~ �ƴϸ� �� ��Ŀ�� �����Ӱ� ��ġ�ص� ����";
            Board.SetActive(true);
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 3)
        {
            SsenDuckTextPanel.SetActive(false);
            Board.SetActive(false);
        }
        else if (curText == 4)
        {
            SsenDuckText.text = "���� �ֳ�~ ���� ���� ����� ���� �˾ƺ���!";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 5)
        {
            SsenDuckText.text = "�߻�ü�� �����뿡 ���� ���� ���� ������ ��! ���뿡 ���缭~";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 6)
        {
            SsenDuckTextPanel.SetActive(false);
            OrderBullet_1();
            Invoke("TutorialRepeat", 2.0f);
        }
        else if (curText == 7)
        {
            SsenDuckText.text = "���� ������ ���� �ִ°�! ������ �߻�ü�� �� �������θ� ���� �ʴ´ٰ�...!";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 8)
        {
            SsenDuckTextPanel.SetActive(false);
            OrderBullet_2();
            Invoke("TutorialRepeat", 2.0f);
        }
        else if (curText == 9)
        {
            SsenDuckText.text = "�Ǹ��� ������! ������ �ִ°� ������? �ٸ� �߻�ü�� �˾ƺ���~";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 10)
        {
            SsenDuckText.text = "���� ���� ������ ���׶� �߻�ü�� ���� ���ٴ� ���� �������� ���� �ž�..";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 11)
        {
            SsenDuckText.text = "�������� �� �� ����.. ���� �� ġ��� �ž�! ���� �ְ�!";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 12)
        {
            SsenDuckTextPanel.SetActive(false);
            OrderLong_1(1.0f);
            Invoke("TutorialRepeat", 2.0f);
        }
        else if (curText == 13)
        {
            SsenDuckText.text = "������ ����! �� ���� ������ ���!";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 14)
        {
            SsenDuckText.text = "�̰͵� �� �� ������? �� ��� ȭ��ǥ �߻�ü�� �� �� ������ �ɷ� ������ �ʾ�";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 15)
        {
            SsenDuckText.text = "�� �� ��� ���� �ٸ� ������� *�ָ�* �ϰ� �������ž�.. ���� ì�ܼ� ��ƺ����~";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 16)
        {
            SsenDuckTextPanel.SetActive(false);
            OrderArrow_1to2();
            Invoke("TutorialRepeat", 2.0f);
        }
        else if (curText == 17)
        {
            SsenDuckText.text = "������ �� ��Ҿ�! ����ѵ�?";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 18)
        {
            SsenDuckText.text = "��, ���� �������̾�. ���� ȭ��ǥ �߻�ü�� ���� �Ͱ� �����";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 19)
        {
            SsenDuckText.text = "��� �ٽ� �ѹ� ��Ҵٰ� �ؼ� ���� �Ƴ�... ��� �������Ŷ��!";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 20)
        {
            SsenDuckTextPanel.SetActive(false);
            OrderRepeat_2(4);
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 21)
        {
            SsenDuckText.text = "�̷� ��������! ���� �˾Ƽ� �ϴ±���!";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 22)
        {
            SsenDuckText.text = "�ʴ� ���� �� ��ü��, ���� �˷��ٰ� ���°�..?";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
        else if (curText == 23)
        {
            SsenDuckText.text = "���� ���� �ְ� �����Ϸ� �����ڰ�~~ (��ư�� ���� ����������)";
            curText++;
            Invoke("TutorialRepeat", 4.0f);
        }
    }
}