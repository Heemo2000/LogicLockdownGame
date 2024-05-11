//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using ExtendedButtons;

namespace Game.PuzzleManagement.QuizPuzzles
{
    public class QuizHandler : MonoBehaviour
    {
        [SerializeField]private TMP_Text questionText;
        [SerializeField]private Button3D[] answerButtons;
        [SerializeField]private QuestionsLoader questionsLoader;
        [SerializeField]private CorrectnessIndicator correctnessIndicator;

        public UnityEvent OnCorrectAnswer;
        public UnityEvent OnWrongAnswer;

        private List<TMP_Text> _answerButtonsTexts;

        private QuizData _currentQuestionData;

        private bool _isAnswerCorrect = false;

        public bool IsAnswerCorrect { get => _isAnswerCorrect; }

        private void ShowQuestionData()
        {
            _isAnswerCorrect = false;
            _currentQuestionData = questionsLoader.GetRandomQuestion();
            questionText.text = _currentQuestionData.question;
            correctnessIndicator.ClearText();

            int i = 0;
            while(i < _currentQuestionData.options.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                _answerButtonsTexts[i].text = _currentQuestionData.options[i];
                i++;
            }

            while(i < answerButtons.Length)
            {
                Debug.Log("Disabling button...");
                answerButtons[i].gameObject.SetActive(false);
                i++;
            }
        }

        private void CheckAnswer(TMP_Text answerButtonText)
        {
            Debug.Log("Checking right answer...");
            if(answerButtonText.text == _currentQuestionData.options[_currentQuestionData.answerIndex])
            {
                Debug.Log("Right");
                OnCorrectAnswer?.Invoke();
            }
            else
            {
                Debug.Log("Wrong");
                OnWrongAnswer?.Invoke();
            }
        }



        private void Awake() 
        {
            _answerButtonsTexts = new List<TMP_Text>();
        }

        private void Start() 
        {
            foreach(Button3D button in answerButtons)
            {
                TMP_Text textContainer = button.GetComponentInChildren<TMP_Text>(true);
                if(textContainer != null)
                {
                    _answerButtonsTexts.Add(textContainer);
                    button.onClick.AddListener(()=> CheckAnswer(textContainer));
                    button.onEnter.AddListener(()=> {Debug.Log("Entered");});
                }
            }

            OnCorrectAnswer.AddListener(correctnessIndicator.IndicateCorrect);
            OnWrongAnswer.AddListener(correctnessIndicator.IndicateWrong);
            OnWrongAnswer.AddListener(ShowQuestionData);
            OnCorrectAnswer.AddListener(()=> _isAnswerCorrect = true);
            OnWrongAnswer.AddListener(()=> _isAnswerCorrect = false);
            

            ShowQuestionData();    
        }
        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                ShowQuestionData();
            }
        }

        private void OnDestroy() 
        {
            for(int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].onClick.RemoveAllListeners();
            }

            OnCorrectAnswer.RemoveAllListeners();
            OnWrongAnswer.RemoveAllListeners();    
        }
    }

}
