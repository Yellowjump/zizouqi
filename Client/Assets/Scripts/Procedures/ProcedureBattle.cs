using DataTable;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using liuchengguanli;
using UnityChan;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace Procedure
{
    public class ProcedureBattle: ProcedureBase
    {
        float battleTime;
        UIForm goumaiUI;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            QiziGuanLi.Instance.dangqianliucheng = 1;
            //��������list���õ������ӷ��������̶���
            for (int i = 0; i < QiziGuanLi.Instance.DirenList.Count; i++)
            {
                EntityQizi qz = QiziGuanLi.Instance.DirenList[i];
                qz.GObj.SetActive(true);
                qz.GObj.transform.position = new Vector3(-qz.x, 0, -qz.y);
                qz.GObj.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
                //����qige[][]�Ƿ�������
                Vector2Int posindex = QiziGuanLi.Instance.getIndexQige(qz.GObj.transform.position);
                QiziGuanLi.Instance.qige[posindex.x][posindex.y] = 1;
            }
            for (int i=0;i<QiziGuanLi.Instance.QiziCSList.Count;i++)
            {
                EntityQizi qz = QiziGuanLi.Instance.QiziCSList[i];
                qz.GObj.transform.position = new Vector3(qz.x, 0, qz.y);
                //����qige[][]�Ƿ�������
                Vector2Int posindex = QiziGuanLi.Instance.getIndexQige(qz.GObj.transform.position);
                QiziGuanLi.Instance.qige[posindex.x][posindex.y] = 1;
            }
            //Log.Info("hfk,����ս��״̬ʱ����:" + Time.time);
            goumaiUI = GameEntry.UI.GetUIForm(UICtrlName.JieMianUIPrefab);
            battleTime = Time.time;
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (Time.time-battleTime>30)
            {
                ChangeState<ProcedurePreBattle>(procedureOwner);
            }
            else
            {
                goumaiUI.GetComponent<UIguanli>()._slderJindu.value = (Time.time - battleTime) / 30;
                goumaiUI.GetComponent<UIguanli>().textjindu.text = "ս���׶�ʣ�ࣺ" + (30 - (int)(Time.time - battleTime)) + "s";
            }
        }
    }
}