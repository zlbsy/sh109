using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    /**
     * 
    */
    public class SSkill : SBase {
        public MSkill[] skills;
        public MItem[] items;
        public MSkill skill;
        public SSkill(){
        }
        public class ResponseLevelUp : ResponseBase
		{
            public MSkill skill;
        }
        public class ResponseUnlock : ResponseBase
        {
            public MSkill[] skills;
            public MItem[] items;
        }
        public IEnumerator RequestLevelUp(int id)
        {
            var url = "skill/level_up";
            WWWForm form = new WWWForm();
            form.AddField("skill_id", id);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form ));
            ResponseLevelUp response = client.Deserialize<ResponseLevelUp>();
            this.skill = response.skill;
        }
        public IEnumerator RequestUnlock(int character_id, int skill_id)
        {
            var url = "skill/unlock";
            WWWForm form = new WWWForm();
            form.AddField("character_id", character_id);
            form.AddField("skill_id", skill_id);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form ));
            ResponseUnlock response = client.Deserialize<ResponseUnlock>();
            this.skills = response.skills;
            this.items = response.items;
        }
        public IEnumerator RequestLeran(int character_id, int item_id)
        {
            var url = "skill/learn";
            WWWForm form = new WWWForm();
            form.AddField("character_id", character_id);
            form.AddField("item_id", item_id);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form ));
            ResponseUnlock response = client.Deserialize<ResponseUnlock>();
            this.skills = response.skills;
            this.items = response.items;
        }
	}
}