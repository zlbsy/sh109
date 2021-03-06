﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.Controller;
using App.Model.Scriptable;
using App.Util.Cacher;

namespace App.Util{
    public class AppInitialize {
        private static bool initComplete = false;
        public static IEnumerator Initialize()
        {
            if (initComplete)
            {
                yield break;
            }
            SMaster sMaster = new SMaster();
            yield return SceneManager.CurrentScene.StartCoroutine (sMaster.RequestVersions());
            App.Util.Global.versions = sMaster.versions;
            CLoadingDialog.ToShow();
            yield return SceneManager.CurrentScene.StartCoroutine(LoadAssetbundle( App.Util.Global.versions ));
            CLoadingDialog.ToClose();
            initComplete = true;
        }
        public static IEnumerator LoadAssetbundle(MVersion versions)
        {
            SUser sUser = Global.SUser;
            List<IEnumerator> list = new List<IEnumerator>();
            list.Add(sUser.Download(PromptMessageAsset.Url, versions.prompt_message, (AssetBundle assetbundle)=>{
                PromptMessageAsset.assetbundle = assetbundle;
            }));
            list.Add(sUser.Download(LanguageAsset.WORD_URL, versions.word, (AssetBundle assetbundle)=>{
                LanguageAsset.assetbundle = assetbundle;
                Language.Reset(LanguageAsset.Data.words);
                LanguageAsset.Clear();
            }));
            list.Add(sUser.Download(ImageAssetBundleManager.horseUrl, versions.horse_img, (AssetBundle assetbundle)=>{
                AvatarSpriteAsset.assetbundle = assetbundle;
                ImageAssetBundleManager.horse = AvatarSpriteAsset.Data.meshs;
            }));
            list.Add(sUser.Download(ImageAssetBundleManager.headUrl, versions.head_img, (AssetBundle assetbundle)=>{
                AvatarSpriteAsset.assetbundle = assetbundle;
                ImageAssetBundleManager.head = AvatarSpriteAsset.Data.meshs;
            }));
            list.Add(sUser.Download(ImageAssetBundleManager.clothesUrl, versions.clothes_img, (AssetBundle assetbundle)=>{
                AvatarSpriteAsset.assetbundle = assetbundle;
                ImageAssetBundleManager.clothes = AvatarSpriteAsset.Data.meshs;
            }));
            list.Add(sUser.Download(ImageAssetBundleManager.weaponUrl, versions.weapon_img, (AssetBundle assetbundle)=>{
                AvatarSpriteAsset.assetbundle = assetbundle;
                ImageAssetBundleManager.weapon = AvatarSpriteAsset.Data.meshs;
            }));
            list.Add(sUser.Download(WorldAsset.Url, versions.world, (AssetBundle assetbundle)=>{
                WorldAsset.assetbundle = assetbundle;
                Global.worlds = WorldAsset.Data.worlds;
            }));
            list.Add(sUser.Download(ConstantAsset.Url, versions.constant, (AssetBundle assetbundle)=>{
                ConstantAsset.assetbundle = assetbundle;
                Global.Constant = ConstantAsset.Data.constant;
            }));
            list.Add(sUser.Download(NpcAsset.Url, versions.npc, (AssetBundle assetbundle)=>{
                NpcAsset.assetbundle = assetbundle;
                NpcCacher.Instance.Reset(NpcAsset.Data.npcs);
                NpcAsset.Clear();
            }));
            list.Add(sUser.Download(NpcEquipmentAsset.Url, versions.npc_equipment, (AssetBundle assetbundle)=>{
                NpcEquipmentAsset.assetbundle = assetbundle;
                NpcEquipmentCacher.Instance.Reset(NpcEquipmentAsset.Data.npc_equipments);
                NpcEquipmentAsset.Clear();
            }));
            list.Add(sUser.Download(CharacterStarAsset.Url, versions.characterstar, (AssetBundle assetbundle)=>{
                CharacterStarAsset.assetbundle = assetbundle;
                CharacterStarCacher.Instance.Reset(CharacterStarAsset.Data.characterStars);
                CharacterStarAsset.Clear();
            }));
            /*list.Add(sUser.Download(AreaAsset.Url, versions.area, (AssetBundle assetbundle)=>{
                AreaAsset.assetbundle = assetbundle;
                AreaCacher.Instance.Reset(AreaAsset.Data.areas);
                AreaAsset.Clear();
            }));*/
            list.Add(sUser.Download(ItemAsset.Url, versions.item, (AssetBundle assetbundle)=>{
                ItemAsset.assetbundle = assetbundle;
                ItemCacher.Instance.Reset(ItemAsset.Data.items);
                ItemAsset.Clear();
            }));
            /*list.Add(sUser.Download(MissionAsset.Url, versions.mission, (AssetBundle assetbundle)=>{
                MissionAsset.assetbundle = assetbundle;
                MissionCacher.Instance.Reset(MissionAsset.Data.missions);
                MissionAsset.Clear();
            }));*/
            list.Add(sUser.Download(SkillAsset.Url, versions.skill, (AssetBundle assetbundle)=>{
                SkillAsset.assetbundle = assetbundle;
                SkillCacher.Instance.Reset(SkillAsset.Data.skills);
                SkillAsset.Clear();
            }));
            list.Add(sUser.Download(StrategyAsset.Url, versions.strategy, (AssetBundle assetbundle)=>{
                StrategyAsset.assetbundle = assetbundle;
                StrategyCacher.Instance.Reset(StrategyAsset.Data.strategys);
                StrategyAsset.Clear();
            }));
            list.Add(sUser.Download(ExpAsset.Url, versions.exp, (AssetBundle assetbundle)=>{
                ExpAsset.assetbundle = assetbundle;
                ExpCacher.Instance.Reset(ExpAsset.Data.exps);
                ExpAsset.Clear();
            }));
            list.Add(sUser.Download(BattlefieldAsset.Url, versions.battlefield, (AssetBundle assetbundle)=>{
                BattlefieldAsset.assetbundle = assetbundle;
                BattlefieldCacher.Instance.Reset(BattlefieldAsset.Data.battlefields);
                BattlefieldAsset.Clear();
            }));
            list.Add(sUser.Download(BuildingAsset.Url, versions.building, (AssetBundle assetbundle)=>{
                BuildingAsset.assetbundle = assetbundle;
                BuildingCacher.Instance.Reset(BuildingAsset.Data.buildings);
                BuildingAsset.Clear();
            }));
            list.Add(sUser.Download(BaseMapAsset.Url, versions.top_map, (AssetBundle assetbundle)=>{
                BaseMapAsset.assetbundle = assetbundle;
                BaseMapCacher.Instance.Reset(BaseMapAsset.Data.baseMaps);
                BaseMapAsset.Clear();
            }));
            list.Add(sUser.Download(CharacterAsset.Url, versions.character, (AssetBundle assetbundle)=>{
                CharacterAsset.assetbundle = assetbundle;
                CharacterCacher.Instance.Reset(CharacterAsset.Data.characters);
                CharacterAsset.Clear();
            }));
            list.Add(sUser.Download(TileAsset.Url, versions.tile, (AssetBundle assetbundle)=>{
                TileAsset.assetbundle = assetbundle;
                TileCacher.Instance.Reset(TileAsset.Data.tiles);
                TileAsset.Clear();
            }));
            list.Add(sUser.Download(LoginBonusAsset.Url, versions.loginbonus, (AssetBundle assetbundle)=>{
                LoginBonusAsset.assetbundle = assetbundle;
                LoginBonusCacher.Instance.Reset(LoginBonusAsset.Data.loginbonuses);
                LoginBonusAsset.Clear();
            }));
            list.Add(sUser.Download(ImageAssetBundleManager.mapUrl, versions.map, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.map = assetbundle;
            }, false));
            list.Add(sUser.Download(ImageAssetBundleManager.equipmentIconUrl, versions.equipmenticon_icon, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.equipmentIcon = assetbundle;
            }, false));
            list.Add(sUser.Download(ImageAssetBundleManager.itemIconUrl, versions.item_icon, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.itemIcon = assetbundle;
            }, false));
            list.Add(sUser.Download(ImageAssetBundleManager.skillIconUrl, versions.skill_icon, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.skillIcon = assetbundle;
            }, false));
            list.Add(sUser.Download(HorseAsset.Url, versions.horse, (AssetBundle assetbundle)=>{
                HorseAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetHorse(HorseAsset.Data.equipments);
                HorseAsset.Clear();
            }));
            list.Add(sUser.Download(WeaponAsset.Url, versions.weapon, (AssetBundle assetbundle)=>{
                WeaponAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetWeapon(WeaponAsset.Data.equipments);
                WeaponAsset.Clear();
            }));
            list.Add(sUser.Download(ClothesAsset.Url, versions.clothes, (AssetBundle assetbundle)=>{
                ClothesAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetClothes(ClothesAsset.Data.equipments);
                ClothesAsset.Clear();
            }));
            list.Add(sUser.Download(StoryProgressAsset.Url, versions.character, (AssetBundle assetbundle)=>{
                StoryProgressAsset.assetbundle = assetbundle;
                foreach(string key in StoryProgressAsset.Data.keys){
                    App.Util.LSharp.LSharpVarlable.SetVarlable(key, "0");
                }
                StoryProgressAsset.Clear();
            }));
            if (App.Util.Global.SUser.self != null)
            {
                list.Add(sUser.RequestGet());
            }
            float step = 100f / list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                CLoadingDialog.SetNextProgress((i + 1) * step);
                yield return SceneManager.CurrentScene.StartCoroutine(list[i]);
            }
            yield return 0;
        }
	}
}