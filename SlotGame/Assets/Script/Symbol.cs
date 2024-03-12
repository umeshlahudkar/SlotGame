using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Symbol : MonoBehaviour
{
    [SerializeField] private Image thisImg;
    [SerializeField] private RectTransform thisTransform;
    [SerializeField] private GameObject frameImg;
    [SerializeField] private GameObject EffectImg;
    [SerializeField] private Animator animator;

    private SymbolType symbolType;

    private void OnEnable()
    {
        animator.enabled = false;
        frameImg.SetActive(false);
        EffectImg.SetActive(false);
    }

    public void InitSymbol(Sprite sprite, SymbolType _symbolType)
    {
        thisImg.sprite = sprite;
        symbolType = _symbolType;
    }

    public RectTransform ThisTransform { get { return thisTransform; } }
    public SymbolType SymbolType { get { return symbolType; } }
}
