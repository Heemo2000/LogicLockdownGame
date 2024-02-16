using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PuzzleManagement
{
    public interface LogicGate
    {
        string GetLogicGateName();
        Voltage GetOutput();
    }
}
