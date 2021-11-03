 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OilGauge : MonoBehaviour
{

    public bool b = true;
    public Image image;
    public float speed = 0.5f;

    float time = 0f;

    public Text progress;

    public Transform oilOilGaugePivot;

    private Player _player;

    private void Start() {
        _player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (b) {
            time = _player.GetHeat() / 100;
            image.fillAmount = time*0.8f+0.1f;

 
            oilOilGaugePivot.localEulerAngles = Vector3.forward*(128f -256 * time  );

            if (progress)
            {
                progress.text = (int)(image.fillAmount * 100) + "%";

            }
        }
    }


}
