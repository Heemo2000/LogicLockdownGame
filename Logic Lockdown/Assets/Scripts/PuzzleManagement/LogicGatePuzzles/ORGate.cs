using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PuzzleManagement.LogicGatePuzzles
{
    public class ORGate : LogicGate
    {
        [SerializeField] private Pin inputPin1;
        [SerializeField] private Pin inputPin2;
        [SerializeField] private LayerMask wireMask;
        [SerializeField]private float wireCheckRadius = 0.3f;
        public override string GetLogicGateName()
        {
            return "OR";
        }

        public override Voltage GetOutput()
        {
            if(Physics.CheckSphere(inputPin1.transform.position, wireCheckRadius, wireMask.value) && 
               Physics.CheckSphere(inputPin2.transform.position, wireCheckRadius, wireMask.value))
            {
                return inputPin1.PinVoltage | inputPin2.PinVoltage;
            }
            
            return Voltage.Low;
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(inputPin1.transform.position, wireCheckRadius);
            Gizmos.DrawWireSphere(inputPin2.transform.position, wireCheckRadius);
        }
    }
}
