using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    public class SShop : SBase {
        public SShop(){
        }
        public class ResponseList : ResponseBase
        {
        }
        public class ResponseBuy : ResponseBase
        {
        }
        /*
        public IEnumerator RequestList(int buildingId, int x, int y)
        {
            var url = "shop/shop_list";
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url));
            ResponseList response = client.Deserialize<ResponseList>();
        }
        */
        public IEnumerator RequestBuyBuild(int buildingId, int x, int y)
        {
            WWWForm form = new WWWForm();
            form.AddField("shop_type", "building");
            form.AddField("child_id", buildingId);
            form.AddField("x", x);
            form.AddField("y", y);
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(RequestBuy( form ));
        }
        public IEnumerator RequestBuy(string shopType, int childId)
        {
            WWWForm form = new WWWForm();
            form.AddField("shop_type", shopType);
            form.AddField("child_id", childId);
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(RequestBuy(form));

        }
        public IEnumerator RequestBuy(WWWForm form)
        {
            var url = "shop/buy";
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));

        }
    }
}