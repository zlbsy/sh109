using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;


namespace App.Model{
	public class MBase {
        protected VMBase viewModel;
        public int id;
        public VMBase VM{
            get{ 
                return viewModel;
            }
        }
	}
}