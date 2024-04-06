using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace liuchengguanli
{
    public class EntityQizi : EntityBase
    {
        public int level;
        public int money;
        public int xueliangsum=1000;//��Ѫ��
        public int powersum=100;//����
        public int xueliangnow;//��ǰѪ��
        public int powernow;//��ǰ��
        public float x;
        public float y;//�������ϵ�λ��
        public float gongjiDistence;//��������
        //GameObject xuetiao;//Ѫ��ui
        public override void Init(int i)
        {
            this.Index = i;
            this.level = 1;
            this.money = QiziGuanLi.Instance.qizi[i];
            this.GObj = Pool.instance.PoolObject[i].Get();
            this.GObj.transform.localScale = Vector3.one;
            QiziGuanLi.Instance.QiziList.Add(this);
            xueliangnow = xueliangsum;
            powernow = 0;
            //this.GObj.GetComponent<Fsm_qizi0>().Init();
            //Log.Info("hfk:qizichushihua:" + this.GObj.name+"list.size: " + Pool.instance.list.Count + "list[0]position:" + Pool.instance.list[0].GObj.transform.localPosition);
        }
    }
}
