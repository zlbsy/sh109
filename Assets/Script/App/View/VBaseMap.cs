using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using Holoville.HOTween;
using System.Linq;
using App.View.Character;

namespace App.View{
    public class VBaseMap : VBase {
        [SerializeField]public int mapWidth;
        [SerializeField]public int mapHeight;
        [SerializeField]public VTile[] tileUnits;
        [SerializeField]protected Camera camera3d;
        [SerializeField]protected GameObject characterPrefab;
        [SerializeField]protected GameObject characterLayer;
        protected Vector2 camera3dPosition;
        protected Vector2 mousePosition = Vector2.zero;
        protected Vector2 dragPosition = Vector2.zero;
        protected Vector2 maxPosition;
        protected Vector2 minPosition;
        protected bool _isDraging;
        protected bool _camera3DEnable = true;
        protected const float tileWidth = 0.69f;
        protected const float tileHeight = 0.6f;
        private List<VCharacter> vCharacters = new List<VCharacter>();
        #region VM处理
        public VMBaseMap ViewModel { get { return (VMBaseMap)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMBaseMap oldVm = oldViewModel as VMBaseMap;
            if (oldVm != null)
            {
                oldVm.MapId.OnValueChanged -= MapIdChanged;
                oldVm.Tiles.OnValueChanged -= TilesChanged;
                oldVm.Characters.OnValueChanged -= CharactersChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.MapId.OnValueChanged += MapIdChanged;
                ViewModel.Tiles.OnValueChanged += TilesChanged;
                ViewModel.Characters.OnValueChanged += CharactersChanged;
            }
        }
        private void MapIdChanged(int oldvalue, int newvalue)
        {
            App.Model.Master.MBaseMap baseMapMaster = BaseMapCacher.Instance.Get(newvalue);
            ResetAll(baseMapMaster);
        }
        private void TilesChanged(App.Model.MTile[] oldvalue, App.Model.MTile[] newvalue)
        {
            ResetAll();
        }
        private void CharactersChanged(App.Model.MCharacter[] oldvalue, App.Model.MCharacter[] newvalue)
        {
            if (newvalue == null || newvalue.Length == 0)
            {
                return;
            }
            foreach (App.Model.MCharacter mCharacter in newvalue)
            {
                VCharacter vCharacter = vCharacters.Find(_=>_.ViewModel.Id.Value == mCharacter.Id);
                if (vCharacter == null)
                {
                    //新建武将
                    GameObject obj = this.Controller.ScrollViewSetChild(characterLayer.transform, characterPrefab, mCharacter);
                    obj.name = string.Format("Character_{0}_{1}", mCharacter.Belong.ToString(), mCharacter.CharacterId.ToString());
                    int i = mCharacter.CoordinateY * mapWidth + mCharacter.CoordinateX;
                    VTile vTile = tileUnits[i];
                    obj.transform.eulerAngles = new Vector3(-30f, 0f, 0f);
                    obj.transform.localPosition = vTile.transform.localPosition;
                    obj.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                    mCharacter.X = obj.transform.localPosition.x;
                    mCharacter.Y = obj.transform.localPosition.y;
                    mCharacter.Action = App.Model.ActionType.idle;
                    obj.GetComponent<VCharacter>().UpdateView();
                }
                else
                {
                    vCharacters.Remove(vCharacter);
                    //更新武将
                }
            }
            if (vCharacters.Count > 0)
            {
                //删除多余武将
            }
            vCharacters = characterLayer.GetComponentsInChildren<VCharacter>(true).ToList();
        }
        #endregion
        public List<VCharacter> Characters{
            get{ 
                return vCharacters;
            }
        }
        public override void UpdateView(){
            ResetAll();
            CharactersChanged(null, ViewModel.Characters.Value);
        }
        public void ResetAll(App.Model.Master.MBaseMap baseMapMaster = null){
            if (baseMapMaster == null)
            {
                Debug.LogError("ViewModel.MapId.Value = " + ViewModel.MapId.Value);
                baseMapMaster = BaseMapCacher.Instance.Get(ViewModel.MapId.Value);
            }
            int widthCount = 0;
            int heightCount = 0;
            int i = 0;
            VTile obj = null;
            foreach(VTile tile in tileUnits){
                tile.gameObject.SetActive(false);
            }
            if (baseMapMaster.tiles != null && baseMapMaster.tiles.Count > 0)
            {
                foreach (App.Model.Master.MTile tile in baseMapMaster.tiles)
                {
                    i = heightCount * mapWidth + widthCount;
                    obj = tileUnits[i];
                    obj.gameObject.SetActive(true);
                    App.Model.MTile building = System.Array.Find(ViewModel.Tiles.Value, _ => _.x == widthCount && _.y == heightCount);
                    //if(building != null)Debug.LogError(widthCount + ","+heightCount + ", " + building.Master);
                    string name = "";
                    if (building != null)
                    {
                        if (building is App.Model.Master.MWorld)
                        {
                            name = (building as App.Model.Master.MWorld).build_name;
                        }/*else if (building is App.Model.Master.MArea)
                    {
                        name = (building as App.Model.Master.MArea).build_name;
                    }*/
                    }
                    obj.SetData(heightCount * baseMapMaster.width + widthCount, widthCount, heightCount, tile.id, building != null ? building.Master.id : 0, name);
                    widthCount++;
                    if (widthCount >= baseMapMaster.width)
                    {
                        widthCount = 0;
                        heightCount++;
                    }
                    if (heightCount >= baseMapMaster.height)
                    {
                        break;
                    }
                }
            }
            BoxCollider collider = this.GetComponent<BoxCollider>();
            collider.size = new Vector3(baseMapMaster.width * tileWidth + 0.345f, baseMapMaster.height * tileHeight + 0.2f, 0f);
            collider.center = new Vector3(collider.size.x * 0.5f - 0.345f, -collider.size.y * 0.5f + 0.4f, 0f);
            mousePosition.x = int.MinValue;
            dragPosition.x = int.MinValue;
        }
        private void SetTilesColor(List<VTile> tiles, Color color){
            foreach (VTile tile in tiles)
            {
                tile.SetColor(color);
            }
        }
        public void ShowMovingTiles(List<VTile> tiles, App.Model.Belong belong){
            foreach (VTile tile in tiles)
            {
                tile.ShowMoving(belong);
            }
        }
        public void ShowAttackTiles(List<VTile> tiles){
            foreach (VTile tile in tiles)
            {
                tile.ShowAttack();
            }
        }
        public void HideMovingTiles(List<VTile> tiles){
            foreach (VTile tile in tiles)
            {
                tile.HideMoving();
            }
        }
        public void HideAttackTiles(List<VTile> tiles){
            foreach (VTile tile in tiles)
            {
                tile.HideAttack();
            }
        }
        public bool IsDraging{
            get{ 
                return _isDraging;
            }
        }
        public void MoveToPosition(int x = int.MinValue, int y = 0, App.Model.Master.MBaseMap baseMapMaster = null){
            if (baseMapMaster == null)
            {
                baseMapMaster = BaseMapCacher.Instance.Get(ViewModel.MapId.Value);
            }
            if (x == int.MinValue)
            {
                x = Mathf.FloorToInt(baseMapMaster.width / 2f);
                y = Mathf.FloorToInt(baseMapMaster.height / 2f);
            }
            int i = y * mapWidth + x;
            VTile obj = tileUnits[i];
            Camera3dToPosition(obj.transform.position.x, obj.transform.position.y - 9f);


            obj = tileUnits[0];
            minPosition = new Vector2(obj.transform.position.x, obj.transform.position.y - 9f);
            i = (baseMapMaster.height - 1) * mapWidth + baseMapMaster.width - 1;
            obj = tileUnits[i];
            maxPosition = new Vector2(obj.transform.position.x, obj.transform.position.y - 9f);
        }
        void OnMouseDrag(){
            if (mousePosition.x == int.MinValue)
            {
                return;
            }
            float x = camera3dPosition.x + (mousePosition.x - Input.mousePosition.x) * 0.005f;
            float y = camera3dPosition.y + (mousePosition.y - Input.mousePosition.y) * 0.005f;
            if (x < minPosition.x)
            {
                x = minPosition.x;
            }
            else if (x > maxPosition.x)
            {
                x = maxPosition.x;
            }
            if (y < maxPosition.y)
            {
                y = maxPosition.y;
            }
            else if (y > minPosition.y)
            {
                y = minPosition.y;
            }
            camera3d.transform.localPosition = new Vector3(x, y, camera3d.transform.localPosition.z);
            dragPosition.x = Input.mousePosition.x;
            dragPosition.y = Input.mousePosition.y;
        }
        void OnMouseUp(){
            if (Global.SceneManager.DialogIsShow() || !Camera3DEnable)
            {
                return;
            }
            _isDraging = Mathf.Abs(Input.mousePosition.x - mousePosition.x) > 4f || Mathf.Abs(Input.mousePosition.y - mousePosition.y) > 4f;
            if (!_isDraging)
            {
                return;
            }
            float mx = Input.mousePosition.x - dragPosition.x;
            float my = Input.mousePosition.y - dragPosition.y;
            if (mx != 0 || my != 0)
            {
                float tx = camera3d.transform.localPosition.x;
                float ty = camera3d.transform.localPosition.y;
                if(mx != 0){
                    tx -= mx * 0.05f;
                }
                if(my != 0){
                    ty -= my * 0.05f;
                }
                float x = tx;
                float y = ty;
                if (x < minPosition.x)
                {
                    x = minPosition.x;
                }
                else if (x > maxPosition.x)
                {
                    x = maxPosition.x;
                }
                if (y < maxPosition.y)
                {
                    y = maxPosition.y;
                }
                else if (y > minPosition.y)
                {
                    y = minPosition.y;
                }
                //惯性
                HOTween.To(camera3d.transform, 0.3f, new TweenParms().Prop("localPosition", 
                    new Vector3(x, y, camera3d.transform.localPosition.z)));
            }
            mousePosition.x = int.MinValue;
        }
        void OnMouseDown(){
            if (Global.SceneManager.DialogIsShow() || !Camera3DEnable)
            {
                mousePosition.x = int.MinValue;
                return;
            }
            mousePosition.x = Input.mousePosition.x;
            mousePosition.y = Input.mousePosition.y;
            camera3dPosition = new Vector2(camera3d.transform.localPosition.x, camera3d.transform.localPosition.y);
        }
        public void Camera3dToPosition(float x, float y){
            HOTween.To(camera3d.transform, 0.3f, new TweenParms().Prop("localPosition", new Vector3(x, y, camera3d.transform.localPosition.z)));
        }
        public bool Camera3DEnable{
            set{ 
                _camera3DEnable = value;
            }
            get{ 
                return _camera3DEnable;
            }
        }
    }
}