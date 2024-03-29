using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PuzzleManagement
{
    public abstract class SolutionAcceptor : MonoBehaviour
    {
        public abstract bool IsSolutionCorrect();
    }
}
