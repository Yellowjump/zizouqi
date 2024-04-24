using GameFramework.Fsm;
using liuchengguanli;
using Procedure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class StateAttack0 : FsmState<Fsm_qizi0>
{
    EntityQizi qizi;//״̬�����ص�������
    int zhenying =0;//0�������Լ���ߵ����ӣ�1�����ǵط���Ӫ��
    EntityQizi qizitarget;//Ŀ������
    float mindistance=10000;
    float timebegin;
    protected override void OnInit(IFsm<Fsm_qizi0> fsm)
    {
        base.OnInit(fsm);
    }
    protected override void OnEnter(IFsm<Fsm_qizi0> fsm)
    {
        base.OnEnter(fsm);
        
        if (qizi == null)
        {
            foreach (EntityQizi qz in QiziGuanLi.Instance.QiziList)
            {
                if (qz.GObj.transform.localPosition == fsm.Owner.transform.localPosition)
                {
                    qizi = qz;
                    zhenying = 0;
                    break;
                }
            }
            foreach (EntityQizi qz in QiziGuanLi.Instance.DirenList)
            {
                if (qz.GObj.transform.localPosition == fsm.Owner.transform.localPosition)
                {
                    qizi = qz;
                    zhenying = 1;
                    break;
                }
            }
            //Log.Info("hfk:" + qizi.level);
        }
        qizi.animator.Play("ATTACK");
        //Log.Info("hfk ���ӽ���attack");
        qizitarget = null;
        mindistance = 10000;
        //�ҵ�������������������
        Findtarget();
        timebegin = Time.time;
    }
    private void Findtarget()
    {
        if (zhenying == 0)
        {
            foreach (EntityQizi qz in QiziGuanLi.Instance.DirenList)
            {
                float distence = (qizi.GObj.transform.position.x - qz.GObj.transform.position.x) * (qizi.GObj.transform.position.x - qz.GObj.transform.position.x) + (qizi.GObj.transform.position.z - qz.GObj.transform.position.z) * (qizi.GObj.transform.position.z - qz.GObj.transform.position.z);
                if (distence < mindistance)
                {
                    qizitarget = qz;
                    mindistance = distence;
                }
            }
        }
        else
        {
            foreach (EntityQizi qz in QiziGuanLi.Instance.QiziCSList)
            {
                float distence = (qizi.GObj.transform.position.x - qz.GObj.transform.position.x) * (qizi.GObj.transform.position.x - qz.GObj.transform.position.x) + (qizi.GObj.transform.position.z - qz.GObj.transform.position.z) * (qizi.GObj.transform.position.z - qz.GObj.transform.position.z);
                if (distence < mindistance)
                {
                    qizitarget = qz;
                    mindistance = distence;
                }
            }
        }
    }
    protected override void OnUpdate(IFsm<Fsm_qizi0> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        if (Time.time-timebegin>0.5f)
        {
            timebegin = Time.time + 0.5f;
            if (QiziGuanLi.Instance.dangqianliucheng == 0 || qizitarget == null)
            {
                ChangeState<StateIdle0>(fsm);
            }
            if (qizi.gongjiDistence * qizi.gongjiDistence >= mindistance)
            {
                qizi.GObj.transform.LookAt(qizitarget.GObj.transform.position);
                //����
            }
            else
            {
                ChangeState<StateMove0>(fsm);
            }
        }
    }
    protected override void OnLeave(IFsm<Fsm_qizi0> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
    }
    protected override void OnDestroy(IFsm<Fsm_qizi0> fsm)
    {
        base.OnDestroy(fsm);
    }
}
