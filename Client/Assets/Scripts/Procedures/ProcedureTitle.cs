using System.Collections.Generic;
using DataTable;
using GameFramework;
using GameFramework.Procedure;
using Entity;
using Maze;
using UnityChan;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace Procedure
{
    public class ProcedureTitle: ProcedureBase
    {
        private bool moveToNewGame = false;
        private bool moveToContinueGame = false;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            //打开titleUI
            GameEntry.UI.OpenUIForm(UICtrlName.MainTitlePanel, "middle");
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (moveToNewGame)
            {
                moveToNewGame = false;
                //初始化关卡数据
                InitNewGameData();
                ChangeState<ProcedureGame>(procedureOwner);
            }
            else if (moveToContinueGame)
            {
                moveToContinueGame = false;
                //读取存档数据
                InitContinueGameData();
                ChangeState<ProcedureGame>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            var mainTitle = GameEntry.UI.GetUIForm(UICtrlName.MainTitlePanel);
            GameEntry.UI.CloseUIForm(mainTitle);
        }

        private void InitNewGameData()
        {
            //一局关卡游戏初始化
            var mazeGen = new MazeGenerator();//todo 后续读取游戏存档获取地图
            SelfDataManager.Instance.CurMazeList = mazeGen.GenerateMaze();
            var oneHero = GameEntry.HeroManager.AddNewFriendHero(1);
            SelfDataManager.Instance.SelfHeroList.Add(oneHero);
            SelfDataManager.Instance.ItemBag.Clear();
        }

        private void InitContinueGameData()
        {
            var gameData = GameEntry.HeroManager.Load();
            if (gameData == null)
            {
                Log.Error("GameData error");
                InitNewGameData();
                return;
            }

            SelfDataManager.Instance.InitDataFormData(gameData);
        }
        public void MoveToGame()
        {
            moveToNewGame = true;
        }

        public void MoveToContinueGame()
        {
            moveToContinueGame = true;
        }
    }
}