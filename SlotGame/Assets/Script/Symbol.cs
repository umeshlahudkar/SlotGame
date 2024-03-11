using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Symbol : MonoBehaviour
{
    private Image thisImg;
    private RectTransform thisTransform;

    private SymbolType symbolType;

    private void Awake()
    {
        thisImg = gameObject.GetComponent<Image>();
        thisTransform = gameObject.GetComponent<RectTransform>();
    }

    public void InitSymbol(Sprite sprite, SymbolType _symbolType)
    {
        thisImg.sprite = sprite;
        symbolType = _symbolType;
    }

    public RectTransform ThisTransform { get { return thisTransform; } }
    public SymbolType SymbolType { get { return symbolType; } }
}
