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
    /// 战场计算相关
    /// </summary>
    public class BattleCalculateManager{
        private CBattlefield cBattlefield;
        //private MBaseMap mBaseMap;
        //private VBaseMap vBaseMap;
        public BattleCalculateManager(CBattlefield controller, MBaseMap model, VBaseMap view){
            cBattlefield = controller;
            //mBaseMap = model;
            //vBaseMap = view;
        }
        /// <summary>
        /// 攻击命中
        /// 技巧+速度*2
        /// </summary>
        /// <returns><c>true</c>, if hitrate was attacked, <c>false</c> otherwise.</returns>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public bool AttackHitrate(MCharacter attackCharacter, MCharacter targetCharacter){
            //游戏教学时100%命中
            if (Global.SUser.self.IsTutorial)
            {
                return true;
            }
            //技能100%命中
            if (attackCharacter.IsForceHit)
            {
                return true;
            }
            //获取地形辅助
            float tileAid = attackCharacter.TileAid(cBattlefield.mapSearch.GetTile(attackCharacter.CoordinateX, attackCharacter.CoordinateY));
            float targetTileAid = attackCharacter.TileAid(cBattlefield.mapSearch.GetTile(targetCharacter.CoordinateX, targetCharacter.CoordinateY));
            int attackValue = (int)((attackCharacter.Ability.Knowledge + attackCharacter.Ability.Speed * 2) * tileAid);
            int targetValue = (int)((targetCharacter.Ability.Knowledge + targetCharacter.Ability.Speed * 2) * targetTileAid);
            int r;
            if(attackValue > 2*targetValue){
                r = 100;
            }else if(attackValue > targetValue){
                r=(attackValue-targetValue)*10/targetValue+90;
            }else if(attackValue > targetValue * 0.5){
                r=(attackValue-targetValue/2)*30/(targetValue/2)+60;
            }else{
                r=(attackValue-targetValue/3)*30/(targetValue/3)+30;
            }
            int randValue = Random.Range(0, 100);
            if (randValue <= r)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否可反击
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public bool CanCounterAttack(MCharacter attackCharacter, MCharacter targetCharacter, int CoordinateX, int CoordinateY, int targetX, int targetY){
            if (targetCharacter.IsForceBackAttack)
            {
                return true;
            }
            if (attackCharacter.IsNoBackAttack || targetCharacter.CurrentSkill == null)
            {
                return false;
            }
            if (!cBattlefield.charactersManager.IsInSkillDistance(CoordinateX, CoordinateY, targetX, targetY, targetCharacter))
            {
                //不在攻击范围内
                return false;
            }
            if (attackCharacter.MoveType == MoveType.infantry && targetCharacter.MoveType == MoveType.cavalry)
            {
                //步兵攻击骑兵时不受反击
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取反击次数
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public int CounterAttackCount(MCharacter attackCharacter, MCharacter targetCharacter){
            int count = 1;
            if (targetCharacter.WeaponType == WeaponType.dualWield)
            {
                //双手兵器或者相关的技能可双击
                count = 2;
            }
            //技能反击次数
            App.Model.Master.MSkill skillMaster = attackCharacter.CurrentSkill.Master;
            if (skillMaster.effect.special == App.Model.Master.SkillEffectSpecial.counter_attack_count)
            {
                count = skillMaster.effect.special_value;
            }
            return count;
        }
        /// <summary>
        /// 获取主动次数
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public int SkillCount(MCharacter currentCharacter, MCharacter targetCharacter){
            int count = 1;
            if (currentCharacter.WeaponType == WeaponType.dualWield)
            {
                //双手兵器或者相关的技能可双击
                count = 2;
            }
            //技能攻击次数
            App.Model.Master.MSkill skillMaster = currentCharacter.CurrentSkill.Master;
            if (skillMaster.effect.special == App.Model.Master.SkillEffectSpecial.attack_count)
            {
                count = skillMaster.effect.special_value;
            }
            return count;
        }
        /// <summary>
        /// 恢复量=
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public int Heal(MCharacter attackCharacter, MCharacter targetCharacter, VTile tile = null, VTile targetTile = null){
            MSkill skill = attackCharacter.CurrentSkill;
            App.Model.Master.MSkill skillMaster = skill.Master;
            return 1 + attackCharacter.Level + attackCharacter.Ability.MagicAttack * skillMaster.strength;
        }
        /// <summary>
        /// 攻击伤害=技能*0.3+攻击力-防御力*2
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public int Hert(MCharacter attackCharacter, MCharacter targetCharacter, VTile tile = null, VTile targetTile = null){
            //获取地形辅助
            float tileAid = attackCharacter.TileAid(tile);
            float targetTileAid = targetCharacter.TileAid(targetTile);
            MSkill skill = attackCharacter.CurrentSkill;
            App.Model.Master.MSkill skillMaster = skill.Master;
            if (tile == null)
            {
                tile = cBattlefield.mapSearch.GetTile(attackCharacter.CoordinateX, attackCharacter.CoordinateY);
            }
            if (targetTile == null)
            {
                targetTile = cBattlefield.mapSearch.GetTile(targetCharacter.CoordinateX, targetCharacter.CoordinateY);
            }
            float attack = System.Array.Exists(skillMaster.types, s=>s==SkillType.attack) ? attackCharacter.Ability.PhysicalAttack : attackCharacter.Ability.MagicAttack;
            //地形辅助
            attack *= tileAid;
            if (attackCharacter.IsPike && targetCharacter.IsKnife)
            {
                //枪剑类克制刀类
                attack *= 1.2f;
            }else if (attackCharacter.IsKnife && targetCharacter.IsAx)
            {
                //刀类克制斧类
                attack *= 1.2f;
            }else if (attackCharacter.IsAx && targetCharacter.IsPike)
            {
                //斧类克制枪剑类
                attack *= 1.2f;
            }
            float defense = System.Array.Exists(skillMaster.types, s=>s==SkillType.attack) ? targetCharacter.Ability.PhysicalDefense : targetCharacter.Ability.MagicDefense;
            //地形辅助
            defense *= targetTileAid;
            if (attackCharacter.IsLongWeapon && targetCharacter.IsShortWeapon)
            {
                //长兵器克制短兵器
                defense *= 0.8f;
            }else if (attackCharacter.IsShortWeapon && targetCharacter.IsArcheryWeapon)
            {
                //短兵器克制远程兵器
                defense *= 0.8f;
            }else if (attackCharacter.IsArcheryWeapon && targetCharacter.IsLongWeapon)
            {
                //远程类兵器克制长兵器
                defense *= 0.8f;
            }
            App.Model.Master.MTile mTile = TileCacher.Instance.Get(targetTile.TileId);
            //地形对技能威力的影响
            int five_elements = (int)skillMaster.five_elements;
            float skillStrength = skillMaster.strength * mTile.strategys[five_elements];
            //抗性对技能威力的影响
            int resistance = targetCharacter.Master.resistances[five_elements];
            if (resistance > 0)
            {
                skillStrength *= ((100 - resistance) * 0.01f);
            }
            float result = skillStrength * 0.3f + attack - defense;
            //Debug.LogError("result="+result + ", skillMaster.strength="+skillMaster.strength +", attack=" + attack+", defense="+defense);
            if (attackCharacter.MoveType == MoveType.cavalry && targetCharacter.MoveType == MoveType.infantry && !targetCharacter.IsArcheryWeapon)
            {
                //骑兵克制近身步兵
                result *= 1.2f;
            }else if (attackCharacter.IsArcheryWeapon && targetCharacter.MoveType == MoveType.cavalry && !targetCharacter.IsArcheryWeapon)
            {
                //远程类克制近身类骑兵
                result *= 1.2f;
            }else if (attackCharacter.MoveType == MoveType.infantry && targetCharacter.WeaponType != WeaponType.archery && targetCharacter.IsArcheryWeapon)
            {
                //近身步兵克制远程类
                result *= 1.2f;
            }
            if (targetCharacter.MoveType == MoveType.cavalry && skillMaster.effect.special == App.Model.Master.SkillEffectSpecial.horse_hert)
            {
                //对骑兵技能伤害加成
                result *= (1f + skillMaster.effect.special_value * 0.01f);
            }else if (skillMaster.effect.special == App.Model.Master.SkillEffectSpecial.move_and_attack && attackCharacter.roadLength > 0)
            {
                //移动攻击
                result *= (1f + attackCharacter.roadLength * skillMaster.effect.special_value * 0.01f);
            }
            result = result > 1 ? result : 1;
            result = result > targetCharacter.Hp ? targetCharacter.Hp : result;
            return (int)result;
        }
    }
}