using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;


namespace App.Service{
	public class SBattlefield : SBase {
        public MContent[] battleRewards;
        public ResponseBase battleStartResponse;
		public SBattlefield(){
			
		}
        public class ResponseBattleList : ResponseBase
		{
            public MBattleChild[] battlelist;
        }
        public class ResponseBattleEnd : ResponseBase
        {
            public MContent[] battle_rewards;
        }
        public IEnumerator RequestBattlelist()
        {
            var url = "battle/battle_list";
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url ));
            ResponseBattleList response = client.Deserialize<ResponseBattleList>();
            App.Util.Global.SUser.self.battlelist = response.battlelist;
            App.Util.LSharp.LSharpScript.Instance.UpdateBattleList();
        }
        public IEnumerator RequestBattleStart(int battlefield_id)
        {
            var url = "battle/battle_start";
            WWWForm form = new WWWForm();
            form.AddField("battlefield_id", battlefield_id);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form ));
            battleStartResponse = client.Deserialize<ResponseBase>();
        }
        public IEnumerator RequestBattleReset()
        {
            var url = "battle/battle_reset";
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url ));
        }
        public IEnumerator RequestBattleEnd(List<int> character_ids, List<int> die_ids, int star)
        {
            var url = "battle/battle_end";
            WWWForm form = new WWWForm();
            form.AddField("character_ids", ImplodeList<int>(character_ids));
            form.AddField("die_ids", ImplodeList<int>(die_ids));
            form.AddField("star", star);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form ));
            ResponseBattleEnd response = client.Deserialize<ResponseBattleEnd>();
            this.battleRewards = response.battle_rewards;
            App.Util.LSharp.LSharpScript.Instance.UpdateBattleList();
        }
	}
}