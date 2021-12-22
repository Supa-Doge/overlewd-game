using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class AdditiveHighlight : MonoBehaviour
    {
        private static GameObject aditiveGO;
        private static Transform parentTransform;
        private Image image;
        private Material material;
        private Color materialColor;
    
        private void Awake()
        {
            image = aditiveGO.GetComponent<Image>();
            image.sprite = parentTransform.GetComponent<Image>().sprite;
            image.SetNativeSize();
    
            material = Resources.Load("Material") as Material;
    
            image.material = material;
            materialColor = material.GetColor("_TintColor");
        }
    
        private void Start()
        {
            ChangeAlpha();
        }
    
        private async void ChangeAlpha()
        {
            var minAlpha = 0.016f;
            var maxAlpha = 0.09f;
            var alphaStep = 0.007f;
            var delay = 60; //Milliseconds
    
            while (true)
            {
                for (float i = maxAlpha; i >= minAlpha; i -= alphaStep)
                {
                    await Task.Delay(delay);
                    materialColor.a = i;
                    material.SetColor("_TintColor", materialColor);
                }
    
                for (float i = minAlpha; i <= maxAlpha; i += alphaStep)
                {
                    await Task.Delay(delay);
                    materialColor.a = i;
                    material.SetColor("_TintColor", materialColor);
                }
            }
        }
    
    
        public static AdditiveHighlight GetInstance(Transform parent)
        {
            parentTransform = parent;
    
            aditiveGO = new GameObject();
            aditiveGO.name = nameof(AdditiveHighlight);
            aditiveGO.AddComponent<CanvasRenderer>();
            aditiveGO.AddComponent<Image>();
    
            aditiveGO.transform.SetParent(parent);
            aditiveGO.transform.position = parent.position;
    
            return aditiveGO.AddComponent<AdditiveHighlight>();
        }
    }

}