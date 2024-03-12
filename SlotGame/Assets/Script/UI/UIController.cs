using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [SerializeField] private Button startButton;
    [SerializeField] private WinScreen winScreen;

    public void OnStartButtonClick()
    {
        ToggleStartButton(false);
        SlotMachineController.Instance.SpinSlotMachine();
    }

    public void OpenWinScreen()
    {
        winScreen.gameObject.SetActive(true);
    }

    public void ToggleStartButton(bool status)
    {
        startButton.interactable = status;
    }
}
