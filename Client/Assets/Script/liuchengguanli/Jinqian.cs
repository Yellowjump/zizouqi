using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jinqian
{
    //��ʼ���Ϊ0
    private int jinbinum = 0;

    public void changejinqian(int change)
    {
        jinbinum += change;
    }
    public void huihejiesuanJQ()
    {
        jinbinum += 5;//ÿ�غϹ̶���5���
        //������Ϣ
        if (jinbinum >= 50)
        {
            jinbinum += 5;
        }
        else
        {
            jinbinum += (jinbinum / 10);
        }
    }
}
