using System.Collections;
using System.Collections.Generic;
using App.ViewModel;
using App.Util.Cacher;


namespace App.Model{
    public class StoryProgress{
        public string k;
        public int v;
    }
	public class MUser : MBase {
        public MUser(){
            viewModel = new VMUser ();
        }
        public VMUser ViewModel { get { return (VMUser)viewModel; } }
        public string name;
        public string password;
        public int lastStageId;
        public int BattlingId;
        /*public int lastAreaId{
            get{ 
                if (lastStageId == 0)
                {
                    return 0;
                }
                App.Model.Master.MArea area = AreaCacher.Instance.GetArea(lastStageId);
                return area.id;
            }
        }
        public int lastWorldId{
            get{ 
                if (lastStageId == 0)
                {
                    return App.Util.Global.worlds[0].id;
                }
                //App.Model.Master.MArea area = AreaCacher.Instance.GetArea(lastStageId);
                App.Model.Master.MWorld world = System.Array.Find(App.Util.Global.worlds, w => w.id == lastStageId);
                return world.id;
            }
        }*/
        public MCharacter[] characters;
        public MEquipment[] equipments;
        public MMission[] missions;
        public MBattleChild[] battlelist;
        public MItem[] items;
        public StoryProgress[] progress;
        public Dictionary<string, int> Progress = new Dictionary<string, int>();
        public int GetValue(string key){
            if (!Progress.ContainsKey(key))
            {
                return 0;
            }
            return Progress[key];
        }
        public bool IsTutorial{
            get{
                return GetValue("tutorial") < App.Util.Global.Constant.tutorial_end;
            }
        }
        public string Nickname{
            get{ 
                return this.ViewModel.Nickname.Value;
            }
            set{ 
                this.ViewModel.Nickname.Value = value;
            }
        }
        public int loginbonus_cnt;
        public bool loginbonus_received;
        public int Face{
            get{ 
                return this.ViewModel.Face.Value;
            }
            set{ 
                this.ViewModel.Face.Value = value;
            }
        }
        public int Level{
            get{ 
                return this.ViewModel.Level.Value;
            }
            set{ 
                this.ViewModel.Level.Value = value;
            }
        }
        public int Gold{
            get{ 
                return this.ViewModel.Gold.Value;
            }
            set{ 
                this.ViewModel.Gold.Value = value;
            }
        }
        public int Silver{
            get{ 
                return this.ViewModel.Silver.Value;
            }
            set{ 
                this.ViewModel.Silver.Value = value;
            }
        }
        public int Ap{
            get{ 
                return this.ViewModel.Ap.Value;
            }
            set{ 
                this.ViewModel.Ap.Value = value;
            }
        }
        public int MaxAp{
            get{ 
                return 99;
            }
        }
        public int GetCurrentAp(System.DateTime now)
        {
            int actionPoint = this.Ap;
            if(this.Ap < this.MaxAp){
                System.DateTime lastStaminaDate = this.LastApDate;
                System.TimeSpan ts = now - lastStaminaDate;
                int totalSeconds = (int)ts.TotalSeconds;
                actionPoint = (int)(totalSeconds / App.Util.Global.Constant.recover_ap_time) + this.Ap;
                if(actionPoint > this.MaxAp){
                    actionPoint = this.MaxAp;
                }
            }
            return actionPoint;
        }
        public int MapId{
            get{ 
                return this.ViewModel.MapId.Value;
            }
            set{ 
                this.ViewModel.MapId.Value = value;
            }
        }
        public MTile[] TopMap{
            get{ 
                return this.ViewModel.TopMap.Value;
            }
            set{ 
                this.ViewModel.TopMap.Value = value;
            }
        }
        public System.DateTime LastApDate{
            get{ 
                return this.ViewModel.LastApDate.Value;
            }
            set{ 
                this.ViewModel.LastApDate.Value = value;
            }
        }
        public MTile GetTile(int x, int y){
            return System.Array.Find(TopMap, _=>_.x == x && _.y == y);
        }
        public void Update(MUser user){
            if(user.Gold != this.Gold){
                this.Gold = user.Gold;
            }
            if(user.Silver != this.Silver){
                this.Silver = user.Silver;
            }
            if(user.Nickname != this.Nickname){
                this.Nickname = user.Nickname;
            }
            if(user.Level != this.Level){
                this.Level = user.Level;
            }
            if(user.Ap != this.Ap){
                this.Ap = user.Ap;
            }
            if(user.MapId != this.MapId){
                this.MapId = user.MapId;
            }
            if(user.LastApDate != this.LastApDate){
                this.LastApDate = user.LastApDate;
            }
            if(user.TopMap != null){
                this.TopMap = user.TopMap;
            }
            if(user.items != null){
                this.items = user.items;
            }
            if(user.equipments != null){
                this.equipments = user.equipments;
            }
            if(user.missions != null){
                this.missions = user.missions;
            }
            if(user.characters != null){
                this.characters = user.characters;
            }
            if(user.battlelist != null){
                this.battlelist = user.battlelist;
            }
            if(user.loginbonus_cnt != this.loginbonus_cnt){
                this.loginbonus_cnt = user.loginbonus_cnt;
            }
            if(user.loginbonus_received != this.loginbonus_received){
                this.loginbonus_received = user.loginbonus_received;
            }
            if(user.progress != null){
                this.Progress.Clear();
                foreach(StoryProgress story in user.progress){
                    this.Progress.Add(story.k, story.v);
                }
            }
        }
	}
}