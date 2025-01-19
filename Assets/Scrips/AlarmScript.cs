using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AlarmScript : MonoBehaviour
{
    public static AlarmScript Instance;

    [SerializeField] float time;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Volume alarmEffect;
    float currentTime;
    Slider slider;
    bool timesUp = false;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        slider = GetComponent<Slider>();

        slider.maxValue = time;
        currentTime = time;
    }

    private void OnEnable()
    {
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
            alarmEffect.weight = (time - currentTime) / time;
        }
        else
        {
            currentTime = 0;
            slider.value = currentTime;
            text.text = "";
            alarmEffect.weight = 1;
            timesUp = true;
            StateHandler.instance.Die();
            InventoryManager.instance.won = true;
        }
    }

    public void StopAlarm()
    {
        currentTime = 0;
        slider.value = currentTime;
        text.text = "";
        alarmEffect.weight = 0;
        gameObject.SetActive(false);
    }
}
