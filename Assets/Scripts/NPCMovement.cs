using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float speed = 2f; // Velocidade de movimento do NPC
    public Transform player; // Referência ao jogador
    public string dialogueKey; // Chave do diálogo para iniciar
    private bool isMovingToPlayer = true; // Indica se está indo em direção ao jogador
    private bool isReturning = false; // Indica se está retornando à esquerda
    private DialogueManager dialogueManager; // Referência ao sistema de diálogos

    private void Start()
    {
        // Inicializa o diálogo
        dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager não encontrado na cena!");
        }
    }

    private void Update()
    {
        if (isMovingToPlayer)
        {
            MoveTowards(player.position);
        }
        else if (isReturning)
        {
            MoveTowards(new Vector3(-10f, transform.position.y, transform.position.z));

            if (transform.position.x < -12f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void MoveTowards(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private void StartDialogue()
    {
        if (dialogueManager != null)
        {
            Debug.Log($"NAMEZADA {dialogueKey}");
            dialogueManager.LoadNpcDialogue(dialogueKey);
            dialogueManager.StartDialogue(dialogueKey);
        }
    }

    public void OnDialogueEnd()
    {
        // Chamada pelo DialogueManager ao finalizar o diálogo
        isReturning = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Para o NPC e inicia o diálogo
            isMovingToPlayer = false;
            StartDialogue();
        }
    }
}
