using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DialogSystem : MonoBehaviour
{
    [System.Serializable]
    class Dialog
    {
        public string[] lines;
    }

    [SerializeField] TMP_Text dialogText;
    [SerializeField] Dialog[] dialogs;
    [SerializeField] float letterDelay = 0.05f;

    private int currentDialogIndex = 0;
    private int currentLineIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    [SerializeField] GameObject AcceptButtons;

    void Start()
    {
        Invoke("StartStart", 2f);
    }

    void StartStart()
    {
        StartDialog(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                if (currentLineIndex == 0)
                {
                    Debug.Log("Dialog index is out of range.");
                }
                else { FinishTyping(); }
            }
            else
            {
                NextLine();
            }
        }
    }

    public void StartDialog(int dialogIndex)
    {
        if (dialogIndex < 0 || dialogIndex >= dialogs.Length)
        {
            Debug.LogError("Dialog index is out of range.");
            return;
        }

        currentDialogIndex = dialogIndex;
        currentLineIndex = 0;
        ShowLine();
    }

    private void ShowLine()
    {
        if (currentLineIndex < dialogs[currentDialogIndex].lines.Length)
        {
            string line = dialogs[currentDialogIndex].lines[currentLineIndex];
            typingCoroutine = StartCoroutine(TypeLine(line));
        }
        else
        {
            Debug.Log("Dialog complete.");
        }
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char letter in line)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }

        isTyping = false;
    }

    private void FinishTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        string line = dialogs[currentDialogIndex].lines[currentLineIndex];
        dialogText.text = line;
        isTyping = false;
    }

    private void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogs[currentDialogIndex].lines.Length)
        {
            ShowLine();
        }
        else
        {
            AcceptButtons.SetActive(true);
        }
    }
}