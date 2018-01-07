using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using UnityEngine.UI;
using App.Util.Cacher;
using Holoville.HOTween;
using App.View.Top;
using System.Linq;
using App.View.Character;
using App.Controller.Common;


namespace App.Controller{
    public class CStage : CBaseMap {
        [SerializeField]Text title;
        //private App.Model.Master.MArea area;
        private App.Model.Master.MWorld world;
        public Request saveRequest{ get;set;}
        public override IEnumerator OnLoad( Request request ) 
        {  
            world = request.Get<App.Model.Master.MWorld>("world");
            this.saveRequest = request;
            title.text = world.build_name;
            yield return this.StartCoroutine(base.OnLoad(request));
        }
        public IEnumerator ReLoad(System.Action onComplete = null){
            yield return this.StartCoroutine(OnLoad( this.saveRequest ));
            if (onComplete != null)
            {
                onComplete();
            }
        }
        protected override void InitMap(){
            mBaseMap = new MBaseMap();
            mBaseMap.MapId = world.map_id;
            mBaseMap.Characters = new MCharacter[]{};
            mBaseMap.Tiles = world.stages;
            vBaseMap.BindingContext = mBaseMap.ViewModel;
            vBaseMap.UpdateView();
            vBaseMap.transform.parent.localScale = Vector3.one;
            vBaseMap.MoveToPosition();
            base.InitMap();
            if (App.Util.Global.SUser.self.IsTutorial)
            {
                return;
            }
            App.Util.LSharp.LSharpFunction.Clear();
            //根据world.id配对scenario脚本
            if (world.id == 5)
            {
                //TODO:
                string str = "Character.add(8,stand,right,12,4,true); \rCharacter.add(19,stand,right,12,5,true); \rCharacter.add(23,stand,right,11,5,true); \rCharacter.add(24,stand,right,12,8,true); \rCharacter.add(21,stand,right,11,1,true); \rCharacter.add(22,stand,right,12,1,true);\rfunction jieshao();\rTalk.setplayer(@player_id,0,史大哥，想不到在这里碰到你了！,true);\rTalk.setnpc(8,0,这不是@player_name小英雄吗？快过来，我给你介绍两位哥哥认识！,false);\rTalk.setplayer(@player_id,0,（这两个人相貌不凡，难道也是我要找的人？）,true);\rTalk.setnpc(8,0,这位是鲁达哥哥，是这里的提辖。这位是李忠哥哥，人称“打虎将”！,false);\rTalk.setplayer(@player_id,0,拜见两位哥哥。,true);\rTalk.setnpc(19,0,既然是史大郎的朋友，一起坐下来饮酒。,false);\rTalk.setplayer(@player_id,0,多谢鲁达哥哥，那@player_claim就不客气了。,true);\rTalk.setnpc(22,0,呜呜！,false);\rTalk.setnpc(19,0,......,false);\rTalk.setnpc(22,0,呜呜！,false);\rTalk.setnpc(19,0,店小儿，你过来，洒家有话问你。,false);\rTalk.setnpc(24,0,提辖，有什么吩咐？,false);\rTalk.setnpc(19,0,洒家问你，你可认得洒家？,false);\rTalk.setnpc(24,0,提辖说笑了，这渭州城有谁不认识提辖大人？,false);\rTalk.setnpc(19,0,既然认得洒家，你这店里为什么找洒家的晦气？,false);\rTalk.setnpc(24,0,提辖说哪里话？就是借小人几个脑袋小人也不敢啊？,false);\rTalk.setnpc(19,0,洒家在这里陪朋友喝酒，你不找个唱戏的也就算了，竟然找人在旁边哭丧？,false);\rTalk.setnpc(24,0,冤枉啊提辖大人，这不干小人的事啊。,false);\rTalk.setnpc(19,0,我看你是找打！,false);\rTalk.setnpc(8,0,哥哥息怒，可能确实不干他的事。,false);\rTalk.setnpc(23,0,是啊，把那哭啼的人找来问清楚了再打也不迟。,false);\rTalk.setnpc(19,0,哼！你还愣着干吗？还不快去把人给我带过来？,false);\rTalk.setnpc(24,0,是是，小人这就去。,false);\rTalk.setnpc(22,0,参见大人，大人恕罪。,false);\rTalk.setnpc(19,0,你们两个是哪里人？为什么在这里啼哭？,false);\rTalk.setnpc(22,0,奴家父女是东京人事，来渭州投奔亲戚，不想亲戚搬到南京去了，我等又没了回去的路费。这里有个财主，叫做“镇关西”郑大官人，见奴家有些姿色，便强要奴家做了妾。,false);\rTalk.setnpc(22,0,还硬写了三千贯卖身钱，实在是虚钱实契，而他家大娘子十分厉害，不久就把我赶了出来，却又追要那三千贯卖身钱。,false);\rTalk.setnpc(22,0,当初实在不曾拿他一文钱啊，没办法只能靠每天卖唱赚一些钱还他，这几天生意不好，怕他来讨债，因此啼哭。,false);\rTalk.setnpc(22,0,没想到冒犯了大人，请大人恕罪。,false);\rTalk.setnpc(19,0,你姓什么？那个郑大官人住在哪？,false);\rTalk.setnpc(21,0,老汉姓金，排行第二，女儿小子翠莲。郑大官人便是此处状元桥下卖肉的郑屠，绰号镇关西。,false);\rTalk.setnpc(19,0,我呸！俺以为是哪个郑大官人，一个杀猪的，也敢这样欺负人！你们三个先在这里坐在，洒家去打死那个杀猪的就回来。,false);\rTalk.setnpc(8,0,哥哥息怒，今天天色晚了，明天再去找他也不迟。,false);\rTalk.setnpc(19,0,哼！真是气死人了！老儿，你来，洒家与你些盘缠。你这便回东京去，如何？,false);\rTalk.setnpc(21,0,如果能回到乡里，您就是我老汉的再生爷娘啊。,false);\rTalk.setnpc(19,0,洒家今日出门只带了五两银子，你们几位借些给洒家，洒家明天还你们。,false);\rTalk.setnpc(8,0,哥哥说的什么话，多少钱还需要还。这是十两银子，哥哥尽管拿去。,false);\rTalk.setnpc(19,0,你也借些出来与洒家。,false);\rTalk.setnpc(24,0,小弟今日没卖出多少药，只能拿出这二两银子了。,false);\rTalk.setnpc(19,0,也不是个爽利的人。罢了，老儿，这些盘缠给你做路费，你现在就动身会东京去。,false);\rTalk.setnpc(24,0,这可不行啊。,false);\rTalk.setnpc(19,0,为什么不可以，他欠你钱了？,false);\rTalk.setnpc(24,0,不是欠小人的钱，是欠郑大官人的典身钱，郑大官人让我看管着。,false);\rTalk.setnpc(19,0,郑屠的钱，洒家会还给他。你要是敢再说个不字，洒家把你脖子拧断。,false);\rTalk.setnpc(24,0,......,false);\rTalk.setnpc(19,0,老儿，你现在就走，我看哪个不怕死的敢拦你。,false);\rTalk.setnpc(21,0,多谢大恩人，多谢大恩人。,false);\rScreen.fadeIn();\rCharacter.hide(21);\rCharacter.hide(22);\rScreen.fadeOut();\rTalk.setnpc(19,0,你们三个在这里守着，别让店小二赶去拦截那老儿，洒家去找那郑屠算账。,false);\rCharacter.hide(19);\rTalk.setnpc(8,0,哥哥......,false);\rTalk.setplayer(@player_id,0,(走的好快......),true);\rTalk.setnpc(23,0,鲁达哥哥正在气头上，这一去恐怕会闹出事情来。,false);\rTalk.setplayer(@player_id,0,两位哥哥在此看守，我追上去看一下。,true);\rTalk.setnpc(8,0,如此最好，小英雄一切小心。,false);\r\rendfunction;\rfunction characterclick_8();\rCall.jieshao();\rendfunction;\rfunction characterclick_19();\rCall.jieshao();\rendfunction;\rfunction characterclick_23();\rCall.jieshao();\rendfunction;\rfunction characterclick_24();  \rTalk.setnpc(24,0,客观，要喝两杯吗？,false); \rendfunction;\rfunction characterclick_21();\rTalk.setnpc(22,0,呜呜...！,false); \rTalk.setplayer(@player_id,0,(这对父女怎么了？我还是先不要多管闲事了。),false); \rendfunction;\rfunction characterclick_22();  \rTalk.setnpc(22,0,呜呜...！,false); \rTalk.setplayer(@player_id,0,(这对父女怎么了？我还是先不要多管闲事了。),false); \rendfunction;";
                List<string> script = str.Split('\r').ToList();
                App.Util.LSharp.LSharpScript.Instance.Analysis(script);
            }
            else
            {
                App.Util.LSharp.LSharpScript.Instance.Analysis(new List<string>{ string.Format("Load.script({0})", world.id * 100) });
            }
        }
        public override void OnClickTile(int index){
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            List<VCharacter> vCharacters = vBaseMap.Characters;
            VCharacter vCharacter = vBaseMap.Characters.Find(_=>_.ViewModel.CoordinateX.Value == coordinate.x && _.ViewModel.CoordinateY.Value == coordinate.y);
            if (vCharacter != null)
            {
                App.Util.LSharp.LSharpScript.Instance.Analysis(new List<string>{string.Format("Call.characterclick_{0}();", vCharacter.ViewModel.Id.Value)});
            }
        }
        public void GotoArea(){
            Request req = Request.Create("worldId", saveRequest.Get<int>("worldId"), "nameKey", saveRequest.Get<string>("nameKey"));
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.World.ToString(), req );
        }
	}
}