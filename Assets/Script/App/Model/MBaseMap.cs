using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;


namespace App.Model{
    public class MBaseMap : MBase {
        public MBaseMap(){
            viewModel = new VMBaseMap ();
        }
        public VMBaseMap ViewModel { get { return (VMBaseMap)viewModel; } }
        public int MapId{
            set{
                this.ViewModel.MapId.Value = value;
            }
            get{ 
                return this.ViewModel.MapId.Value;
            }
        }
        public MCharacter[] Characters{
            set{
                this.ViewModel.Characters.Value = value;
            }
            get{ 
                return this.ViewModel.Characters.Value;
            }
        }
        public MTile[] Tiles{
            set{
                this.ViewModel.Tiles.Value = value;
            }
            get{ 
                return this.ViewModel.Tiles.Value;
            }
        }
	}
}