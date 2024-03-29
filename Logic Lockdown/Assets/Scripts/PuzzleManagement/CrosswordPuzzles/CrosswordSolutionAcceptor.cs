using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PuzzleManagement.CrosswordPuzzles
{
    public class CrosswordSolutionAcceptor : SolutionAcceptor
    {
        private CrosswordManager _crosswordManager;

        public override bool IsSolutionCorrect()
        {
            return _crosswordManager.AllTilesFilledCorrect();
        }

        private void Awake() {
            _crosswordManager = GetComponent<CrosswordManager>();
        }


    }
}
