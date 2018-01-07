using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using App.Controller.Common;


namespace App.Controller{
    public class CBuildingDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        private int tileIndex;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            tileIndex = request.Get<int>("tile_index");
            App.Model.Master.MBuilding[] builds = BuildingCacher.Instance.GetAll(Global.SUser.self.Level);
            List<MBuilding> mBuildings = new List<MBuilding>();
            foreach(App.Model.Master.MBuilding build in builds){
                MBuilding mBuilding = new MBuilding();
                mBuildings.Add(mBuilding);
                ScrollViewSetChild(content, childItem, mBuilding);
                mBuilding.Id = build.id;
            }
            BuildingCacher.Instance.SetBuildings(mBuildings);
            closeEvent = () =>
            {
                    BuildingCacher.Instance.SetBuildings(null);
            };
			yield return 0;
		}
        public void ToBuild(int buildId){
            MBuilding mBuilding = BuildingCacher.Instance.GetBuilding(buildId);
            App.Model.Master.MBuilding buildingMaster = mBuilding.Master;
            VBaseMap vBaseMap = (App.Util.SceneManager.CurrentScene as CTop).GetVBaseMap();
            App.Model.MTile[] tiles = vBaseMap.ViewModel.Tiles.Value;
            int currentNum = System.Array.FindAll(tiles, _ => _.tile_id == buildingMaster.tile_id).Length;
            if (currentNum < buildingMaster.sum)
            {
                if (BuyManager.CanBuy(buildingMaster.price, buildingMaster.price_type))
                {
                    this.StartCoroutine(Build(mBuilding.TileId, vBaseMap, buildingMaster));
                }
                else
                {
                    //Confirm dialog
                }
            }
            else
            {
                //Confirm dialog
                CAlertDialog.Show("已经达到了购买的上限了！");
            }
        }
        private IEnumerator Build(int buildId, VBaseMap vBaseMap, App.Model.Master.MBuilding buildingMaster){
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(vBaseMap.ViewModel.MapId.Value);
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(tileIndex);
            SShop sShop = new SShop();
            yield return StartCoroutine(sShop.RequestBuyBuild(buildId, (int)coordinate.x, (int)coordinate.y));

            App.Model.MTile currentTile = App.Model.MTile.Create(buildingMaster.tile_id, (int)coordinate.x, (int)coordinate.y);
            List<App.Model.MTile> tileList = vBaseMap.ViewModel.Tiles.Value.ToList();
            tileList.Add(currentTile);
            vBaseMap.ViewModel.Tiles.Value = tileList.ToArray();
            this.Close();
        }
	}
}