using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model{
    [System.Serializable]
    public enum ContentType{
        clothes,
        horse,
        weapon,
        item,
        character,
        gold,
        silver,
        ap
    }
    [System.Serializable]
	public class MContent : MBase {
        public MContent(){
            
        }
        public ContentType type;
        public int content_id;
        public int value;
        public string message;
	}
}