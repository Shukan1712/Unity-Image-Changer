using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXplaying : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource Right;  //1.19
    public AudioSource Wrong;  //
    public AudioSource Finish;
   // public AudioSource Wrong2low;
    public AudioSource Outlier;


    public void PlayRight()
    {
        Right.Play();

    }


    public void PlayWrong()
    {
        Wrong.Play();

    }



    public void PlayFinished()
    {
        Finish.Play();

    }

    //public void PlayWrong2low()
    //{
    //    Wrong2low.Play();

    //}


    public void PlayOutlier()
    {
        Outlier.Play();

    }

}