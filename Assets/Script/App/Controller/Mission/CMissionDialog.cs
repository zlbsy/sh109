using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.View.Item;
using App.Controller.Common;
using UnityEngine.UI;
using App.Util.Cacher;


namespace App.Controller.Mission{
    public class CMissionDialog : CDialog {
        [SerializeField]private Text title;
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        private App.Model.Master.MMission.MissionType[] types = new App.Model.Master.MMission.MissionType[]{
            App.Model.Master.MMission.MissionType.normal,
            App.Model.Master.MMission.MissionType.daily,
            App.Model.Master.MMission.MissionType.events
        };
        private int typeIndex = 0;
        private SMission sMission;
        public override IEnumerator OnLoad( Request request ) 
        {  
            yield return StartCoroutine(base.OnLoad(request));
            sMission = new SMission();
            yield return StartCoroutine(sMission.Download(App.Model.Scriptable.MissionAsset.Url, App.Util.Global.versions.mission, (AssetBundle assetbundle)=>{
                App.Model.Scriptable.MissionAsset.assetbundle = assetbundle;
                MissionCacher.Instance.Reset(App.Model.Scriptable.MissionAsset.Data.missions);
                App.Model.Scriptable.MissionAsset.Clear();
            }));
            ScrollViewSets();

            yield return 0;
        }
        public void ClickLeft(){
            typeIndex--;
            if (typeIndex < 0)
            {
                typeIndex = types.Length - 1;
            }
            ScrollViewSets();
        }
        public void ClickRight(){
            typeIndex++;
            if (typeIndex > types.Length - 1)
            {
                typeIndex = 0;
            }
            ScrollViewSets();
        }
        private void ScrollViewSets(){
            App.Model.Master.MMission.MissionType type = types[typeIndex];
            title.text = Language.Get(string.Format("mission_{0}", type.ToString()));
            MMission[] missions = System.Array.FindAll(Global.SUser.self.missions, m=>m.Master.mission_type == type);
            System.Array.Sort(missions, (a, b)=>{
                int va = a.Status == MMission.MissionStatus.clear ? 1 : 0;
                int vb = b.Status == MMission.MissionStatus.clear ? 1 : 0;
                return vb - va;
            });
            ScrollViewSets(content, childItem, missions);
        }
        public void ClickComplete(App.View.Mission.VMissionChild child){
            StartCoroutine(Complete(child));
        }
        public IEnumerator Complete(App.View.Mission.VMissionChild child){
            App.Model.Master.MMission missionMaster = App.Util.Cacher.MissionCacher.Instance.Get(child.ViewModel.MissionId.Value);
            yield return StartCoroutine(sMission.RequestComplete(child.ViewModel.MissionId.Value));
            if (sMission.responseComplete.result)
            {
                ScrollViewSets();

                Request request = Request.Create("title","获取奖励","contents", missionMaster.rewards);
                this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.ContentsConfirmDialog, request));
            }
        }
        /*public override void Close(){
            base.Close();
        }*/
	}
}