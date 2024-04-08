using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace liuchengguanli
{
    public class EntityQizi : EntityBase
    {
        public int level;
        public int money;
        public float xueliangsum=1000;//��Ѫ��
        public float powersum =100;//����
        public float xueliangnow;//��ǰѪ��
        public float powernow;//��ǰ��
        public float x;
        public float y;//�������ϵ�λ��
        public float gongjiDistence;//��������
        Slider xuetiao;
        Slider power;
        Transform xuet;
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
            this.xuetiao = this.GObj.transform.Find("xuetiao_qizi/xuetiao").GetComponent<Slider>();
            this.xuetiao.value = this.xueliangnow / this.xueliangnow;
            this.power = this.GObj.transform.Find("xuetiao_qizi/pow").GetComponent<Slider>();
            this.power.value = this.powernow / this.powersum;
            //GameEntry.UI.OpenUIForm("Assets/UIPrefab/xuetiao_qizi.prefab", "middle");
            //this.GObj.GetComponent<Fsm_qizi0>().Init();
            //Log.Info("hfk:qizichushihua:" + this.GObj.name+"list.size: " + Pool.instance.list.Count + "list[0]position:" + Pool.instance.list[0].GObj.transform.localPosition);
        }
    }
}
