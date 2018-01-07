using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Scenario{
    [System.Serializable]
	public class MScenario : MBase {
        public MScenario(){
        }
        public string class_name;
        public string method_name;
        public string[] arguments;
	}
}