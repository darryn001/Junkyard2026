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
        public float displayDuration = 5f;
    }

    [Header("UI Reference")]
    public TextMeshProUGUI tutorialText;

    [Header("Settings")]
    public float intervalBetweenPrompts = 2f;
    public TutorialPrompt[] prompts;

    private void Start()
    {
#if UNITY_EDITOR
        ResetAllPrompts(); // Always show prompts fresh during Editor playtesting
#endif

        if (tutorialText != null)
        {
            Color c = tutorialText.color;
            c.a = 0f;
            tutorialText.color = c;
            tutorialText.raycastTarget = false;
        }

        StartCoroutine(PlayAllPrompts());
    }

    private IEnumerator PlayAllPrompts()
    {
        foreach (var prompt in prompts)
        {
            if (PlayerPrefs.GetInt(prompt.key, 0) == 1) continue;

            yield return StartCoroutine(ShowPrompt(prompt));
            yield return new WaitForSeconds(intervalBetweenPrompts);
        }
    }

    private IEnumerator ShowPrompt(TutorialPrompt prompt)
    {
        tutorialText.text = prompt.message;

        // Fade In
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            SetTextAlpha(Mathf.Clamp01(t));
            yield return null;
        }

        yield return new WaitForSeconds(prompt.displayDuration);

        // Fade Out
        t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * 2f;
            SetTextAlpha(Mathf.Clamp01(t));
            yield return null;
        }

        SetTextAlpha(0f);
        tutorialText.text = "";

        PlayerPrefs.SetInt(prompt.key, 1);
        PlayerPrefs.Save();
    }

    private void SetTextAlpha(float alpha)
    {
        Color c = tutorialText.color;
        c.a = alpha;
        tutorialText.color = c;
    }

    [ContextMenu("Reset All Tutorial Prompts")]
    public void ResetAllPrompts()
    {
        foreach (var prompt in prompts)
            PlayerPrefs.DeleteKey(prompt.key);
    }
}