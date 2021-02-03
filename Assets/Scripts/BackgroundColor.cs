using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
    public static BackgroundColor instance;

    [SerializeField] private Color[] _colors;
    [SerializeField] private MeshRenderer[] objectsToChangeColor;


    private Color _colorNow;
    private Color _colorNext;
    private Color _colorInTheProcces;

    private int _colorSelected;
    private float _lerpChangeColor;

    private void Awake()
    {
        instance = this;

        _colorSelected = -1;
        _lerpChangeColor = 0;

        ChangeCurrentColor();

        _colorInTheProcces = _colorNow;
        ChangeSelectedMaterialsColor();
    }




    public void ChangeColor()
    {
        _lerpChangeColor += Time.deltaTime / 7;
        

        _colorInTheProcces = Color.Lerp(_colorNow, _colorNext,_lerpChangeColor);
        ChangeSelectedMaterialsColor();

        if (_lerpChangeColor >= 1)
        {
            _lerpChangeColor = 0;
            ChangeCurrentColor();
        }
    }

    private void ChangeSelectedMaterialsColor()
    {
        for (int i = 0; i < objectsToChangeColor.Length; i++)
        {
            objectsToChangeColor[i].material.SetColor("_ColorTop", _colorInTheProcces);
        }
    }

    private void ChangeCurrentColor()
    {
        _colorSelected++;

        if (_colorSelected >= _colors.Length)
        {
            _colorSelected = 0;
        }


        _colorNow = _colors[_colorSelected];
        _colorNext = _colors[GetNextIndexInAColorsTabByCurrentIndex()];
    }



    private int GetNextIndexInAColorsTabByCurrentIndex()
    {
        if (_colorSelected + 1 >= _colors.Length)
            return 0;

        return _colorSelected + 1;
    }


    public Color GetSelectedColor()
    {
        return _colors[_colorSelected];
    }

}
