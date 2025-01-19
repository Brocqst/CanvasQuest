using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SailingMiniGame : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float activationDistance;
    [SerializeField] GameObject textGameObject;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] GameObject miniGame;
    [SerializeField] Animator canvasAnim;
    [SerializeField] float randomCooldownMin;
    [SerializeField] float randomCooldownMax;
    bool active = false;
    float randomTimer = 0;

    private void Start()
    {
        miniGame.SetActive(false);
        IsReady();
    }

    private void Update()
    {
        if (!active)
        {
            randomTimer -= Time.deltaTime;

            if (randomTimer <= 0)
            {
                IsReady();
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position) < activationDistance)
            {
                Debug.Log("Close Enough");
                textGameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    canvasAnim.SetTrigger("StartMiniGame");
                    Invoke("ActivateMiniGame", 0.5f);
                    NotReady();
                    textGameObject.SetActive(false);
                    randomTimer = Random.Range(randomCooldownMin, randomCooldownMax);
                }
            }
            else
            {
                Debug.Log("Too far away");
                textGameObject.SetActive(false);
            }
        }
    }

    void ActivateMiniGame()
    {
        miniGame.SetActive(true);
    }

    void IsReady()
    {
        active = true;
    }

    void NotReady()
    {
        active = false;
    }
}
