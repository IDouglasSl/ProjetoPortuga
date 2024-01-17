using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class temaScene : MonoBehaviour
{

    public Text         nomeTemaTxt;
    public Button       btnJogar;

    [Header("Configuração da páginação")]
    public GameObject[]     btnPaginacao;
    public GameObject[]     Paineltemas;
    private bool            ativarbtnPaginacacao;
    private int             idPagina;

    // Start is called before the first frame update
    void Start()
    {

        onOffButtonsAndPaineis();

    }

    void onOffButtonsAndPaineis()
    {
        btnJogar.interactable = false; // o botão de jogar inicial inativo

        foreach(GameObject p in Paineltemas)
        {
            p.SetActive(false);
        }

        Paineltemas[0].SetActive(true);


        if (Paineltemas.Length > 1)
        {
            ativarbtnPaginacacao = true;
        }
        else
        {
            ativarbtnPaginacacao = false;
        }


        foreach(GameObject b in btnPaginacao)
        {
            
            b.SetActive(ativarbtnPaginacacao);
        }

    }

    public void jogar()
    {
        int idCena = PlayerPrefs.GetInt("idTema");
        if(idCena != 0)
        {
            SceneManager.LoadScene(idCena.ToString());
        }
    }


    public void btnPagina(int i)
    {


        idPagina += i;

        if(idPagina < 0) { idPagina = Paineltemas.Length - 1; }
        else if(idPagina >= Paineltemas.Length) { idPagina = 0; }


        btnJogar.interactable = false;
        nomeTemaTxt.color = Color.white;
        nomeTemaTxt.text = "Selecione um tema";

        foreach (GameObject p in Paineltemas)
        {
            p.SetActive(false);
        }

        Paineltemas[idPagina].SetActive(true);


    }


}
