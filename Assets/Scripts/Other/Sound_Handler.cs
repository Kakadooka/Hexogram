using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sound_Handler : MonoBehaviour
{
    AudioSource audioSourceAction;
    AudioSource audioSourceHover;
        
    public AudioClip[] audioClip = new AudioClip[1];
    float timePassHover = 0f;

    void Start()
    {   if(GameObject.Find("Audio Source Action")){
            audioSourceAction = GameObject.Find("Audio Source Action").GetComponent<AudioSource>();
        }
        audioSourceHover = GameObject.Find("Audio Source Hover").GetComponent<AudioSource>();
    }

    public void playHover(){
        if(timePassHover > 0.05f){
            audioSourceHover.Play();
            timePassHover = 0f;
        }
    }

    System.Random random = new System.Random(Environment.TickCount);

    void playSoundWithRandomPitch(int audioIndex){
        audioSourceAction.pitch = (Convert.ToSingle(random.NextDouble()) / 10f) + 0.95f;
        audioSourceAction.Stop();
        audioSourceAction.clip = audioClip[audioIndex];
        audioSourceAction.Play();
    }
    void playSoundNormal(int audioIndex){
        audioSourceAction.Stop();
        audioSourceAction.clip = audioClip[audioIndex];
        audioSourceAction.Play();
    }



    public void playCancel(){
        playSoundWithRandomPitch(0);
    }

    public void playConfirmPerfect(){
        playSoundWithRandomPitch(1);
    }

    public void playConfirmError(){
        playSoundWithRandomPitch(2);
    }

    public void playUncrumblePaper(){
        playSoundNormal(3);
    }

    public void playCrumblePaper(){
        playSoundNormal(4);
    }
    public void playButtonClick(){
        playSoundWithRandomPitch(5);
    }
    

    void Update()
    {
        timePassHover += Time.deltaTime;
    }
}
