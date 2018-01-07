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

namespace App.View.Present{
    public class VPresentChild : VBase {
        [SerializeField]private VContentsChild vContentsChild;
        [SerializeField]private Text lblName;
        [SerializeField]private Text lblMessage;
        [SerializeField]private Text lblLimit;
        #region VM处理
        public VMPresent ViewModel { get { return (VMPresent)BindingContext; } }
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
            vContentsChild.UpdateView(ViewModel.Content.Value);
            lblName.text = vContentsChild.ContentName;
            lblMessage.text = ViewModel.Content.Value.message;
            System.TimeSpan timeSpan = ViewModel.LimitTime.Value - App.Service.HttpClient.Now;
            if (timeSpan.Days > 1000)
            {
                lblLimit.text = "无期限";
            }else if (timeSpan.Days > 1)
            {
                lblLimit.text = string.Format("{0}天{1}小时", timeSpan.Days, timeSpan.Hours);
            }else if (timeSpan.Hours > 1)
            {
                lblLimit.text = string.Format("{0}小时{1}分", timeSpan.Hours, timeSpan.Minutes);
            }else
            {
                lblLimit.text = string.Format("{0}分", timeSpan.Minutes);
            }
        }
        #endregion
        public void ClickReceive(){
            this.Controller.SendMessage("ClickReceive", this);
        }
    }
}