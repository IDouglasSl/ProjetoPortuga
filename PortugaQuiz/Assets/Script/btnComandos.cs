using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btnComandos : MonoBehaviour
{
    

    // crrega uma nova cena
    public void irParaCena(string nomeCena)
    {
        SceneManager.LoadScene(nomeCena);
    }


    //fechar o aplicativo
    public void sair()
    {
        Application.Quit();
    }

    public void jogarNovamente()
    {
        int idCena = PlayerPrefs.GetInt("idTema");
        if (idCena != 0)
        {
            SceneManager.LoadScene(idCena.ToString());
        }
    }


}
