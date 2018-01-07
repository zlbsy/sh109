using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    /**
     * 
    */
    public class SRegister : SBase {
        public ResponseInsert responseInsert;
        public MCharacter[] characters;
        public SRegister(){
        }
        public class ResponseList : ResponseBase
		{
            public MCharacter[] characters;
        }
        public class ResponseInsert : ResponseBase
        {
            public string ssid;
        }
        public IEnumerator RequestList(){
            var url = "register/character_list";
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url ));
            ResponseList response = client.Deserialize<ResponseList>();
            this.characters = response.characters;
        }
        public IEnumerator RequestInsert(int characterId, string account, string password, string name)
		{
            var url = "register/insert";
            HttpClient client = new HttpClient();
            WWWForm form = new WWWForm();
            form.AddField("character_id", characterId);
            form.AddField("account", account);
            form.AddField("password", password);
            form.AddField("name", name);
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form ));
            responseInsert = client.Deserialize<ResponseInsert>();
            if (responseInsert.result)
            {
                App.Util.Global.ssid = responseInsert.ssid;
                App.Util.Global.SUser.self = App.Util.Cacher.UserCacher.Instance.Get(responseInsert.user.id);
                PlayerPrefs.SetString("account", account);
                PlayerPrefs.SetString("password", password);
                PlayerPrefs.Save();
            }
        }
	}
}