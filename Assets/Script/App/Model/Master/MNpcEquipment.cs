using System.Collections;
using System.Collections.Generic;
namespace App.Model.Master{
    [System.Serializable]
	public class MNpcEquipment : MBase {
        public MNpcEquipment(){
		}
        public int equipment_id;
        public App.Model.Master.MEquipment.EquipmentType equipment_type;
        public int level;
	}
}