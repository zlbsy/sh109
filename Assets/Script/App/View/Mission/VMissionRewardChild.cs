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
    public class VMissionRewardChild : VBase {
        [SerializeField]private VContentsChild vContentsChild;
        /*#region VM处理
        public VMPresent ViewModel { get { return (VMPresent)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
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
        }
        #endregion*/
        public override void UpdateView(App.Model.MBase model){
            //App.Model.MContent content = model as App.Model.MContent;
            vContentsChild.UpdateView(model);
        }
    }
}