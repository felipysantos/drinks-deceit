using System.Collections.Generic;
using UnityEngine;
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
    private readonly Dialogue currentDialogue;

    public string dialoguesFolder = "Dialogues";

    void Start()
    {
        LoadNpcDialogue("Barman");
        SetDialogue("start");
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

            Dialogue currentDialogue = dialogueDatabase.dialogues.Find(d => d.key == dialogueKey);

            if (currentDialogue != null)
            {
                DisplayDialogue(currentDialogue);
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

    private void DisplayDialogue(Dialogue current)
    {
        if (current != null)
        {
            ClearCards();

            for (int i = 0; i < current.options.Count; i++)
            {
                GameObject card = Instantiate(cardPrefab, cardParent);
                Card dialogueCard = card.GetComponent<Card>();
                dialogueCard.Setup(current.options[i].text, i, this);
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
}
