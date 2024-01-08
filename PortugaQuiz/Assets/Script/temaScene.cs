using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class temaScene : MonoBehaviour
{

    public Text         nomeTemaTxt;
    public Button       btnJogar;

    // Start is called before the first frame update
    void Start()
    {
        btnJogar.interactable = false; // o botão de jogar inicial inativo
    }

    public void jogar()
    {
        int idCena = PlayerPrefs.GetInt("idTema");
        if(idCena != 0)
        {
            SceneManager.LoadScene(idCena.ToString());
        }
    }




}
