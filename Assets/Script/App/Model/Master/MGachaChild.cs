using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace App.Model.Master{
    [System.Serializable]
	public class MGachaChild : MBase {
        public enum GachaType
        {
            item,
            weapon,
            horse,
            clothes,
            character
        }
        public MGachaChild(){
        }
        public GachaType type;//
        public int child_id;
        public int probability;
	}
}