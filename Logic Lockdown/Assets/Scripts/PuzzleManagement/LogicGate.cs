using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PuzzleManagement
{
    public abstract class LogicGate : MonoBehaviour
    {
        [SerializeField]private Pin outputPin;
        public abstract string GetLogicGateName();
        public abstract Voltage GetOutput();

        protected virtual void Update()
        {
            outputPin.SetVoltage(GetOutput());
        }
    }
}
