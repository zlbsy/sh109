using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;


namespace App.Model{
	public class MBattlefield : MBase {
		public MBattlefield(){
            viewModel = new VMBattlefield();
        }
        public VMBattlefield ViewModel { get { return (VMBattlefield)viewModel; } }
        public int Id{
            set{ 
                ViewModel.Id.Value = value;
                App.Model.Master.MBattlefield master = this.Master;
                this.ViewModel.MapId.Value = master.world_id;
                this.ViewModel.Tiles.Value = master.tiles;
                //this.ViewModel.Characters.Value = App.Service.HttpClient.Deserialize<MCharacter[]>(master.enemys);
            }
            get{ 
                return ViewModel.Id.Value;
            }
        }
        public App.Model.Master.MBattlefield Master{
            get{ 
                return App.Util.Cacher.BattlefieldCacher.Instance.Get(Id);
            }
        }
	}
}