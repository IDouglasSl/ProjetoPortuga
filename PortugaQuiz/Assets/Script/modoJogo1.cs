using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class modoJogo1 : MonoBehaviour
{
    [Header("Configuração dos textos")]
    public Text                 nomeTemaTXT;
    public Text                 perguntaTXT;
    public Text                 inforRespostaTXT;
    public Text                 notaFinalTXT;
    public Text                 msg1TXT;
    public Text                 msg2TXT;

    [Header("Configuração das barras")]
    public GameObject           barraProgresso;
    public GameObject           barraTempo;

    [Header("Configuração dos botões")]
    public Button[]             botoes;

    [Header("Configuração das Modo de Jogo")]
    public bool perguntasAleatorias;
    public bool jogarComTempo;
    public float tempoResponder;

    
    [Header("Configuração das perguntas")]
    public string[]             perguntas;
    public string[]             correta;

    [Header("Configuração dos Paineis")]
    public GameObject[]         paineis;
    public GameObject[]         estrela;

    [Header("Configuração das Mensagens")]
    public string[] mensagem1;
    public string[] mensagem2;
    public Color[] corMensagem;
    //-------

    private int                 idResponder; // identificador das respostas
    public float                qtdRespondida; // quantidade de perguntas RESPONDIDAS. O tipo é float por causa da divisão para encontrar o percentual da barra de progresso
    private float               percProgresso; // percentual da barra de progresso
    private float               percTempo; // percentual da barra de tempo
    private float               tempTime; 
    private float               notaFinal; 
    private float               valorQuestao;
    private int                 qtdAcertos;
    private int                 notaMinimaUmEstrela, notaMinimaDuasEstrela, nEstrelas;




    // Start is called before the first frame update
    void Start()
    {
        barraTempo.SetActive(false);

        progressaoBarra();
        montarListaPerguntas();

        valorQuestao = 10 /(float) perguntas.Length; // precisa converter para float para as quentões valerem fração

        controleBarratempo();

        paineis[0].SetActive(true);
        paineis[1].SetActive(false);

        notaMinimaUmEstrela = 5;
        notaMinimaDuasEstrela = 7;
    }

    // Update is called once per frame
    void Update()
    {
        if (jogarComTempo)
        {
            tempTime += Time.deltaTime;
            controleBarratempo();

            if(tempTime >= tempoResponder) { proximaPergunta(); } // se o tempo acabar, chama a proxima pergunta
        }
    }

    public void montarListaPerguntas()
    {
        perguntaTXT.text = perguntas[idResponder];
    }


    public void responder(string alternativa)
    {
        if (correta[idResponder] == alternativa)
        {
            print("Correta");
            qtdAcertos += 1;
        }
        else
        {
            print("Errou");
        }

        
        proximaPergunta();
    }

    public void proximaPergunta()
    {
        idResponder += 1;
        tempTime = 0; // toda nova questão o tempo volta ao inicio

        qtdRespondida += 1; // atualiza a parra de progresso independente se o player respondeu ou o tempo acabou 

        progressaoBarra();


        if (idResponder < perguntas.Length) // o idResponder não pode ultrapassar o quantitativo de perguntas
        {
            perguntaTXT.text = perguntas[idResponder];
           
            
        }
        else
        {

            calcularNotaFinal();
        }

    }

    void progressaoBarra() // calcula a progressão da barra
    {

        inforRespostaTXT.text = "Pergunta " + (qtdRespondida+1) + " de " + perguntas.Length;

        percProgresso = (qtdRespondida+1) / perguntas.Length;
        barraProgresso.transform.localScale = new Vector3(percProgresso, 1, 1);

    }


    void controleBarratempo()
    {
        if (jogarComTempo) // se for com tempo, a barra de tempo aparece
            barraTempo.SetActive(true);

        percTempo = ((tempTime - tempoResponder) / tempoResponder) * -1; // calcular o tempo pecorrido

        if(percTempo < 0) // para a barrar de tempo não ficar crescendo negativamente
           percTempo = 0;
        

        barraTempo.transform.localScale = new Vector3(percTempo, 1, 1);

    }

    void calcularNotaFinal()
    {
        notaFinal = Mathf.RoundToInt(qtdAcertos * valorQuestao); // a função Mathf arredonda o número
       

        if (notaFinal == 10){nEstrelas = 3;   }
        else if (notaFinal >= notaMinimaDuasEstrela){nEstrelas = 2;}
        else if (notaFinal >= notaMinimaUmEstrela){nEstrelas = 1;}


        notaFinalTXT.text = notaFinal.ToString();
        notaFinalTXT.color = corMensagem[nEstrelas];

        msg1TXT.text = mensagem1[nEstrelas];
        msg1TXT.color = corMensagem[nEstrelas];
        msg2TXT.text = mensagem2[nEstrelas];

        foreach (GameObject e in estrela) // desligando as estrelas
        {
            e.SetActive(false);
        }

        for (int i = 0; i < nEstrelas; i++)// Ativando estrelas
        {
            estrela[i].SetActive(true);
        }

        paineis[0].SetActive(false);
        paineis[1].SetActive(true);
    }
}
