using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
    public class MLoginBonus : MBase {
        public MLoginBonus(){
        }
        public int year;
        public int month;
        public MContent[] contents;
	}
}