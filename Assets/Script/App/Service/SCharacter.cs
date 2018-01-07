using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    /**
     * 
    */
    public class SCharacter : SBase {
        public MCharacter[] characters;
        public SCharacter(){
        }
        public class ResponseList : ResponseBase
		{
            public MCharacter[] characters;
		}
        public IEnumerator RequestList(int userId)
		{
            var url = "character/character_list";
            WWWForm form = new WWWForm();
            form.AddField("user_id", userId);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));
            ResponseList response = client.Deserialize<ResponseList>();
            this.characters = response.characters;
        }
	}
}