using System;
using DataTable;
using Entity;
using GameFramework.Event;
using GameFramework.Fsm;
using SkillSystem;
using UnityGameFramework.Runtime;
using SelfEventArg;

namespace Procedure.GameStates
{
    /// <summary>
    /// 战前编队
    /// </summary>
    public class GameState_FormationBeforeBattle:FsmState<ProcedureGame>
    {
        private bool _willChangeToBattle = false;
        private int _UIIndex;
        protected override void OnInit(IFsm<ProcedureGame> fsm)
        {
            base.OnInit(fsm);
            GameEntry.Event.Subscribe(FormationToBattleEventArgs.EventId,OnFormationToBattle);
        }

        protected override void OnEnter(IFsm<ProcedureGame> fsm)
        {
            base.OnEnter(fsm);
            //初始化 敌人
            InitEnemy();
            InitFriendHero();
            _UIIndex = GameEntry.UI.OpenUIForm(UICtrlName.BattleFormationPanel, "middle");
            GameEntry.UI.OpenUIForm(UICtrlName.BattleMainPanel, "middle");
        }

        protected override void OnUpdate(IFsm<ProcedureGame> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if (_willChangeToBattle)
            {
                _willChangeToBattle = false;
                ChangeState<GameState_Battle>(fsm);
            }
        }

        protected override void OnLeave(IFsm<ProcedureGame> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            GameEntry.UI.CloseUIForm(_UIIndex);
        }

        protected override void OnDestroy(IFsm<ProcedureGame> fsm)
        {
            base.OnDestroy(fsm);
            GameEntry.Event.Unsubscribe(FormationToBattleEventArgs.EventId,OnFormationToBattle);
        }

        private void InitEnemy()
        {
            var mazeGen = SelfDataManager.Instance.CurMaze;
            var curPoint = mazeGen.CurMazePoint;
            var enemyConfigs = GameEntry.DataTable.GetDataTable<DREnemyConfig>("EnemyConfig");
            if (enemyConfigs.HasDataRow(1))
            {
                 var enemyInfo = enemyConfigs[1].EnemyInfo;
                 foreach (var oneInfo in enemyInfo.InfoList)
                 {
                     QiziGuanLi.instance.InitOneEnemy(oneInfo);
                 }
            }
        }
        private void InitFriendHero()
        {
            
        }
        private void OnFormationToBattle(object sender, GameEventArgs e)
        {
            FormationToBattleEventArgs ne = (FormationToBattleEventArgs)e;
            if (ne == null)
            {
                return;
            }

            _willChangeToBattle = true;
        }
    }
}