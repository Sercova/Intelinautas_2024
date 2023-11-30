using System;
using System.Collections.Generic;
using System.IO;
using Comunications.AWS;
using IntelliChallenge.RavenMatrix.ChallengeFormatter;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Rendering;

namespace IntelliChallenge.RavenMatrix
{
    public class ChallengeDesigner
    {

        private int elementsQty;
        private int itemsByElement;
        private String tipoPregunta;

        public ChallengeFormat challengeSelected { get; set; }
        public ChallengeFormat alternativesForChallenge { get; set; }
        public ChallengeFormat Diseñar()
        {
           // ChallengeBuilder builder = new ChallengeBuilder();
            ChallengeFormat challengeFormat = new ChallengeFormat();
            ChallengeItemFormat itemFormat;
            ChallengeElementFormat elementFormat;
            
            challengeFormat.elementsQty = elementsQty;
            challengeFormat.questionType = tipoPregunta;
            //diseñar los elementos
            for (int i = 0; i < elementsQty; i++)
            {
                elementFormat = new ChallengeElementFormat();
                elementFormat.itemsQty = itemsByElement;
                //diseñar los item
                for (int j = 0; j < itemsByElement; j++)
                {

                    // lee los comportamientos por json
                    itemFormat = new ChallengeItemFormat();
                    // TODO: evitar dejar los datos en un archivo debido a la concurrencia
                    itemFormat.Behaviors = JsonUtility.FromJson<List<ItemBehavior>>("ItemBehavior.json");
                    elementFormat.ItemFormatsList.Add(itemFormat);
                }
                challengeFormat.ElementFormatsList.Add(elementFormat);
            }

            foreach (var e in challengeFormat.ElementFormatsList)
            {
                Debug.Log(e.ToString());
            }
            return challengeFormat;
            //    builder.format = challengeFormat;
            //   builder.Construir();
        }

        public void getChallengeFromCloud()
        {
            ServiceReader serviceReader = new ServiceReader();
            //var readAllText = serviceReader.readAWS();
            // With a file stored in Resources folder
            var readAllText = File.ReadAllText("Assets/Resources/PlayerChallenge.json");

            Debug.Log("CHALLENGE: " + readAllText);
            CloudResponseChallenge cloudResponseChallenge =
                JsonConvert.DeserializeObject<CloudResponseChallenge>(readAllText);
            challengeSelected = cloudResponseChallenge.Challenge;
            alternativesForChallenge = cloudResponseChallenge.Alternatives;
        }
        //public ChallengeFormat readJson()
        //{
        //    var readAllText = File.ReadAllText("Assets/Resources/challengeFormat.json");
        //  //  ChallengeFormat format = JsonUtility.FromJson<ChallengeFormat>(readAllText);
        //  ChallengeFormat format=  JsonConvert.DeserializeObject<ChallengeFormat>(readAllText);
        //    return format;
        //    //return JsonUtility.FromJson<ChallengeFormat>("challengeFormat.json");
        //}
        
        public ChallengeFormat readJsonAlernatives()
        {
            ServiceReader serviceReader = new ServiceReader();

            //#region With AWS (comment for debugging)
            //var readAllText = serviceReader.readAWS();
            //ChallengeFormat format = JsonConvert.DeserializeObject<ChallengeFormat>(readAllText);
            //#endregion

            #region With a file stored in Resources folder
            var readAllText = File.ReadAllText("Assets/Resources/PlayerChallenge.json");
            ChallengeFormat format = JsonUtility.FromJson<ChallengeFormat>(readAllText);
            #endregion

            // Uncomment for Debugging
            Debug.Log("ALTERNATIVES: " + readAllText);

            return format;
            //return JsonUtility.FromJson<ChallengeFormat>("challengeFormat.json");
        }
    }
}