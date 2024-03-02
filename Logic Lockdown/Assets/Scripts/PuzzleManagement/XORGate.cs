using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PuzzleManagement
{
    public class XORGate : LogicGate
    {
        [SerializeField]private Pin inputPin1;
        [SerializeField]private Pin inputPin2;

        public override string GetLogicGateName()
        {
            return "XOR";
        }

        public override Voltage GetOutput()
        {
            if(inputPin1.PinVoltage == Voltage.Low && inputPin2.PinVoltage == Voltage.Low)
            {
                return Voltage.Low;
            }

            if(inputPin1.PinVoltage == Voltage.Low && inputPin2.PinVoltage == Voltage.High)
            {
                return Voltage.High;
            }

            if(inputPin1.PinVoltage == Voltage.High && inputPin2.PinVoltage == Voltage.Low)
            {
                return Voltage.High;
            }

            return Voltage.Low;
        }
    }
}