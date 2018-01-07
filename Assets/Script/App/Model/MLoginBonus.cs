using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model{
    public class MLoginBonus : MBase {
        public MLoginBonus(){
            viewModel = new VMLoginBonus ();
        }
        public VMLoginBonus ViewModel { get { return (VMLoginBonus)viewModel; } }

        public int Id{
            set{ 
                ViewModel.Id.Value = value;
            }
            get{ 
                return ViewModel.Id.Value;
            }
        }
        public int Day{
            set{ 
                ViewModel.Day.Value = value;
            }
            get{ 
                return ViewModel.Day.Value;
            }
        }
        public MContent[] Contents{
            set{ 
                ViewModel.Contents.Value = value;
            }
            get{ 
                return ViewModel.Contents.Value;
            }
        }
        public bool Received{
            set{ 
                ViewModel.Received.Value = value;
            }
            get{ 
                return ViewModel.Received.Value;
            }
        }
	}
}