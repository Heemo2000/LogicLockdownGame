using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leguar.TotalJSON;
namespace Game
{
    public class TotalJSONTest : MonoBehaviour
    {
        [SerializeField]private TextAsset jsonFile;


        // Start is called before the first frame update
        void Start()
        {
              try
              {
                JSON json = JSON.ParseString(jsonFile.text);
                var crosswordDatas = json.GetJArray("crosswordDatas");

                foreach(JValue value in crosswordDatas.Values)
                {
                    string dataInString = value.CreateString();
                    JSON dataJSON = JSON.ParseString(dataInString);

                    Debug.Log(dataJSON.GetString("word") + ": " + dataJSON.GetString("clue"));
                }
              }
              catch(JSONKeyNotFoundException exception)
              {
                Debug.Log(exception.ToString());
              }
              


        }
    }
}
