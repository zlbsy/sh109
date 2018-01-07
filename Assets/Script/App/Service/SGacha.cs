using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    public class SGacha : SBase {
        public App.Model.MGacha[] gachas;
        public App.Model.MGacha currentGacha;
        public App.Model.MContent[] contents;
        public SGacha(){
        }
        public class ResponseFreeLog : ResponseBase
        {
            public App.Model.MGacha[] gachas;
        }
        public class ResponseSlot : ResponseBase
        {
            public App.Model.MContent[] contents;
            public App.Model.MGacha gacha;
        }
        public IEnumerator RequestFreeLog()
        {
            var url = "gacha/freelog";
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url ));
            ResponseFreeLog response = client.Deserialize<ResponseFreeLog>();
            this.gachas = response.gachas;
        }
        public IEnumerator RequestSlot(int gacha_id, int priceId, int cnt, bool free_gacha)
        {
            var url = "gacha/slot";
            WWWForm form = new WWWForm();
            form.AddField("gacha_id", gacha_id);
            form.AddField("price_id", priceId);
            form.AddField("cnt", cnt);
            form.AddField("free_gacha", free_gacha ? 1 : 0);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));
            ResponseSlot response = client.Deserialize<ResponseSlot>();
            contents = response.contents;
            currentGacha = response.gacha;
        }
    }
}