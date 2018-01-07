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
using System.Linq;


namespace App.Controller.Common{
    public partial class CBaseMap : CScene {
        [SerializeField]protected VBaseMap vBaseMap;
        protected MBaseMap mBaseMap;
        public App.Util.Search.TileMap mapSearch{ get; set;}
        public App.Util.Search.AStar aStar{ get; set;}
        public App.Util.Search.BreadthFirst breadthFirst{ get; set;}
        public override IEnumerator OnLoad( Request request ) 
        {  
            this.vBaseMap.gameObject.SetActive(false);
            InitMap();
            yield return this.StartCoroutine(base.OnLoad(request));
            this.vBaseMap.gameObject.SetActive(true);
        }
        protected virtual void InitManager(){
            mapSearch = new App.Util.Search.TileMap(mBaseMap, vBaseMap);
            aStar = new App.Util.Search.AStar(this, mBaseMap, vBaseMap);
            breadthFirst = new App.Util.Search.BreadthFirst(this, mBaseMap, vBaseMap);
        }
        protected virtual void InitMap(){
            InitManager();
        }
        public virtual void CameraTo(int id){
            App.Model.MTile tile = System.Array.Find(mBaseMap.Tiles, w=>w.id==id);
            vBaseMap.MoveToPosition(tile.x, tile.y);
            App.Util.LSharp.LSharpScript.Instance.Analysis();
        }
        /// <summary>
        /// 点击地图块儿
        /// </summary>
        /// <param name="index">地图块儿索引</param>
        public virtual void OnClickTile(int index){
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            App.Model.MTile tile = System.Array.Find(mBaseMap.Tiles, _=>_.x == coordinate.x && _.y == coordinate.y);
            OnClickTile(tile);
        }
        /// <summary>
        /// 点击地图块儿
        /// </summary>
        /// <param name="tile">地图块儿</param>
        public virtual void OnClickTile(App.Model.MTile tile){
        }
        public VBaseMap GetVBaseMap(){
            return vBaseMap;
        }
        public MBaseMap GetMBaseMap(){
            return mBaseMap;
        }
        public virtual void OnDestroy(){
        }
        public App.View.Character.VCharacter GetCharacterView(MCharacter mCharacter){
            App.View.Character.VCharacter vCharacter = this.vBaseMap.Characters.Find(_=>_.ViewModel.CharacterId.Value == mCharacter.CharacterId && _.ViewModel.Belong.Value == mCharacter.Belong);
            return vCharacter;
        }
        public MCharacter GetCharacterModel(App.View.Character.VCharacter vCharacter){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, _=>_.CharacterId == vCharacter.ViewModel.CharacterId.Value && _.Belong == vCharacter.ViewModel.Belong.Value);
            return mCharacter;
        }
        public MCharacter GetCharacterFromNpc(int npcId){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, c=>c.Id == npcId && c.Belong != Belong.self);
            return mCharacter;
        }
	}
}