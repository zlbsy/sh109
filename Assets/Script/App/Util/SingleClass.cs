using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;

namespace App.Util{
    public class SingleClass<T> where T : class,new(){
        protected static T instance;

        public static T Instance 
        {
            get { 
                return instance ?? (instance = new T()); 
            }
        }
	}
}