using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace App.Util.Cacher{
    public class UserCacher: CacherBase<UserCacher, App.Model.MUser> {
        private List<App.Model.MUser> userList = new List<App.Model.MUser>();
        public void Update(App.Model.MUser user){
            App.Model.MUser userData = Get(user.id);
            if (userData == null)
            {
                userList.Add(user);
            }
            else
            {
                //Debug.LogError("userData=" + userData.id);
                userData.Update(user);
                //Debug.LogError("userData.TopMap=" + userData.TopMap);
            }
        }
        public override App.Model.MUser Get(int id){
            return userList.Find(_=>_.id == id);
        }
        public override App.Model.MUser[] GetAll(){
            return userList.ToArray();
        }
        public void Remove(int id){
            int index = userList.FindIndex(_=>_.id == id);
            if (index >= 0)
            {
                userList.RemoveAt(index);
            }
        }
    }
}