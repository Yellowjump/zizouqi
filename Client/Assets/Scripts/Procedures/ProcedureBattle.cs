using DataTable;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
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
            //��ȡ������״̬��
            FsmBase[] fsms = GameEntry.Fsm.GetAllFsms();
            foreach (FsmBase fsm in fsms)
            {
                if (fsm.CurrentStateName.StartsWith("StateIdle"))
                {
                    Log.Info("hfk:" + fsm.Name + fsm.CurrentStateName);
                    //������״̬���ı��attack
                    //ChangeState<StateAttack0>(fsm);
                }
            }

            Log.Info("hfk,����ս��״̬ʱ����:" + Time.time);
            goumaiUI = GameEntry.UI.GetUIForm(UICtrlName.JieMianUIPrefab);
            battleTime = Time.time;
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (Time.time-battleTime>5)
            {
                ChangeState<ProcedurePreBattle>(procedureOwner);
            }
            else
            {
                goumaiUI.GetComponent<UIguanli>()._slderJindu.value = (Time.time - battleTime) / 5;
            }
        }
    }
}