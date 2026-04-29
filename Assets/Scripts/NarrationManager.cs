using UnityEngine;
using TMPro;
using System.Collections;
using StarterAssets;

public class NarrationManager : MonoBehaviour
{
    [System.Serializable]
    public class NarrationPoint
    {
        public string key;
        [TextArea] public string message;
        public AudioClip audioClip;
        public Vector3 worldPosition;
        public float triggerRadius = 5f;
        public float displayDuration = 5f;
        [HideInInspector] public bool triggeredThisSession;
    }

    [Header("UI Reference")]
    public TextMeshProUGUI narrationText;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Typewriter Settings")]
    public float typingSpeed = 0.04f;

    [Header("Settings")]
    public float intervalBetweenNarrations = 2f;
    public float narrationSpeedMultiplier = 0.15f;
    public NarrationPoint[] narrationPoints;

    private Transform _player;
    private ThirdPersonController _playerController;
    private bool _isPlaying;

    private void Start()
    {
#if UNITY_EDITOR
        ResetAllNarrations();
#endif

        _player = GameObject.FindWithTag("Player").transform;
        _playerController = _player.GetComponent<ThirdPersonController>();

        if (narrationText != null)
        {
            narrationText.text = "";
            Color c = narrationText.color;
            c.a = 0f;
            narrationText.color = c;
            narrationText.raycastTarget = false;
        }

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (_player == null || _isPlaying) return;

        foreach (var point in narrationPoints)
        {
            if (point.triggeredThisSession) continue;
            if (PlayerPrefs.GetInt(point.key, 0) == 1) continue;

            float dist = Vector3.Distance(_player.position, point.worldPosition);
            if (dist <= point.triggerRadius)
            {
                point.triggeredThisSession = true;
                StartCoroutine(PlayNarration(point));
                break;
            }
        }
    }

    private IEnumerator PlayNarration(NarrationPoint point)
    {
        _isPlaying = true;
        if (_playerController != null) _playerController.MoveSpeed *= narrationSpeedMultiplier;

        yield return StartCoroutine(ShowMessage(point));

        if (_playerController != null) _playerController.MoveSpeed /= narrationSpeedMultiplier;
        _isPlaying = false;

        yield return new WaitForSeconds(intervalBetweenNarrations);
    }

    private IEnumerator ShowMessage(NarrationPoint point)
    {
        narrationText.text = "";
        SetTextAlpha(1f);

        if (audioSource != null && point.audioClip != null)
        {
            audioSource.clip = point.audioClip;
            audioSource.Play();
        }

        // Typewriter
        foreach (char letter in point.message)
        {
            narrationText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Hold
        yield return new WaitForSeconds(point.displayDuration);

        // Fade Out
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * 2f;
            SetTextAlpha(Mathf.Clamp01(t));
            yield return null;
        }

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        SetTextAlpha(0f);
        narrationText.text = "";

        PlayerPrefs.SetInt(point.key, 1);
        PlayerPrefs.Save();
    }

    private void SetTextAlpha(float alpha)
    {
        Color c = narrationText.color;
        c.a = alpha;
        narrationText.color = c;
    }

    [ContextMenu("Reset All Narrations")]
    public void ResetAllNarrations()
    {
        foreach (var point in narrationPoints)
        {
            PlayerPrefs.DeleteKey(point.key);
            point.triggeredThisSession = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (narrationPoints == null) return;
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.3f);
        foreach (var point in narrationPoints)
            Gizmos.DrawSphere(point.worldPosition, point.triggerRadius);
    }
}