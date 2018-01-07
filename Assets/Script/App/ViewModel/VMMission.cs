using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;

namespace App.ViewModel
{
	public class VMMission : VMBase
    {
        public VMProperty<int> Id = new VMProperty<int>();
        public VMProperty<int> UserId = new VMProperty<int>();
        public VMProperty<int> MissionId = new VMProperty<int>();
        public VMProperty<App.Model.MMission.MissionStatus> Status = new VMProperty<App.Model.MMission.MissionStatus>();
        public VMProperty<int> Counts = new VMProperty<int>();
	}
}