using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model{
    public class MPresent : MBase {
        public MPresent(){
            viewModel = new VMPresent ();
        }
        public VMPresent ViewModel { get { return (VMPresent)viewModel; } }

        public int Id{
            set{ 
                ViewModel.Id.Value = value;
            }
            get{ 
                return ViewModel.Id.Value;
            }
        }
        public MContent Content{
            set{ 
                ViewModel.Content.Value = value;
            }
            get{ 
                return ViewModel.Content.Value;
            }
        }
        public System.DateTime LimitTime{
            set{ 
                ViewModel.LimitTime.Value = value;
            }
            get{ 
                return ViewModel.LimitTime.Value;
            }
        }
	}
}