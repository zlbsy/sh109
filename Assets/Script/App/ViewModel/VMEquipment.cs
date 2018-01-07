using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;

namespace App.ViewModel
{
	public class VMEquipment : VMBase
    {
        public VMProperty<int> Id = new VMProperty<int>();
        public VMProperty<int> EquipmentId = new VMProperty<int>();
        public VMProperty<App.Model.Master.MEquipment.EquipmentType> EquipmentType = new VMProperty<App.Model.Master.MEquipment.EquipmentType>();
        public VMProperty<int> Level = new VMProperty<int>();
        public VMProperty<string> Name = new VMProperty<string>();
	}
}