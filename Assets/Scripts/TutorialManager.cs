using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialPrompt
    {
        public string key;
        public string message;
        public float displayDuration = 15f;
    }

    [Header("UI")]
    public TextMeshProUGUI tutorialText;
    public CanvasGroup canvasGroup;

    [Header("Timing")]
    public float intervalBetweenPrompts = 15f;

    [Header("Prompts")]
    public TutorialPrompt[] prompts;

    private void Start()
    {
        ResetAllPrompts(); // TEMP — remove this line after testing
        canvasGroup.alpha = 0f;
        StartCoroutine(PlayAllPrompts());
    }

    private IEnumerator PlayAllPrompts()
    {
        foreach (var prompt in prompts)
        {
            if (PlayerPrefs.GetInt(prompt.key, 0) == 1)
                continue;

            yield return StartCoroutine(ShowPrompt(prompt));
            yield return new WaitForSeconds(intervalBetweenPrompts);
        }
    }

    private IEnumerator ShowPrompt(TutorialPrompt prompt)
    {
        tutorialText.text = prompt.message;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            canvasGroup.alpha = Mathf.Clamp01(t);
            yield return null;
        }

        yield return new WaitForSeconds(prompt.displayDuration);

        t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * 2f;
            canvasGroup.alpha = Mathf.Clamp01(t);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        tutorialText.text = "";

        PlayerPrefs.SetInt(prompt.key, 1);
        PlayerPrefs.Save();
    }

    public void ResetAllPrompts()
    {
        foreach (var prompt in prompts)
            PlayerPrefs.DeleteKey(prompt.key);
    }
}