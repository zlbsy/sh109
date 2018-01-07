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
    public class BattleCharactersManager{
        private CBattlefield cBattlefield;
        private MBaseMap mBaseMap;
        private VBaseMap vBaseMap;
        private App.Model.Master.MBaseMap baseMapMaster;
        public BattleCharactersManager(CBattlefield controller, MBaseMap model, VBaseMap view){
            cBattlefield = controller;
            mBaseMap = model;
            vBaseMap = view;
            baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
        }
        /// <summary>
        /// 是否在攻击范围内
        /// </summary>
        public bool IsInSkillDistance(MCharacter checkCharacter, MCharacter distanceCharacter){
            //Debug.LogError("checkCharacter = " + checkCharacter);
            //Debug.LogError("distanceCharacter = " + distanceCharacter);
            return IsInSkillDistance(checkCharacter.CoordinateX, checkCharacter.CoordinateY, distanceCharacter.CoordinateX, distanceCharacter.CoordinateY, distanceCharacter);
        }
        /// <summary>
        /// 是否在攻击范围内
        /// </summary>
        public bool IsInSkillDistance(int CoordinateX, int CoordinateY, int targetX, int targetY, MCharacter distanceCharacter){
            return IsInSkillDistance(CoordinateX, CoordinateY, targetX, targetY, distanceCharacter, distanceCharacter.CurrentSkill);
        }
        /// <summary>
        /// 是否在攻击范围内
        /// </summary>
        public bool IsInSkillDistance(int CoordinateX, int CoordinateY, int targetX, int targetY, MCharacter distanceCharacter, MSkill targetSkill){
            //MSkill targetSkill = distanceCharacter.CurrentSkill;
            App.Model.Master.MSkill targetSkillMaster = targetSkill.Master;
            int distance = cBattlefield.mapSearch.GetDistance(CoordinateX, CoordinateY, targetX, targetY);
            if (distance >= targetSkillMaster.distance[0] && distance <= targetSkillMaster.distance[1])
            {
                return true;
            }
            //技能攻击扩展范围
            List<int[]> distances = distanceCharacter.SkillDistances;
            if (distances.Count == 0)
            {
                return false;
            }
            foreach (int[] child in distances)
            {
                if (distance >= child[0] && distance <= child[1])
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取攻击到的所有敌人
        /// </summary>
        /// <returns>The damage characters.</returns>
        /// <param name="vCharacter">攻击方</param>
        /// <param name="targetView">攻击目标</param>
        /// <param name="skill">Skill.</param>
        public List<VCharacter> GetTargetCharacters(VCharacter vCharacter, VCharacter targetView, App.Model.Master.MSkill skill){
            List<VCharacter> result = new List<VCharacter>(){ targetView };
            if (skill.radius_type == RadiusType.point)
            {
                return result;
            }
            List<VCharacter> characters;
            if (System.Array.Exists(skill.types, s => s == SkillType.heal))
            {
                characters = vBaseMap.Characters.FindAll(c=>c.ViewModel.Hp.Value > 0 && this.IsSameBelong(c.ViewModel.Belong.Value, vCharacter.ViewModel.Belong.Value) && !this.IsSameCharacter(targetView,c));
            }
            else/* if (System.Array.Exists(skill.types, s => s == SkillType.attack))*/
            {
                characters = vBaseMap.Characters.FindAll(c=>c.ViewModel.Hp.Value > 0 && this.IsSameBelong(c.ViewModel.Belong.Value, targetView.ViewModel.Belong.Value) && !this.IsSameCharacter(targetView,c));
            }
            VTile targetTile;
            if (skill.effect.special == App.Model.Master.SkillEffectSpecial.attack_all_near)
            {
                targetTile = cBattlefield.mapSearch.GetTile(vCharacter.ViewModel.CoordinateX.Value, vCharacter.ViewModel.CoordinateY.Value);
            }
            else
            {
                targetTile = cBattlefield.mapSearch.GetTile(targetView.ViewModel.CoordinateX.Value, targetView.ViewModel.CoordinateY.Value);
            }
            if (skill.radius_type == RadiusType.range)
            {
                foreach (VCharacter child in characters)
                {
                    VTile tile = cBattlefield.mapSearch.GetTile(child.ViewModel.CoordinateX.Value, child.ViewModel.CoordinateY.Value);
                    if (targetTile.Index != tile.Index && cBattlefield.mapSearch.GetDistance(targetTile, tile) <= skill.radius)
                    {
                        result.Add(child);
                    }
                }
                bool quantity_plus = skill.effect.special == App.Model.Master.SkillEffectSpecial.quantity_plus;
                if (quantity_plus)
                {
                    List<VCharacter> resultPlus = new List<VCharacter>();
                    while (result.Count > 1 && resultPlus.Count < skill.effect.special_value)
                    {
                        int index = Random.Range(1, result.Count - 1);
                        VCharacter plusView = result[index];
                        resultPlus.Add(plusView);
                        result.RemoveAt(index);
                    }
                    resultPlus.Add(targetView);
                    return resultPlus;
                }
            }else if (skill.radius_type == RadiusType.direction)
            {
                VTile tile = cBattlefield.mapSearch.GetTile(vCharacter.ViewModel.CoordinateX.Value, vCharacter.ViewModel.CoordinateY.Value);
                int distance = cBattlefield.mapSearch.GetDistance(targetTile, tile);
                if (distance > 1)
                {
                    return result;
                }
                var direction = cBattlefield.mapSearch.GetDirection(tile, targetTile);
                var radius = skill.radius;
                while (radius-- > 0)
                {
                    tile = cBattlefield.mapSearch.GetTile(targetTile, direction);
                    VCharacter child = GetCharacter(tile.Index, characters);
                    if (child == null)
                    {
                        break;
                    }
                    result.Add(child);
                    targetTile = tile;
                }
            }
            return result;
        }
        public bool IsSameCharacter(MCharacter character1, MCharacter character2){
            return character1.Belong == character2.Belong && character1.Id == character2.Id;
        }
        public bool IsSameCharacter(VCharacter character1, VCharacter character2){
            return character1.ViewModel.Belong.Value == character2.ViewModel.Belong.Value && character1.ViewModel.Id.Value == character2.ViewModel.Id.Value;
        }
        public bool IsSameBelong(Belong belong1, Belong belong2){
            if (belong1 == Belong.enemy)
            {
                return belong2 == Belong.enemy;
            }
            return belong2 == Belong.self || belong2 == Belong.friend;
        }
        public void ActionRestore(){
            foreach (MCharacter character in this.mBaseMap.Characters)
            {
                if (character.ActionOver)
                {
                    character.ActionOver = false;
                    character.Action = ActionType.idle;
                }
            }
        }
        public MCharacter GetCharacter(int index, MCharacter[] characters = null){
            Vector2 coordinate = baseMapMaster.GetCoordinateFromIndex(index);
            MCharacter mCharacter = System.Array.Find(characters == null ? mBaseMap.Characters : characters, c=>c.Hp > 0 && !c.IsHide && c.CoordinateX == coordinate.x && c.CoordinateY == coordinate.y);
            return mCharacter;
        }
        public App.View.Character.VCharacter GetCharacter(int index, List<App.View.Character.VCharacter> characters){
            Vector2 coordinate = baseMapMaster.GetCoordinateFromIndex(index);
            App.View.Character.VCharacter vCharacter = characters.Find(c=>c.ViewModel.Hp.Value > 0 && !c.ViewModel.IsHide.Value && c.ViewModel.CoordinateX.Value == coordinate.x && c.ViewModel.CoordinateY.Value == coordinate.y);
            return vCharacter;
        }
    }
}