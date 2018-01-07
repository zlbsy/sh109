using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using UnityEngine.UI;
using App.Util.Cacher;
using Holoville.HOTween;
using App.View.Top;
using App.Controller.Common;


namespace App.Controller{
    public class CArea : CBaseMap {
        [SerializeField]Text title;
        private int worldId;
        private string nameKey;
        public override IEnumerator OnLoad( Request request ) 
        {  
            worldId = request.Get<int>("worldId");
            nameKey = request.Get<string>("nameKey");
            title.text = App.Util.Language.Get(nameKey);
            yield return this.StartCoroutine(base.OnLoad(request));
        }
        protected override void InitMap(){
            mBaseMap = new MBaseMap();
            mBaseMap.MapId = worldId;
            mBaseMap.Tiles = AreaCacher.Instance.GetAreas(worldId);
            mBaseMap.Characters = new MCharacter[]{};
            vBaseMap.BindingContext = mBaseMap.ViewModel;
            vBaseMap.UpdateView();
            vBaseMap.transform.parent.localScale = Vector3.one;
            base.InitMap();
            if (App.Util.Global.SUser.self.IsTutorial)
            {
                return;
            }
            //int lastAreaId = App.Util.Global.SUser.self.lastAreaId > 0 ? App.Util.Global.SUser.self.lastAreaId : mBaseMap.Tiles[0].id;
            //CameraTo(lastAreaId);
        }
        public void OnClickTutorialTile(){
            App.Model.MTile tile = mBaseMap.Tiles[0];
            OnClickTile(tile);
        }
        /*public override void CameraTo(int id){
            App.Model.MTile tile = System.Array.Find(mBaseMap.Tiles, w=>w.id==id);
            vBaseMap.MoveToPosition(tile.x, tile.y);
            base.CameraTo(id);
        }*/
        public override void OnClickTile(App.Model.MTile tile){
            App.Model.Master.MArea area = tile as App.Model.Master.MArea;
            if (area != null)
            {
                Request req = Request.Create("area", area, "worldId", worldId, "nameKey", nameKey);
                App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Stage.ToString(), req );
            }
        }
        public void GotoWorld(){
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.World.ToString() );
        }
	}
}