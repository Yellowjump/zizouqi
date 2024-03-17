using liuchengguanli;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIguanli : UIFormLogic
{
    [SerializeField]
    private Button _btnShuaxin;
    [SerializeField]
    private Button _btnShouqi;
    [SerializeField]
    private Button _btnOne;
    [SerializeField]
    private Button _btnTwo;
    [SerializeField]
    private Button _btnThree;
    [SerializeField]
    private Button _btnFour;
    [SerializeField]
    private Button _btnFive;
    [SerializeField]
    private Canvas _cavJiemian;
    private float lerpTime=0;
    private float Y = 0;

    [SerializeField]
    private AnimationCurve MoveInCurve;

    [SerializeField]
    private Text SqOrFx;//��ʾ�����Ƿ���
    private int ShouqiOrFangxia = -1;//-1������1�Ƿ���

    [SerializeField]
    private Text JinBi;//��ʾ�����
    
    Jinqian jinqian=new Jinqian();//����һ����Ǯ�����
    
    private List<Sprite>ListQiziSprite = new List<Sprite>();//��������ͼƬ
    protected override void OnInit(object userData)
    {
        JinBi.text = jinqian.GetJinBiNum().ToString();
        transform.position = Vector3.zero;
        _btnShuaxin.onClick.AddListener(OnClickBtnShuaxin);//ˢ�°���
        _btnShouqi.onClick.AddListener(OnClickBtnShouqi);//������°���
        //���ӹ��򰴼���1-5
        _btnOne.onClick.AddListener(OnClickBtnGouMaiOne);
        _btnTwo.onClick.AddListener(OnClickBtnGouMaiTwo);
        _btnThree.onClick.AddListener(OnClickBtnGouMaiThree);
        _btnFour.onClick.AddListener(OnClickBtnGouMaiFour);
        _btnFive.onClick.AddListener(OnClickBtnGouMaiFive);
        
        InitQiziPicture();//�������ӹ������ͼƬ��ʹ�õ�ʱ����ע��ListQiziPicture[qiziindex];
        //��ʼ�ȼ����飬��ˢ��
        jinqian.changejinqian(2);
        OnClickBtnShuaxin();

        base.OnInit(userData);
    }
    private void InitQiziPicture()
    {
        Sprite spr = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Image/qizi_0.jpg");
        //_btnOne.image.sprite = img;
        //���δ���img
        ListQiziSprite.Add(spr);
        //
    }
    private void OnClickBtnShouqi()
    {
        
        //JinBi.text = jinqian.GetJinBiNum().ToString();
        if (ShouqiOrFangxia == -1)
        {
            SqOrFx.text = "����";
        }
        else
        {
            SqOrFx.text = "����";
        }
        StopCoroutine(ChuangkouYidong());
        lerpTime = 0;
        StartCoroutine(ChuangkouYidong());
        ShouqiOrFangxia = -ShouqiOrFangxia;
    }
    private void OnClickBtnShuaxin()
    {
        if (jinqian.GetJinBiNum()>=2)
        {
            //�ȿ�Ǯ
            jinqian.changejinqian(-2);
            JinBi.text = jinqian.GetJinBiNum().ToString();
            //��ȡ5�����qiziIndex

            //����5������ť
            _btnOne.gameObject.SetActive(true);
            _btnTwo.gameObject.SetActive(true);
            _btnThree.gameObject.SetActive(true);
            _btnFour.gameObject.SetActive(true);
            _btnFive.gameObject.SetActive(true);
            //ˢ��UI���ӹ������
            _btnOne.image.sprite = ListQiziSprite[QiziGuanLi.Instance.goumaiUIqiziIndex[0]];
            _btnTwo.image.sprite = ListQiziSprite[QiziGuanLi.Instance.goumaiUIqiziIndex[1]];
            _btnThree.image.sprite = ListQiziSprite[QiziGuanLi.Instance.goumaiUIqiziIndex[2]];
            _btnFour.image.sprite = ListQiziSprite[QiziGuanLi.Instance.goumaiUIqiziIndex[3]];
            _btnFive.image.sprite = ListQiziSprite[QiziGuanLi.Instance.goumaiUIqiziIndex[4]];
        }
        //Log.Info("TestBtn OnClick");
    }
    IEnumerator ChuangkouYidong()
    {
        var rectTrans = _cavJiemian.transform as RectTransform;
        Y = rectTrans.anchoredPosition.y;
        while (lerpTime <40)
        {
            lerpTime++;
            rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, (Mathf.Lerp(Y, -150*ShouqiOrFangxia, lerpTime/40)));
            //Log.Info("hfk nowY : " + MoveInCurve.Evaluate(lerpTime) + " lerptime: " + lerpTime + " Lerp: " + Mathf.Lerp(rectTrans.anchoredPosition.y, -150 * ShouqiOrFangxia, lerpTime / 40) + " y: " + rectTrans.anchoredPosition.y);
            yield return null;
        }
        //rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, 150);
        yield return null;
        StopCoroutine(ChuangkouYidong());
    }
    private void OnClickBtnGouMaiOne()//�����һ�������������
    {
        int index = QiziGuanLi.Instance.goumaiUIqiziIndex[0];
        goumaiqizi(index, _btnOne);
    }
    private void OnClickBtnGouMaiTwo()
    {
        int index = QiziGuanLi.Instance.goumaiUIqiziIndex[1];
        goumaiqizi(index, _btnTwo);

    }
    private void OnClickBtnGouMaiThree()
    {
        int index = QiziGuanLi.Instance.goumaiUIqiziIndex[2];
        goumaiqizi(index, _btnThree);
    }
    private void OnClickBtnGouMaiFour()
    {
        int index = QiziGuanLi.Instance.goumaiUIqiziIndex[3];
        goumaiqizi(index, _btnFour);
    }
    private void OnClickBtnGouMaiFive()
    {
        int index = QiziGuanLi.Instance.goumaiUIqiziIndex[4];
        goumaiqizi(index, _btnFive);
    }
    private void goumaiqizi(int index,Button btn)
    {
        int kwCx = QiziGuanLi.Instance.findkongweiCX();
        int feiyong = QiziGuanLi.Instance.qizi[index];
        if (kwCx != -1 && jinqian.GetJinBiNum() >= feiyong)//�ȳ�����λ�ã��������
        {
            QiziGuanLi.Instance.goumaiqizi(index, kwCx);
            EntityQizi qizi = Pool.instance.PoolEntity.Get() as EntityQizi;
            qizi.Init(index);
            qizi.GObj.transform.localPosition = new Vector3(-4 + kwCx, 0, -4.5f);
            jinqian.changejinqian(-feiyong);
            JinBi.text = jinqian.GetJinBiNum().ToString();
            btn.gameObject.SetActive(false);
        }
    }
}
