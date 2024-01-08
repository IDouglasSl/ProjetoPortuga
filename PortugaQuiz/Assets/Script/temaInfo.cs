using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class temaInfo : MonoBehaviour
{
    private temaScene           temaScene;

    [Header("Configuração do Tema")]
    public int                  idTema;
    public string               nomeTema;
    public Color                corTema;


    [Header("Configuração das Estrelas")]
    public int                  notaMinimaUmEstrela;
    
    public int                  notaMinimaDuasEstrela;



    [Header("Configuração do Botão")]
    public Text                 idTemaTxt; // campo de texto na interface
    public GameObject[]         estrela;

    public int                  tempNota;


    // Start is called before the first frame update
    void Start()
    {
        temaScene = FindObjectOfType(typeof(temaScene)) as temaScene;

        idTemaTxt.text = idTema.ToString();

        estrelas(); // chama o método estrelas

    }


    public void selecionarTema() // função para mudar o tema selecionado e a cor
    {
        temaScene.nomeTemaTxt.text = nomeTema;
        temaScene.nomeTemaTxt.color = corTema;

    }


    public void estrelas()
    {

        foreach (GameObject e in estrela) // desligando as estrelas
        {
            e.SetActive(false);
        }

        int nEstrelas = 0;

        if(tempNota == 10) { 
            nEstrelas = 3;
        }else if(tempNota >= notaMinimaDuasEstrela)
        {
            nEstrelas = 2;
        }else if(tempNota >= notaMinimaUmEstrela)
        {
            nEstrelas = 1;
        }

        for (int i = 0; i <nEstrelas; i++)
        {
            estrela[i].SetActive(true);
        }
    }


}
