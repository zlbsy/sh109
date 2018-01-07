using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    /**
     * 州府
     */
    [System.Serializable]
    public class MWorld : App.Model.MTile {
        public MWorld(){
        }
        public int map_id;
        public string build_name;
        public App.Model.MTile[] stages;
	}
}