using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    public Text optionText;
    public Button button;
    private int optionIndex;

    public void Setup(string text, int index, DialogueManager dialogueManager)
    {
        optionText.text = text;
        optionIndex = index;
        button.onClick.AddListener(() =>
        {
            dialogueManager.ChooseOption(optionIndex);
        });
    }
}
