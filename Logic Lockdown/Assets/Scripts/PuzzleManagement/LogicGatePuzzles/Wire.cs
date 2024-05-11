using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.PuzzleManagement.LogicGatePuzzles
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
        private LineRenderer _lineRenderer;
        private Material _wireMaterial;

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private MeshCollider _collider;

        public UnityEvent<Voltage> OnSetVoltage;

        public Vector3 StartPinPos { get => startPinPos; set => startPinPos = value; }
        public Vector3 EndPinPos { get => endPinPos; set => endPinPos = value; }

        public Pin StartingPin { get => startingPin; set=> startingPin = value;}
        public Pin EndingPin { get => endingPin; set=> endingPin = value;}

        private Voltage _previousStartingPinVoltage = Voltage.Low;

        public void Init()
        {
            _lineRenderer.startWidth = width;
            _lineRenderer.endWidth = width;

            _wireMaterial = Material.Instantiate(wireMaterialPrefab);
            _lineRenderer.sharedMaterial = _wireMaterial;

            _lineRenderer.startColor = lowVoltColor;
            _lineRenderer.endColor = lowVoltColor;
            OnSetVoltage.AddListener(SetWireColor);

            //Done to keep transform in the center of two points.
            transform.position = Vector3.Lerp(startPinPos, endPinPos, 0.5f);

        }

        public void GenerateMeshAndCollider()
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[8];
            Vector2[] uv = new Vector2[8];
            int[] triangles = new int[6*3*2];

            Vector3 startingPinPos = transform.InverseTransformPoint(startingPin.transform.position);
            Vector3 endingPinPos = transform.InverseTransformPoint(endingPin.transform.position);

            vertices[0] = startingPinPos + startingPin.transform.up * width; 
            vertices[1] = endingPinPos + endingPin.transform.up * width;
            vertices[2] = endingPinPos - endingPin.transform.up * width;
            vertices[3] = startingPinPos - startingPin.transform.up * width;

            vertices[4] = startingPinPos + startingPin.transform.up * width + startingPin.transform.forward * width; 
            vertices[5] = endingPinPos + endingPin.transform.up * width + endingPin.transform.forward * width;
            vertices[6] = endingPinPos - endingPin.transform.up * width + endingPin.transform.forward * width;
            vertices[7] = startingPinPos - startingPin.transform.up * width + startingPin.transform.forward * width;

            uv[0] = new Vector2(0,0);
            uv[1] = new Vector2(0,1);
            uv[2] = new Vector2(1,1);
            uv[3] = new Vector2(1,0);

            uv[4] = new Vector2(0,0);
            uv[5] = new Vector2(0,1);
            uv[6] = new Vector2(1,1);
            uv[7] = new Vector2(1,0);

            //Front face
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 3;

            triangles[3] = 3;
            triangles[4] = 1;
            triangles[5] = 2;

            //Back face
            triangles[6] = 5;
            triangles[7] = 4;
            triangles[8] = 6;

            triangles[9] = 6;
            triangles[10] = 4;
            triangles[11] = 7;

            //Left face
            triangles[12] = 4;
            triangles[13] = 0;
            triangles[14] = 7;

            triangles[15] = 7;
            triangles[16] = 0;
            triangles[17] = 3;

            //Right face
            triangles[18] = 2;
            triangles[19] = 1;
            triangles[20] = 5;

            triangles[21] = 2;
            triangles[22] = 5;
            triangles[23] = 6;


            //Top face
            triangles[24] = 4;
            triangles[25] = 5;
            triangles[26] = 0;

            triangles[27] = 0;
            triangles[28] = 5;
            triangles[29] = 1;

            //Bottom face
            triangles[30] = 3;
            triangles[31] = 2;
            triangles[32] = 7;

            triangles[33] = 7;
            triangles[34] = 2;
            triangles[35] = 6;


            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();

            _meshFilter.mesh = mesh;
            _meshRenderer.material = _wireMaterial;
            _collider.sharedMesh = mesh;

            CheckWiring();
        }
        
        public void SetLineRendererActive(bool active)
        {
            _lineRenderer.enabled = active;
        }
        private void CheckWiring()
        {
            if(startingPin == null || endingPin == null)
            {
                return;
            }
            Voltage startingPinVoltage = startingPin.PinVoltage;
            Voltage endingPinVoltage = endingPin.PinVoltage;
            /*
            

            if(startingPinVoltage != _previousStartingPinVoltage)
            {
                OnSetVoltage?.Invoke(startingPinVoltage);
                endingPin.SetVoltage(startingPinVoltage);
            }
            
            _previousStartingPinVoltage = startingPinVoltage;
            */
            /*
            if(startingPinVoltage == Voltage.Low && endingPinVoltage == Voltage.High)
            {
                Pin temp = startingPin;
                startingPin = endingPin;
                endingPin = temp;
            }
            else
            {
                OnSetVoltage?.Invoke(startingPinVoltage);
                endingPin.SetVoltage(startingPinVoltage);
            }
            */
            
            OnSetVoltage?.Invoke(startingPinVoltage);
            endingPin.SetVoltage(startingPinVoltage);
            
        }

        private void Render()
        {
            if(!_lineRenderer.enabled)
            {
                return;
            }
            _lineRenderer.SetPosition(0, startPinPos);
            _lineRenderer.SetPosition(1, endPinPos);
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
            if(_lineRenderer.enabled)
            {
                if(voltage == Voltage.Low)
                {
                    _lineRenderer.startColor = lowVoltColor;
                    _lineRenderer.endColor = lowVoltColor;
                }
                else if(voltage == Voltage.High)
                {
                    _lineRenderer.startColor = highVoltColor;
                    _lineRenderer.endColor = highVoltColor;
                }    
            }
            if(_wireMaterial != null)
            {
                _wireMaterial.color = (voltage == Voltage.Low) ? lowVoltColor : highVoltColor;
            }
        }

        

        private void Awake() 
        {
            if(!TryGetComponent<MeshFilter>(out _meshFilter))
            {
                _meshFilter = gameObject.AddComponent<MeshFilter>();
            }

            if(!TryGetComponent<MeshRenderer>(out _meshRenderer))
            {
                _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }

            if(!TryGetComponent<LineRenderer>(out _lineRenderer))
            {
                _lineRenderer = gameObject.AddComponent<LineRenderer>();
            }

            if (!TryGetComponent<MeshCollider>(out _collider))
            {
                _collider = gameObject.AddComponent<MeshCollider>();
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

            if(endingPin != null)
            {
                endingPin.SetVoltage(Voltage.Low);
            }
            OnSetVoltage.RemoveAllListeners();    
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(startPinPos, pinIndicatorRadius);
            Gizmos.DrawSphere(endPinPos, pinIndicatorRadius);
        }
    }
}
