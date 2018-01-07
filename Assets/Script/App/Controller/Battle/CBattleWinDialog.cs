using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using App.Controller.Common;
using Holoville.HOTween;
using App.Util;
using App.Model.Battle;


namespace App.Controller.Battle{
    public class CBattleWinDialog : CDialog {
        [SerializeField]private Transform expContent;
        [SerializeField]private GameObject expChildItem;
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        [SerializeField]private Image[] stars;
        public override IEnumerator OnLoad( Request request ) 
        {  
            yield return StartCoroutine(base.OnLoad(request));
            List<int> characterIds = request.Get<List<int>>("characterIds");
            List<int> dieIds = request.Get<List<int>>("dieIds");
            int star = request.Get<int>("star");
            for (int i = 0; i < stars.Length; i++)
            {
                Image starImg = stars[i];
                starImg.color = i < star ? Color.yellow : Color.white;
            }
            List<App.Model.MBase> expCharacters = new List<App.Model.MBase>();
            foreach (int characterId in characterIds)
            {
                App.Model.MCharacter mCharacter = System.Array.Find(Global.SUser.self.characters, c=>c.CharacterId == characterId);
                MExpCharacter expCharacter = new MExpCharacter();
                expCharacter.id = characterId;
                expCharacter.fromExp = mCharacter.Exp;
                expCharacter.fromLevel = mCharacter.Level;
                expCharacters.Add(expCharacter);
            }
            yield return this.StartCoroutine(Global.SBattlefield.RequestBattleEnd(characterIds, dieIds, star));
            App.Model.MContent[] battleRewards = Global.SBattlefield.battleRewards;
            ScrollViewSets(content, childItem, battleRewards);

            foreach (MExpCharacter expCharacter in expCharacters)
            {
                App.Model.MCharacter mCharacter = System.Array.Find(Global.SUser.self.characters, c=>c.CharacterId == expCharacter.id);
                expCharacter.toExp = mCharacter.Exp;
                expCharacter.toLevel = mCharacter.Level;
            }
            ScrollViewSets(expContent, expChildItem, expCharacters);
        }
        public void BattleOver(){
            (App.Util.SceneManager.CurrentScene as CBattlefield).BattleEnd();
        }
        public void GoldRelive(){
            CAlertDialog.Show("待开发");
        }
        public void ItemRelive(){
            CAlertDialog.Show("待开发");
        }
    }
}