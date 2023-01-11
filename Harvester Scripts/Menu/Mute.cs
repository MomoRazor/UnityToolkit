using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mute : MonoBehaviour
{
    public Sprite muted, unmuted;
    bool isMuted = false;
    Persistor persistor;
    // Start is called before the first frame update
    void Start()
    {
        persistor = GameObject.FindGameObjectWithTag("Persistor").GetComponent<Persistor>();
        ToggleMute();
        ToggleMute();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMute () {
        isMuted = persistor.toggleMute();
        
        if(isMuted){
            this.GetComponent<Image>().sprite = muted;
        }else{
            this.GetComponent<Image>().sprite = unmuted;
        }
    }
}
