using GameFramework.Fsm;
using System;
using System.Collections;
using System.Collections.Generic;
using DataTable;
using GameFramework;
using SkillSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Entity
{
    public partial class EntityQizi
    {
        public List<Skill> NormalSkillList = new List<Skill>();
        public List<Skill> NoAnimAtkSkillList = new List<Skill>();
        public Skill SpSkill;
        public List<Skill> PassiveSkillList = new List<Skill>();
        public Dictionary<TriggerType, List<OneTrigger>> CurTriggerDic = new Dictionary<TriggerType, List<OneTrigger>>();
        public List<Buff> CurBuffList = new List<Buff>();

        public EntityQizi CurAttackTarget;
        /// <summary>
        /// 普攻动画加速时间
        /// </summary>
        /*public float CurNormalAtkAniRate
        {
            get
            {
                if (NormalSkill.DefaultAnimationDurationMs > 1 / AtkSpeed)
                {
                    return NormalSkill.DefaultAnimationDurationMs * AtkSpeed;
                }
                return 1;
            }
        }*/
        
        private void InitSkill()
        {
            InitItemSkillAndAttr();
            InitHeroTableSkill();
            CastPassiveSkill();
        }

        private void InitItemSkillAndAttr()
        {
            var heroTable = GameEntry.DataTable.GetDataTable<DRHero>("Hero");
            if (!heroTable.HasDataRow(HeroID))
            {
                Log.Error($"heroID:{HeroID} invalid no match TableRow");
                return;
            }
            var itemTable = GameEntry.DataTable.GetDataTable<DRItem>("Item");
            foreach (var oneItemID in EquipItemList)
            {
                if (!itemTable.HasDataRow(oneItemID))
                {
                    Log.Error($"Error ItemID{oneItemID}");
                    continue;
                }

                var itemData = itemTable[oneItemID];
                if (itemData.SkillID != 0)
                {
                    var skillID = itemData.SkillID;
                    var oneNewSkill = AddOneNewSkill(skillID);
                    if (oneNewSkill == null)
                    {
                        continue;
                    }
                    if (oneNewSkill.CurSkillType == SkillType.NormalSkill)
                    {
                        NormalSkillList.Add(oneNewSkill);
                    }
                    else if (oneNewSkill.CurSkillType == SkillType.PassiveSkill)
                    {
                        PassiveSkillList.Add(oneNewSkill);
                    }
                    else if (oneNewSkill.CurSkillType == SkillType.NoAnimSkill)
                    {
                        NoAnimAtkSkillList.Add(oneNewSkill);
                    }
                }
                if (itemData.AttrAdd != null && itemData.AttrAdd.Count != 0)
                {
                    //添加属性
                    foreach (var typeAndValue in itemData.AttrAdd)
                    {
                        TryAddAttrValue((AttributeType)typeAndValue.Item1, typeAndValue.Item2);
                    }
                }
            }
            NormalSkillList.Sort((a,b)=>b.SkillRange.CompareTo(a.SkillRange));//按照攻击距离排序
        }
        /// <summary>
        /// 初始化被动技能
        /// </summary>
        public void InitHeroTableSkill()
        {
            var heroTable = GameEntry.DataTable.GetDataTable<DRHero>("Hero");
            if (!heroTable.HasDataRow(HeroID))
            {
                Log.Error($"heroID:{HeroID} invalid no match TableRow");
                return;
            }

            var heroTableData = heroTable[HeroID];
            var passiveSkillID = heroTableData.PassiveSkillID;
            var passiveSkill = AddOneNewSkill(passiveSkillID);
            if (passiveSkill != null && passiveSkill.CurSkillType == SkillType.PassiveSkill)
            {
                PassiveSkillList.Add(passiveSkill);
            }

            var spSkillID = heroTableData.SpSkillID;
            var oneSpSkill = AddOneNewSkill(spSkillID);
            if (oneSpSkill != null && oneSpSkill.CurSkillType == SkillType.SpSkill)
            {
                SpSkill = oneSpSkill;
            }
        }

        private Skill AddOneNewSkill(int skillID)
        {
            if (skillID != 0)
            {
                var skillTable = GameEntry.DataTable.GetDataTable<DRSkill>("Skill");
                if (!skillTable.HasDataRow(skillID))
                {
                    Log.Error($"heroID:{HeroID} skillID{skillID} invalid no match TableRow");
                    return null;
                }

                //持有技能
                var skillTableData = skillTable[skillID];
                var skillTemplates = GameEntry.DataTable.GetDataTable<DRSkillTemplate>("SkillTemplate");
                if (!skillTemplates.HasDataRow(skillTableData.TemplateID))
                {
                    Log.Error($"skillID{skillID} no match Template{skillTableData.TemplateID}");
                    return null;
                }
                var oneSkill = SkillFactory.CreateNewSkill();
                
                oneSkill.SkillID = skillID;
                oneSkill.CurSkillType = (SkillType)skillTableData.SkillType;
                oneSkill.SkillRange = skillTableData.SkillRange;
                oneSkill.Caster = this;
                oneSkill.DefaultAnimationDurationMs = skillTableData.AniDuration;
                oneSkill.ShakeBeforeMs = skillTableData.BeforeShakeEndMs;
                oneSkill.CurSkillCastTargetType = (SkillCastTargetType)skillTableData.TargetType;
                oneSkill.DefaultSkillCDMs = skillTableData.CDMs;
                var temp = skillTemplates[skillTableData.TemplateID].SkillTemplate;
                temp.Clone(oneSkill);
                oneSkill.OwnTriggerList.Owner = this;
                oneSkill.SetSkillValue(skillTableData);
                return oneSkill;
            }
            return null;
        }
        public void AddTriggerListen(OneTrigger oneTrigger)
        {
            if (oneTrigger == null||CurTriggerDic == null)
            {
                return;
            }
            var triggerType = oneTrigger.CurTriggerType;
            if (!CurTriggerDic.ContainsKey(triggerType))
            {
                CurTriggerDic.Add(triggerType,ListPool<OneTrigger>.Get());
            }

            var curTypeList = CurTriggerDic[triggerType];
            curTypeList.Add(oneTrigger);
        }
        public void RemoveTriggerListen(OneTrigger oneTrigger)
        {
            if (oneTrigger == null||CurTriggerDic == null)
            {
                return;
            }
            var triggerType = oneTrigger.CurTriggerType;
            if (!CurTriggerDic.ContainsKey(triggerType))
            {
                return;
            }

            var curTypeList = CurTriggerDic[triggerType];
            curTypeList.Remove(oneTrigger);
            if (curTypeList.Count == 0)
            {
                CurTriggerDic.Remove(triggerType);
                ListPool<OneTrigger>.Release(curTypeList);
            }
        }
        
        public void OnTrigger(TriggerType triggerType, object arg = null)
        {
            if (CurTriggerDic == null||!CurTriggerDic.ContainsKey(triggerType)||CurTriggerDic[triggerType]==null||CurTriggerDic[triggerType].Count==0)
            {
                return;
            }

            var typeList = CurTriggerDic[triggerType];
            foreach (var oneTrigger in typeList)
            {
                oneTrigger.OnTrigger(arg);
            }
        }

        public override void AddBuff(Buff buff)
        {
            if (buff == null)
            {
                return;
            }
            // todo 判断是否有 免疫之类的判断
            CurBuffList.Add(buff);
            buff.OnActive();
        }

        private void CastPassiveSkill()
        {
            foreach (var passiveSkill in PassiveSkillList)
            {
                passiveSkill.Cast();
            }
        }
        private void UpdateSkill(float elapseSeconds, float realElapseSeconds)
        {
            if (CurTriggerDic != null && CurTriggerDic.TryGetValue(TriggerType.PerTick, out var perTickList))
            {
                if (perTickList != null && perTickList.Count > 0)
                {
                    List<OneTrigger> tempList = ListPool<OneTrigger>.Get();
                    tempList.AddRange(perTickList);
                    foreach (var oneT in tempList)
                    {
                        oneT.OnTrigger();
                    }
                    ListPool<OneTrigger>.Release(tempList);
                }
            }
            UpdateBuffTime(elapseSeconds, realElapseSeconds);
            foreach (var oneNormalSkill in NormalSkillList)
            {
                oneNormalSkill.LogicUpdate(elapseSeconds,realElapseSeconds);
            }

            foreach (var noAnimSkill in NoAnimAtkSkillList)
            {
                noAnimSkill.LogicUpdate(elapseSeconds,realElapseSeconds);
                if (!noAnimSkill.InCD)
                {
                    noAnimSkill.Cast();
                }
            }
        }

        private void UpdateBuffTime(float elapseSeconds, float realElapseSeconds)
        {
            List<Buff> tempBuffList = ListPool<Buff>.Get();
            tempBuffList.AddRange(CurBuffList);
            foreach (var buff in tempBuffList)
            {
                if (buff.IsValid == false)
                {
                    continue;
                }
                buff.RemainMs += elapseSeconds*1000;
                if (buff.DurationMs!=0&&buff.RemainMs >= buff.DurationMs)
                {
                    buff.OnDestory();
                }
            }

            for (var i = CurBuffList.Count - 1; i >= 0; i--)
            {
                if (CurBuffList[i].IsValid == false)
                {
                    CurBuffList.RemoveAt(i);
                }
            }
        }
        public CheckCastSkillResult CheckCanCastSkill(out EntityQizi target,bool isSpSkill = false,int normalSkillIndex = 0)
        {
            target = null;
            //判断是否需要蓝量
            if (isSpSkill)
            {
                if (power.value < 1)
                {
                    return CheckCastSkillResult.NoPower;
                }
            }

            Skill willCastSkill = null;
            //判断目标
            if (isSpSkill)
            {
                willCastSkill = SpSkill;
            }
            else
            {
                if (normalSkillIndex < NormalSkillList.Count)
                {
                    willCastSkill = NormalSkillList[normalSkillIndex];
                }
            }
            if (willCastSkill == null)
            {
                return CheckCastSkillResult.Error;
            }

            var inAttackRange = false;
            if (willCastSkill.CurSkillCastTargetType != SkillCastTargetType.NoNeedTarget)
            {
                bool canUseOldTarget = false;
                if (CurAttackTarget != null&&CurAttackTarget.IsValid)
                {
                    //如果之前存在攻击目标
                    //判断是否已死亡
                    //判断是否在范围内
                    int hp = (int)CurAttackTarget.GetAttribute(AttributeType.Hp).GetFinalValue();
                    canUseOldTarget = hp >= 0;
                    //判断是否有不可选中之类
                    canUseOldTarget = canUseOldTarget&&!(Utility.TruncateFloat(GetDistanceSquare(CurAttackTarget),4)  > willCastSkill.SkillRange * willCastSkill.SkillRange);
                    if (canUseOldTarget)
                    {
                        inAttackRange = true;
                        target = CurAttackTarget;
                    }
                }

                if (canUseOldTarget == false)
                {
                    //之前目标不可用，重新选择目标
                    inAttackRange = GetSkillNewTarget(out target, isSpSkill,normalSkillIndex);
                }
            }
            if (target == null)
            {
                return CheckCastSkillResult.NoValidTarget;
            }
            else if (inAttackRange == false)
            {
                return CheckCastSkillResult.TargetOutRange;
            }

            if (isSpSkill == false&&willCastSkill.InCD)
            {
                return CheckCastSkillResult.NormalAtkWait;
            }
            return CheckCastSkillResult.CanCast;
        }
        /// <summary>
        /// 获取技能目标以及是否在攻击范围内
        /// </summary>
        /// <param name="target">技能目标</param>
        /// <param name="isSpSkill"></param>
        /// <returns>是否在攻击范围内</returns>
        public bool GetSkillNewTarget(out EntityQizi target, bool isSpSkill = false,int normalSkillIndex =0)
        {
            target = null;
            Skill willCastSkill = null;
            //判断目标
            if (isSpSkill)
            {
                willCastSkill = SpSkill;
            }
            else
            {
                if (normalSkillIndex < NormalSkillList.Count)
                {
                    willCastSkill = NormalSkillList[normalSkillIndex];
                }
            }
            if (willCastSkill == null)
            {
                return false;
            }
            bool inAttackRange = false;
            switch (willCastSkill.CurSkillCastTargetType)
            {
                case SkillCastTargetType.NearestEnemy:
                    return GameEntry.HeroManager.GetNearestTarget(this,CampType.Enemy,out target,willCastSkill.SkillRange);
                case SkillCastTargetType.NoNeedTarget:
                    return true;
            }
            return false;
        }

        public void CastSkill(bool isSpSkill,int normalSkillIndex = 0)
        {
            if (isSpSkill)
            {
                SpSkill?.Cast();
            }
            else
            {
                if (normalSkillIndex < NormalSkillList.Count)
                {
                    NormalSkillList[normalSkillIndex].Cast();
                }
            }
        }

        public void BeCauseDamage(CauseDamageData damageData)
        {
            if (damageData == null)
            {
                return;
            }
            //todo 计算护盾
            var hpAttr = GetAttribute(AttributeType.Hp);
            hpAttr.AddNum(-(int)damageData.DamageValue);
            damageData.Caster.OnTrigger(TriggerType.AfterCauseDamage,damageData);
            OnTrigger(TriggerType.AfterBeCauseDamage,damageData);
            
            GameEntry.HeroManager.ShowDamageNum(damageData);
            
            var curHp = (int)hpAttr.GetFinalValue();
            if (curHp == 0)
            {
                //death
                OnDead();
            }
        }

        private void DestorySkill()
        {
            NormalSkillList.Clear();
            NoAnimAtkSkillList.Clear();
            SpSkill = null;
            PassiveSkillList.Clear();
            CurBuffList.Clear();
            CurTriggerDic.Clear();
        }
    }
}