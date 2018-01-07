using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    /**
     * 
    */
    public class SItem : SBase {
        public MItem[] items;
        public SItem(){
        }
        public class ResponseList : ResponseBase
		{
            public MItem[] items;
		}
        public IEnumerator RequestList()
		{
            var url = "item/item_list";
            //WWWForm form = new WWWForm();
            //form.AddField("user_id", userId);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url ));
            ResponseList response = client.Deserialize<ResponseList>();
            this.items = response.items;
        }
	}
}