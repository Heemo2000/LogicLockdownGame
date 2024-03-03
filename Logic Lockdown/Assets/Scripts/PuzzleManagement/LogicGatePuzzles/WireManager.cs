using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Controls;


namespace Game.PuzzleManagement.LogicGatePuzzles
{
    public class WireManager : MonoBehaviour
    {

        [Min(0.1f)]
        [SerializeField]private float pinCheckRadius = 0.2f;
        [SerializeField]private LayerMask pinMask;
        [SerializeField]private Camera lookCamera;
        [SerializeField]private Wire wirePrefab;


        [Min(0.1f)]
        [SerializeField]private float minWireLength = 1.0f;

        [Min(0.1f)]
        [SerializeField]private float maxWireLength = 2.0f;

        //private Pin _startingPin;
        //private Pin _endingPin;
        private Wire _wire;
        private int _clickCount = 0;

        private WireManagerControls _controls;
        private Vector2 _posOnScreen = Vector2.zero;

        private Plane _checkPlane;


        private void OnScreenClick(InputAction.CallbackContext context)
        {
            
            Ray ray = lookCamera.ScreenPointToRay(_posOnScreen);

            if(Physics.SphereCast(ray, pinCheckRadius, out RaycastHit hit, 1000.0f, pinMask.value))
            {
                _clickCount++;
                Pin pin = hit.transform.GetComponent<Pin>();
                if(_clickCount == 1)
                {
                    _checkPlane.SetNormalAndPosition(lookCamera.transform.forward, pin.transform.position);

                    _wire = Instantiate(wirePrefab, Vector3.zero, Quaternion.identity);
                    _wire.StartPinPos = hit.point;
                    _wire.EndPinPos = hit.point;
                    _wire.StartingPin = pin;
                    _wire.Init();
                }
                else if(_clickCount == 2)
                {
                    
                    _wire.EndPinPos = hit.point;

                    float squareDistance = Vector3.SqrMagnitude(_wire.EndPinPos - _wire.StartPinPos);
                    if(squareDistance < minWireLength * minWireLength)
                    {
                        _clickCount--;
                        return;
                    }
                    
                    //_wire.EndPinPos = _endingPin.transform.position;

                    _wire.EndingPin = pin;
                    _clickCount = 0;
                    _wire = null;
                }
            }
        }

        private void OnPosOnScreen(InputAction.CallbackContext context)
        {
            _posOnScreen = context.ReadValue<Vector2>();
            if(_wire != null)
            {
                
                Ray ray = lookCamera.ScreenPointToRay(_posOnScreen);

                Vector3 endPosition = Vector3.zero;
                if(_checkPlane.Raycast(ray, out float distance))
                {
                    endPosition = ray.origin + ray.direction * distance;
                }
                else if(Physics.SphereCast(ray, pinCheckRadius, out RaycastHit hit, 1.0f, pinMask.value))
                {
                    endPosition = hit.point;
                }

                float squareDistance = Vector3.SqrMagnitude(endPosition - _wire.StartPinPos);

                if(squareDistance < minWireLength * minWireLength)
                {
                    Vector3 direction = (endPosition - _wire.StartPinPos).normalized;
                    endPosition = _wire.StartPinPos + direction * minWireLength;
                }

                else if(squareDistance > maxWireLength * maxWireLength)
                {
                    Vector3 direction = (endPosition - _wire.StartPinPos).normalized;
                    endPosition = _wire.StartPinPos + direction * maxWireLength;
                }

                _wire.EndPinPos = endPosition;
            }
        }


        private void Awake() {
            _controls = new WireManagerControls();
            _checkPlane = new Plane(transform.forward, transform.position);
        }


        // Start is called before the first frame update
        void Start()
        {
            _controls.WireManagerActionMap.ScreenClick.Enable();
            _controls.WireManagerActionMap.PosOnScreen.Enable();
            _controls.WireManagerActionMap.ScreenClick.performed += OnScreenClick;
            _controls.WireManagerActionMap.PosOnScreen.performed += OnPosOnScreen;
        }

        private void OnEnable() 
        {
            _controls.WireManagerActionMap.ScreenClick.Enable();
            _controls.WireManagerActionMap.PosOnScreen.Enable();
            
        }

        private void OnDisable() 
        {
            _controls.WireManagerActionMap.ScreenClick.Disable();
            _controls.WireManagerActionMap.PosOnScreen.Disable();
            
        }
    }
}
