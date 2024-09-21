using UnityEngine;
using UnityEngine.UI; // Make sure to include this for Button
using TMPro;

public class DifficultyController : MonoBehaviour
{
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    public enum DifficultyLevel { Easy, Medium, Hard }
    public DifficultyLevel selectedDifficulty = DifficultyLevel.Easy;

    void Start()
    {
        SelectHard();
    }

    public void SelectEasy()
    {
        selectedDifficulty = DifficultyLevel.Easy;
        Debug.Log("selected easy");
        UpdateButtonColors();
    }

    public void SelectMedium()
    {
        selectedDifficulty = DifficultyLevel.Medium;
        Debug.Log("selected Mid");
        UpdateButtonColors();
    }

    public void SelectHard()
    {
        selectedDifficulty = DifficultyLevel.Hard;
        Debug.Log("selected hard");
        UpdateButtonColors();
    }

    private void UpdateButtonColors()
    {
        easyButton.GetComponentInChildren<TMP_Text>().color = (selectedDifficulty == DifficultyLevel.Easy) ? Color.green : Color.white;
        mediumButton.GetComponentInChildren<TMP_Text>().color = (selectedDifficulty == DifficultyLevel.Medium) ? Color.green : Color.white;
        hardButton.GetComponentInChildren<TMP_Text>().color = (selectedDifficulty == DifficultyLevel.Hard) ? Color.green : Color.white;
    }
}
