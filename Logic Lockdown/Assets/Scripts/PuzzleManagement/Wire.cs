using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.PuzzleManagement
{
    [RequireComponent(typeof(LineRenderer))]
    public class Wire : MonoBehaviour
    {
        [SerializeField]private Vector3 startPinPos = Vector3.zero;
        [SerializeField]private Vector3 endPinPos = Vector3.zero;

        [SerializeField]private Pin startingPin;
        [SerializeField]private Pin endingPin;
        
        [SerializeField]private Color lowVoltColor;
        [SerializeField]private Color highVoltColor;

        [SerializeField]private Material wireMaterialPrefab;
        [Min(0.1f)]
        [SerializeField]private float width = 1.0f;

        [Min(0.1f)]
        [SerializeField]private float pinIndicatorRadius = 0.2f;
        private LineRenderer _renderer;
        private Material _wireMaterial;

        public UnityEvent<Voltage> OnSetVoltage;

        public Vector3 StartPinPos { get => startPinPos; set => startPinPos = value; }
        public Vector3 EndPinPos { get => endPinPos; set => endPinPos = value; }

        public Pin StartingPin { get => startingPin; set=> startingPin = value;}
        public Pin EndingPin { get => endingPin; set=> endingPin = value;}

        private Voltage _previousStartingPinVoltage = Voltage.Low;

        public void Init()
        {
            _renderer.startWidth = width;
            _renderer.endWidth = width;

            _wireMaterial = Material.Instantiate(wireMaterialPrefab);
            _renderer.sharedMaterial = _wireMaterial;

            _renderer.startColor = lowVoltColor;
            _renderer.endColor = lowVoltColor;
            OnSetVoltage.AddListener(SetWireColor);

            //Done to keep transform in the center of two points.
            transform.position = Vector3.Lerp(startPinPos, endPinPos, 0.5f);

        }
        private void CheckWiring()
        {
            if(startingPin == null || endingPin == null)
            {
                return;
            }
            Voltage startingPinVoltage = startingPin.PinVoltage;

            if(startingPinVoltage != _previousStartingPinVoltage)
            {
                OnSetVoltage?.Invoke(startingPinVoltage);
                endingPin.SetVoltage(startingPinVoltage);
            }
            
            _previousStartingPinVoltage = startingPinVoltage;
        }

        private void Render()
        {
            _renderer.SetPosition(0, startPinPos);
            _renderer.SetPosition(1, endPinPos);
        }

        private void ResetStartingPin()
        {
            if(startingPin != null && startingPin != endingPin)
            {
                startingPin.SetVoltage(Voltage.Low);
            }
            
            startingPin = null;
        }

        private void ResetEndingPin()
        {
            if(endingPin != null && startingPin != endingPin)
            {
                endingPin.SetVoltage(Voltage.Low);
            }
            
            endingPin = null;
        }

        private void SetWireColor(Voltage voltage)
        {
            if(voltage == Voltage.Low)
            {
                _renderer.startColor = lowVoltColor;
                _renderer.endColor = lowVoltColor;
            }
            else if(voltage == Voltage.High)
            {
                _renderer.startColor = highVoltColor;
                _renderer.endColor = highVoltColor;
            }
        }

        

        private void Awake() 
        {
            if(!TryGetComponent<LineRenderer>(out _renderer))
            {
                _renderer = gameObject.AddComponent<LineRenderer>();
            }
        }

        private void Start() 
        {
            CheckWiring();    
        }

        private void Update() 
        {
            CheckWiring();
            Render();
        }

        private void OnDestroy() 
        {
            OnSetVoltage.RemoveAllListeners();    
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(startPinPos, pinIndicatorRadius);
            Gizmos.DrawSphere(endPinPos, pinIndicatorRadius);
        }
    }
}
