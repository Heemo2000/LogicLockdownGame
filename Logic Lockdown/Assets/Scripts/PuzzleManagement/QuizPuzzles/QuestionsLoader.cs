using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leguar.TotalJSON;


namespace Game.PuzzleManagement.QuizPuzzles
{

    [System.Serializable]
    public class QuizData
    {
        public string question;
        public string[] options;
        public int answerIndex;

        public QuizData(string question, string[] options, int answerIndex)
        {
            this.question = question;
            this.options = options;
            this.answerIndex = answerIndex;
        }
    }
    public class QuestionsLoader : MonoBehaviour
    {
        [SerializeField]private TextAsset quizFile;

        [SerializeField]private List<QuizData> quizDatas = new List<QuizData>();

        public QuizData GetRandomQuestion()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            int randomIndex = Random.Range(0, quizDatas.Count);
            return quizDatas[randomIndex];
        }

        private void LoadQuestions()
        {
            JSON json = JSON.ParseString(quizFile.text);
            var data = json.GetJArray("quizDatas");


            try
            {
                foreach(JValue value in data.Values)
                {
                    string dataInString = value.CreateString();
                    JSON dataJSON = JSON.ParseString(dataInString);

                    string question = dataJSON.GetString("question");
                    string[] options = dataJSON.GetJArray("options").AsStringArray();
                    int answerIndex = dataJSON.GetInt("answerIndex");
                    QuizData quizData = new QuizData(question, options, answerIndex);
                    quizDatas.Add(quizData);
                }
            }
            catch(JSONKeyNotFoundException exception)
            {
                Debug.Log(exception.StackTrace);
            }
            catch(JValueNullException exception)
            {
                Debug.Log(exception.StackTrace);
            }
            catch(JValueTypeException exception)
            {
                Debug.Log(exception.StackTrace);
            }        
        }

        private void Awake() 
        {
            LoadQuestions();
        }
    }
}
