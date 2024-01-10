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
    public Color                corAcerto, corErro;

    [Header("Configuração das Modo de Jogo")]
    public bool perguntasAleatorias;
    public bool jogarComTempo;
    public float tempoResponder;
    public bool mostrarCorreta;
    public int qtdPiscar;

    
    [Header("Configuração das perguntas")]
    public string[]             perguntas;
    public string[]             correta;
    public int                  qtdperguntas;
    public List<int>            listaPerguntas; // necessário para que a s perguntas venham de form aleatória



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
    private float               tempTime; // váriavel para auxiliar atualização do tempo
    public float                notaFinal; 
    private float               valorQuestao; // valor de cada questão
    private int                 qtdAcertos;
    private int                 notaMinimaUmEstrela, notaMinimaDuasEstrela, nEstrelas;
    private int                 idTema; // identificador do tema
    private bool                fimPartida; // variável para identificar quando a aprtida encerrou
    private int                 idBtnCorreto; // qual botão´será o correto
    private bool                exibindoCorreta; // vai evitar que o qualquer botão seja pressionado enquanto a corrotina mostra a resposta certa

    // Start is called before the first frame update
    void Start()
    {
        idTema = PlayerPrefs.GetInt("idTema");
        notaMinimaUmEstrela = PlayerPrefs.GetInt("notaMinUmEstrela");
        notaMinimaDuasEstrela = PlayerPrefs.GetInt("notaMinDuasEstrelas");
        nomeTemaTXT.text = PlayerPrefs.GetString("nomeTema");
        
        barraTempo.SetActive(false);

        montarListaPerguntas();
        progressaoBarra();

        valorQuestao = 10 /(float) listaPerguntas.Count; // precisa converter para float para as questões valerem fração

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

    public void montarListaPerguntas()
    {
        if (perguntasAleatorias)
        {
            bool addPergunta = true;

            if(qtdperguntas > perguntas.Length) { qtdperguntas = perguntas.Length; }
            while(listaPerguntas.Count < qtdperguntas)
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
        else
        {
            for( int i = 0; i < perguntas.Length; i++)
            {
                listaPerguntas.Add(i);
            }
        }

        perguntaTXT.text = perguntas[listaPerguntas[idResponder]];
    }


    public void responder(string alternativa)
    {
        if (exibindoCorreta) { return; }
        if (correta[listaPerguntas[idResponder]] == alternativa)
        {
            qtdAcertos += 1;
        }


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



        if (mostrarCorreta) // para saber se vai mostrar o item certo ou não
        {
           foreach(Button b in botoes) // fará todos os botões ficarem vermelho
            {
                b.image.color = corErro;
            }
            exibindoCorreta = true;
            botoes[idBtnCorreto].image.color = corAcerto; // fará com que o botão correto fique verde
            StartCoroutine("mostrarAlternativaCorreta");

        }
        else
        {
            proximaPergunta();
        }

        
        
    }

    public void proximaPergunta()
    {
        idResponder += 1;
        tempTime = 0; // toda nova questão o tempo volta ao inicio

        qtdRespondida += 1; // atualiza a parra de progresso independente se o player respondeu ou o tempo acabou 

        progressaoBarra();


        if (idResponder < listaPerguntas.Count) // o idResponder não pode ultrapassar o quantitativo de perguntas
        {
            perguntaTXT.text = perguntas[listaPerguntas[idResponder]];
           
        }
        else
        {

            calcularNotaFinal();
        }

    }

    void progressaoBarra() // calcula a progressão da barra
    {

        inforRespostaTXT.text = "Pergunta " + (qtdRespondida+1) + " de " + listaPerguntas.Count;

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
        fimPartida = true;
        notaFinal = Mathf.RoundToInt(qtdAcertos * valorQuestao); // a função Mathf arredonda o número

        if (notaFinal > PlayerPrefs.GetInt("notaFinal_" + idTema.ToString())) // só armazena a nova nova nota no playerprefs se for maior que a anterior
        {
            PlayerPrefs.SetInt("notaFinal_" + idTema.ToString(), (int)notaFinal);

        }


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



    IEnumerator mostrarAlternativaCorreta()
    {
        for(int i = 0; i < qtdPiscar; i++){
            botoes[idBtnCorreto].image.color = corAcerto;
            yield return new WaitForSeconds(0.1f);
            botoes[idBtnCorreto].image.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        foreach (Button b in botoes) // fará todos os botões ficarem vermelho
        {
            b.image.color = Color.white;
        }
        exibindoCorreta = false;
        proximaPergunta();
    }

}
