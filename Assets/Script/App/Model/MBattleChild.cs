using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model{
	public class MBattleChild : MBase {
        public MBattleChild(){
            viewModel = new VMBattleChild ();
        }
        public VMBattleChild ViewModel { get { return (VMBattleChild)viewModel; } }
        public int Id{
            set{ 
                ViewModel.Id.Value = value;
            }
            get{ 
                return ViewModel.Id.Value;
            }
        }
        public int BattlefieldId{
            set{ 
                ViewModel.BattlefieldId.Value = value;
            }
            get{ 
                return ViewModel.BattlefieldId.Value;
            }
        }
        public int Level{
            set{ 
                ViewModel.Level.Value = value;
            }
            get{ 
                return ViewModel.Level.Value;
            }
        }
        public int Star{
            set{ 
                ViewModel.Star.Value = value;
            }
            get{ 
                return ViewModel.Star.Value;
            }
        }
	}
}