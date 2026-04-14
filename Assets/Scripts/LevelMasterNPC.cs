using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LevelMasterNPC : MonoBehaviour
{
    public Button submitButton;

    [Header("Win Settings")]
    public GameObject winPanel;

    [Header("Gate Settings")]
    public LevelGate gateToOpen;

    [Header("Code Settings")]
    public string correctCode;

    [Header("UI Elements")]
    public GameObject uiPanel;
    public TMP_InputField codeInputField;
    public TextMeshProUGUI errorMessage;
    public TextMeshProUGUI successMessage;

    private void Start()
    {
        uiPanel.SetActive(false);
        errorMessage.gameObject.SetActive(false);
        successMessage.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiPanel.SetActive(true);
            submitButton.onClick.AddListener(OnSubmitCode); // wire THIS npc to the button
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiPanel.SetActive(false);
            submitButton.onClick.RemoveListener(OnSubmitCode); // unwire when player leaves
        }
    }

    public void OnSubmitCode()
    {
        string playerInput = codeInputField.text.ToUpper().Trim();

        if (playerInput == correctCode.ToUpper().Trim())
        {
            errorMessage.gameObject.SetActive(false);
            successMessage.gameObject.SetActive(true);
            Invoke("OpenGateAndClosePanel", 2f);
        }
        else
        {
            errorMessage.gameObject.SetActive(true);
        }
    }

    private void OpenGateAndClosePanel()
    {
        uiPanel.SetActive(false);

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
        else
        {
            gateToOpen.Open();
        }
    }
}