using System.IO;
using DataTable;
using Entity;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SkillSystem
{
    public class CommandHuDun:CommandBase
    {
        public override CommandType CurCommandType => CommandType.CreateHuDun;
        public TableParamInt ParamInt1;

        public override void OnExecute(OneTrigger trigger, object arg = null)
        {
            if (trigger.ParentTriggerList is Buff curBuff)
            {
                if (curBuff.CheckBuffTag(BuffTag.Shield))
                {
                    curBuff.paramInt = ParamInt1.Value;
                }
            }
        }

        public override void Clone(CommandBase copy)
        {
            if (copy is CommandHuDun copyCreateBullet)
            {
                ParamInt1.Clone(copyCreateBullet.ParamInt1);
            }
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            ParamInt1.ReadFromFile(reader);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            ParamInt1.WriteToFile(writer);
        }

        public override void SetSkillValue(DataRowBase dataTable)
        {
            ParamInt1.SetSkillValue(dataTable);
        }

        public override void Clear()
        {
            ReferencePool.Release(ParamInt1);
            ParamInt1 = null;
        }
        
    }
}