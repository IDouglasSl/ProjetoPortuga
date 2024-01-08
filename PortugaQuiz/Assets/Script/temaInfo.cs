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

    private int                  notaFinal;


    // Start is called before the first frame update
    void Start()
    {

        temaScene = FindObjectOfType(typeof(temaScene)) as temaScene;

        notaFinal = PlayerPrefs.GetInt("notaFinal_" + idTema.ToString());
        idTemaTxt.text = idTema.ToString();

        estrelas(); // chama o método estrelas

    }


    public void selecionarTema() // função para mudar o tema selecionado e a cor
    {
        temaScene.nomeTemaTxt.text = nomeTema;
        temaScene.nomeTemaTxt.color = corTema;

        PlayerPrefs.SetInt("idTema", idTema);
        PlayerPrefs.SetString("nomeTema", nomeTema);
        PlayerPrefs.SetInt("notaMinUmEstrela", notaMinimaUmEstrela);
        PlayerPrefs.SetInt("notaMinDuasEstrelas", notaMinimaDuasEstrela);

        temaScene.btnJogar.interactable = true;

    }


    public void estrelas()
    {

        foreach (GameObject e in estrela) // desligando as estrelas
        {
            e.SetActive(false);
        }

       


        int nEstrelas = 0;

        if(notaFinal == 10) { 
            nEstrelas = 3;
        }else if(notaFinal >= notaMinimaDuasEstrela)
        {
            nEstrelas = 2;
        }else if(notaFinal >= notaMinimaUmEstrela)
        {
            nEstrelas = 1;
        }

        for (int i = 0; i <nEstrelas; i++)
        {
            estrela[i].SetActive(true);
        }
    }


}
