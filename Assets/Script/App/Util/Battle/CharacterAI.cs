using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model;
using App.Controller;
using App.Util.Cacher;
using App.View;
using App.Controller.Battle;
using App.View.Character;

namespace App.Util.Battle{
    /// <summary>
    /// 战场武将操作相关
    /// </summary>
    public class CharacterAI{
        private CBattlefield cBattlefield;
        private MBaseMap mBaseMap;
        //private VBaseMap vBaseMap;
        //private App.Model.Master.MBaseMap baseMapMaster;
        private Belong belong;
        private MCharacter mCharacter;
        private MCharacter attackTarget = null;
        private VTile targetTile = null;
        public CharacterAI(CBattlefield controller, MBaseMap model, VBaseMap view){
            cBattlefield = controller;
            mBaseMap = model;
            //vBaseMap = view;
            //baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
        }
        public void Execute(Belong belong){
            this.belong = belong;
            cBattlefield.StartCoroutine(Execute());
        }
        public IEnumerator Execute(){
            //行动顺序
            MCharacter[] characters = System.Array.FindAll(mBaseMap.Characters, c=>c.Belong == this.belong && c.Hp > 0 && !c.IsHide && !c.ActionOver);
            System.Array.Sort(characters, (a, b)=>{
                VTile aTile = cBattlefield.mapSearch.GetTile(a.CoordinateX, a.CoordinateY);
                VTile bTile = cBattlefield.mapSearch.GetTile(b.CoordinateX, b.CoordinateY);
                App.Model.Master.MTile aMTile = TileCacher.Instance.Get(aTile.TileId);
                App.Model.Master.MTile bMTile = TileCacher.Instance.Get(bTile.TileId);
                //恢复地形
                if(aMTile.heal > bMTile.heal){
                    return 1;
                }else if(aMTile.heal > bMTile.heal){
                    return -1;
                }
                bool aPant = a.IsPant;
                bool bPant = b.IsPant;
                //残血状态
                if(aPant && !bPant){
                    return 1;
                }else if(!aPant && bPant){
                    return -1;
                }
                bool aMagic = a.WeaponType == WeaponType.magic;
                bool bMagic = b.WeaponType == WeaponType.magic;
                bool aHeal = a.CanHeal;
                bool bHeal = b.CanHeal;
                //攻击型法师
                if(aMagic && !bMagic && !aHeal){
                    return 1;
                }else if(!aMagic && bMagic && !bHeal){
                    return -1;
                }
                bool aArchery = a.IsArcheryWeapon;
                bool bArchery = b.IsArcheryWeapon;
                //远程类
                if(aArchery && !bArchery){
                    return 1;
                }else if(!aArchery && bArchery){
                    return -1;
                }
                //近战类
                if(!aMagic && bMagic){
                    return 1;
                }else if(aMagic && !bMagic){
                    return -1;
                }
                //恢复型法师
                return 0;
            });
            mCharacter = characters[0];
            MSkill attackSkill = System.Array.Find(mCharacter.Skills, delegate(MSkill skill){
                App.Model.Master.MSkill skillMaster = skill.Master;
                return System.Array.Exists(skillMaster.types, s=>(s==SkillType.attack || s==SkillType.magic)) 
                    && System.Array.IndexOf(skillMaster.weapon_types, mCharacter.WeaponType) >= 0;
            });
            mCharacter.CurrentSkill = attackSkill;
            int index = cBattlefield.mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY).Index;
            cBattlefield.manager.ClickNoneNode(index);
            yield return new WaitForEndOfFrame();
            FindAttackTarget();
            //yield return cBattlefield.StartCoroutine();
            bool canKill = false;
            if (targetTile != null)
            {
                canKill = cBattlefield.calculateManager.Hert(mCharacter, attackTarget, targetTile) - attackTarget.Hp >= 0;
            }
            if (canKill)
            {
                yield return cBattlefield.StartCoroutine(Attack());
            }
            else
            {
                bool needHeal = false;
                MSkill healSkill = System.Array.Find(mCharacter.Skills, delegate(MSkill skill){
                    App.Model.Master.MSkill skillMaster = skill.Master;
                    return System.Array.Exists(skillMaster.types, s=>s==SkillType.heal) 
                        && System.Array.IndexOf(skillMaster.weapon_types, mCharacter.WeaponType) >= 0;
                });
                if (healSkill != null)
                {
                    mCharacter.CurrentSkill = healSkill;
                    cBattlefield.manager.CharacterReturnNone();
                    cBattlefield.manager.ClickNoneNode(index);
                    yield return new WaitForEndOfFrame();
                    MCharacter healTarget = null;
                    VTile healTile = null;
                    FindHealTarget(out healTarget, out healTile);
                    if (healTarget != null)
                    {
                        attackTarget = healTarget;
                        targetTile = healTile;
                        needHeal = true;
                    }
                }
                if (needHeal)
                {
                    yield return cBattlefield.StartCoroutine(Heal());
                    mCharacter.CurrentSkill = attackSkill;
                }
                else
                {
                    if (healSkill != null)
                    {
                        cBattlefield.manager.CharacterReturnNone();
                        cBattlefield.manager.ClickNoneNode(index);
                        yield return new WaitForEndOfFrame();
                        mCharacter.CurrentSkill = attackSkill;
                    }
                    yield return cBattlefield.StartCoroutine(Attack());
                }
            }
        }

        private IEnumerator MoveToNearestTarget(){
            List<VTile> tileList = null;
            foreach (MCharacter character in mBaseMap.Characters)
            {
                if (character.Hp == 0 || character.IsHide)
                {
                    continue;
                }
                if (cBattlefield.charactersManager.IsSameBelong(mCharacter.Belong, character.Belong))
                {
                    continue;
                }
                VTile startTile = cBattlefield.mapSearch.GetTile(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
                VTile endTile = cBattlefield.mapSearch.GetTile(character.CoordinateX, character.CoordinateY);
                List<VTile> tiles = cBattlefield.aStar.Search(this.mCharacter, startTile, endTile);
                if (tileList == null || tileList.Count > tiles.Count)
                {
                    tileList = tiles;
                }
            }
            for (int i = tileList.Count - 1; i >= 0; i--)
            {
                VTile tile = tileList[i];
                if (!cBattlefield.tilesManager.IsInMovingCurrentTiles(tile))
                {
                    continue;
                }
                MCharacter character = System.Array.Find(mBaseMap.Characters, chara=>chara.Hp > 0 && !chara.IsHide && chara.CoordinateX == tile.CoordinateX && chara.CoordinateY == tile.CoordinateY);
                if (character != null)
                {
                    continue;
                }
                cBattlefield.manager.ClickMovingNode(tile.Index);
                break;
            }
            do{
                yield return new WaitForEndOfFrame();
            }
            while (cBattlefield.battleMode == CBattlefield.BattleMode.moving);
            yield return new WaitForEndOfFrame();
        }
        private IEnumerator WaitMoving(){
            if (targetTile != null)
            {
                cBattlefield.manager.ClickMovingNode(targetTile.Index);
            }
            do{
                yield return new WaitForEndOfFrame();
            }
            while (cBattlefield.battleMode == CBattlefield.BattleMode.moving);
            yield return new WaitForEndOfFrame();
        }
        private void FindAttackTarget(){
            attackTarget = null;
            targetTile = null;
            if (mCharacter.CurrentSkill == null)
            {
                return;
            }
            float tileAid = 0;
            foreach (MCharacter character in mBaseMap.Characters)
            {
                if (character.Hp == 0 || character.IsHide)
                {
                    continue;
                }
                if (cBattlefield.charactersManager.IsSameBelong(mCharacter.Belong, character.Belong))
                {
                    continue;
                }
                VTile vTile = GetNearestNode(character, cBattlefield.tilesManager.CurrentMovingTiles);
                //可否攻击
                bool canAttack = cBattlefield.charactersManager.IsInSkillDistance(character.CoordinateX, character.CoordinateY, vTile.CoordinateX, vTile.CoordinateY, mCharacter);
                if (!canAttack)
                {
                    continue;
                }
                if (attackTarget == null)
                {
                    attackTarget = character;
                    targetTile = vTile;
                    tileAid = 0;
                    continue;
                }
                //是否可杀死
                bool aCanKill = cBattlefield.calculateManager.Hert(mCharacter, attackTarget, targetTile) - attackTarget.Hp >= 0;
                if (aCanKill)
                {
                    continue;
                }
                bool bCanKill = cBattlefield.calculateManager.Hert(mCharacter, character, targetTile) - character.Hp >= 0;
                if (!aCanKill && bCanKill)
                {
                    attackTarget = character;
                    targetTile = vTile;
                    tileAid = 0;
                    continue;
                }
                //是否反击
                bool aCanCounter = cBattlefield.calculateManager.CanCounterAttack(mCharacter, attackTarget, targetTile.CoordinateX, targetTile.CoordinateY, attackTarget.CoordinateX, attackTarget.CoordinateY);
                bool bCanCounter = cBattlefield.calculateManager.CanCounterAttack(mCharacter, character, vTile.CoordinateX, vTile.CoordinateY, character.CoordinateX, character.CoordinateY);
                if (!aCanCounter && bCanCounter)
                {
                    continue;
                }else if (aCanCounter && !bCanCounter)
                {
                    attackTarget = character;
                    targetTile = vTile;
                    tileAid = 0;
                    continue;
                }
                //地形优势
                float aTileAid = tileAid;
                if (aTileAid == 0)
                {
                    aTileAid = attackTarget.TileAid(targetTile);
                }
                float bTileAid = character.TileAid(vTile);
                if (aTileAid > bTileAid)
                {
                    attackTarget = character;
                    targetTile = vTile;
                    tileAid = bTileAid;
                    continue;
                }

            }
        }
        private void FindHealTarget(out MCharacter healTarget, out VTile healTile){
            healTarget = null;
            healTile = null;
            foreach (MCharacter character in mBaseMap.Characters)
            {
                if (character.Hp == 0 || character.IsHide)
                {
                    continue;
                }
                if (!cBattlefield.charactersManager.IsSameBelong(mCharacter.Belong, character.Belong))
                {
                    continue;
                }
                if (character.Hp * 1f / character.Ability.HpMax > App.Util.Global.Constant.weak_hp)
                {
                    continue;
                }
                VTile vTile = GetNearestNode(character, cBattlefield.tilesManager.CurrentMovingTiles);
                bool canAttack = cBattlefield.charactersManager.IsInSkillDistance(character.CoordinateX, character.CoordinateY, vTile.CoordinateX, vTile.CoordinateY, mCharacter);
                if (!canAttack)
                {
                    continue;
                }
                if (healTarget == null)
                {
                    healTarget = character;
                    healTile = vTile;
                    continue;
                }
                if (character.Hp < healTarget.Hp)
                {
                    healTarget = character;
                    healTile = vTile;
                }
            }
        }
        private VTile GetNearestNode(MCharacter target, List<VTile> tiles){
            if (mCharacter.Mission == Mission.defensive)
            {
                return tiles.Find(t=>t.CoordinateX == mCharacter.CoordinateX && t.CoordinateY == mCharacter.CoordinateY);
            }
            if (cBattlefield.tilesManager.CurrentMovingTiles.Count == 1)
            {
                return cBattlefield.tilesManager.CurrentMovingTiles[0];
            }
            tiles.Sort((a, b)=>{
                bool aNotRoad = System.Array.Exists(mBaseMap.Characters, c=>c.Hp > 0 && !c.IsHide && c.CoordinateX == a.CoordinateX && c.CoordinateY == a.CoordinateY);
                if(aNotRoad){
                    return 1;
                }
                bool aCanAttack = cBattlefield.charactersManager.IsInSkillDistance(target.CoordinateX, target.CoordinateY, a.CoordinateX, a.CoordinateY, mCharacter);
                bool bCanAttack = cBattlefield.charactersManager.IsInSkillDistance(target.CoordinateX, target.CoordinateY, b.CoordinateX, b.CoordinateY, mCharacter);
                if(aCanAttack && !bCanAttack){
                    return -1;
                }else if(!aCanAttack && bCanAttack){
                    return 1;
                }else if(aCanAttack && bCanAttack){
                    bool aCanCounter = cBattlefield.calculateManager.CanCounterAttack(mCharacter, target, a.CoordinateX, a.CoordinateY, target.CoordinateX, target.CoordinateY);
                    bool bCanCounter = cBattlefield.calculateManager.CanCounterAttack(mCharacter, target, b.CoordinateX, b.CoordinateY, target.CoordinateX, target.CoordinateY);
                    if(aCanCounter && !bCanCounter){
                        return 1;
                    }else if(!aCanCounter && bCanCounter){
                        return -1;
                    }else if(aCanCounter && bCanCounter){
                        //地形优势
                        float aTileAid = mCharacter.TileAid(a);
                        float bTileAid = mCharacter.TileAid(b);
                        if(aTileAid < bTileAid){
                            return -1;
                        }else if(aTileAid > bTileAid){
                            return 1;
                        }
                    }
                }
                int aDistance = cBattlefield.mapSearch.GetDistance(mCharacter.CoordinateX, mCharacter.CoordinateY, a.CoordinateX, a.CoordinateY);
                int bDistance = cBattlefield.mapSearch.GetDistance(mCharacter.CoordinateX, mCharacter.CoordinateY, b.CoordinateX, b.CoordinateY);
                return aDistance - bDistance;
            });
            return cBattlefield.tilesManager.CurrentMovingTiles[0];
        }
        private IEnumerator Attack(){
            yield return cBattlefield.StartCoroutine(WaitMoving());
            if (targetTile == null)
            {
                if(mCharacter.Mission == Mission.initiative){
                    //向最近武将移动
                    yield return cBattlefield.StartCoroutine(MoveToNearestTarget());
                }
                cBattlefield.StartCoroutine(cBattlefield.manager.ActionOver());
            }
            else
            {
                //攻击
                VTile vTile = cBattlefield.mapSearch.GetTile(attackTarget.CoordinateX, attackTarget.CoordinateY);
                cBattlefield.manager.ClickSkillNode(vTile.Index);
            }
        }
        private IEnumerator Heal(){
            yield return cBattlefield.StartCoroutine(WaitMoving());
            VTile vTile = cBattlefield.mapSearch.GetTile(attackTarget.CoordinateX, attackTarget.CoordinateY);
            cBattlefield.manager.ClickSkillNode(vTile.Index);
            while (!mCharacter.ActionOver)
            {
                yield return 0;
            }
        }
        public void MoveAfterAttack(){
            List<VTile> vTiles = cBattlefield.tilesManager.CurrentMovingTiles;
            VTile vTile = cBattlefield.mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY);
            VTile fTile = cBattlefield.mapSearch.GetTile(cBattlefield.manager.oldCoordinate[0], cBattlefield.manager.oldCoordinate[1]);
            vTiles.Sort((a, b)=>{
                int vA = cBattlefield.mapSearch.GetDistance(a, vTile);
                int vB = cBattlefield.mapSearch.GetDistance(b, vTile);
                if(vA != vB){
                    return vB - vA;
                }
                int fA = cBattlefield.mapSearch.GetDistance(a, fTile);
                int fB = cBattlefield.mapSearch.GetDistance(b, fTile);
                return fA - fB;
            });
            vTile = vTiles[0];
            if (cBattlefield.charactersManager.GetCharacter(vTile.Index) == null)
            {
                cBattlefield.manager.ClickMovingNode(vTile.Index);
            }
            else
            {
                cBattlefield.StartCoroutine(cBattlefield.manager.ActionOverNext());
            }
        }
    }
}