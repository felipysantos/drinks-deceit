using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    public Text optionText;
    public Button button;
    private int optionIndex;

    public void Setup(Dialogue current, int index, DialogueManager dialogueManager)
    {
        optionText.text = current.options[index].text;
        optionIndex = index;
        button.onClick.AddListener(() =>
        {
            dialogueManager.ChooseOption(optionIndex);
        });
    }
}
