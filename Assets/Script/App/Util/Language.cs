using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Util{
    public class Language{
        private static Dictionary<string, string> dictionaryDatas = new Dictionary<string, string>();
        private static Dictionary<string, string> dictionaryCharacterDatas = new Dictionary<string, string>();
        public static void Reset(App.Model.Master.MWord[] datas){
            dictionaryDatas.Clear();
            foreach(App.Model.Master.MWord data in datas){
                dictionaryDatas.Add(data.key, data.value);   
            }
        }
        public static void ResetCharacterWord(App.Model.Master.MWord[] datas){
            dictionaryCharacterDatas.Clear();
            foreach(App.Model.Master.MWord data in datas){
                dictionaryCharacterDatas.Add(data.key, data.value);   
            }
        }
        public static string Get(string key){
            string val;
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            if (dictionaryDatas.TryGetValue (key, out val))
            {
                return val;
            }
            return key;
        }
        public static string GetCharacterWord(string key){
            string val;
            if (dictionaryCharacterDatas.TryGetValue (key, out val))
            {
                return val;
            }
            return key;
        }
        public static bool IsReady{
            get{ 
                return dictionaryDatas.Count > 0;
            }
        }
    }
}