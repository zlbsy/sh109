using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using UnityEngine.UI;
using App.Util.Cacher;
using Holoville.HOTween;
using App.View.Top;
using App.Controller.Common;
using System.Linq;
using App.Controller.Battle;


namespace App.Controller.Common{
    public partial class CBaseMap {
        #region LSharp处理
        public void AddCharacter(int npcId, ActionType action, Direction direction, int x, int y){
            MCharacter mCharacter = NpcCacher.Instance.GetFromNpc(npcId);
            mCharacter.Belong = Belong.friend;
            mCharacter.StatusInit();
            mCharacter.Action = action;
            VTile vTile = this.mapSearch.GetTile(x, y);
            mCharacter.X = vTile.transform.localPosition.x;
            mCharacter.Y = vTile.transform.localPosition.y;

            mCharacter.CoordinateX = x;
            mCharacter.CoordinateY = y;
            mCharacter.Direction = direction;
            List<MCharacter> characters = mBaseMap.Characters == null ? new List<MCharacter>() : mBaseMap.Characters.ToList();
            characters.Add(mCharacter);
            mBaseMap.Characters = characters.ToArray();
        }
        public IEnumerator SetAction(MCharacter mCharacter, ActionType action){
            MapMoveToPosition(mCharacter.CoordinateX, mCharacter.CoordinateY);
            mCharacter.Action = action;
            if (mCharacter.Action != ActionType.idle && mCharacter.Action != ActionType.move)
            {
                while (mCharacter.Action == action)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            App.Util.LSharp.LSharpScript.Instance.Analysis();
        }
        public void SetNpcAction(int npcId, ActionType action){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.Id == npcId && c.Belong != Belong.self);
            this.StartCoroutine(SetAction(mCharacter, action));
        }
        public void SetSelfAction(int index, ActionType action){
            int characterId = (this as App.Controller.Battle.CBattlefield).characterIds[index];
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.CharacterId == characterId && c.Belong == Belong.self);
            this.StartCoroutine(SetAction(mCharacter, action));
        }
        public void SetNpcDirection(int npcId, Direction direction){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.Id == npcId && c.Belong != Belong.self);
            mCharacter.Direction = direction;
        }
        public void SetSelfDirection(int index, Direction direction){
            int characterId = (this as App.Controller.Battle.CBattlefield).characterIds[index];
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.CharacterId == characterId && c.Belong == Belong.self);
            mCharacter.Direction = direction;
        }
        public void HideNpc(int npcId, bool isHide){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.Id == npcId && c.Belong != Belong.self);
            mCharacter.IsHide = isHide;
        }
        public void HideCharacter(int characterId, bool isHide){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.CharacterId == characterId && c.Belong == Belong.self);
            mCharacter.IsHide = isHide;
        }
        public void MoveNpc(int npcId, int x, int y){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.Id == npcId && c.Belong != Belong.self);
            MoveCharacter(mCharacter, x, y);
        }
        /*public void MovePlayer(int x, int y){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.CharacterId >= App.Util.Global.Constant.user_characters[0] && c.CharacterId <= App.Util.Global.Constant.user_characters[1]);
            MoveCharacter(mCharacter, x, y);
        }*/
        public void MoveSelf(int index, int x, int y){
            int characterId = (this as App.Controller.Battle.CBattlefield).characterIds[index];
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.CharacterId == characterId && c.Belong == Belong.self);
            MoveCharacter(mCharacter, x, y);
        }
        public void SetNpcMission(int npcId, App.Model.Mission mission){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.Id == npcId && c.Belong != Belong.self);
            mCharacter.Mission = mission;
        }
        public void MoveCharacter(int characterId, int x, int y){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.CharacterId == characterId && c.Belong == Belong.self);
            MoveCharacter(mCharacter, x, y);
        }
        private void MoveCharacter(MCharacter mCharacter, int x, int y){
            MapMoveToPosition(mCharacter.CoordinateX, mCharacter.CoordinateY);
            VTile startTile = mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY);
            VTile endTile = mapSearch.GetTile(x, y);
            List<VTile> tiles = aStar.Search(mCharacter, startTile, endTile);

            Holoville.HOTween.Core.TweenDelegate.TweenCallback moveComplete = () =>
                {
                    mCharacter.CoordinateY = endTile.CoordinateY;
                    mCharacter.CoordinateX = endTile.CoordinateX;
                    mCharacter.Action = ActionType.idle;
                    MapMoveToPosition(mCharacter.CoordinateX, mCharacter.CoordinateY);
                    App.Util.LSharp.LSharpScript.Instance.Analysis();
                };
            if (tiles.Count > 0)
            {
                mCharacter.Action = ActionType.move;
                Sequence sequence = new Sequence();
                foreach (VTile tile in tiles)
                {
                    TweenParms tweenParms = new TweenParms().Prop("X", tile.transform.localPosition.x, false).Prop("Y", tile.transform.localPosition.y, false).Ease(EaseType.Linear);
                    if (tile.Index == endTile.Index)
                    {
                        tweenParms.OnComplete(moveComplete);
                    }
                    else
                    {
                        tweenParms.OnComplete(()=>{
                            MapMoveToPosition(tile.CoordinateX, tile.CoordinateY);
                        });
                    }
                    sequence.Append(HOTween.To(mCharacter, 0.5f, tweenParms));
                }
                sequence.Play();
            }
            else
            {
                moveComplete();
            }
        }
        public void AddCharacterSkill(int characterId, App.Model.Belong belong, int skillId, int skillLevel){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.CharacterId == characterId && c.Belong == belong);
            App.Model.MSkill mSkill = new App.Model.MSkill();
            mSkill.Id = mCharacter.Skills[mCharacter.Skills.Length - 1].Id + 1;
            mSkill.SkillId = skillId;
            mSkill.Level = skillLevel;
            List<App.Model.MSkill> skills = mCharacter.Skills.ToList();
            skills.Add(mSkill);
            mCharacter.Skills = skills.ToArray();
            if (App.Model.Master.MSkill.IsSkillType(mSkill.Master, App.Model.SkillType.ability))
            {
                int hp = mCharacter.Hp;
                int mp = mCharacter.Mp;
                mCharacter.StatusInit();
                mCharacter.Hp = hp + mSkill.Master.hp;
                mCharacter.Mp = mp + mSkill.Master.mp;
            }
        }
        public void RemoveCharacterSkill(int characterId, App.Model.Belong belong, int skillId){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.CharacterId == characterId && c.Belong == belong);
            List<App.Model.MSkill> skills = mCharacter.Skills.ToList();
            int index = skills.FindIndex(s=>s.SkillId == skillId);
            skills.RemoveAt(index);
            mCharacter.Skills = skills.ToArray();
            if (mCharacter.CurrentSkill.SkillId == skillId)
            {
                mCharacter.CurrentSkill = System.Array.Find(mCharacter.Skills, s=>App.Model.Master.MSkill.IsWeaponType(s.Master, mCharacter.WeaponType));

            }
        }
        public void MapMoveToPosition(int x, int y){
            this.vBaseMap.MoveToPosition(x, y);
        }
        public void NpcFocus(int npcId){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.Id == npcId && c.Belong != Belong.self);
            if (mCharacter == null)
            {
                return;
            }
            MapMoveToPosition(mCharacter.CoordinateX, mCharacter.CoordinateY);
        }
        public void CharacterFocus(int characterId){
            if (mBaseMap.Characters == null)
            {
                return;
            }
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.CharacterId == characterId);
            if (mCharacter == null)
            {
                return;
            }
            MapMoveToPosition(mCharacter.CoordinateX, mCharacter.CoordinateY);
        }
        #endregion
	}
}