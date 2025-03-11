using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour
{
    public bool isLongGauge;
    private Image curGauge;
    private float div;
    public bool isFillGoes = false;
    [Space()]
    public bool isZoomRepeat;
    public float damp;
    public float dampPercent;
    private Image img;
    private Vector2 imgInitSize;

    // Start is called before the first frame update
    void Start()
    {
        if(isLongGauge)
        {
            curGauge = transform.GetChild(1).GetComponent<Image>();

            gameObject.SetActive(false);
            curGauge.fillAmount = 1;
            div = GetComponentInParent<BulletCtrl>().howLong;
        }
        else if(isZoomRepeat)
        {
            img = GetComponent<Image>();

            imgInitSize = img.rectTransform.sizeDelta;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isLongGauge)
        {
            transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform);

            if (isFillGoes)
            {
                curGauge.fillAmount -= Time.deltaTime / div;
            }
        }
        else if(isZoomRepeat)
        {
            damp = img.rectTransform.sizeDelta.x * dampPercent;
            img.rectTransform.sizeDelta -= new Vector2(damp, damp) * Time.deltaTime;

            if(img.rectTransform.sizeDelta.x < imgInitSize.x * 0.97)
            {
                img.rectTransform.sizeDelta = imgInitSize;
            }
        }
    }
}
