using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using App.Model.Scriptable;
using App.View.Gacha;
using App.Controller.Common;


namespace App.Controller.Gacha{
    public class CGachaDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        public override IEnumerator OnLoad( Request request ) 
		{  
            StartCoroutine(base.OnLoad(request));
            yield return StartCoroutine(Global.SGacha.Download(ImageAssetBundleManager.gachaIconUrl, App.Util.Global.versions.gacha, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.gachaIcon = assetbundle;
            }, false));
            yield return StartCoroutine(Global.SGacha.Download(GachaAsset.Url, App.Util.Global.versions.gacha, (AssetBundle assetbundle)=>{
                GachaAsset.assetbundle = assetbundle;
                GachaCacher.Instance.Reset(GachaAsset.Data.gachas);
                GachaAsset.Clear();
            }));
            yield return StartCoroutine (Global.SGacha.RequestFreeLog());
            //int gachaId = request.Get<int>("gachaId");
            App.Model.Master.MGacha[] gachas = GachaCacher.Instance.GetAllOpen();
            foreach(App.Model.Master.MGacha gacha in gachas){
                App.Model.MGacha mGacha = System.Array.Find(Global.SGacha.gachas, _=>_.GachaId == gacha.id);
                if (mGacha == null)
                {
                    mGacha = new MGacha();
                    mGacha.GachaId = gacha.id;
                }
                ScrollViewSetChild(content, childItem, mGacha);
            }
		}

        public void OnClickGacha(int gachaId, int priceId, int cnt = 1, bool free_gacha = false){
            StartCoroutine (GachaSlot(gachaId, priceId, cnt, free_gacha));
        }
        public IEnumerator GachaSlot(int gachaId, int priceId, int cnt = 1, bool free_gacha = false){
            yield return StartCoroutine (Global.SGacha.RequestSlot(gachaId, priceId, cnt, free_gacha));
            App.Model.MGacha mGacha = System.Array.Find(Global.SGacha.gachas, _=>_.GachaId == gachaId);
            mGacha.Update(Global.SGacha.currentGacha);
            Request req = Request.Create("contents", Global.SGacha.contents);
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.GachaResultDialog, req));
        }
        public void OnClickDetail(int gachaId){
            string url = string.Format("{0}webview/gacha/{1}.php", HttpClient.docmainBase, gachaId.ToString());
            Request req = Request.Create("url", url);
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.WebviewDialog, req));
        }
	}
}