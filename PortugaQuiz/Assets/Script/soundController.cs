using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundController : MonoBehaviour
{

    private AudioSource AudioS;
    public AudioClip somAcerto, somErro;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        AudioS = GetComponent<AudioSource>();
    }


    public void playAcerto()
    {
        AudioS.PlayOneShot(somAcerto);
    }

    public void playErro()
    {
        AudioS.PlayOneShot(somErro);
    }


}
