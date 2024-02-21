using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PuzzleManagement
{
    public class ORGate : LogicGate
    {
        [SerializeField] private Pin inputPin1;
        [SerializeField] private Pin inputPin2;

        public override string GetLogicGateName()
        {
            return "OR";
        }

        public override Voltage GetOutput()
        {
            return inputPin1.PinVoltage | inputPin2.PinVoltage;
        }
    }
}
