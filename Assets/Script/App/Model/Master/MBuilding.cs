using System.Collections;
using System.Collections.Generic;
namespace App.Model.Master{
    [System.Serializable]
    public class MBuilding : MBase {
        public MBuilding(){
        }
        public int from_level;
        public int to_level;
        public int tile_id;
        public int price;
        public App.Model.PriceType price_type;//gold, silver
        public int sum;
	}
}