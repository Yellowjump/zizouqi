using System.Collections.Generic;
using DataTable;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Entity
{
    public partial class EntityQizi
    {
        private List<SfxEntity> SfxList = new List<SfxEntity>();
        public void AddSfx(int sfxID)
        {
            var sfxTable = GameEntry.DataTable.GetDataTable<DRSfx>("Sfx");
            if (sfxTable.HasDataRow(sfxID))
            {
                var sfxData = sfxTable[sfxID];
                SfxEntity oneNewSfx = null;
                if (sfxData.IsOnlyOne)
                {
                    oneNewSfx = SfxList.Find(entity => entity.SfxID == sfxID);
                }

                if (oneNewSfx == null)
                {
                    oneNewSfx = ReferencePool.Acquire<SfxEntity>();
                    oneNewSfx.SfxID = sfxID;
                    oneNewSfx.DurationMs = sfxData.DurationMs;
                    oneNewSfx.Owner = this;
                    oneNewSfx.PosOffset = sfxData.PosOffset;
                    if (sfxData.PosRandom != Vector3.zero)
                    {
                        oneNewSfx.PosOffset +=new Vector3((float)Utility.Random.GetRandomNoLogic(-sfxData.PosRandom.x, sfxData.PosRandom.x),(float)Utility.Random.GetRandomNoLogic(-sfxData.PosRandom.y, sfxData.PosRandom.y),(float)Utility.Random.GetRandomNoLogic(-sfxData.PosRandom.z, sfxData.PosRandom.z))  ;
                    }
                    oneNewSfx.SizeOffset = sfxData.SizeOffset;
                    oneNewSfx.InitGObj();
                    SfxList.Add(oneNewSfx);
                }
                oneNewSfx.RemainDurationMs = 0;//刷新特效存在时间
                oneNewSfx.ExistNum++;
            }
        }

        public void UpdateSfxRemainTime(float elapseSeconds, float realElapseSeconds)
        {
            if (SfxList != null && SfxList.Count > 0)
            {
                for (var index = SfxList.Count-1; index >=0; index--)
                {
                    var oneSfx = SfxList[index];
                    if (oneSfx.DurationMs == 0)//持续时间无线
                    {
                        continue;
                    }
                    oneSfx.RemainDurationMs += elapseSeconds * 1000;
                    if (oneSfx.RemainDurationMs >= oneSfx.DurationMs)
                    {
                        ReferencePool.Release(oneSfx);
                        SfxList.Remove(oneSfx);
                    }
                }
            }
        }
        public void RemoveSfx(int sfxID)
        {
            var oneSfx = SfxList.Find(entity => entity.SfxID == sfxID);
            oneSfx.ExistNum--;
            if (oneSfx.ExistNum <= 0)
            {
                ReferencePool.Release(oneSfx);
                SfxList.Remove(oneSfx);
            }
        }

        private void RemoveAllSfx()
        {
            foreach (var oneSfx in SfxList)
            {
                ReferencePool.Release(oneSfx);
            }
            SfxList.Clear();
        }
    }
}