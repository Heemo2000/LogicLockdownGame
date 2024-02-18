using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PuzzleManagement
{
    public class ANDGate : LogicGate
    {
        public override string GetLogicGateName()
        {
            return "AND";   
        }

        public override Voltage GetOutput()
        {
            return base.inputPin1.PinVoltage & base.inputPin2.PinVoltage;
        }
    }
}
