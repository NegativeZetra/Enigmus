using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogadorMovimento : MonoBehaviour
{

    public CharacterController Controlador;

    public float Gravidade = -9.81f;
    public float AlturaDoPulo = 3f;
    public Transform ChecarChao; //O nome é bem auto explicativo mas é só pra checar se o player tá no chão.
    public float DistanciaChao = 0.4f;
    public LayerMask MascaraChao; // Pelo que vi o sistema de layers funciona igual o sistema de tags.
    public float Velocidade = 6f;
    public float ViradaSuave = 0.1f;
    float ViradaSuaveVelocidade;

    bool EstaNoChao;
    public Transform cam;

    Vector3 Velocity;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MovimentacaoPersonagem();
    }

    void MovimentacaoPersonagem(){
               // Programação do caminhar e pulo do personagem

        EstaNoChao = Physics.CheckSphere(ChecarChao.position, DistanciaChao, MascaraChao); //Basicamente cria como se fosse aquelas caixa de colisão de jogo 2d só que no caso é uma esfera que quando ligada com o chão consegue verificar se pode pular ou não. (Lembrete: o y é oque vai pra cima)

        if(EstaNoChao && Velocity.y < 0 ){ // Isso reseta a velocidade do pulo caso não tenha isso cada vez que a gente pulasse o pulo ia ficar mais rapido kkkk
            Velocity.y = -2f;
        }




        float Horizontal = Input.GetAxisRaw("Horizontal");        // O Raw serve para quando clicarmos na tecla ela não ter nenhum delay como se fosse os controle buxa do crash bandicoot. 
        float Vertical = Input.GetAxisRaw("Vertical");
        Velocity.y += Gravidade * Time.deltaTime;
        Controlador.Move(Velocity * Time.deltaTime);
        Vector3 Direcao = new Vector3(Horizontal, 0f, Vertical).normalized; // O do meio está zerado pq ele seria no caso o pulo pois estaria indo pra cima oque seria um problema andar e ir pra cima.


        if(Input.GetKey(KeyCode.Space) && EstaNoChao){
            Velocity.y = Mathf.Sqrt(AlturaDoPulo * 2f * Gravidade);
             Debug.Log("Pulando!"); // Mensagem de debug para verificar se o pulo está sendo acionado
        }

        if (Direcao.magnitude >= 0.1f){
            float AnguloDoAlvo = Mathf.Atan2(Direcao.x, Direcao.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // Basicamente o Atan2 é uma função matematica que retorna o angulo entre o eixo x começando pelo vector de valor zero e indo até o x,z (basicamente um grafico de coordenada KKKK).
            float Angulo = Mathf.SmoothDampAngle(transform.eulerAngles.y, AnguloDoAlvo, ref ViradaSuaveVelocidade, ViradaSuave); // Esse codigo está suavizando a virada do nosso personagem com funções matematicas.
            transform.rotation = Quaternion.Euler(0f, Angulo, 0f); // Isso faz a rotação da camera só saiba dissoKKKKKKKk


            Vector3 MovimentoDaDirecao =  Quaternion.Euler(0f, AnguloDoAlvo, 0f) * Vector3.forward; //Converte rotação para direção
            Controlador.Move(MovimentoDaDirecao.normalized * Velocidade * Time.deltaTime); // Não não faço ideia da teoria por trás do DeltaTime só sei que ele faz com que o jogador se mova independente da quantidade de frames, acho que isso seria tipo quando seu jogo tá lagando e mesmo assim seu personagem consegue andar pra onde você quer.


        }

    }
}
