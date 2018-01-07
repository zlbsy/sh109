using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;

namespace App.ViewModel
{
    public class VMUser : VMBase
    {
        public VMProperty<int> Id = new VMProperty<int>();
        public VMProperty<string> Nickname = new VMProperty<string>();
        public VMProperty<int> Face = new VMProperty<int>();
        public VMProperty<int> Level = new VMProperty<int>();
        public VMProperty<int> Gold = new VMProperty<int>();
        public VMProperty<int> Silver = new VMProperty<int>();
        public VMProperty<int> Ap = new VMProperty<int>();
        public VMProperty<int> MapId = new VMProperty<int>();
        public VMProperty<System.DateTime> LastApDate = new VMProperty<System.DateTime>();
        public VMProperty<App.Model.MTile[]> TopMap = new VMProperty<App.Model.MTile[]>();
	}
}