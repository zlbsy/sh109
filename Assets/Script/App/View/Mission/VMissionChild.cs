using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;
using App.View.Common;

namespace App.View.Mission{
    public class VMissionChild : VBase {
        [SerializeField]private Text lblName;
        [SerializeField]private Text lblMessage;
        [SerializeField]private Button btnComplete;
        [SerializeField]private Transform rewardsContent;
        [SerializeField]private GameObject rewardsChildItem;
        #region VM处理
        public VMMission ViewModel { get { return (VMMission)BindingContext; } }
        /*protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMPresent oldVm = oldViewModel as VMItem;
            if (oldVm != null)
            {
                oldVm.Cnt.OnValueChanged -= CntChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Cnt.OnValueChanged += CntChanged;
            }
        }
        private void CntChanged(int oldvalue, int newvalue)
        {
            cnt.text = newvalue.ToString();
        }*/
        public override void UpdateView(){
            App.Model.Master.MMission missionMaster = MissionCacher.Instance.Get(ViewModel.MissionId.Value);
            lblName.text = missionMaster.name;
            lblMessage.text = missionMaster.message;
            btnComplete.enabled = ViewModel.Status.Value == App.Model.MMission.MissionStatus.clear;
            btnComplete.GetComponentInChildren<Text>().text = Language.Get(string.Format("mission_button_{0}", ViewModel.Status.Value));
            this.Controller.ScrollViewSets(rewardsContent, rewardsChildItem,missionMaster.rewards);
        }
        #endregion
        public void ClickComplete(){
            this.Controller.SendMessage("ClickComplete", this);
        }
    }
}