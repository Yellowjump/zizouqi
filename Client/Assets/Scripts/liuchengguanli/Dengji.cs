using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dengji 
{
    //��ʼ�ȼ�Ϊ1,����Ϊ0
    private int dj = 1;
    private int jinyan = 0;
    private int []shengjixuqiu = {2,4,8,10,20,32,48,60};
    //ÿ�غϸ�2����
    public void huihejiesuanJY()
    {
        jinyan += 2;
        if (jinyan >=shengjixuqiu[dj])
        {
            jinyan -= shengjixuqiu[dj];
            dj++;
        }
    }
    //���Ի���ҹ�����
    public void changejinyan(int changenum)
    {
        jinyan += changenum;
        if (jinyan >= shengjixuqiu[dj])
        {
            jinyan -= shengjixuqiu[dj];
            dj++;
        }
    }
}
