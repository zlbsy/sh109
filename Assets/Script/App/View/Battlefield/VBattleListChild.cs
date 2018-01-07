using UnityEngine.UI;
using App.Model;
using UnityEngine;
using App.ViewModel;
using App.Util;
using App.Util.Cacher;


namespace App.View.Character{
    public class VBattleListChild : VBase, UnityEngine.EventSystems.IPointerClickHandler {
        [SerializeField]private Text title;
        [SerializeField]private Text num;
        [SerializeField]private Text level;
        [SerializeField]private Image[] stars;
        [SerializeField]private VCharacterIcon icon;

        #region VM处理
        public VMBattleChild ViewModel { get { return (VMBattleChild)BindingContext; } }
        #endregion
        public override void UpdateView()
        {
            App.Model.Master.MBattlefield battlefieldMaster = App.Util.Cacher.BattlefieldCacher.Instance.Get(ViewModel.BattlefieldId.Value);
            title.text = battlefieldMaster.name;
            num.text = ViewModel.BattlefieldId.Value.ToString();
            level.text = string.Format("Lv.{0}", ViewModel.Level.Value);
            for (int i=0;i<stars.Length;i++)
            {
                Image obj = stars[i];
                obj.color = i < ViewModel.Star.Value ? Color.yellow : Color.white;
            }
            App.Model.Master.MBattleNpc battleNpc = System.Array.Find(battlefieldMaster.enemys, _=>_.boss == 1);
            MCharacter mCharacter = NpcCacher.Instance.GetFromBattleNpc(battleNpc);
            icon.BindingContext = mCharacter.ViewModel;
        }
        public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData){
            this.Controller.SendMessage("BattleChildClick", ViewModel.Id.Value);
        }
    }
}