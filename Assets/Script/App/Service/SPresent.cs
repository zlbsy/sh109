using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    /**
     * 
    */
	public class SPresent : SBase {
        public MPresent[] presents;
        public ResponseBase responseReceive;
        public SPresent(){
        }
        public class ResponseList : ResponseBase
		{
            public MPresent[] presents;
		}
        public IEnumerator RequestList()
		{
            var url = "present/present_list";
            //WWWForm form = new WWWForm();
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url));
            ResponseList response = client.Deserialize<ResponseList>();
            presents = response.presents;
        }
        public IEnumerator RequestReceive(int id)
        {
            var url = "present/receive";
            HttpClient client = new HttpClient();
            WWWForm form = new WWWForm();
            form.AddField("present_id", id);
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));
            responseReceive = client.Deserialize<ResponseBase>();
        }
	}
}