using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;


namespace App.Model{
    public class MTopMap : MBaseMap {
        public MTopMap(){
            viewModel = new VMTopMap ();
        }
	}
}