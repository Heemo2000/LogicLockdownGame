using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
//using UnityEngine.EventSystems;


namespace Game.PuzzleManagement.CrosswordPuzzles
{
    public class CrosswordTile : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]private TMP_InputField inputField;
        [SerializeField]private Image backgroundImage;
        [SerializeField]private Sprite whiteBackground;
        [SerializeField]private Sprite transparentBackground;  
        [SerializeField]private Vector2 maxCheckDistance;
        [SerializeField]private LayerMask tileMask;
        [SerializeField]private CrosswordTileInfo position;
    
        public TMP_InputField InputField { get => inputField; }
        public List<CrosswordInfo> Crosswords { get => _crosswords; }
        public CrosswordTileInfo Position { get => position; set => position = value; }
        public char CluePart { get => _cluePart; set => _cluePart = value; }
        public List<CrosswordTile> OtherTiles { get=> _otherTiles; }
        public bool ShowCluePart { get => _showCluePart; set => _showCluePart = value; }
        public bool ShowBackgroundGraphic { get => _showBackgroundGraphic; set => _showBackgroundGraphic = value; }

        private List<CrosswordInfo> _crosswords = new List<CrosswordInfo>();
        private char _cluePart = '_';
        private bool _showCluePart = false;
        private bool _showBackgroundGraphic = false;
        
        private List<CrosswordTile> _otherTiles = new List<CrosswordTile>();
    
        public void SetChar(char value)
        {
            inputField.text = value + "";
        }
        public void AddCrosswordInfo(CrosswordInfo crossword)
        {
            if(!_crosswords.Contains(crossword))
            {
                _crosswords.Add(crossword);
            }
        }
    
        public void ShowBackground()
        {
            backgroundImage.sprite = whiteBackground;
        }
    
        public void HideBackground()
        {
            backgroundImage.sprite = transparentBackground;
        }
        
        /*
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Tile is clicked");
            inputField.Select();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Showing all tiles");
            Show();
            for(int i = 0; i < _otherTiles.Count; i++)
            {
                _otherTiles[i].Show();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Hiding all tiles");
            Hide();
            for(int i = 0; i < _otherTiles.Count; i++)
            {
                _otherTiles[i].Hide();
            }
        }
        */
    
        private void CheckOtherTiles()
        {
            if(inputField.isFocused && Keyboard.current.tabKey.wasPressedThisFrame)
            {
                RaycastHit rightHit;
                RaycastHit downHit;
                bool rightCheck = Physics.Raycast(transform.position, transform.right, out rightHit, maxCheckDistance.x, tileMask.value);
                bool downCheck = Physics.Raycast(transform.position, -transform.up, out downHit, maxCheckDistance.y, tileMask.value);
    
                //If both exist, then choose right one.
                if(rightCheck && downCheck)
                {
                    var rightTile = rightHit.transform.GetComponent<CrosswordTile>();
                    rightTile.InputField.Select();
                }
                else
                {
                    if(rightCheck)
                    {
                        var rightTile = rightHit.transform.GetComponent<CrosswordTile>();
                        rightTile.InputField.Select();
                    }
                    else if(downCheck)
                    {
                        var downTile = downHit.transform.GetComponent<CrosswordTile>();
                        downTile.InputField.Select();
                    }
                }
    
            }
        }

        private void Capitalize(string word)
        {
            inputField.text = word.ToUpper();
        }
        private void Start() {
            HideBackground();
            for(int i = 0; i < _otherTiles.Count; i++)
            {
                _otherTiles[i].HideBackground();
            }
            if(_showCluePart)
            {
                SetChar(_cluePart);
            }
            else
            {
                SetChar(' ');
            }

            inputField.onValueChanged.AddListener(Capitalize);
        }
    
        private void Update() 
        {
            CheckOtherTiles(); 
        }

        
        private void OnMouseEnter()
        {
            if(!_showBackgroundGraphic)
            {
                return;
            }
            ShowBackground();
            for(int i = 0; i < _otherTiles.Count; i++)
            {
                _otherTiles[i].ShowBackground();
            }
        }
        private void OnMouseExit()
        {
            HideBackground();
            for(int i = 0; i < _otherTiles.Count; i++)
            {
                _otherTiles[i].HideBackground();
            }
        }

        private void OnMouseDown() {
            inputField.Select();
        } 
          
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * maxCheckDistance.x);
            Gizmos.DrawLine(transform.position, transform.position - transform.up * maxCheckDistance.y);
        }

        
    }

}
