using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
namespace Game
{
    public class FadeScreen : MonoBehaviour
    {
        [Range(0.1f,5.0f)]
        [SerializeField]private float fadeRate = 2.0f;
        [SerializeField]private Image fadeImage;
        [SerializeField]private CinemachineBrain cinemachineBrain;
        
        public IEnumerator Fade()
        {
            // cause IsBlending has little bit delay so it's need to wait
            yield return new WaitUntil(() => cinemachineBrain.IsBlending);
 
            // wait until blending is finished
            yield return new WaitUntil(() => !cinemachineBrain.IsBlending);
 
            float alpha = 0.0f;
            Color currentColor = fadeImage.color;
            currentColor.a = 0.0f;
            fadeImage.color = currentColor;
            while(alpha <= 1.0f)
            {
                currentColor.a = alpha;
                fadeImage.color = currentColor;
                yield return null;
                alpha += fadeRate * Time.deltaTime;
            }

            currentColor.a = 1.0f;
            fadeImage.color = currentColor;

        }
    }
}
