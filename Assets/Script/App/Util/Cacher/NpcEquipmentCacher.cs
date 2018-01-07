using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace App.Util.Cacher{
    public class NpcEquipmentCacher: CacherBase<NpcEquipmentCacher, App.Model.Master.MNpcEquipment> {
        private List<App.Model.MEquipment> equipmentList = new List<App.Model.MEquipment>();
        public App.Model.MEquipment GetEquipment(int id){
            App.Model.MEquipment equipment = equipmentList.Find(_=>_.Id == id);
            if (equipment == null)
            {
                equipment = App.Model.MEquipment.Create(this.Get(id));
            }
            return equipment;
        }
        public void ClearEquipmentList(){
            equipmentList.Clear();
        }
        /*public void Set(App.Model.MEquipment equipment){
            App.Model.MEquipment equipmentData = Get(equipment.Id);
            if (equipmentData == null)
            {
                equipmentList.Add(equipmentData);
            }
        }
        public override App.Model.MEquipment Get(int id){
            return equipmentList.Find(_=>_.Id == id);
        }
        public App.Model.MEquipment Get(int characterId, int equipmentId, App.Model.Master.MEquipment.EquipmentType equipmentType){
            return equipmentList.Find(_=>_.character_id == characterId && _.EquipmentId == equipmentId && _.EquipmentType == equipmentType);
        }
        public override App.Model.MEquipment[] GetAll(){
            return equipmentList.ToArray();
        }*/
    }
}