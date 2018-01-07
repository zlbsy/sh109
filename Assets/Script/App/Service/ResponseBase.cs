using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;


namespace App.Service{
    public class ResponseBase {
        public System.DateTime now;
        public bool result = false;
        public string message;
        public bool goto_login = false;
        public App.Model.MUser user;
	}
}