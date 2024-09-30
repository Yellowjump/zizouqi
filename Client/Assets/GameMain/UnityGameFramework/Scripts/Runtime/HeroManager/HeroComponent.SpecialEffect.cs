﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using GameFramework;
using System.Collections.Generic;
using System.IO;
using DataTable;
using Entity;
using Entity.Bullet;
using Maze;
using SkillSystem;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace UnityGameFramework.Runtime
{
    public class SfxEntity:IReference
    {
        public EntityBase Owner;
        public GameObject GObj;
        public float RemainDurationMs;
        public int DurationMs;
        public Vector3 PosOffset= Vector3.zero;
        public Vector3 SizeOffset = Vector3.one;
        public int SfxID;
        public int ExistNum;
        public void Clear()
        {
            GameEntry.HeroManager.ReleaseSfxGameObject(SfxID, GObj, OnGetHeroGObjCallback);
            Owner = null;
            PosOffset= Vector3.zero;
            SizeOffset = Vector3.one;
            SfxID = 0;
            ExistNum = 0;
        }
        public void InitGObj()
        {
            GameEntry.HeroManager.GetSfxByID(SfxID,OnGetHeroGObjCallback);
        }
        protected virtual void OnGetHeroGObjCallback(GameObject obj,string path)
        {
            GObj = obj;
            GObj.transform.localScale = SizeOffset;
            GObj.transform.SetParent(Owner.GObj.transform);
            GObj.transform.localPosition = PosOffset;
        }
    }

    public class WeaponEntity : IReference
    {
        public GameObject GObj;
        public EntityQizi Owner;
        public WeaponHandleType CurHandleType;
        public List<int> WaitLoadWeaponAssetIDList;
        private List<(string,GameObject)> _weaponObjList;
        private List<int> _itemPosOffset;
        public void Init(EntityQizi owner, WeaponHandleType handleType, int itemID)
        {
            Owner = owner;
            CurHandleType = handleType;
            _weaponObjList ??= ListPool<(string,GameObject)>.Get();
            _weaponObjList.Clear();
            WaitLoadWeaponAssetIDList = ListPool<int>.Get();
            var itemTable = GameEntry.DataTable.GetDataTable<DRItem>("Item");
            if (itemTable.HasDataRow(itemID))
            {
                var itemData = itemTable[itemID];
                WaitLoadWeaponAssetIDList.AddRange(itemData.AssetIDList);
                _itemPosOffset ??= ListPool<int>.Get();
                _itemPosOffset.AddRange(itemData.AssetObjLength);
                foreach (var oneID in WaitLoadWeaponAssetIDList)
                {
                    GameEntry.HeroManager.GetPrefabByAssetID(oneID,OnGetOneWeaponObjCallback);
                }
            }
        }
        public void Clear()
        {
            if (_weaponObjList != null&&WaitLoadWeaponAssetIDList!=null&&_weaponObjList.Count==WaitLoadWeaponAssetIDList.Count)
            {
                for (var index = 0; index < _weaponObjList.Count; index++)
                {
                    var assetID = WaitLoadWeaponAssetIDList[index];
                    var onePair = _weaponObjList[index];
                    GameEntry.HeroManager.ReleaseAssetObj(assetID, onePair.Item2,OnGetOneWeaponObjCallback);
                }
                ListPool<int>.Release(WaitLoadWeaponAssetIDList);
                ListPool<(string,GameObject)>.Release(_weaponObjList);
                WaitLoadWeaponAssetIDList = null;
                _weaponObjList = null;
            }
            CurHandleType = WeaponHandleType.None;
            Owner = null;
            if (_itemPosOffset != null)
            {
                ListPool<int>.Release(_itemPosOffset);
                _itemPosOffset = null;
            }

            if (GObj != null)
            {
                GameEntry.HeroManager.ReleaseEmptyObj(GObj);
                GObj = null;
            }
            
        }
        private void OnGetOneWeaponObjCallback(GameObject obj,string path)
        {
            _weaponObjList.Add((path,obj));
            if (_weaponObjList.Count == WaitLoadWeaponAssetIDList.Count)
            {
                //所有需要的asset以获取
                GObj = GameEntry.HeroManager.GetNewEmptyObj();
                // weaponObjList 按照WaitLoadWeaponAssetIDList里的顺序排序
                List<string> weaponSort = ListPool<string>.Get();
                var assetPathTable = GameEntry.DataTable.GetDataTable<DRAssetsPath>("AssetsPath");
                foreach (var assetID in WaitLoadWeaponAssetIDList)
                {
                    if (assetPathTable.HasDataRow(assetID))
                    {
                        var assetData = assetPathTable[assetID];
                        weaponSort.Add(assetData.AssetPath);
                    }
                }
                _weaponObjList.Sort((x, y) => weaponSort.IndexOf(x.Item1).CompareTo(weaponSort.IndexOf(y.Item1)));
                ListPool<string>.Release(weaponSort);
                int posOffset = 0;
                for (var weaponIndex = 0; weaponIndex < _weaponObjList.Count; weaponIndex++)
                {
                    var oneWeaponObjPack = _weaponObjList[weaponIndex];
                    var weaponObj = oneWeaponObjPack.Item2;
                    weaponObj.transform.SetParent(GObj.transform);
                    if (weaponIndex > 0)
                    {
                        weaponObj.transform.localPosition = new Vector3(0,posOffset / 1000f,0);
                    }
                    if (_itemPosOffset != null && _itemPosOffset.Count > weaponIndex)
                    {
                        posOffset += _itemPosOffset[weaponIndex];
                    }
                    weaponObj.SetActive(true);
                }
                var paTrans = Owner.GetWeaponHandle(CurHandleType);
                GObj.transform.SetParent(paTrans);
                GObj.transform.localPosition = Vector3.zero;
                GObj.transform.localRotation = Quaternion.identity;
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }
    public sealed partial class HeroComponent
    {
        
    }
}