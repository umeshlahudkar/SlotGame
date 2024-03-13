using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Symbol - is a single icon which seen on the display 
/// Handles the sprite Update and Win Animation of Symbol triggering
/// </summary>
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
        ResetSymbol();
    }

    /// <summary>
    /// Disables animation component
    /// </summary>
    public void ResetSymbol()
    {
        ToggleWinAnimation(false);
    }

    /// <summary>
    /// Assigns Symbol sprite, symbol type
    /// </summary>
    /// <param name="sprite"> Symbol Sprite </param>
    /// <param name="_symbolType"> Symbol Type  </param>
    public void InitSymbol(Sprite sprite, SymbolType _symbolType)
    {
        thisImg.sprite = sprite;
        symbolType = _symbolType;
    }

    /// <summary>
    /// Plays win animation
    /// </summary>
    public void PlayWinAnimation()
    {
        animator.Play(0);
        ToggleWinAnimation(true);
    }

    /// <summary>
    /// Disable/Enables the animator components 
    /// </summary>
    /// <param name="status"></param>
    private void ToggleWinAnimation(bool status)
    {
        frameImg.SetActive(status);
        EffectImg.SetActive(status);
        animator.enabled = status;
    }

    /// <summary>
    /// Gets called when win animation completes
    /// Referenced in Win Animation event
    /// </summary>
    private void OnWinAnimationComplete()
    {
        ResetSymbol();
    }

    public RectTransform ThisTransform { get { return thisTransform; } }
    public SymbolType SymbolType { get { return symbolType; } }
}
