using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Game.PuzzleManagement.QuizPuzzles
{
    public class CorrectnessIndicator : MonoBehaviour
    {
        [Min(0.01f)]
        [SerializeField]private float fadeTime = 5.0f;
    
        private TextMeshProUGUI _indicatorText;
    
        private Coroutine _fadeCoroutine;
    
        public void IndicateCorrect()
        {
            _indicatorText.text = "Right";
            if(_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            _fadeCoroutine = StartCoroutine(FadeText());
        }
    
        public void IndicateWrong()
        {
            _indicatorText.text = "Wrong";
            if(_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            _fadeCoroutine = StartCoroutine(FadeText());
        }
    
        public void ClearText()
        {
            _indicatorText.text = "";
        }
    
        private IEnumerator FadeText()
        {
            float remainingTime = fadeTime;
            float alphaValue = 1.0f;
            float fadeWaitTime = 1.0f/fadeTime;
            while(remainingTime > 0.0f)
            {
            
                alphaValue -= fadeWaitTime * Time.deltaTime;
                
                Color newColor = _indicatorText.color;
                newColor.a = alphaValue;
                _indicatorText.color = newColor;
    
                yield return null;
                remainingTime -= Time.deltaTime;
            }
    
            _fadeCoroutine = null;
        }
        private void Awake() 
        {
            _indicatorText = GetComponent<TextMeshProUGUI>();
        }
    }

}
