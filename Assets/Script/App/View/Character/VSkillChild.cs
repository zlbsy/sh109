using UnityEngine.UI;
using App.Model;
using UnityEngine;
using App.ViewModel;
using App.Util;


namespace App.View.Character{
    public class VSkillChild : VBase {
        [SerializeField]private Text skillName;
        [SerializeField]private Text skillLevel;
        [SerializeField]private Text money;
        [SerializeField]private Text strength;
        [SerializeField]private Image icon;
        [SerializeField]private Image background;
        [SerializeField]private GameObject content;
        [SerializeField]private GameObject emptyContent;
        [SerializeField]private GameObject levelupContent;
        [SerializeField]private GameObject unlockContent;
        [SerializeField]private GameObject levelUpButton;
        [SerializeField]private GameObject unlockButton;
        [SerializeField]private Text unlockLabel;
        private Color32 learnedColor = new Color32(135, 255, 255, 128);
        private Color32 wakeColor = new Color32(135, 255, 255, 255);
        private Color32 disabledColor = new Color32(204, 204, 204, 255);
        #region VM处理
        public VMSkill ViewModel { get { return (VMSkill)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMSkill oldVm = oldViewModel as VMSkill;
            if (oldVm != null)
            {
                oldVm.Level.OnValueChanged -= LevelChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Level.OnValueChanged += LevelChanged;
            }
        }
        private void LevelChanged(int oldvalue, int newvalue)
        {
            App.Model.Master.MSkill skillMaster = App.Util.Cacher.SkillCacher.Instance.Get(ViewModel.SkillId.Value, ViewModel.Level.Value);
            skillLevel.text = string.Format("Lv.{0}", newvalue);
            money.text = skillMaster.character_level > 0 ? skillMaster.price.ToString() : "MAX";
            levelUpButton.SetActive(skillMaster.character_level > 0);
            strength.text = string.Format("威力：{0}", skillMaster.strength);
        }
        #endregion
        public override void UpdateView()
        {
            content.SetActive(ViewModel.SkillId.Value > 0);
            emptyContent.SetActive(ViewModel.SkillId.Value == 0);
            if (ViewModel.SkillId.Value == 0)
            {
                background.color = wakeColor;
                return;
            }
            App.Model.Master.MSkill skillMaster = App.Util.Cacher.SkillCacher.Instance.Get(ViewModel.SkillId.Value);
            skillName.text = skillMaster.name;
            icon.sprite = ImageAssetBundleManager.GetSkillIcon(ViewModel.SkillId.Value);
            levelupContent.SetActive(ViewModel.Id.Value > 0);
            unlockContent.SetActive(ViewModel.Id.Value == 0);
            if (ViewModel.Id.Value == 0)
            {
                unlockButton.SetActive(ViewModel.CanUnlock.Value);
                unlockLabel.gameObject.SetActive(!ViewModel.CanUnlock.Value);
                background.color = ViewModel.CanUnlock.Value ? wakeColor : disabledColor;
                return;
            }
            background.color = learnedColor;
            LevelChanged(0, ViewModel.Level.Value);
        }
        public void ShowDetail(){
            this.Controller.SendMessage("ShowSkillDetail", ViewModel.SkillId.Value);
        }
        public void LevelUp(){
            this.Controller.SendMessage("SkillLevelUp", ViewModel.Id.Value);
        }
        public void LearnNewSkill(){
            this.Controller.SendMessage("SkillLearn");
        }
        public void SkillUnlock(){
            this.Controller.SendMessage("SkillUnlock", ViewModel.SkillId.Value);
        }
    }
}