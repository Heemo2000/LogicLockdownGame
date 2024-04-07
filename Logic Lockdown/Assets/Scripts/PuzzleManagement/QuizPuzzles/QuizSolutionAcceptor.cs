using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PuzzleManagement.QuizPuzzles
{
    public class QuizSolutionAcceptor : SolutionAcceptor
    {
        private QuizHandler _quizHandler;
        public override bool IsSolutionCorrect()
        {
            return _quizHandler.IsAnswerCorrect;
        }

        private void Awake() {
            _quizHandler = GetComponent<QuizHandler>();
        }

        
    }
}
