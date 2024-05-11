using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.PuzzleManagement;
using UnityEngine.UI;
namespace Game.UI
{
    public class PlayerKeyHolderUI : MonoBehaviour
    {
        
        [SerializeField]private Vector2 offset = new Vector2(80.0f,0.0f) ;
        [SerializeField]private Transform keyTemplate;
        private Transform _container;
        
        
        public void UpdateVisual(List<KeyType> keyList)
        {
            foreach(Transform child in _container)
            {
                Destroy(child.gameObject);
            }

            //Vector2 initialPosition = _container.GetComponent<RectTransform>().anchoredPosition;
            for(int i = 0; i < keyList.Count; i++)
            {
                KeyType keyType = keyList[i];
                Transform keyTransform = Instantiate(keyTemplate, _container);
                keyTransform.gameObject.SetActive(true);
                keyTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offset.x * (i+1), offset.y);//initialPosition + new Vector2(offset * i, 0);
                Image keyImage = keyTransform.Find("image").GetComponent<Image>();
                switch (keyType) 
                {
                    case KeyType.Red:   keyImage.color = Color.red;     break;
                    case KeyType.Green: keyImage.color = Color.green;   break;
                    case KeyType.Blue:  keyImage.color = Color.blue;    break;
                    case KeyType.Black: keyImage.color = Color.black;   break;
                }
            }
        }
        
        // Start is called before the first frame update
        void Start()
        {
            _container = transform.Find("container");
        }

        
    }
}
