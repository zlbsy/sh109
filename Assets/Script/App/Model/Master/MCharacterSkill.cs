using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model.Master{
    [System.Serializable]
	public class MCharacterSkill : MBase {
        public MCharacterSkill(){
        }
		public int skill_id;//
        public int star;//习得条件
        public int skill_point;//消耗技能点
	}
}