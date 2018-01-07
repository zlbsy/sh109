using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;

namespace App.Util.LSharp{
    public class LSharpIf : LSharpBase<LSharpIf> {
        
        public static void GetIf(string lineValue){
            int start = lineValue.IndexOf("(");
            int end = lineValue.IndexOf(")");
            string str = lineValue.Substring(start + 1,end - start - 1);
            string[] ifArr = str.Split(new string[] {"&&"}, System.StringSplitOptions.RemoveEmptyEntries);
            bool ifvalue = LSharpIf.CheckCondition(ifArr);
            bool ifvalueend = false;
            int sCount = 0;
            int eCount = 0;
            List<string> childArray = new List<string>();
            while (LSharpScript.Instance.LineListCount > 0)
            {
                sCount = 0;
                string child = LSharpScript.Instance.ShiftLine();
                if (child.IndexOf("elseif") >= 0)
                {
                    if (ifvalue)
                    {
                        ifvalueend = true;
                        continue;
                    }
                    start = child.IndexOf("(");
                    end = child.IndexOf(")");
                    str = child.Substring(start + 1,end - start - 1);
                    str = LSharpVarlable.GetVarlable(str);
                    ifArr = str.Split(new string[] {"&&"}, System.StringSplitOptions.RemoveEmptyEntries);
                    ifvalue = LSharpIf.CheckCondition(ifArr);
                    continue;
                }else if (child.IndexOf("else") >= 0)
                {
                    if(ifvalue){
                        ifvalueend = true;
                        continue;
                    }
                    ifvalue = true;
                    continue;

                }else if (child.IndexOf("endif") >= 0)
                {
                    break;
                }else if (child.IndexOf("if") >= 0)
                {
                    if(ifvalue && !ifvalueend){
                        childArray.Add(child);
                    }
                    sCount = 1;
                    eCount = 0;
                    while (sCount > eCount)
                    {
                        string subChild = LSharpScript.Instance.ShiftLine();
                        if(subChild.IndexOf("if") >= 0 && 
                            subChild.IndexOf("else") < 0 && 
                            subChild.IndexOf("end") < 0){
                            sCount++;
                        }else if(subChild.IndexOf("endif") >= 0){
                            eCount++;
                        }
                        if(ifvalue && !ifvalueend){
                            childArray.Add(subChild);
                        }
                    }
                }

                if (sCount == 0)
                {
                    if (ifvalue && !ifvalueend)
                    {
                        childArray.Add(child);
                    }
                }
            }
            for(var i=childArray.Count - 1;i>=0;i--){
                LSharpScript.Instance.UnshiftLine(childArray[i]);
            }
            LSharpScript.Instance.Analysis();
        }
        private static bool CheckCondition(string[] arr){
            for(var i = 0;i<arr.Length;i++){
                if(!LSharpIf.Condition(arr[i])){
                    return false;
                }   
            }
            return true;
        }
        private static bool Condition(string value){
            if(value.IndexOf("==") >= 0){
                int[] arr=LSharpIf.GetCheckInt(value,"==");
                return arr[0] == arr[1];
            }else if(value.IndexOf("!=") >= 0){
                int[] arr=LSharpIf.GetCheckInt(value,"!=");
                return arr[0] != arr[1];
            }else if(value.IndexOf(">=") >= 0){
                int[] arr=LSharpIf.GetCheckInt(value,">=");
                return arr[0] >= arr[1];
            }else if(value.IndexOf("<=") >= 0){
                int[] arr=LSharpIf.GetCheckInt(value,"<=");
                return arr[0] <= arr[1];
            }else if(value.IndexOf(">") >= 0){
                int[] arr=LSharpIf.GetCheckInt(value,">");
                return arr[0] > arr[1];
            }else if(value.IndexOf("<") >= 0){
                int[] arr=LSharpIf.GetCheckInt(value,"<");
                return arr[0] < arr[1];
            }
            return false;

        }
        private static int[] GetCheckInt(string value, string strChar){
            string[] arr = value.Split(new string[] {strChar}, System.StringSplitOptions.RemoveEmptyEntries);
            return new int[]{int.Parse(arr[0].Trim()), int.Parse(arr[1].Trim())};
        }
        private static string[] GetCheckStr(string value, string strChar){
            string[] arr = value.Split(new string[] {strChar}, System.StringSplitOptions.RemoveEmptyEntries);
            arr[0] = arr[0].Trim().Replace("\"", "");
            arr[1] = arr[1].Trim().Replace("\"", "");
            return arr;
        }
	}
}