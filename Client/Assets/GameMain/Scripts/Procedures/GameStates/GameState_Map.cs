using GameFramework.Fsm;
using UnityGameFramework.Runtime;

namespace Procedure.GameStates
{
    /// <summary>
    /// 地图选点
    /// </summary>
    public class GameState_Map:FsmState<ProcedureGame>
    {
        private int _mapUIIndex;
        protected override void OnEnter(IFsm<ProcedureGame> fsm)
        {
            base.OnEnter(fsm);
            //打开titleUI
            _mapUIIndex = GameEntry.UI.OpenUIForm(UICtrlName.MazePointList, "middle");
            var battleMainForm = GameEntry.UI.GetUIForm(UICtrlName.BattleMainPanel);
            if (battleMainForm != null)
            {
                GameEntry.UI.CloseUIForm(battleMainForm);
            }
        }

        protected override void OnUpdate(IFsm<ProcedureGame> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            GameEntry.HeroManager.UpdateNoBattle(GameEntry.LogicDeltaTime,realElapseSeconds);
        }

        protected override void OnLeave(IFsm<ProcedureGame> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            GameEntry.UI.CloseUIForm(_mapUIIndex);
        }
    }
}