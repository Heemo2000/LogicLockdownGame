using System.Collections;
using UnityEngine;
using Game.PuzzleManagement;
using Game.Core;

namespace Game.InteractionManagement
{
    public class Door : MonoBehaviour
    {
        [SerializeField]private SolutionAcceptor[] solutionAcceptors;
        [SerializeField]private float checkInterval = 1.0f;
        [Min(0.0f)]
        [SerializeField]private float openSpeed = 2.0f;

        [SerializeField]private Transform leftPart;
        [SerializeField]private Transform rightPart;

        [Min(0.1f)]
        [SerializeField]private float maxOpenDistance = 2.23f;
        [SerializeField]private bool allowOpeningAtStarting;
        
        private Coroutine _checkCoroutine;
        private Coroutine _openCoroutine;

        private IEnumerator Check()
        {
            while(!AreAllSolutionsCorrect())
            {
                yield return new WaitForSeconds(checkInterval);
            }

            Open();
        }

        private void Open()
        {
            if(_openCoroutine != null)
            {
                StopCoroutine(_openCoroutine);
            }

            _openCoroutine = StartCoroutine(OpenCoroutine());    
        }

        private IEnumerator OpenCoroutine()
        {
            float delta = 0.0f;

            Vector3 leftPartStartPos = leftPart.position;
            Vector3 rightPartStartPos = rightPart.position;

            Vector3 leftPartFinalPos = leftPart.position + leftPart.forward * maxOpenDistance;
            Vector3 rightPartFinalPos = rightPart.position - rightPart.forward * maxOpenDistance;
            while(delta <= 1.0f)
            {
                if(GameManager.Instance != null && GameManager.Instance.GamePauseStatus == GamePauseStatus.UnPaused)
                {
                    Vector3 leftLerpedPos = Vector3.Lerp(leftPartStartPos, leftPartFinalPos, delta);
                    Vector3 rightLerpedPos = Vector3.Lerp(rightPartStartPos, rightPartFinalPos, delta);

                    leftPart.position = leftLerpedPos;
                    rightPart.position = rightLerpedPos;
                    delta += openSpeed * Time.deltaTime;
                }
                yield return null;
            }

            leftPart.position = leftPartFinalPos;
            rightPart.position = rightPartFinalPos;

            yield return null;
        }
        
        private bool AreAllSolutionsCorrect()
        {
            foreach(var solutionAcceptor in solutionAcceptors)
            {
                if(!solutionAcceptor.IsSolutionCorrect())
                {
                    return false;
                }
            }

            return true;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            if(_checkCoroutine == null && !allowOpeningAtStarting)
            {
                _checkCoroutine = StartCoroutine(Check());
            }
            else if(allowOpeningAtStarting)
            {
                Open();
            }
        }

        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(leftPart.position, leftPart.position + leftPart.forward * maxOpenDistance);
            Gizmos.DrawLine(rightPart.position, rightPart.position - rightPart.forward * maxOpenDistance);
                
        }
    }
}
