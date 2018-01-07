using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Character{
    public class VCharacterSkill : VBase {
        [SerializeField]private GameObject skillChild;
        [SerializeField]private Transform skillContent;
        #region VM处理
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMCharacter oldVm = oldViewModel as VMCharacter;
            if (oldVm != null)
            {
                oldVm.Skills.OnValueChanged -= SkillsChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Skills.OnValueChanged += SkillsChanged;
            }
        }
        private void SkillsChanged(App.Model.MSkill[] oldvalue, App.Model.MSkill[] newvalue)
        {
            Debug.LogError("SkillsChanged run");
            App.Util.Global.ClearChild(skillContent.gameObject);

            this.Controller.ScrollViewSets(skillContent, skillChild, newvalue);
            App.Model.Master.MCharacterSkill[] skills = CharacterCacher.Instance.Get(ViewModel.CharacterId.Value).skills;
            foreach (App.Model.Master.MCharacterSkill skill in skills)
            {
                if (System.Array.Exists(ViewModel.Skills.Value, s => s.SkillId == skill.skill_id))
                {
                    continue;
                }
                App.Model.MSkill mSkill = new App.Model.MSkill();
                mSkill.SkillId = skill.skill_id;
                mSkill.CanUnlock = skill.star <= ViewModel.Star.Value;
                this.Controller.ScrollViewSetChild(skillContent, skillChild, mSkill);
            }
            int skillCount = skillContent.childCount;
            for (int i = skillCount; i < 5; i++)
            {
                App.Model.MSkill mSkill = new App.Model.MSkill();
                this.Controller.ScrollViewSetChild(skillContent, skillChild, mSkill);
            }
        }
        #endregion

        public override void UpdateView()
        {
            SkillsChanged(null, ViewModel.Skills.Value);
        }
    }
}