using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadTask : MonoBehaviour
{
    // Start is called before the first frame update
    public Text cardCode;
    public Text inputCode;
    public int codeLength = 5;
    public float codeResetTimeInSeconds = 0.5f;
    private bool isResetting = false;

    [SerializeField]
    PuzzleController controller;

    private void OnEnable()
    {
        string code = string.Empty;
        for(int i = 0; i < codeLength; i++)
        {
            code += Random.Range(1, 10);
        }
        cardCode.text = code;
        inputCode.text = string.Empty;
    }

    public void ButtonClick(int number)
    {
        if (isResetting) { return; }

        inputCode.text += number;
        if (inputCode.text == cardCode.text)
        {
            inputCode.text = "Correct";
            StartCoroutine(ResetCode());
            Invoke("ApplyWin", 0.5f);
        }
        else if (inputCode.text.Length >= codeLength)
        {
            inputCode.text = "Failed";
            StartCoroutine(ResetCode());
        }
    }

    private IEnumerator ResetCode()
    {
        isResetting = true;

        yield return new WaitForSeconds(codeResetTimeInSeconds);
        inputCode.text = string.Empty;
        isResetting = false;
    }

    void ApplyWin()
    {
        controller.ClosePuzzle(true);
    }
    
}
