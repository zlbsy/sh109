using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model.Master;

namespace App.Util.Cacher{
    public class EquipmentCacher: CacherBase<EquipmentCacher, MEquipment> {
        private MEquipment[] weapons;
        private MEquipment[] horses;
        private MEquipment[] clothes;
        public void ResetEquipment(MEquipment[] datas, MEquipment.EquipmentType type){
            switch(type){
                case MEquipment.EquipmentType.weapon:
                    ResetWeapon(datas);
                    break;
                case MEquipment.EquipmentType.clothes:
                    ResetClothes(datas);
                    break;
                case MEquipment.EquipmentType.horse:
                    ResetHorse(datas);
                    break;
            }
        }
        public void ResetWeapon(MEquipment[] datas){
            this.weapons = datas;
        }
        public void ResetHorse(MEquipment[] datas){
            this.horses = datas;
        }
        public void ResetClothes(MEquipment[] datas){
            this.clothes = datas;
        }
        public MEquipment GetEquipment(int id, MEquipment.EquipmentType type){
            MEquipment[] equipments;
            switch(type){
                case MEquipment.EquipmentType.weapon:
                    equipments = weapons;
                    break;
                case MEquipment.EquipmentType.clothes:
                    equipments = clothes;
                    break;
                case MEquipment.EquipmentType.horse:
                default:
                    equipments = horses;
                    break;
            }
            return System.Array.Find(equipments, _=>_.id == id);
        }
    }
}