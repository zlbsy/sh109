using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model{
	public class MItem : MBase {
        public MItem(){
            viewModel = new VMItem ();
        }
        public VMItem ViewModel { get { return (VMItem)viewModel; } }
        public int Id{
            set{ 
                ViewModel.Id.Value = value;
            }
            get{ 
                return ViewModel.Id.Value;
            }
        }
        public int ItemId{
            set{ 
                ViewModel.ItemId.Value = value;
            }
            get{ 
                return ViewModel.ItemId.Value;
            }
        }
        public int Cnt{
            set{ 
                ViewModel.Cnt.Value = value;
            }
            get{ 
                return ViewModel.Cnt.Value;
            }
        }
        public App.Model.Master.MItem Master{
            get{ 
                return App.Util.Cacher.ItemCacher.Instance.Get(this.ItemId);
            }
        }
	}
}