using System.Collections;
using System.Collections.Generic;
using Game.PuzzleManagement.CrosswordPuzzles;
using UnityEngine;

namespace Game.PuzzleManagement.LogicGatePuzzles
{
    public class LogicGateSolutionAcceptor : SolutionAcceptor
    {
        [SerializeField]private LogicGate[] outputs;

        public override bool IsSolutionCorrect()
        {
            foreach(LogicGate gate in outputs)
            {
                if(gate.GetOutput() == Voltage.Low)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
