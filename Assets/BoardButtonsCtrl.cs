using UnityEngine;
using UnityEngine.UI;

public class BoardButton : MonoBehaviour
{
    public int index; 
    public GameController gameController;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        gameController.PlayerMove(index);
    }
}
