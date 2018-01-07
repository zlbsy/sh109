using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
    public class MItem : MBase {
        public enum ItemType
        {
            exp,
            skillPoint,
            skillBook
        }
        public MItem(){
        }
        public string name;//
        public ItemType item_type;
        public int price;
        public int child_id;
        public int content_value;
        public string explanation;
	}
}