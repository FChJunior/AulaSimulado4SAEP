// Importa o namespace UnityEngine para acessar as funcionalidades básicas do Unity
using UnityEngine;

// Define a classe Kunai que herda de MonoBehaviour, permitindo que seja anexada a objetos no Unity
public class Kunai : MonoBehaviour
{
    // Variáveis públicas que podem ser ajustadas no Inspector do Unity
    public float speed;          // Velocidade de movimento do kunai
    public float timeDestroy = 2; // Tempo em segundos antes que o kunai seja destruído automaticamente

    // Método chamado na inicialização do objeto
    void Start()
    {
        // Agenda a destruição do objeto kunai após 'timeDestroy' segundos
        Destroy(gameObject, timeDestroy);
    }

    // Método chamado a cada frame
    void Update()
    {
        // Move o kunai na direção à direita do objeto (transform.right) multiplicado pela velocidade e pelo tempo delta
        transform.position += speed * Time.deltaTime * transform.right;
    }

    // Método chamado quando o kunai entra em colisão com um Collider2D marcado como "Trigger"
    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto colidido NÃO tem a tag "Player"
        if(other.gameObject.tag != "Player") 
            // Se não for o jogador, destrói o kunai
            Destroy(gameObject);
    }
}
