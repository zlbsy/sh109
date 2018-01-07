using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View{
    public class VHeaderFace : VBase {
        [SerializeField]public App.View.Character.VRawFace icon;
        [SerializeField]public Text nickname;
        [SerializeField]public Text level;
        #region VM处理
        public VMUser ViewModel { get { return (VMUser)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMUser oldVm = oldViewModel as VMUser;
            if (oldVm != null)
            {
                oldVm.Face.OnValueChanged -= FaceChanged;
                oldVm.Level.OnValueChanged -= LevelChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Face.OnValueChanged += FaceChanged;
                ViewModel.Level.OnValueChanged += LevelChanged;
            }
        }
        private void FaceChanged(int oldvalue, int newvalue)
        {
            icon.CharacterId = newvalue;
        }
        private void LevelChanged(int oldvalue, int newvalue)
        {
            level.text = newvalue.ToString();
        }
        #endregion
        public override void UpdateView(){
            level.text = ViewModel.Level.Value.ToString();
            nickname.text = ViewModel.Nickname.Value.ToString();
            icon.CharacterId = ViewModel.Face.Value;
        }
        void OnMouseUp(){
            
        }

    }
}