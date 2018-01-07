using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace App.Model.Master{
    [System.Serializable]
	public class MGachaPrice : MBase {
        public MGachaPrice(){
        }
        public int child_id;//
        public App.Model.PriceType price_type;
		public int price;
        public int cnt;
        public int free_time;
        public int free_count;
	}
}