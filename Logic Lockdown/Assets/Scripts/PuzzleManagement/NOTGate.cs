using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PuzzleManagement
{
    public class NOTGate : LogicGate
    {
        [SerializeField]private Pin inputPin;

        public override string GetLogicGateName()
        {
            return "NOT";
        }

        public override Voltage GetOutput()
        {
            if(inputPin.PinVoltage == Voltage.Low)
            {
                return Voltage.High;
            }

            return Voltage.Low;
        }
    }
}
