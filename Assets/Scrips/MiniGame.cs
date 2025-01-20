using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MiniGame : MonoBehaviour
{
    [SerializeField] Slider slider;
    float currentPower = 0;
    float currentTime = 0;
    bool isPlaying = false;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI extraText;
    [SerializeField] TextMeshProUGUI extraTextBg;
    [SerializeField] Animator canvasAnim;
    [SerializeField] Timer timer;
    [SerializeField] GameObject winSound;

    private void Start()
    {
        slider.maxValue = .6f;
    }

    private void OnEnable()
    {
        currentPower = 0;
        currentTime = 0;
        isPlaying = true;
        scoreText.text = "";
    }

    private void Update()
    {
        if (isPlaying)
        {
            IncrementPower();
            UpdateSlider();
        }

        if (Input.GetMouseButtonDown(0))
        {
            isPlaying = false;
            CalculateDifference();
        }
    }

    void UpdateSlider()
    {
        slider.value = currentPower;
    }
    
    void IncrementPower()
    {
        currentTime += Time.deltaTime;
        currentPower = Mathf.PingPong(currentTime, .6f);
    }

    void CalculateDifference()
    {
        float difference;

        if (currentPower < 0.3f)
        {
            difference = .3f - currentPower;
        }
        else if (currentPower > 0.3f)
        {
            difference = currentPower - .3f;
        }
        else
        {
            difference = 0f;
        }

        difference = 100 * difference / 0.3f;
        difference = 100 - difference;

        scoreText.text = System.Math.Round(difference, 2).ToString() + "%";
        
        Invoke("Close", 1);

        CalculateTimeReward(difference);
    }

    void Close()
    {
        gameObject.SetActive(false);
        AlarmScript.Instance.StopAlarm();
    }

    void CalculateTimeReward(float percent)
    {
        float timeReward;

        if (percent < 70f)
        {
            float timeRewardTemp = 100 - percent;
            timeReward = timeRewardTemp / 10;
            float timeRewardString = (float)System.Math.Round(timeReward, 2);
            extraText.text = $"You lost {timeRewardString} seconds";
            extraTextBg.text = $"You lost {timeRewardString} seconds";
            timeReward *= -1;
            timer.AddTime(timeReward);
        }
        else
        {
            float timeRewardTemp = 100 - percent;
            timeReward = timeRewardTemp / 10;
            float timeRewardString = (float)System.Math.Round(timeReward, 2);
            extraText.text = $"You earned {timeRewardString} seconds";
            extraTextBg.text = $"You earned {timeRewardString} seconds";
            timer.AddTime(timeReward);
        }

        canvasAnim.SetTrigger("ExtraTime");
        Instantiate(winSound);
    }
}
