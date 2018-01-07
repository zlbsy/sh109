using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using App.View.Equipment;
using App.Controller.Common;


namespace App.Controller.Equipment{
    public class CEquipmentListDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        private System.Action<int> selectEvent;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            if (Global.SUser.self.equipments == null)
            {
                SEquipment sEquipment = new SEquipment();
                yield return StartCoroutine(sEquipment.RequestList());
                Global.SUser.self.equipments = sEquipment.equipments;
            }
            if (request != null && request.Has("selectEvent"))
            {
                selectEvent = request.Get<System.Action<int>>("selectEvent");
            }
            else
            {
                selectEvent = null;
            }
            App.Model.Master.MEquipment.EquipmentType equipmentType = request.Get<App.Model.Master.MEquipment.EquipmentType>("equipmentType");
            App.Model.MEquipment[] equipments = System.Array.FindAll(Global.SUser.self.equipments, 
                _=>_.EquipmentType == equipmentType && _.character_id == 0);
            Debug.LogError("equipmentType="+equipmentType);
            if (equipmentType == App.Model.Master.MEquipment.EquipmentType.horse && request.Has("moveType"))
            {
                MoveType moveType = request.Get<MoveType>("moveType");
                Debug.LogError("moveType="+moveType);
                equipments = System.Array.FindAll(equipments, e=>e.Master.move_type == moveType);
            }
            foreach(App.Model.MEquipment equipment in equipments){
                ScrollViewSetChild(content, childItem, equipment);
            }
			yield return 0;
        }
        public void EquipmentIconClick(VEquipmentIcon icon){
            if (selectEvent != null)
            {
                selectEvent(icon.ViewModel.Id.Value);
            }
            this.Close();
        }
	}
}