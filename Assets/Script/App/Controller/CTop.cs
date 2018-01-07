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
using App.Util;
using App.View.Common;
using App.Controller.Common;
using App.Model.Scriptable;


namespace App.Controller{
    public class CTop : CBaseMap {
        [SerializeField]VBuildingMenu buildingMenu;
        [SerializeField]VStrengthenMenu strengthenMenu;
        [SerializeField]GameObject menuBackground;
        [SerializeField]VHeaderFace headerFace;
        [SerializeField]VHeaderTop headerTop;
        [SerializeField]GameObject tilesContent;
        [SerializeField]VCharacterContents charactersContent;
        private VBottomMenu currentMenu;
        public override IEnumerator OnLoad( Request request ) 
        {
            tilesContent.SetActive(false);
            InitHeader();
            InitMap();
            yield return StartCoroutine(base.OnLoad(request));
            yield return StartCoroutine(OnLoadEnd());
        }
        private IEnumerator OnLoadEnd(){
            bool isTutorial = TutorialStart();
            if (!isTutorial && !Global.SUser.self.loginbonus_received)
            {
                App.Model.Master.MLoginBonus[] loginBonusesList = App.Util.Cacher.LoginBonusCacher.Instance.GetAll();
                if (loginBonusesList.Length > 0)
                {
                    App.Model.Master.MLoginBonus loginBonuses = loginBonusesList[Global.SUser.self.loginbonus_cnt];
                    SLoginBonus sLoginBonus = new SLoginBonus();
                    yield return StartCoroutine(sLoginBonus.RequestGet());
                    Request req = Request.Create("loginBonuses", loginBonuses);
                    this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.LoginBonusDialog, req));
                }

            }
            yield break;
        }
        public bool TutorialStart(){
            if (!Global.SUser.self.IsTutorial)
            {
                return false;
            }
            SUser sUser = Global.SUser;
            int tutorial = sUser.self.GetValue("tutorial");
            StartCoroutine(sUser.Download(TutorialAsset.TutorialUrl(tutorial), Global.versions.tutorial, (AssetBundle assetbundle)=>{
                TutorialAsset.assetbundle = assetbundle;
                App.Util.LSharp.LSharpScript.Instance.Analysis(TutorialAsset.Data.tutorial);

                /*
                if(tutorial <= 0){
                    App.Util.LSharp.LSharpScript.Instance.Analysis(TutorialAsset.Data.tutorial);
                }else{c
              
                    List<string> script = new List<string>();     
                    script.Add("Talk.set(4100,0,@player_name，恭喜你打了胜仗啊！,false);");
                    script.Add("Talk.setplayer(@player_id,0,差点儿被你害死了，还好友军比较厉害！,true);");
                    script.Add("Talk.set(4100,0,你也要继续变强啊，才能完成玄女娘娘交给你的任务！,false);");
                    script.Add("Talk.setplayer(@player_id,0,你说的容易，我的法力都被封起来了，一个凡人怎么有那么大的本事啊！,true);");
                    script.Add("Talk.set(4100,0,你也不用太担心，被你放走的妖魔星中，也不全是坏人！,false);");
                    script.Add("Talk.setplayer(@player_id,0,妖魔中还有好人？,true);");
                    script.Add("Talk.set(4100,0,被放走的魔星中，有三十六天罡星和七十二地煞星，这一百单八星虽然也被叫做魔星，但是都心存善念，而且讲义气！,false);");
                    script.Add("Talk.setplayer(@player_id,0,哦！？这么说，我可以先找他们帮忙，一起对付另外的妖星魔星？,true);");
                    script.Add("Talk.set(4100,0,真聪明！现在再跟你介绍一个比较重要的功能，打开任务菜单看一下！,false);");
                    script.Add("Tutorial.clickmask(SceneTop.UICamera.Canvas.LeftMenu.MissionButton,0,0,96,96);");
                    script.Add("Tutorial.call(SceneTop,OpenMission);");
                    script.Add("Tutorial.wait(MissionDialog(Clone).Panel.Scroll View.Viewport.Content.MissionChild(Clone));");
                    script.Add("Talk.set(4100,0,当你完成了一些任务之后，就可以获取一些奖励，你刚刚完成了【援助王进】的任务，先领取奖励吧！,false);");
                    script.Add("Tutorial.clickmask(MissionDialog(Clone).Panel.Scroll View.Viewport.Content.MissionChild(Clone).GetButton,0,0,120,40);");
                    script.Add("Tutorial.call(MissionDialog(Clone).Panel.Scroll View.Viewport.Content.MissionChild(Clone),ClickComplete);");
                    script.Add("Tutorial.wait(ContentsConfirmDialog(Clone));");
                    script.Add("Var.setprogress(tutorial,2);");
                    script.Add("Talk.set(4100,0,每个任务都有相应的说明，你也可以把这些任务当作是游戏进程的提醒，每天通过任务就可以获得很多奖励哦！,false);");
                    script.Add("Talk.set(4100,0,任务功能就介绍到这里了，现在关闭任务窗口！,false);");
                    script.Add("Tutorial.clickmask(ContentsConfirmDialog(Clone).Panel.Close,0,0,96,96);");
                    script.Add("Tutorial.call(ContentsConfirmDialog(Clone),Close);");
                    script.Add("Wait.time(0.1);");
                    script.Add("Tutorial.call(MissionDialog(Clone),Close);");
                    script.Add("Wait.time(0.4);");
                    script.Add("Tutorial.call(SceneTop,TutorialStart);");


                    script.Add("Tutorial.wait(GachaDialog(Clone).Panel.Scroll View.Viewport.Content.Gacha_tutorial);");
                    script.Add("Tutorial.clickmask(GachaDialog(Clone).Panel.Scroll View.Viewport.Content.Gacha_tutorial.ButtonSingle,0,0,160,70);");
                    script.Add("Tutorial.call(GachaDialog(Clone).Panel.Scroll View.Viewport.Content.Gacha_tutorial.ButtonSingle,OnClickGacha);");
                    script.Add("Tutorial.wait(GachaResultDialog(Clone));");
                    script.Add("Wait.time(0.4);");
                    script.Add("Talk.set(4100,0,太棒了！召唤除了紫将啊！记得以后要召唤更多的伙伴啊！,false);");
                    script.Add("Tutorial.call(GachaResultDialog(Clone),BackgroundClick);");
                    script.Add("Wait.time(0.3);");
                    script.Add("Tutorial.call(GachaResultDialog(Clone),Close);");
                    script.Add("Tutorial.call(GachaDialog(Clone),Close);");
                    script.Add("Wait.time(0.3);");
                    script.Add("Talk.set(4100,0,好了，我暂时就帮你到这里了，你以后要自己努力了！,false);");
                    script.Add("Talk.setplayer(@player_id,0,等等......？,true);");
                    script.Add("Talk.setplayer(@player_id,0,走的好快啊...看来以后得靠自己了。,true);");
                    script.Add("Talk.setplayer(@player_id,0,总之，要先去寻找天罡星和地煞星这些人的下落了。,true);");
                    script.Add("Talk.setplayer(@player_id,0,上次一起帮忙打强盗的那个叫史大郎的英雄好像很厉害，会不会是我要找的人呢，我先去找他聊一聊吧。,true);");
                    script.Add("Tutorial.close();");
                    //script.Add("Var.setprogress(tutorial,2);");


                    App.Util.LSharp.LSharpScript.Instance.Analysis(script);
                }*/
            }));
            return true;
        }
        private void InitHeader(){
            MUser mUser = App.Util.Global.SUser.self;
            headerFace.BindingContext = mUser.ViewModel;
            headerFace.UpdateView();
            headerTop.BindingContext = mUser.ViewModel;
            headerTop.UpdateView();
        }
        protected override void InitMap(){
            MUser mUser = App.Util.Global.SUser.self;
            //地图需要判断是否变化，所以另准备一个Model
            mBaseMap = new MTopMap();
            mBaseMap.MapId = mUser.MapId;
            mBaseMap.Tiles = mUser.TopMap.Clone() as App.Model.MTile[];
            base.InitMap();
            System.Array.Sort(mUser.characters, (a, b)=>{
                App.Model.Master.MCharacter aMaster = a.Master;
                App.Model.Master.MCharacter bMaster = b.Master;
                if(bMaster.qualification != aMaster.qualification){
                    return bMaster.qualification - aMaster.qualification;
                }
                if(b.Star != a.Star){
                    return b.Star - a.Star;
                }
                return b.Level - a.Level;
            });

            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
            int x = Mathf.FloorToInt(topMapMaster.width * 0.5f);
            int y = Mathf.FloorToInt(topMapMaster.height * 0.5f);
            int[][] vecs = new int[][]{ 
                new int[]{0, 0},
                new int[]{-1, 3},
                new int[]{1, 2},
                new int[]{-1, -2},
                new int[]{1, -3}
            };
            //MCharacter[] characters = new MCharacter[5];
            //System.Array.Copy(mUser.characters, characters, 5);
            //int i = 0;
            List<MCharacter> characters = new List<MCharacter>();
            foreach (MCharacter character in mUser.characters)
            {
                int[] vec = vecs[characters.Count];
                character.CoordinateX = x + vec[0];
                character.CoordinateY = y + vec[1];
                character.StatusInit();
                characters.Add(character);
                if (characters.Count >= vecs.Length)
                {
                    break;
                }
            }
            mBaseMap.Characters = characters.ToArray();
            vBaseMap.BindingContext = mBaseMap.ViewModel;
            vBaseMap.UpdateView();
            vBaseMap.transform.parent.localScale = Vector3.one;
            vBaseMap.MoveToPosition();
            charactersContent.UpdateView(mUser.characters);
        }
        public override void OnClickTile(int index){
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            App.Model.MTile tile = System.Array.Find(mBaseMap.Tiles, _=>_.x == coordinate.x && _.y == coordinate.y);
            if (tile == null)
            {
                buildingMenu.currentIndex = index;
                OpenMenu(buildingMenu);
            }
            else
            {
                OpenMenu(strengthenMenu);
            }
        }
        public void OpenMenu(VBottomMenu menu){
            currentMenu = menu;
            currentMenu.gameObject.SetActive(true);
            vBaseMap.Camera3DEnable = false;
            menuBackground.SetActive(true);
            menu.Open();
        }
        public void CloseMenu(){
            CloseMenu(null);
        }
        public void CloseMenu(System.Action complete){
            currentMenu.Close(()=>{
                if(complete != null){
                    complete();
                }
                //currentMenu.gameObject.SetActive(false);
                currentMenu = null;
                vBaseMap.Camera3DEnable = true;
                menuBackground.SetActive(false);
            });
        }
        public void GotoWorld(){
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.World.ToString() );
        }
        public void OpenBattleList(){
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.BattleListDialog));
        }
        public void OpenCharacterList(){
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.CharacterListDialog));
        }
        public void OpenItemList(){
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.ItemListDialog));
        }
        public void OpenLoginBonus(){
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.LoginBonusDialog));
        }
        public void OpenPresentBox(){
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.PresentBoxDialog));
        }
        public void OpenMission(){
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.MissionDialog));
        }
        public void OpenNews(){
            string url = string.Format("{0}webview/news/index.php", HttpClient.docmainBase);
            Request req = Request.Create("url", url);
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.WebviewDialog, req));
        }
        public void OpenShop(){
            App.Controller.shop.CShopDialog.Show();
        }
        public void ChangeContents(){
            tilesContent.SetActive(!tilesContent.activeSelf);
            charactersContent.gameObject.SetActive(!charactersContent.gameObject.activeSelf);
        }
    }
}