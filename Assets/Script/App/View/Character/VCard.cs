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
    public class VCard : VBase {
        [SerializeField]private Image background;
        [SerializeField]private VFace faceIcon;
        [SerializeField]private Text txtName;
        [SerializeField]private Text txtNickname;
        [SerializeField]private GameObject[] stars;
        [SerializeField]private bool hideName = false;
        #region VM处理
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMCharacter oldVm = oldViewModel as VMCharacter;
            if (oldVm != null)
            {
                oldVm.CharacterId.OnValueChanged -= CharacterIdChanged;
                oldVm.Star.OnValueChanged -= StarChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.CharacterId.OnValueChanged += CharacterIdChanged;
                ViewModel.Star.OnValueChanged += StarChanged;
            }
        }
        private void CharacterIdChanged(int oldvalue, int newvalue)
        {
            if (hideName)
            {
                txtName.gameObject.SetActive(false);
                txtNickname.gameObject.SetActive(false);
            }
            else
            {
                txtName.gameObject.SetActive(true);
                txtNickname.gameObject.SetActive(true);
                txtName.text = ViewModel.Name.Value;
                txtNickname.text = ViewModel.Nickname.Value;
            }
            faceIcon.CharacterId = ViewModel.CharacterId.Value;
            background.color = App.Model.MCharacter.GetColor(ViewModel.CharacterId.Value);
        }
        private void StarChanged(int oldvalue, int newvalue)
        {
            int count = 0;
            foreach (GameObject star in stars)
            {
                star.SetActive(count++ < newvalue);
            }
        }
        public override void UpdateView()
        {
            CharacterIdChanged(0, ViewModel.CharacterId.Value);
            StarChanged(0, ViewModel.Star.Value);
        }
        #endregion
    }
}