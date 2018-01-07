using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Util.Cacher;


namespace App.Model.Master{
	/**
	 * 玩家主页面原始地图
	*/
    [System.Serializable]
	public class MBaseMap : MBase {
        public MBaseMap(){
		}
        public int width;//横向格数
        public int height;//纵向格数
		public int[] tile_ids;//小地图块
        private List<MTile> _tiles;
        public List<MTile> tiles{
            get{ 
                if (_tiles == null)
                {
                    _tiles = new List<MTile>();
                    foreach (int tille_id in tile_ids)
                    {
                        MTile tile = TileCacher.Instance.Get(tille_id);
                        _tiles.Add(tile);
                    }
                }
                return _tiles;
            }
        }
        public Vector2 GetCoordinateFromIndex(int index){
            int x = index % this.width;
            int y = Mathf.FloorToInt(index / this.width);
            return new Vector2(x, y);
        }
	}
}