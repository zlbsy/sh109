using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;
using System;


namespace App.Model{
	public class MGacha : MBase {
        public MGacha(){
			viewModel = new VMGacha ();
		}
        public VMGacha ViewModel { get { return (VMGacha)viewModel; } }

        public int GachaId{
            set{
                this.ViewModel.GachaId.Value = value;
            }
            get{ 
                return this.ViewModel.GachaId.Value;
            }
        }
        public int LimitCount{
            set{
                this.ViewModel.LimitCount.Value = value;
            }
            get{ 
                return this.ViewModel.LimitCount.Value;
            }
        }
        public DateTime LastTime{
            set{
                this.ViewModel.LastTime.Value = value;
            }
            get{ 
                return this.ViewModel.LastTime.Value;
            }
        }
        public Master.MGacha Master{
            get{ 
                return App.Util.Cacher.GachaCacher.Instance.Get(GachaId);
            }
        }
        public void Update(MGacha newGacha){
            if (LimitCount != newGacha.LimitCount)
            {
                LimitCount = newGacha.LimitCount;
            }
            if (LastTime.CompareTo(newGacha.LastTime) != 0)
            {
                LastTime = newGacha.LastTime;
            }
        }
	}
}