using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Model.Master;
using App.Util.Cacher;
using App.Util.Battle;

namespace App.View.Character{
    public class VCharacterAnimation : VBase {
        [SerializeField]private VCharacter vCharacter;
        public void AttackToHert(){
            vCharacter.AttackToHert();
        }
        public void ActionEnd(){
            vCharacter.ActionEnd();
		}
        public void SetOrders(string jsons){
            Dictionary<string,int> meshs = App.Service.HttpClient.Deserialize<Dictionary<string,int>>(jsons);
            vCharacter.SetOrders(meshs);
		}
    }
}