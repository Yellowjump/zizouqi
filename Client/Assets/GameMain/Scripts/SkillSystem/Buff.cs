using System;
using System.IO;
using Entity;
using UnityGameFramework.Runtime;

namespace SkillSystem
{
    public class Buff:TriggerList
    {
        public int BuffID;
        public int TempleteID;
        public BuffTag OwnBuffTag;
        public int DurationMs;
        public float RemainMs;
        public bool IsValid = true;
        public override void Clone(TriggerList copy)
        {
            if (copy is Buff copyBuff)
            {
                copyBuff.BuffID = BuffID;
                copyBuff.OwnBuffTag = OwnBuffTag;
            }
            base.Clone(copy);
        }
        public void ReadFromFile(BinaryReader reader)
        {
            TempleteID = reader.ReadInt32();
            OwnBuffTag = GetCombinedTags(reader.ReadInt32());
            base.ReadFromFile(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(TempleteID);
            writer.Write((int)OwnBuffTag);
            base.WriteToFile(writer);
        }
        private BuffTag GetCombinedTags(int combinedValue)
        {
            BuffTag result = BuffTag.None;
            foreach (BuffTag tag in Enum.GetValues(typeof(BuffTag)))
            {
                if ((combinedValue & (int)tag) != 0)
                {
                    result |= tag;
                }
            }
            return result;
        }

        public override void OnDestory()
        {
            if (IsValid == false)
            {
                return;
            }
            IsValid = false;
            base.OnDestory();
        }
    }
}