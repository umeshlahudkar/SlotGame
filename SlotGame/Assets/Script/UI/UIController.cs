using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the Button click event and Screen open and Close
/// </summary>
public class UIController : Singleton<UIController>
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button testButton;
    [SerializeField] private WinScreen winScreen;

    /// <summary>
    /// Referenced on Start Button
    /// gets called when start button pressed
    /// </summary>
    public void OnStartButtonClick()
    {
        ToggleStartButton(false);
        SlotMachineController.Instance.SpinSlotMachine();
    }


    /// <summary>
    /// Referenced on Test button
    /// gets called when Test button pressed 
    /// </summary>
    public void OnTestButtonClick()
    {
        ToggleStartButton(false);
        SlotMachineController.Instance.SpinSlotMachineForTest();
    }

    /// <summary>
    /// opens game win screen
    /// </summary>
    public void OpenWinScreen()
    {
        winScreen.gameObject.SetActive(true);
    }

    /// <summary>
    /// Enables/Disables the start and Test button
    /// </summary>
    /// <param name="status"></param>
    public void ToggleStartButton(bool status)
    {
        startButton.interactable = status;
        testButton.interactable = status;
    }
}
