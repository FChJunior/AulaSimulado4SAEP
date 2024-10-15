// Importa namespaces necessários para funcionalidades básicas do Unity, gerenciamento de cenas e interface de usuário
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Define a classe HUD que herda de MonoBehaviour, permitindo que seja anexada a objetos no Unity
public class HUD : MonoBehaviour
{
    // Variável estática que representa a vida atual do jogador
    public static int hp = 3;
    // Vida máxima do jogador e a vida anterior para detecção de mudanças
    public int maxHP, hpPreviour;
    // Array de Sprites que representam os estados de vida (por exemplo, cheio e vazio)
    public Sprite[] states;
    // Array de componentes Image que representam os indicadores de vida na interface de usuário
    public Image[] lifeLights;

    // Referência ao objeto de Game Over para exibição quando a vida chega a zero
    public GameObject gameOver;

    // Variável privada para armazenar a cena atual
    private Scene scene;

    // Método chamado na inicialização do objeto
    void Start()
    {
        // Obtém a cena atualmente ativa
        scene = SceneManager.GetActiveScene();
        // Define a vida inicial com base no índice da cena
        // Se a cena for a de índice 1, define hp para 3, caso contrário, mantém o valor atual de hp
        hp = scene.buildIndex == 1 ? 3 : hp;
        // Atualiza a interface de usuário com o estado inicial da vida
        UpdateHUD();
    }

    // Método chamado a cada frame
    void Update()
    {
        // Garante que a vida não exceda maxHP nem seja menor que 0
        hp = hp > maxHP ? maxHP : hp < 0 ? 0 : hp;

        // Verifica se a vida atual difere da vida anterior
        if (hp != hpPreviour)
        {
            // Atualiza a vida anterior para o valor atual
            hpPreviour = hp;
            // Atualiza a interface de usuário para refletir a nova quantidade de vida
            UpdateHUD();
        }

        // Ativa ou desativa o objeto de Game Over com base na vida
        gameOver.SetActive(hp <= 0);
    }

    // Método para atualizar os indicadores de vida na interface de usuário
    void UpdateHUD()
    {
        // Itera sobre todos os indicadores de vida
        for (int i = 0; i < lifeLights.Length; i++)
        {
            // Define o sprite do indicador de vida:
            // Se a vida atual for maior que o índice, usa o sprite de vida cheia (states[1])
            // Caso contrário, usa o sprite de vida vazia (states[0])
            lifeLights[i].sprite = hp > i ? states[1] : states[0];
        }
    }
}
