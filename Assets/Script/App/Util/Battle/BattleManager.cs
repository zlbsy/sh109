using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model;
using App.Controller;
using App.Util.Cacher;
using App.View;
using Holoville.HOTween;
using App.Controller.Battle;
using System.Linq;

namespace App.Util.Battle{
    /// <summary>
    /// 战场总操作相关
    /// </summary>
    public class BattleManager{
        private CBattlefield cBattlefield;
        private MCharacter mCharacter;
        private MBaseMap mBaseMap;
        //private VBaseMap vBaseMap;
        //private App.Model.Master.MBaseMap baseMapMaster;
        private System.Action returnAction;
        public int[] oldCoordinate = new int[]{0,0};
        private List<MCharacter> actionCharacterList = new List<MCharacter>();
        public BattleManager(CBattlefield controller, MBaseMap model, VBaseMap view){
            cBattlefield = controller;
            mBaseMap = model;
            //vBaseMap = view;
            //baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
        }
        public MCharacter CurrentCharacter{
            get{ 
                return this.mCharacter;
            }
        }
        public void ClickNoneNode(int index){
            MCharacter mCharacter = cBattlefield.charactersManager.GetCharacter(index);
            if (mCharacter != null)
            {
                this.mCharacter = mCharacter;
                this.mCharacter.roadLength = 0;
                cBattlefield.tilesManager.ShowCharacterMovingArea(mCharacter);
                cBattlefield.tilesManager.ShowCharacterSkillArea(mCharacter);
                cBattlefield.OpenBattleCharacterPreviewDialog(mCharacter);
                oldCoordinate[0] = mCharacter.CoordinateX;
                oldCoordinate[1] = mCharacter.CoordinateY;
                ActionType action = mCharacter.Action;
                float x = mCharacter.X;
                Direction direction = mCharacter.Direction;
                if (mCharacter.Belong == Belong.self)
                {
                    cBattlefield.OpenOperatingMenu();
                }
                returnAction = () =>
                    {
                        this.mCharacter.CoordinateY = oldCoordinate[1];
                        this.mCharacter.CoordinateX = oldCoordinate[0];
                        this.mCharacter.X = x;
                        this.mCharacter.Direction = direction;
                        this.mCharacter.Action = action;
                };
            }
            //Debug.LogError("this.mCharacter = " + this.mCharacter + ", mCharacter = " + mCharacter);
        }
        public void ClickSkillNode(int index){
            MCharacter mCharacter = cBattlefield.charactersManager.GetCharacter(index);
            if (mCharacter == null || !cBattlefield.charactersManager.IsInSkillDistance(mCharacter, this.mCharacter))
            {
                CharacterReturnNone();
                return;
            }
            bool sameBelong = cBattlefield.charactersManager.IsSameBelong(mCharacter.Belong, this.mCharacter.Belong);
            bool useToEnemy = this.mCharacter.CurrentSkill.UseToEnemy;
            if (!(useToEnemy ^ sameBelong))
            {
                CAlertDialog.Show("belong不对");
                return;
            }
            this.mCharacter.Target = mCharacter;
            mCharacter.Target = this.mCharacter;
            //attackCharacterList.Clear();
            if (useToEnemy)
            {
                bool forceFirst = (mCharacter.CurrentSkill != null && mCharacter.CurrentSkill.Master.effect.special == App.Model.Master.SkillEffectSpecial.force_first);
                if (forceFirst && cBattlefield.charactersManager.IsInSkillDistance(this.mCharacter, mCharacter))
                {
                    //先手攻击
                    SetActionCharacterList(mCharacter, this.mCharacter, true);
                }
                else
                {
                    SetActionCharacterList(this.mCharacter, mCharacter, true);
                }
            }
            else
            {
                SetActionCharacterList(this.mCharacter, mCharacter, false);
            }
            cBattlefield.tilesManager.ClearCurrentTiles();
            cBattlefield.CloseOperatingMenu();
            cBattlefield.HideBattleCharacterPreviewDialog();
            cBattlefield.battleMode = CBattlefield.BattleMode.actioning;
            cBattlefield.ActionEndHandler += OnActionComplete;
            OnActionComplete();
        }
        private void SetActionCharacterList(MCharacter actionCharacter, MCharacter targetCharacter, bool canCounter){
            int count = cBattlefield.calculateManager.SkillCount(actionCharacter, targetCharacter);
            int countBack = count;
            while(count-- > 0){
                actionCharacterList.Add(actionCharacter);
            }
            if (!canCounter || !cBattlefield.calculateManager.CanCounterAttack(actionCharacter, targetCharacter, actionCharacter.CoordinateX, actionCharacter.CoordinateY, targetCharacter.CoordinateX, targetCharacter.CoordinateY))
            {
                return;
            }
            count = cBattlefield.calculateManager.CounterAttackCount(actionCharacter, targetCharacter);
            while (count-- > 0)
            {
                actionCharacterList.Add(targetCharacter);
            }
            //反击后反击
            if (actionCharacter.CurrentSkill.Master.effect.special == App.Model.Master.SkillEffectSpecial.attack_back_attack)
            {
                while(countBack-- > 0){
                    actionCharacterList.Add(actionCharacter);
                }
            }
        }
        /// <summary>
        /// 动作结束时，判断是否继续进行
        /// </summary>
        public void OnActionComplete(){
            if (actionCharacterList.Count > 0)
            {
                MCharacter currentCharacter = actionCharacterList[0];
                actionCharacterList.RemoveAt(0);
                bool isContinue = ActionStart(currentCharacter);
                //Debug.LogError("isContinue" + isContinue + ", " + currentCharacter.Master.name);
                if (isContinue)
                {
                    return;
                }
            }
            cBattlefield.ActionEndHandler -= OnActionComplete;
            cBattlefield.StartCoroutine(ActionOver());
        }
        /// <summary>
        /// 开始动作
        /// </summary>
        /// <returns><c>true</c>, if start run was actioned, <c>false</c> otherwise.</returns>
        /// <param name="currentCharacter">Current character.</param>
        private void ActionStartRun(MCharacter currentCharacter){
            System.Action actionStartEvent = () =>
                {
                    cBattlefield.MapMoveToPosition(currentCharacter.CoordinateX, currentCharacter.CoordinateY);
                    currentCharacter.Direction = (currentCharacter.X > currentCharacter.Target.X ? Direction.left : Direction.right);
                    currentCharacter.Action = ActionType.attack;
                    App.Model.Master.MSkill skillMaster = currentCharacter.CurrentSkill.Master;
                    if(!string.IsNullOrEmpty(skillMaster.animation)){
                        VTile vTile = this.cBattlefield.mapSearch.GetTile(currentCharacter.Target.CoordinateX, currentCharacter.Target.CoordinateY);
                        this.cBattlefield.CreateEffect(skillMaster.animation, vTile.transform);
                    }
                };
            if (currentCharacter.CurrentSkill.Master.effect.special == App.Model.Master.SkillEffectSpecial.back_thrust)
            {
                //回马枪
                VTile currentTile = cBattlefield.mapSearch.GetTile(currentCharacter.CoordinateX, currentCharacter.CoordinateY);
                VTile targetTile = cBattlefield.mapSearch.GetTile(currentCharacter.Target.CoordinateX, currentCharacter.Target.CoordinateY);
                Direction direction = cBattlefield.mapSearch.GetDirection(targetTile, currentTile);
                VTile backTile = cBattlefield.mapSearch.GetTile(currentTile, direction);
                if (cBattlefield.charactersManager.GetCharacter(backTile.Index) != null)
                {
                    actionStartEvent();
                    return;
                }
                Sequence sequence = new Sequence();
                TweenParms tweenParms = new TweenParms().Prop("X", backTile.transform.localPosition.x, false).Prop("Y", backTile.transform.localPosition.y, false).Ease(EaseType.Linear);
                TweenParms tweenParmsTarget = new TweenParms().Prop("X", currentTile.transform.localPosition.x, false).Prop("Y", currentTile.transform.localPosition.y, false).Ease(EaseType.Linear);
                Holoville.HOTween.Core.TweenDelegate.TweenCallback moveComplete = () =>
                    {
                        currentCharacter.CoordinateY = backTile.CoordinateY;
                        currentCharacter.CoordinateX = backTile.CoordinateX;
                        currentCharacter.Target.CoordinateY = currentTile.CoordinateY;
                        currentCharacter.Target.CoordinateX = currentTile.CoordinateX;
                        actionStartEvent();
                    };
                tweenParms.OnComplete(moveComplete);
                sequence.Insert (0f, HOTween.To(currentCharacter, 0.5f, tweenParms));
                sequence.Insert (0f, HOTween.To(currentCharacter.Target, 0.5f, tweenParmsTarget));
                sequence.Play();
            }
            else
            {
                actionStartEvent();
            }
        }
        /// <summary>
        /// 开始动作
        /// </summary>
        /// <returns><c>true</c>, if start was actioned, <c>false</c> otherwise.</returns>
        /// <param name="currentCharacter">Current character.</param>
        private bool ActionStart(MCharacter currentCharacter){
            if (currentCharacter.Hp > 0)
            {
                //目标已死
                if (currentCharacter.Target.Hp == 0)
                {
                    return false;
                }
                ActionStartRun(currentCharacter);
                return true;
            }
            actionCharacterList.Clear();
            if (cBattlefield.charactersManager.IsSameCharacter(currentCharacter, this.mCharacter))
            {
                return true;
            }
            //是否引导攻击
            bool continueAttack = (this.mCharacter.CurrentSkill.Master.effect.special == App.Model.Master.SkillEffectSpecial.continue_attack);
            if (continueAttack)
            {
                VTile vTile = cBattlefield.mapSearch.GetTile(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
                MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, (c)=>{
                    if(c.Hp == 0){
                        return false;
                    }
                    if (cBattlefield.charactersManager.IsSameBelong(this.mCharacter.Belong, c.Belong))
                    {
                        return false;
                    }
                    bool canAttack = cBattlefield.charactersManager.IsInSkillDistance(c.CoordinateX, c.CoordinateY, vTile.CoordinateX, vTile.CoordinateY, this.mCharacter);
                    return canAttack;
                });
                if (mCharacter != null)
                {
                    cBattlefield.ActionEndHandler -= OnActionComplete;
                    VTile tile = cBattlefield.mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY);
                    ClickSkillNode(tile.Index);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 特技导致武将状态改变
        /// </summary>
        /// <param name="skill">Skill.</param>
        private void AddAidToCharacter(App.Model.Master.MSkillEffect mSkillEffect, App.Model.MCharacter[] targetCharacters){
            List<int> aids = mSkillEffect.strategys.ToList();
            int index = 0;
            List<App.Model.Master.MStrategy> strategys = new List<App.Model.Master.MStrategy>();
            while (index++ < mSkillEffect.count)
            {
                int i = Random.Range(0, aids.Count - 1);
                App.Model.Master.MStrategy strategy = StrategyCacher.Instance.Get(aids[i]);
                aids.RemoveAt(i);
                strategys.Add(strategy);
            }
            foreach (App.Model.Master.MStrategy strategy in strategys)
            {
                foreach (App.Model.MCharacter target in targetCharacters)
                {
                    //特效
                    if(strategy.effect_type == App.Model.Master.StrategyEffectType.aid){
                        //this.mCharacter.Target.AddAid(strategy);
                        VTile vTile = this.cBattlefield.mapSearch.GetTile(target.CoordinateX, target.CoordinateY);
                        this.cBattlefield.CreateEffect(strategy.effect, vTile.transform);
                    }else if(strategy.effect_type == App.Model.Master.StrategyEffectType.status){
                        //this.mCharacter.Target.AddStatus(strategy);
                        VTile vTile = this.cBattlefield.mapSearch.GetTile(target.CoordinateX, target.CoordinateY);
                        this.cBattlefield.CreateEffect(strategy.effect, vTile.transform);
                    }
                }
            }
            cBattlefield.StartCoroutine(AddAidToCharacterComplete(strategys, targetCharacters));
        }
        private IEnumerator AddAidToCharacterComplete(List<App.Model.Master.MStrategy> strategys, App.Model.MCharacter[] targetCharacters){
            while (App.View.Effect.VEffectAnimation.IsRunning)
            {
                yield return new WaitForEndOfFrame();
            }
            foreach (App.Model.Master.MStrategy strategy in strategys)
            {
                foreach (App.Model.MCharacter target in targetCharacters)
                {
                    if(strategy.effect_type == App.Model.Master.StrategyEffectType.aid){
                        target.AddAid(strategy);
                    }else if(strategy.effect_type == App.Model.Master.StrategyEffectType.status){
                        target.AddStatus(strategy);
                    }
                }
            }
        }
        /// <summary>
        /// 动作结束后处理
        /// </summary>
        public IEnumerator ActionOver(){
            //Debug.LogError("ActionOver" + this.mCharacter.Master.name);
            /*MSkill skill = this.mCharacter.CurrentSkill;
            if (skill.Master.effect.special == App.Model.Master.SkillEffectSpecial.aid)
            {
                if (skill.Master.effect.enemy.count > 0 && skill.Master.effect.enemy.time == App.Model.Master.SkillEffectBegin.attack_end)
                {
                    AddAidToCharacter(skill.Master.effect.enemy);
                }
                else if(skill.Master.effect.self.count > 0 && skill.Master.effect.self.time == App.Model.Master.SkillEffectBegin.attack_end)
                {
                    AddAidToCharacter(skill.Master.effect.self);
                }
            }*/
            if (this.mCharacter.Target != null)
            {
                if (this.mCharacter.Target.Hp > 0 && this.mCharacter.Target.attackEndEffects.Count > 0)
                {
                    foreach(App.Model.Master.MSkillEffect mSkillEffect in this.mCharacter.Target.attackEndEffects){
                        AddAidToCharacter(mSkillEffect, new MCharacter[]{this.mCharacter.Target});
                    }
                    while (App.View.Effect.VEffectAnimation.IsRunning)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    yield return new WaitForEndOfFrame();
                    this.mCharacter.Target.attackEndEffects.Clear();
                }
                this.mCharacter.Target.Target = null;
                this.mCharacter.Target = null;

            }
            if (!System.Array.Exists(mBaseMap.Characters, c => c.Hp > 0 && !c.IsHide && c.Belong == Belong.enemy))
            {
                //敌军全灭
                Debug.LogError("敌军全灭");
                //cBattlefield.BattleWin();
                List<string> script = new List<string>();
                script.Add("Call.battle_win();");
                script.Add("Battle.win();");
                App.Util.LSharp.LSharpScript.Instance.Analysis(script);
                yield break;
            }else if (!System.Array.Exists(mBaseMap.Characters, c => c.Hp > 0 && !c.IsHide && c.Belong == Belong.self))
            {
                //我军全灭
                Debug.LogError("我军全灭");
                cBattlefield.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.BattleFailDialog));
                yield break;
            }
            if (this.mCharacter.Hp > 0 && this.mCharacter.IsMoveAfterAttack && this.mCharacter.Ability.MovingPower - this.mCharacter.roadLength > 0)
            {
                Debug.LogError("MoveAfterAttack");
                cBattlefield.tilesManager.ShowCharacterMovingArea(this.mCharacter, this.mCharacter.Ability.MovingPower - this.mCharacter.roadLength);
                cBattlefield.battleMode = CBattlefield.BattleMode.move_after_attack;
                if (this.mCharacter.Belong != Belong.self)
                {
                    cBattlefield.ai.MoveAfterAttack();
                }
            }
            else
            {
                Debug.LogError("ActionOverNext");
                cBattlefield.StartCoroutine(ActionOverNext());
            }
        }
        private IEnumerator ActionEndSkillsRun(){
            List<App.Model.Master.MSkill> skills = this.mCharacter.ActionEndSkills;
            if (skills == null || skills.Count == 0)
            {
                yield break;
            }
            foreach(App.Model.Master.MSkill skill in skills){
                if (skill.effect.special == App.Model.Master.SkillEffectSpecial.status)
                {
                    if (skill.effect.enemy.count > 0 && skill.effect.enemy.time == App.Model.Master.SkillEffectBegin.action_end)
                    {
                        
                    }
                    else if (skill.effect.self.count > 0 && skill.effect.self.time == App.Model.Master.SkillEffectBegin.action_end)
                    {
                        MCharacter[] targets = System.Array.FindAll(mBaseMap.Characters, (character)=>{
                            if(character.Hp == 0 || !cBattlefield.charactersManager.IsSameBelong(character.Belong, this.mCharacter.Belong)){
                                return false;
                            }
                            int distance = cBattlefield.mapSearch.GetDistance(character.CoordinateX, character.CoordinateY, this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
                            return distance >= skill.distance[0] && distance <= skill.distance[1];
                        } );
                        AddAidToCharacter(skill.effect.self, targets);
                    }
                }
            }
            while (App.View.Effect.VEffectAnimation.IsRunning)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        /// <summary>
        /// 动作结束后处理
        /// </summary>
        public IEnumerator ActionOverNext(){
            if (cBattlefield.battleMode == CBattlefield.BattleMode.moving)
            {
                this.mCharacter.Action = ActionType.idle;
            }
            yield return cBattlefield.StartCoroutine(ActionEndSkillsRun());
            this.mCharacter.ActionOver = true;
            this.mCharacter.roadLength = 0;
            cBattlefield.tilesManager.ClearCurrentTiles();
            cBattlefield.HideBattleCharacterPreviewDialog();
            cBattlefield.battleMode = CBattlefield.BattleMode.none;
            Belong belong = this.mCharacter.Belong;
            this.mCharacter = null;
            if (!System.Array.Exists(mBaseMap.Characters, c => c.Hp > 0 && !c.IsHide && c.Belong == belong && !c.ActionOver))
            {
                ChangeBelong(belong);
            }
            else
            {
                cBattlefield.CloseOperatingMenu();
            }
        }
        public void ChangeBelong(Belong belong){
            if (belong == Belong.self)
            {
                if (System.Array.Exists(mBaseMap.Characters, c => c.Hp > 0 && !c.IsHide && c.Belong == Belong.friend && !c.ActionOver))
                {
                    cBattlefield.BoutWave(Belong.friend);
                }
                else
                {
                    cBattlefield.BoutWave(Belong.enemy);
                }
            }else if (belong == Belong.friend)
            {
                cBattlefield.BoutWave(Belong.enemy);
            }else if (belong == Belong.enemy)
            {
                cBattlefield.BoutWave(Belong.self);
            }
        }
        private void MoveStart(int index){
            VTile startTile = cBattlefield.mapSearch.GetTile(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
            VTile endTile = cBattlefield.mapSearch.GetTile(index);

            cBattlefield.MapMoveToPosition(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
            Holoville.HOTween.Core.TweenDelegate.TweenCallback moveComplete;
            if (cBattlefield.battleMode == CBattlefield.BattleMode.move_after_attack)
            {
                moveComplete = () =>
                    {
                        this.mCharacter.CoordinateY = endTile.CoordinateY;
                        this.mCharacter.CoordinateX = endTile.CoordinateX;
                        cBattlefield.MapMoveToPosition(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
                        cBattlefield.StartCoroutine(ActionOverNext());
                    };
            }
            else
            {
                moveComplete = () =>
                    {
                        this.mCharacter.Action = ActionType.idle;
                        cBattlefield.tilesManager.ClearCurrentTiles();
                        cBattlefield.battleMode = CBattlefield.BattleMode.move_end;
                        this.mCharacter.CoordinateY = endTile.CoordinateY;
                        this.mCharacter.CoordinateX = endTile.CoordinateX;
                        cBattlefield.MapMoveToPosition(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
                        cBattlefield.tilesManager.ShowCharacterSkillArea(this.mCharacter);
                        cBattlefield.OpenOperatingMenu();
                    };
            }

            List<VTile> tiles = cBattlefield.aStar.Search(this.mCharacter, startTile, endTile);
            this.mCharacter.roadLength = tiles.Count;
            if (tiles.Count > 0)
            {
                cBattlefield.CloseOperatingMenu();
                cBattlefield.tilesManager.ClearCurrentTiles();
                this.mCharacter.Action = ActionType.move;
                cBattlefield.battleMode = CBattlefield.BattleMode.moving;
                Sequence sequence = new Sequence();
                foreach (VTile tile in tiles)
                {
                    TweenParms tweenParms = new TweenParms().Prop("X", tile.transform.localPosition.x, false).Prop("Y", tile.transform.localPosition.y, false).Ease(EaseType.Linear);
                    if (tile.Index == endTile.Index)
                    {
                        tweenParms.OnComplete(moveComplete);
                    }
                    sequence.Append(HOTween.To(this.mCharacter, 0.5f, tweenParms));
                }
                sequence.Play();
            }
            else
            {
                moveComplete();
            }
        }
        public void ClickMovingNode(int index){
            if (this.mCharacter.Belong != cBattlefield.currentBelong || this.mCharacter.ActionOver)
            {
                CharacterReturnNone();
                return;
            }
            MCharacter mCharacter = cBattlefield.charactersManager.GetCharacter(index);
            if (mCharacter != null)
            {
                bool sameBelong = cBattlefield.charactersManager.IsSameBelong(mCharacter.Belong, this.mCharacter.Belong);
                bool useToEnemy = this.mCharacter.CurrentSkill.UseToEnemy;
                if (useToEnemy ^ sameBelong)
                {
                    ClickSkillNode(index);
                }
                return;
            }
            if (cBattlefield.tilesManager.IsInMovingCurrentTiles(index))
            {
                MoveStart(index);
            }
            else if(cBattlefield.battleMode != CBattlefield.BattleMode.move_after_attack)
            {
                CharacterReturnNone();
            }

        }
        public void CharacterReturnNone(){
            returnAction();
            this.mCharacter = null;
            //Debug.LogError("this.mCharacter = null;");
            cBattlefield.tilesManager.ClearCurrentTiles();
            cBattlefield.CloseOperatingMenu();
            cBattlefield.HideBattleCharacterPreviewDialog();
            cBattlefield.battleMode = CBattlefield.BattleMode.none;
        }
    }
}