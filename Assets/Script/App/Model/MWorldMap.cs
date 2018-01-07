using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;


namespace App.Model{
    public class MWorldMap : MBaseMap {
        public MWorldMap(){
            viewModel = new VMBaseMap ();
        }
	}
}