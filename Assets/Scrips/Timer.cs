using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Volume nauseaEffect;
    float currentTime;
    Slider slider;
    bool timesUp = false;

    private void Start()
    {
        slider = GetComponent<Slider>();

        slider.maxValue = time;
        currentTime = time;
    }

    private void Update()
    {
        if (timesUp) return;
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            slider.value = currentTime;
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            text.text = $"{minutes}m {seconds}s";
            nauseaEffect.weight = (time - currentTime) / time;
        }
        else
        {
            currentTime = 0;
            slider.value = currentTime;
            text.text = "";
            nauseaEffect.weight = 1;
            timesUp = true;
            StateHandler.instance.Die();
            InventoryManager.instance.won = true;
        }
    }
}
