using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class modoJogo1 : MonoBehaviour
{
    [Header("Configura��o dos textos")]
    public Text                 nomeTemaTXT;
    public Text                 perguntaTXT;
    public Image                perguntaIMG;
    public Text                 inforRespostaTXT;
    public Text                 notaFinalTXT;
    public Text                 msg1TXT;
    public Text                 msg2TXT;

    [Header("Configura��o dos textos alternativas")]
    public Text                 altAtxt;
    public Text                 altBtxt;
    public Text                 altCtxt;
    public Text                 altDtxt;


    [Header("Configura��o das barras")]
    public GameObject           barraProgresso;
    public GameObject           barraTempo;

    [Header("Configura��o dos bot�es")]
    public Button[]             botoes;
    public Color                corAcerto, corErro;

    [Header("Configura��o das Modo de Jogo")]
    public bool perguntasComIMG;
    public bool utilizarAltenativa;
    public bool perguntasAleatorias;
    public bool jogarComTempo;
    public float tempoResponder;
    public bool mostrarCorreta;
    public int qtdPiscar;

    
    [Header("Configura��o das perguntas")]
    public Sprite[]             perguntasIMG;
    public string[]             perguntas;
    public string[]             correta;
    public int                  qtdperguntas;
    public List<int>            listaPerguntas; // necess�rio para que a s perguntas venham de form aleat�ria

    [Header("Configura��o das alternativas")]
    
    public string[]             altenativaA;
    public string[]             altenativaB;
    public string[]             altenativaC;
    public string[]             altenativaD;

    [Header("Configura��o dos Paineis")]
    public GameObject[]         paineis;
    public GameObject[]         estrela;

    [Header("Configura��o das Mensagens")]
    public string[] mensagem1;
    public string[] mensagem2;
    public Color[] corMensagem;
    //-------

    private int                 idResponder; // identificador das respostas
    public float                qtdRespondida; // quantidade de perguntas RESPONDIDAS. O tipo � float por causa da divis�o para encontrar o percentual da barra de progresso
    private float               percProgresso; // percentual da barra de progresso
    private float               percTempo; // percentual da barra de tempo
    private float               tempTime; // v�riavel para auxiliar atualiza��o do tempo
    public float                notaFinal; 
    private float               valorQuestao; // valor de cada quest�o
    private int                 qtdAcertos;
    private int                 notaMinimaUmEstrela, notaMinimaDuasEstrela, nEstrelas;
    private int                 idTema; // identificador do tema
    private bool                fimPartida; // vari�vel para identificar quando a aprtida encerrou
    private int                 idBtnCorreto; // qual bot�o�ser� o correto
    private bool                exibindoCorreta; // vai evitar que o qualquer bot�o seja pressionado enquanto a corrotina mostra a resposta certa

    // Start is called before the first frame update
    void Start()
    {
        idTema = PlayerPrefs.GetInt("idTema");
        notaMinimaUmEstrela = PlayerPrefs.GetInt("notaMinUmEstrela");
        notaMinimaDuasEstrela = PlayerPrefs.GetInt("notaMinDuasEstrelas");
        nomeTemaTXT.text = PlayerPrefs.GetString("nomeTema");
        
        barraTempo.SetActive(false);

        if (perguntasComIMG)
        {
            montarListaPerguntasIMG(); // monstar as perguntas com imagens

        }
        else
        {
            montarListaPerguntas(); // montar as perguntas com texto

        }
        progressaoBarra();

        valorQuestao = 10 /(float) listaPerguntas.Count; // precisa converter para float para as quest�es valerem fra��o

        fimPartida = false;
        controleBarratempo();

        paineis[0].SetActive(true);
        paineis[1].SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (jogarComTempo && !exibindoCorreta && !fimPartida )
        {
            tempTime += Time.deltaTime;
            controleBarratempo();

            if(tempTime >= tempoResponder) { proximaPergunta(); } // se o tempo acabar, chama a proxima pergunta
        }
    }

    // Respo�ns�vel por montar a lista de pergunta a serem respondidas.
    
    public void montarListaPerguntas()
    {
        if (perguntasAleatorias) // modo aleat�rio
        {
            bool addPergunta = true;

       
            if (qtdperguntas > perguntas.Length) { qtdperguntas = perguntas.Length; }


            while (listaPerguntas.Count < qtdperguntas)
            {
                addPergunta = true;

                int rand = Random.Range(0, perguntas.Length);
                
                       
                foreach (int idP in listaPerguntas)
                {
                    if(idP == rand)
                    {
                        addPergunta = false;
                    }
                }
                
                if (addPergunta)
                {
                    listaPerguntas.Add(rand);
                }

            }

        }
        else // modo n�o aleat�rio
        {

                for (int i = 0; i < perguntas.Length; i++)
                {
                    listaPerguntas.Add(i);
                }
           
            
        }

       
        perguntaTXT.text = perguntas[listaPerguntas[idResponder]];

        

        if (utilizarAltenativa)
        {
            altAtxt.text = altenativaA[listaPerguntas[idResponder]];
            altBtxt.text = altenativaB[listaPerguntas[idResponder]];
            altCtxt.text = altenativaC[listaPerguntas[idResponder]];
            altDtxt.text = altenativaD[listaPerguntas[idResponder]];
        }
    }

    //Montar perguntas com Imagem
    public void montarListaPerguntasIMG()
    {
        if (perguntasAleatorias) // modo aleat�rio
        {
            bool addPergunta = true;

            if (qtdperguntas > perguntasIMG.Length) { qtdperguntas = perguntasIMG.Length; }

          



            while (listaPerguntas.Count < qtdperguntas)
            {
                addPergunta = true;

                int rand = Random.Range(0, perguntasIMG.Length);
               




                foreach (int idP in listaPerguntas)
                {
                    if (idP == rand)
                    {
                        addPergunta = false;
                    }
                }

                if (addPergunta)
                {
                    listaPerguntas.Add(rand);
                }

            }

        }
        else // modo n�o aleat�rio
        {

                for (int i = 0; i < perguntasIMG.Length; i++)
                {
                    listaPerguntas.Add(i);
                }
            


        }

         perguntaIMG.sprite = perguntasIMG[listaPerguntas[idResponder]];

        if (utilizarAltenativa)
        {
            altAtxt.text = altenativaA[listaPerguntas[idResponder]];
            altBtxt.text = altenativaB[listaPerguntas[idResponder]];
            altCtxt.text = altenativaC[listaPerguntas[idResponder]];
            altDtxt.text = altenativaD[listaPerguntas[idResponder]];
        }
    }

    // FUn��o respons�vel por analisar as respsotas do jogador
    public void responder(string alternativa)
    {
        if (exibindoCorreta) { return; } // verifica se � para mostrar ou n�o as respostas
        
        
        // verifica se a respsota est� correta e aumenta um acerto
        if (correta[listaPerguntas[idResponder]] == alternativa)
        {
            qtdAcertos += 1;
        }

        // indica qual a op��o � a correta
        switch (correta[listaPerguntas[idResponder]])
        {
            case "A":
                idBtnCorreto = 0;
                break;
            case "B":
                idBtnCorreto = 1;
                break;
            case "C":
                idBtnCorreto = 2;
                break;
            case "D":
                idBtnCorreto = 3;
                break;
        }



        if (mostrarCorreta) // para saber se vai mostrar o item certo ou n�o
        {
           foreach(Button b in botoes) // far� todos os bot�es ficarem vermelho
            {
                b.image.color = corErro;
            }
            exibindoCorreta = true;
            botoes[idBtnCorreto].image.color = corAcerto; // far� com que o bot�o correto fique verde
            StartCoroutine("mostrarAlternativaCorreta");

        }
        else // chama aproxima pergunta
        {
            proximaPergunta();
        }

        
        
    }

    //Fun��o respons�vel por chamar a pr�xima pergunta
    public void proximaPergunta()
    {
        idResponder += 1;
        tempTime = 0; // toda nova quest�o o tempo volta ao inicio

        qtdRespondida += 1; // atualiza a parra de progresso independente se o player respondeu ou o tempo acabou 

        progressaoBarra();


        if (idResponder < listaPerguntas.Count) // o idResponder n�o pode ultrapassar o quantitativo de perguntas
        {
            if (perguntasComIMG)
            {
                perguntaIMG.sprite = perguntasIMG[listaPerguntas[idResponder]];

            }
            else
            {
                perguntaTXT.text = perguntas[listaPerguntas[idResponder]];

            }

            if (utilizarAltenativa)
            {
                altAtxt.text = altenativaA[listaPerguntas[idResponder]];
                altBtxt.text = altenativaB[listaPerguntas[idResponder]];
                altCtxt.text = altenativaC[listaPerguntas[idResponder]];
                altDtxt.text = altenativaD[listaPerguntas[idResponder]];
            }

        }
        else // caso n�o tenha mais perguntas, calcula a nota final
        {

            calcularNotaFinal();
        }

    }

    void progressaoBarra() // calcula a progress�o da barra
    {

        inforRespostaTXT.text = "Pergunta " + (qtdRespondida+1) + " de " + listaPerguntas.Count;

        percProgresso = (qtdRespondida+1) / listaPerguntas.Count;
        barraProgresso.transform.localScale = new Vector3(percProgresso, 1, 1);

    }

    void controleBarratempo()
    {
        if (jogarComTempo) // se for com tempo, a barra de tempo aparece
            barraTempo.SetActive(true);

        percTempo = ((tempTime - tempoResponder) / tempoResponder) * -1; // calcular o tempo pecorrido

        if(percTempo < 0) // para a barrar de tempo n�o ficar crescendo negativamente
           percTempo = 0;
        

        barraTempo.transform.localScale = new Vector3(percTempo, 1, 1);

    }

    void calcularNotaFinal()
    {
        fimPartida = true;
        notaFinal = Mathf.RoundToInt(qtdAcertos * valorQuestao); // a fun��o Mathf arredonda o n�mero

        if (notaFinal > PlayerPrefs.GetInt("notaFinal_" + idTema.ToString())) // s� armazena a nova nova nota no playerprefs se for maior que a anterior
        {
            PlayerPrefs.SetInt("notaFinal_" + idTema.ToString(), (int)notaFinal);

        }

        //Quantidade de estrelas de acordo com a nota tirada do aluno
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



    IEnumerator mostrarAlternativaCorreta() // corrotina repsons�vel por fazer a quest�o correta piscar
    {
        for(int i = 0; i < qtdPiscar; i++){
            botoes[idBtnCorreto].image.color = corAcerto;
            yield return new WaitForSeconds(0.1f);
            botoes[idBtnCorreto].image.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        foreach (Button b in botoes) // far� todos os bot�es ficarem vermelho
        {
            b.image.color = Color.white;
        }
        exibindoCorreta = false;
        proximaPergunta();
    }

}
