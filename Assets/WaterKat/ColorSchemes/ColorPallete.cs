using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace WaterKat.WKColor
{
    [CreateAssetMenu(fileName = "New_ColorPallete", menuName = "WaterKat/WKColor/ColorPallete", order = 1)]
    public class ColorPallete : ScriptableObject
    {
        [Serializable]
        public struct ColorScheme
        {
            public string colorName;
            public Color color;
        }
        [SerializeField]
        List<ColorScheme> colorSchemes;
    }
}