using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using Holoville.HOTween;

namespace App.View{
    public class VAreaMap : VBaseMap {
        #region VM处理
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {
            base.OnBindingContextChanged(oldViewModel, newViewModel);
            /*
            VMTopMap oldVm = oldViewModel as VMTopMap;
            if (oldVm != null)
            {
                //oldVm.MapId.OnValueChanged -= MapIdChanged;
            }
            if (ViewModel!=null)
            {
                //ViewModel.MapId.OnValueChanged += MapIdChanged;
            }*/
        }
        #endregion
    }
}