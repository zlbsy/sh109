using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model.Master{
    [System.Serializable]
	public class MCharacterStar : MBase {
        public MCharacterStar(){
        }
        public int star;//星级
        public int cost;//消耗碎片
	}
}