
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OpeningSequence : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject controlsPanel;
    public GameObject blackPanel;
    public GameObject interactionPanel;

    public TMP_Text dialogueText;
    public Button startButton;
    public FirstPersonController playerController;

    private int dialogueIndex = 0;
    private bool dialogueActive = false;
    private bool transitioning = false;

    private string[] openingLines =
    {
        "Where am I…?",
        "How did I get here?"
    };

    void Start()
    {
        titlePanel.SetActive(true);
        controlsPanel.SetActive(false);
        blackPanel.SetActive(false);
        interactionPanel.SetActive(false);

        if (playerController != null)
            playerController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        startButton.onClick.AddListener(StartGame);
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || transitioning)
            return;

        // Controls screen: begin the opening dialogue.
        if (controlsPanel.activeSelf)
        {
            controlsPanel.SetActive(false);
            blackPanel.SetActive(true);

            dialogueIndex = 0;
            dialogueText.text = openingLines[dialogueIndex];
            dialogueActive = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }

        // Advance the opening dialogue.
        if (dialogueActive)
        {
            dialogueIndex++;

            if (dialogueIndex < openingLines.Length)
            {
                dialogueText.text = openingLines[dialogueIndex];
            }
            else
            {
                blackPanel.SetActive(false);
                dialogueActive = false;

                if (playerController != null)
                    playerController.enabled = true;

                interactionPanel.SetActive(true);
            }

            return;
        }

        // Space confirms YES on the interaction prompt.
        if (interactionPanel.activeSelf)
        {
            StartCoroutine(EnterHouseScene());
        }
    }

    void StartGame()
    {
        titlePanel.SetActive(false);
        blackPanel.SetActive(false);
        interactionPanel.SetActive(false);
        controlsPanel.SetActive(true);

        dialogueIndex = 0;
        dialogueActive = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator EnterHouseScene()
    {
        transitioning = true;

        if (playerController != null)
            playerController.enabled = false;

        interactionPanel.SetActive(false);
        blackPanel.SetActive(true);
        dialogueText.text = "";

        // Hold the black screen briefly before loading the next scene.
        yield return new WaitForSecondsRealtime(0.8f);

        SceneManager.LoadScene("HouseScene");
    }
}
