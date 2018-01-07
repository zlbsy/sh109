using App.Model;
using App.View.Character;
using System;
using System.Collections;
using System.Collections.Generic;


namespace App.Controller.Battle{
    public enum CharacterEvent{
        OnDamage,
        OnHeal,
        OnBlock,
        OnHealWithoutAction,
    }
    public partial class CBattlefield{
        public delegate void EventHandler();
        public event EventHandler ActionEndHandler;
        private void ActionEnd(){
            if (ActionEndHandler != null)
            {
                ActionEndHandler();
            }
        }
        public void WaitActionEnd(){
            this.StartCoroutine(WaitActionEndCoroutine());
        }
        public IEnumerator WaitActionEndCoroutine(){
            while (HasDynamicCharacter())
            {
                yield return new UnityEngine.WaitForEndOfFrame();
            }
            ActionEnd();
        }
        public IEnumerator OnBoutStart(){
            while(true){
                MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.Belong == this.currentBelong && c.Hp > 0 && !c.IsHide && !c.boutEventComplete);
                if (mCharacter == null)
                {
                    break;
                }
                App.Model.Master.MSkill skill = mCharacter.BoutFixedDamageSkill;
                if (skill != null)
                {
                    List<VCharacter> characters = vBaseMap.Characters.FindAll(c=>c.ViewModel.Hp.Value > 0 && !c.ViewModel.IsHide.Value && !this.charactersManager.IsSameBelong(c.ViewModel.Belong.Value, currentBelong));
                    yield return OnBoutFixedDamage(mCharacter, skill, characters);
                }
                mCharacter.boutEventComplete = true;
            }
            if (currentBelong != Belong.self)
            {
                ai.Execute(currentBelong);
            }
            else
            {
                CloseOperatingMenu();
                if (scriptWaitPaths != null)
                {
                    WaitScript(scriptWaitPaths);
                }
            }
        }
        public IEnumerator OnBoutFixedDamage(MCharacter mCharacter, App.Model.Master.MSkill skill, List<VCharacter> characters){
            App.View.VTile targetTile = this.mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY);
            foreach (VCharacter child in characters)
            {
                App.View.VTile tile = this.mapSearch.GetTile(child.ViewModel.CoordinateX.Value, child.ViewModel.CoordinateY.Value);
                if (this.mapSearch.GetDistance(targetTile, tile) <= skill.radius)
                {
                    int hert = skill.strength;
                    if (child.ViewModel.Hp.Value - hert <= 0)
                    {
                        hert = child.ViewModel.Hp.Value - 1;
                    }
                    App.Model.Battle.MDamageParam arg = new App.Model.Battle.MDamageParam(-hert);
                    child.SendMessage(CharacterEvent.OnDamage.ToString(), arg);
                }
            }
            while (HasDynamicCharacter())
            {
                yield return new UnityEngine.WaitForEndOfFrame();
            }
        }
        public void OnDamage(VCharacter vCharacter){
            MCharacter mCharacter = this.GetCharacterModel(vCharacter);
            MCharacter targetModel = vCharacter.ViewModel.Target.Value;
            VCharacter target = this.GetCharacterView(targetModel);
            List<VCharacter> characters = this.charactersManager.GetTargetCharacters(vCharacter, target, vCharacter.ViewModel.CurrentSkill.Value.Master);
            App.View.VTile tile = mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY);
            foreach (VCharacter child in characters)
            {
                MCharacter childModel = this.GetCharacterModel(child);
                bool hit = calculateManager.AttackHitrate(mCharacter, childModel);
                if (!hit)
                {
                    child.SendMessage(CharacterEvent.OnBlock.ToString());
                    continue;
                }
                App.Model.Battle.MDamageParam arg = new App.Model.Battle.MDamageParam(-this.calculateManager.Hert(mCharacter, childModel, tile));
                child.SendMessage(CharacterEvent.OnDamage.ToString(), arg);
                if (child.ViewModel.CharacterId.Value != targetModel.CharacterId)
                {
                    continue;
                }
                if (mCharacter.CurrentSkill.Master.effect.enemy.time == App.Model.Master.SkillEffectBegin.enemy_hert)
                {
                    if (mCharacter.CurrentSkill.Master.effect.special == App.Model.Master.SkillEffectSpecial.vampire)
                    {
                        App.Model.Master.MStrategy strategy = App.Util.Cacher.StrategyCacher.Instance.Get(mCharacter.CurrentSkill.Master.effect.enemy.strategys[0]);
                        VCharacter currentCharacter = this.GetCharacterView(mCharacter);

                        int addHp = -UnityEngine.Mathf.FloorToInt(arg.value * strategy.hert * 0.01f);
                        App.Model.Battle.MDamageParam arg2 = new App.Model.Battle.MDamageParam(addHp);
                        currentCharacter.SendMessage(CharacterEvent.OnHealWithoutAction.ToString(), arg2);
                    }
                }else if (mCharacter.CurrentSkill.Master.effect.enemy.time == App.Model.Master.SkillEffectBegin.attack_end)
                {
                    if (mCharacter.CurrentSkill.Master.effect.special == App.Model.Master.SkillEffectSpecial.status)
                    {
                        int specialValue = mCharacter.CurrentSkill.Master.effect.special_value;
                        if(specialValue > 0 && UnityEngine.Random.Range(0,50) > specialValue){
                            continue;
                        }
                        childModel.attackEndEffects.Add(mCharacter.CurrentSkill.Master.effect.enemy);
                    }
                }
            }
        }
        public void OnHeal(VCharacter vCharacter){
            //UnityEngine.Debug.LogError("OnHeal");
            MCharacter mCharacter = this.GetCharacterModel(vCharacter);
            MCharacter targetModel = vCharacter.ViewModel.Target.Value;
            VCharacter target = this.GetCharacterView(targetModel);
            List<VCharacter> characters = this.charactersManager.GetTargetCharacters(vCharacter, target, vCharacter.ViewModel.CurrentSkill.Value.Master);
            App.View.VTile tile = mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY);
            foreach (VCharacter child in characters)
            {
                App.Model.Battle.MDamageParam arg = new App.Model.Battle.MDamageParam(this.calculateManager.Heal(mCharacter, this.GetCharacterModel(child), tile));
                child.SendMessage(CharacterEvent.OnHeal.ToString(), arg);
            }
        }
    }
}