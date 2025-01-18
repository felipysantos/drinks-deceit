using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class DialogueOption
{
    public string text;
    public string next;
}

[System.Serializable]
public class Dialogue
{
    public string characterName;
    public string line;
    public string key;
    public List<DialogueOption> options;
}

[System.Serializable]
public class DialogueDatabase
{
    public List<Dialogue> dialogues;
}

public class DialogueManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform cardParent;
    private DialogueDatabase dialogueDatabase;
    private Dialogue currentDialogue;

    public string dialoguesFolder = "Dialogues";

    [Header("UI Elements")]
    public GameObject dialoguePanel; // Painel de diálogo
    public Image characterImage; // Imagem do personagem
    public Text characterNameText; // Nome do personagem
    public Text dialogueText;

    void Start()
    {
    }

    public void LoadNpcDialogue(string npcName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, dialoguesFolder, npcName + ".json");

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            dialogueDatabase = JsonUtility.FromJson<DialogueDatabase>(jsonData);
            Debug.Log($"Diálogos do NPC {npcName} carregados com sucesso.");
        }
        else
        {
            Debug.LogError($"Arquivo {npcName}.json não encontrado em {filePath}!");
        }
    }

    public void SetDialogue(string dialogueKey)
    {
        if (dialogueDatabase != null)
        {

             currentDialogue = dialogueDatabase.dialogues.Find(d => d.key == dialogueKey);

            if (currentDialogue != null)
            {
                DisplayDialogue();
            }
            else
            {
                Debug.LogError($"Diálogo com a chave {dialogueKey} não encontrado!");
            }
        }
        else
        {
            Debug.LogError($"Diálogo com a chave {dialogueKey} não encontrado!");
        }
    }

    private void DisplayDialogue()
    {
        if (currentDialogue != null)
        {
            // characterImage.sprite = current.characterImage; 
            characterNameText.text = currentDialogue.characterName;
            dialogueText.text = currentDialogue.line;
            ClearCards();

            for (int i = 0; i < currentDialogue.options.Count; i++)
            {
                GameObject card = Instantiate(cardPrefab, cardParent);
                Card dialogueCard = card.GetComponent<Card>();
                dialogueCard.Setup(currentDialogue, i, this);
            }
        }
        else
        {
            Debug.LogError("currentDialogue está nulo durante o Display!");
        }
    }

    public void ChooseOption(int optionIndex)
    {
        if (optionIndex >= 0 && optionIndex < currentDialogue.options.Count)
        {
            string nextDialogueKey = currentDialogue.options[optionIndex].next;
            SetDialogue(nextDialogueKey);
        }
        else
        {
            Debug.LogError("Índice de opção inválido ou diálogo atual não definido!");
        }
    }

    private void ClearCards()
    {
        foreach (Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void StartDialogue(string npcName)
    {
        LoadNpcDialogue(npcName);
        SetDialogue("start");
        ShowDialoguePanel(true);
    }
    private void ShowDialoguePanel(bool isVisible)
    {
        dialoguePanel.SetActive(isVisible);
    }
    public void EndDialogue()
    {
        ShowDialoguePanel(false);
        currentDialogue = null;
    }
}
