using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class QiziGuanLi
{
    public static QiziGuanLi instance;//��ʵ����
    public static QiziGuanLi Instance
    {
        get
        {
            if (instance == null)
                instance = new QiziGuanLi();
            return instance;
        }
    }

    //private int _curUid = 1;
    int []changshang= { -1, -1, -1, -1, -1, -1, -1, -1, -1 };//��¼�������ӵ�index
    int []changxia= { -1, -1, -1, -1, -1, -1, -1, -1,-1 };//��¼�������ӵ�index

    public int[] qizi = {1 };//��¼���ӵļ۸�i�����ӵ�index
    public int[] qizishu = {20 };//i�����ӵ�index��������������ʣ������

    int[] beixuan = new int[100];
    public int []goumaiUIqiziIndex = new int[5];//��¼UI������������index

    public int findkongweiCS()
    {
        for (int i=0;i<9;i++)
        {
            if (changshang[i]==-1)
            {
                return i;
            }
        }
        return -1;
    }
    public int findkongweiCX()
    {
        for (int i = 0; i < 9; i++)
        {
            if (changxia[i] == -1)
            {
                return i;
            }
        }
        return -1;
    }
    public void goumaiqizi(int qiziindex,int kongweiCX)
    {
        if (kongweiCX != -1)
        {
            changxia[kongweiCX] = qiziindex;
        }
    }

}
