using UnityEngine.UI;
using App.Model;
using UnityEngine;
using App.ViewModel;
using App.Util;
using App.Util.Cacher;
using App.View.Common;


namespace App.View.LoginBonus{
    public class VLoginBonusChild : VBase {
        [SerializeField]private Text day;
        [SerializeField]private Image receivedIcon;
        [SerializeField]private VContentsChild vContentsChild;

        #region VM处理
        public VMLoginBonus ViewModel { get { return (VMLoginBonus)BindingContext; } }
        #endregion
        public override void UpdateView()
        {
            vContentsChild.UpdateView(ViewModel.Contents.Value[0]);
            receivedIcon.gameObject.SetActive(ViewModel.Received.Value);
            day.text = string.Format("第{0}天", ViewModel.Day.Value);
        }
        public void ClickChild(){
            this.Controller.SendMessage("ContentClick", this, SendMessageOptions.DontRequireReceiver);
        }
    }
}