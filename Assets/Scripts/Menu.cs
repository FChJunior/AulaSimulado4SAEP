// Importa o namespace UnityEngine para acessar as funcionalidades básicas do Unity
using UnityEngine;
// Importa o namespace SceneManagement para gerenciar cenas no jogo
using UnityEngine.SceneManagement;

// Define a classe Menu que herda de MonoBehaviour, permitindo que seja anexada a objetos no Unity
public class Menu : MonoBehaviour
{
    // Método público para iniciar o jogo
    public void StartGame()
    {
        // Carrega a cena chamada "Cena 1"
        SceneManager.LoadScene("Cena 1");
    }

    // Método público para sair do jogo
    public void ExitGame()
    {
        // Encerra a aplicação
        Application.Quit();
        
        // Nota: Application.Quit() funciona apenas em builds do jogo. No editor do Unity, ele não fará nada.
    }

    // Método público para retomar o jogo ou retornar ao menu principal
    public void ResumeGame()
    {
        // Carrega a cena chamada "Menu"
        SceneManager.LoadScene("Menu");
    }
}
