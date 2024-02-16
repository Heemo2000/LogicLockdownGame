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

        [SerializeField]private LayerMask pinMask;
        
        [SerializeField]private Color lowVoltColor;
        [SerializeField]private Color highVoltColor;

        [SerializeField]private Material wireMaterialPrefab;
        [Min(0.1f)]
        [SerializeField]private float width = 1.0f;

        [Min(0.1f)]
        [SerializeField]private float pinIndicatorRadius = 0.2f;
        private LineRenderer _renderer;
        private Material _wireMaterial;
        private Collider[] _detectedCollider = new Collider[1];

        public UnityEvent<Voltage> OnSetVoltage;

        public Vector3 StartPinPos { get => startPinPos; set => startPinPos = value; }
        public Vector3 EndPinPos { get => endPinPos; set => endPinPos = value; }

        public Pin StartingPin { get => startingPin; }
        public Pin EndingPin { get => endingPin; }

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
        private void UpdateWireStatus()
        {
            Voltage startingPinVoltage = Voltage.Low;

            int count = Physics.OverlapSphereNonAlloc(startPinPos, pinIndicatorRadius, _detectedCollider, pinMask.value);

            if(count > 0)
            {
                startingPin = _detectedCollider[0].transform.GetComponent<Pin>();
                if(startingPin != null)
                {
                    startingPinVoltage = startingPin.PinVoltage;
                    //_renderer.SetPosition(0, startingPin.transform.position);
                    _renderer.SetPosition(0, startPinPos);
                }
            }
            
            
            OnSetVoltage?.Invoke(startingPinVoltage);

            count = Physics.OverlapSphereNonAlloc(endPinPos, pinIndicatorRadius, _detectedCollider, pinMask.value);

            if(count > 0)
            {
                endingPin = _detectedCollider[0].transform.GetComponent<Pin>();
                if(endingPin != null)
                {
                    endingPin.SetVoltage(startingPinVoltage);
                    //_renderer.SetPosition(1, endingPin.transform.position); 
                    _renderer.SetPosition(1, endPinPos);   
                }
            }
            

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
            UpdateWireStatus();    
        }

        private void Update() 
        {
            UpdateWireStatus();
        }

        private void OnDestroy() 
        {
            OnSetVoltage.RemoveAllListeners();    
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(startPinPos, pinIndicatorRadius);
            Gizmos.DrawWireSphere(endPinPos, pinIndicatorRadius);
        }
    }
}
