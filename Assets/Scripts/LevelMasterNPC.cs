using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelMasterNPC : MonoBehaviour
{
    public Button submitButton;

    [Header("Win Settings")]
    public GameObject winPanel;
    public Button winPanelCloseButton;

    [Header("Gate Settings")]
    public LevelGate gateToOpen;

    [Header("Code Settings")]
    public string correctCode;

    [Header("UI Elements")]
    public GameObject uiPanel;
    public TMP_InputField codeInputField;
    public TextMeshProUGUI errorMessage;
    public TextMeshProUGUI successMessage;
    public Button uiPanelCloseButton;

    private void Start()
    {
        uiPanel.SetActive(false);
        errorMessage.gameObject.SetActive(false);
        successMessage.gameObject.SetActive(false);

        if (winPanelCloseButton != null)
            winPanelCloseButton.onClick.AddListener(CloseWinPanel);

        if (uiPanelCloseButton != null)
            uiPanelCloseButton.onClick.AddListener(CloseUIPanel);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(OnSubmitCode);

        codeInputField.text = "";
        errorMessage.gameObject.SetActive(false);
        successMessage.gameObject.SetActive(false);
        uiPanel.SetActive(true);

        // Unlock cursor so player can click and type — movement still works
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        CloseUIPanel();
    }

    private void CloseUIPanel()
    {
        uiPanel.SetActive(false);
        submitButton.onClick.RemoveAllListeners();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnSubmitCode()
    {
        string playerInput = codeInputField.text.ToUpper().Trim();

        if (playerInput == correctCode.ToUpper().Trim())
        {
            errorMessage.gameObject.SetActive(false);
            successMessage.gameObject.SetActive(true);
            Invoke(nameof(OpenGateAndClosePanel), 2f);
        }
        else
        {
            errorMessage.gameObject.SetActive(true);
        }
    }

    private void OpenGateAndClosePanel()
    {
        uiPanel.SetActive(false);

        if (gateToOpen != null)
            gateToOpen.Open();

        if (winPanel != null)
            winPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CloseWinPanel()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}