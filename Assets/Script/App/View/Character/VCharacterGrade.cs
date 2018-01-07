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
    public class VCharacterGrade : VBase {
        [SerializeField]private Text label;
        [SerializeField]private Image background;
        #region VM处理
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMCharacter oldVm = oldViewModel as VMCharacter;
            if (oldVm != null)
            {
                oldVm.Fragment.OnValueChanged -= FragmentChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Fragment.OnValueChanged += FragmentChanged;
            }
        }
        private void FragmentChanged(int oldvalue, int newvalue)
        {
            int cost = CharacterStarCacher.Instance.Cost(ViewModel.Star.Value + 1);
            label.text = string.Format("{0}/{1}", newvalue, cost == 0 ? "Max" : cost.ToString());
        }
        #endregion

        public override void UpdateView()
        {
            FragmentChanged(0, ViewModel.Fragment.Value);
        }
    }
}