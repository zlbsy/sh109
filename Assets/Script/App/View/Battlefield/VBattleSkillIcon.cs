using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Battlefield{
    public class VBattleSkillIcon : VBase, UnityEngine.EventSystems.IPointerDownHandler, UnityEngine.EventSystems.IPointerUpHandler, UnityEngine.EventSystems.IPointerClickHandler {
        [SerializeField]private Image icon;
        [SerializeField]private Text mp;
        #region VM处理
        public VMSkill ViewModel { get { return (VMSkill)BindingContext; } }
        public override void UpdateView(){
            icon.sprite = ImageAssetBundleManager.GetSkillIcon(ViewModel.SkillId.Value);
            mp.text = string.Format("M {0}", 14);
            this.gameObject.name = string.Format("Skill_{0}", ViewModel.SkillId.Value);
        }
        #endregion
        public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData){
            this.Controller.SendMessage("SkillIconClick", ViewModel.SkillId.Value);
        }
        public void OnPointerDown (UnityEngine.EventSystems.PointerEventData eventData) {
            this.Controller.SendMessage("ShowPreview", ViewModel.SkillId.Value);
        }
        public void OnPointerUp (UnityEngine.EventSystems.PointerEventData eventData) {
            this.Controller.SendMessage("HidePreview");
        }
    }
}