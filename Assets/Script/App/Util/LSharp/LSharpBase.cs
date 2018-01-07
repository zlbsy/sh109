using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;
using System;
using System.Reflection;

namespace App.Util.LSharp{
    public class LSharpBase<T> where T : class,new(){
        protected static T instance;

        public static T Instance 
        {
            get { 
                return instance ?? (instance = new T()); 
            }
        }
        public virtual void Analysis(){
        }
        protected void Analysis(string value, out string methodName, out string[] arguments){
            int methodStart = value.IndexOf('.');
            int start = value.IndexOf("(");
            int end = value.LastIndexOf(")");
            methodName = value.Substring(methodStart + 1, start - methodStart - 1).Trim();
            //Debug.LogError("methodName="+methodName);
            methodName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(methodName);
            //Debug.LogError("methodName ToTitleCase ="+methodName);
            arguments = value.Substring(start + 1, end - start - 1).Split(',');
        }
        public virtual void Analysis(string value){
            string methodName;
            string[] arguments;
            Analysis(value, out methodName, out arguments);
            Debug.Log("Analysis methodName = " + methodName);
            Type t = this.GetType();
            MethodInfo mi = t.GetMethod(methodName,new Type[]{typeof(string[])});
            if (mi != null)
            {
                mi.Invoke(this, new string[][]{arguments});
            }
            else
            {
                LSharpScript.Instance.Analysis();
            }
        }
	}
}