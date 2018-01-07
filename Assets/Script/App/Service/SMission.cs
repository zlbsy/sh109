using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    public class SMission : SBase {
        public SMission(){
        }
        public ResponseBase responseComplete;
        public IEnumerator RequestComplete(int missionId)
        {
            var url = "mission/complete";
            WWWForm form = new WWWForm();
            form.AddField("mission_id", missionId);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));
            responseComplete = client.Deserialize<ResponseBase>();
        }
    }
}