// Importa namespaces necessários para funcionalidades básicas do Unity e gerenciamento de cenas
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Define a classe PlayerController que herda de MonoBehaviour, permitindo que seja anexada a objetos no Unity
public class PlayerController : MonoBehaviour
{
    // Componentes públicos que podem ser atribuídos via Inspector no Unity
    public Rigidbody2D player;           // Referência ao Rigidbody2D do jogador para controlar a física
    public Animator anim;                // Referência ao Animator para controlar animações
    public SpriteRenderer skin;          // Referência ao SpriteRenderer para alterar a aparência do jogador

    // Variáveis de controle de movimento e salto
    private float speed, jumpForce;      // Velocidade atual e força de salto do jogador
    public float maxSpeed, maxJumpForce;// Valores máximos para velocidade e força de salto
    public Vector2 move;                 // Vetor para armazenar o movimento do jogador

    // Variáveis para detecção de chão e colisões
    public Transform foot, hand;         // Transforms para os pontos de verificação do pé e da mão
    public LayerMask ground;             // Máscara de camada para identificar o chão
    public float ray, timeStop = 2;      // Raio para detecção de chão e tempo de parada durante ações
    public bool inGround, inJump, inFall, inAttack, inHit, invesible, isDead, animDead; // Flags de estado do jogador

    // Referência ao objeto de kunai para lançamentos
    public GameObject kunai;

    // Referências para efeitos sonoros
    public AudioSource[] controllerSFX;  // Array de fontes de áudio para efeitos sonoros
    public AudioClip[] sfx;               // Array de clipes de áudio para diferentes ações

    // Método chamado na inicialização do objeto
    void Start()
    {
        speed = maxSpeed;         // Define a velocidade inicial como a velocidade máxima
        jumpForce = maxJumpForce; // Define a força de salto inicial como a força máxima
    }

    // Método chamado a cada frame
    void Update()
    {
        Dead();          // Verifica se o jogador está morto
        if (inAttack || isDead) return; // Se estiver atacando ou morto, não executa outras ações
        Jumping();       // Gerencia o salto do jogador
        Throw();         // Gerencia o lançamento de kunai
        Attack();        // Gerencia o ataque do jogador
        Hit();           // Gerencia quando o jogador é atingido
    }

    // Método para gerenciar o salto do jogador
    void Jumping()
    {
        // Verifica se o jogador está no chão usando uma sobreposição de círculo
        inGround = Physics2D.OverlapCircle(foot.position, ray, ground);
        if (inGround)
        {
            // Se o jogador estiver no chão e a tecla Espaço for pressionada, executa o salto
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Aplica força de salto
                anim.SetTrigger("Jump"); // Dispara a animação de salto
            }
        }

        // Atualiza os estados de salto e queda com base na velocidade vertical
        inJump = player.velocity.y > 0;
        inFall = player.velocity.y < 0;

        // Dispara a animação de queda se estiver caindo e não estiver no chão
        if (inFall && !inGround) anim.SetTrigger("Fall");
        // Dispara a animação de estar no chão se estiver no chão
        if (inGround) anim.SetTrigger("Ground");
    }

    // Método para gerenciar quando o jogador é atingido
    void Hit()
    {
        if (inHit)
        {
            if (invesible) return; // Se estiver invencível, não processa o hit
            invesible = true;       // Define o jogador como invencível
            anim.SetTrigger("Hit"); // Dispara a animação de hit
            controllerSFX[0].clip = sfx[2]; // Define o clipe de áudio para hit
            controllerSFX[0].Play();          // Toca o efeito sonoro de hit
            HUD.hp -= 1;                       // Reduz a vida do jogador
            StartCoroutine(Stop());            // Inicia a corrotina de parada temporária
            StartCoroutine(Invesible());       // Inicia a corrotina de invencibilidade
        }
    }

    // Método para verificar se o jogador está morto
    void Dead()
    {
        if (HUD.hp <= 0)
        {
            isDead = true;       // Define o estado como morto
            speed = 0;           // Zera a velocidade
            jumpForce = 0;       // Zera a força de salto

            if (!animDead)
            {
                animDead = true;       // Marca que a animação de morte foi iniciada
                anim.SetTrigger("Dead"); // Dispara a animação de morte
            }
        }
    }

    // Corrotina para gerenciar a invencibilidade temporária do jogador
    IEnumerator Invesible()
    {
        for (int i = 0; i < 6; i++)
        {
            skin.color = Color.black; // Alterna a cor para preto
            yield return new WaitForSeconds(0.25f); // Espera 0.25 segundos
            skin.color = Color.white; // Alterna a cor para branco
            yield return new WaitForSeconds(0.25f); // Espera 0.25 segundos
        }
        invesible = false; // Remove a invencibilidade
        yield return new WaitForSeconds(0.25f); // Espera mais 0.25 segundos
    }

    // Método para gerenciar o ataque do jogador
    void Attack()
    {
        if (inGround && !inAttack)
        {
            // Se o jogador estiver no chão e não estiver atacando, verifica a tecla 'J'
            if (Input.GetKeyDown(KeyCode.J))
            {
                inAttack = true;              // Define o estado de ataque
                anim.SetTrigger("Attack");    // Dispara a animação de ataque
                controllerSFX[0].clip = sfx[0]; // Define o clipe de áudio para ataque
                controllerSFX[0].Play();         // Toca o efeito sonoro de ataque
                StartCoroutine(Stop());           // Inicia a corrotina de parada temporária
            }
        }
    }

    // Método para gerenciar o lançamento de kunai
    void Throw()
    {
        if (inGround && !inAttack)
        {
            // Se o jogador estiver no chão e não estiver atacando, verifica a tecla 'K'
            if (Input.GetKeyDown(KeyCode.K))
            {
                inAttack = true;              // Define o estado de ataque
                anim.SetTrigger("Throw");     // Dispara a animação de lançamento
                controllerSFX[0].clip = sfx[1]; // Define o clipe de áudio para lançamento
                controllerSFX[0].Play();         // Toca o efeito sonoro de lançamento
                StartCoroutine(Stop());           // Inicia a corrotina de parada temporária
            }
        }
    }

    // Método para instanciar o objeto de kunai
    void SpawnKunai()
    {
        Instantiate(kunai, hand.position, transform.rotation); // Cria uma instância de kunai na posição da mão
    }

    // Corrotina para parar o movimento e ações temporariamente
    IEnumerator Stop()
    {
        speed = 0;           // Zera a velocidade
        jumpForce = 0;      // Zera a força de salto
        yield return new WaitForSeconds(timeStop); // Espera o tempo definido
        speed = maxSpeed;    // Restaura a velocidade máxima
        jumpForce = maxJumpForce; // Restaura a força de salto máxima
        inAttack = false;    // Remove o estado de ataque
        yield return new WaitForSeconds(0.25f); // Espera mais 0.25 segundos
    }

    // Método para desenhar gizmos no editor do Unity para visualização
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(foot.position, ray); // Desenha um círculo para a detecção de chão
    }

    // Método chamado a cada frame fixo, ideal para física
    void FixedUpdate()
    {
        Movimente(); // Chama o método de movimento
    }

    // Método para gerenciar o movimento do jogador
    void Movimente()
    {
        move.x = Input.GetAxisRaw("Horizontal") * speed; // Obtém a entrada horizontal e multiplica pela velocidade
        move.y = player.velocity.y;                       // Mantém a velocidade vertical atual

        player.velocity = move; // Aplica a velocidade ao Rigidbody2D

        // Altera a direção do sprite com base na direção do movimento
        transform.eulerAngles = move.x > 0 ? Vector3.zero : move.x < 0 ? Vector3.up * 180 : transform.eulerAngles;
        anim.SetBool("Run", move.x != 0); // Define o parâmetro "Run" na animação com base no movimento
    }

    // Método chamado quando o jogador entra em colisão com um trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Crystal")
        {
            inHit = true; // Define que o jogador foi atingido por um cristal
        }
        if (other.gameObject.tag == "Water")
        {
            HUD.hp = 0; // Define a vida do jogador para 0 se colidir com água
        }
        if (other.gameObject.tag == "Takoyake")
        {
            HUD.hp += 1;                      // Incrementa a vida do jogador
            controllerSFX[1].Play();          // Toca o efeito sonoro de coleta
            Destroy(other.gameObject);        // Destroi o objeto coletado
        }
        if(other.gameObject.tag == "Cena2")
        {
            SceneManager.LoadScene("Cena 2"); // Carrega a cena "Cena 2"
        }
    }

    // Método chamado quando o jogador sai de uma colisão com um trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Crystal")
        {
            inHit = false; // Remove o estado de ser atingido quando sai do trigger do cristal
        }
    }
}
