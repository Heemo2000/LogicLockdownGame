using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Test
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField]private Transform startPoint;
        [SerializeField]private Transform endPoint;

        [SerializeField]private float moveSpeed = 20.0f;

        [SerializeField]private float waitTime = 2.0f;

        private Rigidbody _platformRB;
        private Collider _platformCollider;

        private Vector3 _previousPos = Vector3.zero;
        private Vector3 _velocity = Vector3.zero;
        private IEnumerator MovePlatform()
        {
            Vector3 startPos = startPoint.position;
            Vector3 endPos = endPoint.position;

            float delta = 0.0f;
            while(this.enabled)
            {
                Vector3 lerpedPosition = Vector3.Lerp(startPos, endPos, delta);
                _platformRB.MovePosition(lerpedPosition);
                delta += moveSpeed * Time.fixedDeltaTime;

                if(delta >= 1.0f)
                {
                    yield return new WaitForSeconds(waitTime);
                    Vector3 temp = startPos;
                    startPos = endPos;
                    endPos = temp;
                    delta = 0.0f;
                }
                
                yield return null;
            }
        }


        private void Awake() {
            _platformRB = GetComponent<Rigidbody>();
            _platformCollider = GetComponent<Collider>();
        }

        private void Start() {
            _platformRB.isKinematic = true;
            _platformCollider.isTrigger = true;
            StartCoroutine(MovePlatform());
        }

        private void FixedUpdate() 
        {
            _velocity = (_platformRB.position - _previousPos) / Time.fixedDeltaTime;
            _previousPos = _platformRB.position;
        }

        private void OnTriggerEnter(Collider other) 
        {
            Debug.Log("Detected something.");
            CharacterController controller = other.transform.GetComponent<CharacterController>();
            if(controller != null)
            {
                Debug.Log("Detected character controller.");
                controller.Move(_velocity * Time.fixedDeltaTime);
            }
        }
        private void OnTriggerStay(Collider other) {
            Debug.Log("Detected something.");
            CharacterController controller = other.transform.GetComponent<CharacterController>();
            if(controller != null)
            {
                Debug.Log("Detected character controller.");
                controller.Move(_velocity * Time.fixedDeltaTime);
            }
            
        }
    }
}
