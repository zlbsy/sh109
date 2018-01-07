using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;


namespace App.Model{
	public class MBuilding : MBase {
        public MBuilding(){
			viewModel = new VMBuilding ();
		}
        public VMBuilding ViewModel { get { return (VMBuilding)viewModel; } }

        public int Id{
            set{
                this.ViewModel.Id.Value = value;
                this.Name = Tile.name;
                this.TileId = Master.tile_id;
            }
            get{ 
                return this.ViewModel.Id.Value;
            }
        }
        public string Name{
            set{
                this.ViewModel.Name.Value = value;
            }
            get{ 
                return this.ViewModel.Name.Value;
            }
        }
        public int TileId{
            set{
                this.ViewModel.TileId.Value = value;
            }
            get{ 
                return this.ViewModel.TileId.Value;
            }
        }
        public Master.MBuilding Master{
            get{ 
                return App.Util.Cacher.BuildingCacher.Instance.Get(Id);
            }
        }
        public Master.MTile Tile{
            get{ 
                return App.Util.Cacher.TileCacher.Instance.Get(Master.tile_id);
            }
        }
	}
}