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
using App.Util;


namespace App.Controller{
    /// <summary>
    /// 大地图场景
    /// </summary>
    public class CWorld : CBaseMap {
        public override IEnumerator OnLoad( Request request ) 
        {  
            if (Global.SUser.self.battlelist == null)
            {
                yield return StartCoroutine(Global.SBattlefield.RequestBattlelist());
            }
            yield return this.StartCoroutine(base.OnLoad(request));
            Debug.LogError("CWorld OnLoad");
        }
        protected override void InitMap(){
            mBaseMap = new MWorldMap();
            mBaseMap.MapId = App.Util.Global.Constant.world_map_id;
            mBaseMap.Tiles = App.Util.Global.worlds;
            vBaseMap.BindingContext = mBaseMap.ViewModel;
            vBaseMap.UpdateView();
            vBaseMap.transform.parent.localScale = Vector3.one;
            base.InitMap();
            if (App.Util.Global.SUser.self.IsTutorial)
            {
                return;
            }
            int lastStageId = App.Util.Global.SUser.self.lastStageId;
            CameraTo(lastStageId > 0 ? lastStageId : 1);
        }
        /*public override void CameraTo(int id){
            App.Model.Master.MWorld mWorld = System.Array.Find(App.Util.Global.worlds, w=>w.id==id);
            vBaseMap.MoveToPosition(mWorld.x, mWorld.y);
            base.CameraTo(id);
        }*/
        public void OnClickTutorialTile(){
            App.Model.Master.MWorld tile = System.Array.Find(App.Util.Global.worlds, w=>w.id == Global.Constant.tutorial_world_id);
            OnClickTile(tile);
        }
        /*
        /// <summary>
        /// 点击州府县，进入州府县场景
        /// </summary>
        /// <param name="index">州府县索引</param>
        public override void OnClickTile(int index){
            //地图信息
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
            //根据索引获取所点击的州府县坐标
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            //根据州府县坐标获取州府县
            App.Model.Master.MWorld tile = System.Array.Find(mBaseMap.Tiles, _=>_.x == coordinate.x && _.y == coordinate.y) as App.Model.Master.MWorld;
            OnClickTile(tile);
        }*/
        public override void OnClickTile(App.Model.MTile tile){
            App.Model.Master.MWorld world = tile as App.Model.Master.MWorld;
            if (world != null)
            {
                Request req = Request.Create("world", world);
                App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Stage.ToString(), req );
                //App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Area.ToString(), req );
            }
        }
        public void GotoTop(){
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Top.ToString() );
        }
	}
}