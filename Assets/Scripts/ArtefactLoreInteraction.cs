using System.Collections;
using UnityEngine;
using TMPro;

public class ArtefactLoreInteraction : MonoBehaviour
{
    [Header("Proximity Settings")]
    [Tooltip("How close the player must be to trigger the interaction.")]
    public float interactionRadius = 3f;

    [Header("Lore Content")]
    [Tooltip("The name of this artefact, shown as the panel heading.")]
    public string artifactName = "Unknown Artefact";

    [Tooltip("The lore text unique to this artefact.")]
    [TextArea(5, 15)]
    public string loreText = "Enter lore here...";

    [Header("UI References — assign per artefact")]
    [Tooltip("Drag THIS artefact's own LorePanel here.")]
    public GameObject lorePanel;

    [Tooltip("Drag the TMP heading text inside THIS artefact's LorePanel.")]
    public TextMeshProUGUI artifactNameText;

    [Tooltip("Drag the TMP body text inside THIS artefact's LorePanel.")]
    public TextMeshProUGUI loreBodyText;

    [Tooltip("(Optional) '[E] Inspect' prompt — can be shared or per artefact.")]
    public GameObject interactPrompt;

    [Header("Timing")]
    [Tooltip("Seconds before the panel auto-dismisses.")]
    public float displayDuration = 15f;

    [Tooltip("(Optional) TMP text showing a live countdown.")]
    public TextMeshProUGUI countdownText;

    private Transform _player;
    private bool _playerInRange = false;
    private bool _panelVisible = false;
    private Coroutine _dismissCoroutine = null;


    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _player = playerObj.transform;
        else
            Debug.LogWarning("[ArtifactLoreInteraction] No GameObject tagged 'Player' found.");

        SetPanelVisible(false);
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (_player == null) return;
        CheckProximity();
        HandleInput();
    }

    //Proximity

    void CheckProximity()
    {
        float dist = Vector3.Distance(transform.position, _player.position);
        bool inRange = dist <= interactionRadius;

        if (inRange && !_playerInRange)
        {
            _playerInRange = true;
            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
        else if (!inRange && _playerInRange)
        {
            _playerInRange = false;
            if (interactPrompt != null)
                interactPrompt.SetActive(false);

            if (_panelVisible)
                HideLorePanel();
        }
    }

    //Input

    void HandleInput()
    {
        if (!_playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_panelVisible)
                HideLorePanel();
            else
                ShowLorePanel();
        }
    }

    //PanelControl

    void ShowLorePanel()
    {
        // Write THIS artefact's unique text into ITS OWN panel components
        if (artifactNameText != null) artifactNameText.text = artifactName;
        if (loreBodyText != null) loreBodyText.text = loreText;

        SetPanelVisible(true);

        if (_dismissCoroutine != null)
            StopCoroutine(_dismissCoroutine);
        _dismissCoroutine = StartCoroutine(AutoDismiss());
    }

    void HideLorePanel()
    {
        if (_dismissCoroutine != null)
        {
            StopCoroutine(_dismissCoroutine);
            _dismissCoroutine = null;
        }
        SetPanelVisible(false);
    }

    void SetPanelVisible(bool visible)
    {
        _panelVisible = visible;
        if (lorePanel != null)
            lorePanel.SetActive(visible);
    }

    //Auto-dismiss

    IEnumerator AutoDismiss()
    {
        float elapsed = 0f;

        while (elapsed < displayDuration)
        {
            elapsed += Time.deltaTime;

            if (countdownText != null)
            {
                float remaining = Mathf.Ceil(displayDuration - elapsed);
                countdownText.text = $"Closes in {remaining}s";
            }

            yield return null;
        }

        HideLorePanel();
        _dismissCoroutine = null;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 0.4f, 0.3f);
        Gizmos.DrawSphere(transform.position, interactionRadius);
        Gizmos.color = new Color(0.2f, 0.8f, 0.4f, 1f);
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}