using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class NpcEquipmentAsset : AssetBase<NpcEquipmentAsset> {
        [SerializeField]public App.Model.Master.MNpcEquipment[] npc_equipments;
	}
}