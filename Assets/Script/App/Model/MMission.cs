using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model{
    public class MMission : MBase {
        public enum MissionStatus
        {
            init,
            clear,
            complete
        }
        public MMission(){
            viewModel = new VMMission ();
        }
        public VMMission ViewModel { get { return (VMMission)viewModel; } }
        public int Id{
            set{ 
                ViewModel.Id.Value = value;
            }
            get{ 
                return ViewModel.Id.Value;
            }
        }
        public int MissionId{
            set{ 
                ViewModel.MissionId.Value = value;
            }
            get{ 
                return ViewModel.MissionId.Value;
            }
        }
        public int Counts{
            set{ 
                ViewModel.Counts.Value = value;
            }
            get{ 
                return ViewModel.Counts.Value;
            }
        }
        public MissionStatus Status{
            set{ 
                ViewModel.Status.Value = value;
            }
            get{ 
                return ViewModel.Status.Value;
            }
        }
        public App.Model.Master.MMission Master{
            get{ 
                return App.Util.Cacher.MissionCacher.Instance.Get(this.MissionId);
            }
        }
	}
}