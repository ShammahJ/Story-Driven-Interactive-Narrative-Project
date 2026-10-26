
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class FinalSceneController : MonoBehaviour
{
    [Header("Scene Objects")]
    public Transform player;
    public Transform cabin;
    public GameObject figure;
    public Transform escapePoint;
    public FirstPersonController playerController;

    [Header("UI")]
    public GameObject promptPanel;
    public GameObject blackPanel;
    public GameObject figureDialoguePanel;
    public TMP_Text figureDialogueText;
    public TMP_Text promptText;
    public TMP_Text endingText;

    [Header("Impact Flash")]
    public Image blackPanelImage;
    public float whiteFlashDuration = 0.08f;
    public float crimsonFlashDuration = 0.12f;

    [Header("Audio")]
    public AudioSource tripSound;
    public AudioSource FigureRunSound;
    public AudioSource roadSound;

    [Header("Distances")]
    public float tripDistance = 3f;
    public float figureDistance = 5f;
    public float escapeDistance = 8f;

    private bool endingStarted = false;
    private bool figurePromptActive = false;
    private int figureDialogueStep = 0;

    private readonly Color deepCrimson =
        new Color32(120, 0, 20, 255);

    void Start()
    {
        if (blackPanel != null)
            blackPanel.SetActive(false);

        // Show the opening prompt.
        if (promptPanel != null)
            promptPanel.SetActive(true);

        if (promptText != null)
            promptText.text = "Something feels familiar... that cabin...";

        // Hide the prompt after 3 seconds.
        StartCoroutine(HidePromptAfterDelay());

        if (figureDialoguePanel != null)
            figureDialoguePanel.SetActive(false);

        if (endingText != null)
            endingText.text = "";

        if (blackPanelImage != null)
            blackPanelImage.color = Color.black;

        if (playerController != null)
            playerController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    IEnumerator HidePromptAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    void Update()
    {
        if (endingStarted || player == null)
            return;

        // Figure dialogue takes priority.
        if (figurePromptActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                AdvanceFigureDialogue();

            return;
        }

        // SECRET ENDING: approach the figure.
        if (figure != null &&
            figure.activeInHierarchy &&
            DistanceTo(figure.transform) <= figureDistance)
        {
            figurePromptActive = true;
            figureDialogueStep = 0;

            if (figureDialoguePanel != null)
                figureDialoguePanel.SetActive(true);

            if (figureDialogueText != null)
            {
                figureDialogueText.text =
                    "<color=#780014>I see, you've figured it out?</color>";
            }

            if (playerController != null)
                playerController.enabled = false;

            return;
        }

        // ESCAPE ENDING: find FreedomRoad.
        if (escapePoint != null &&
            DistanceTo(escapePoint) <= escapeDistance)
        {
            StartCoroutine(PlayEscapeEnding());
            return;
        }

        // CABIN LOOP ENDING.
        if (cabin != null &&
            DistanceTo(cabin) <= tripDistance)
        {
            StartCoroutine(PlayEnding());
        }
    }

    float DistanceTo(Transform target)
    {
        Vector3 playerPosition = player.position;
        Vector3 targetPosition = target.position;

        playerPosition.y = 0f;
        targetPosition.y = 0f;

        return Vector3.Distance(playerPosition, targetPosition);
    }

    void StopPlayer()
    {
        if (playerController != null)
            playerController.enabled = false;

        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    void AdvanceFigureDialogue()
    {
        figureDialogueStep++;

        if (figureDialogueText == null)
            return;

        if (figureDialogueStep == 1)
        {
            figureDialogueText.text =
                "<color=#780014>You weren't supposed to remember...</color>";
        }
        else if (figureDialogueStep == 2)
        {
            figureDialogueText.text =
                "<color=#FFFFFF>I wasn't... I'm sorry it has to be this way.</color>";
        }
        else if (figureDialogueStep == 3)
        {
            figureDialogueText.text =
                "<color=#780014>Good</color><color=#FFFFFF>bye.</color>";
        }
        else
        {
            if (figureDialoguePanel != null)
                figureDialoguePanel.SetActive(false);

            StartCoroutine(PlayDoubleEnding());
        }
    }

    // Plays the white -> crimson -> black impact flash.
    IEnumerator PlayImpactFlash()
    {
        if (blackPanel != null)
            blackPanel.SetActive(true);

        if (blackPanelImage != null)
            blackPanelImage.color = Color.white;

        yield return new WaitForSeconds(whiteFlashDuration);

        if (blackPanelImage != null)
            blackPanelImage.color = deepCrimson;

        yield return new WaitForSeconds(crimsonFlashDuration);

        if (blackPanelImage != null)
            blackPanelImage.color = Color.black;
    }

    // ENDING 1: The cabin loop.
    IEnumerator PlayEnding()
    {
        endingStarted = true;
        StopPlayer();

        if (FigureRunSound != null)
            FigureRunSound.Play();

        yield return new WaitForSeconds(0.7f);

        if (FigureRunSound != null)
            FigureRunSound.Stop();

        if (tripSound != null)
            tripSound.Play();

        yield return StartCoroutine(PlayImpactFlash());

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("ForestScene");
    }

    // ENDING 2: THE DOUBLE.
    IEnumerator PlayDoubleEnding()
    {
        endingStarted = true;

        if (promptPanel != null)
            promptPanel.SetActive(false);

        if (figureDialoguePanel != null)
            figureDialoguePanel.SetActive(false);

        if (playerController != null)
            playerController.enabled = false;

        if (tripSound != null)
            tripSound.Play();

        yield return StartCoroutine(PlayImpactFlash());

        if (endingText != null)
            endingText.text = "";

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("ForestScene");
    }

    // ENDING 3: Escape.
    IEnumerator PlayEscapeEnding()
    {
        endingStarted = true;
        StopPlayer();

        if (blackPanel != null)
            blackPanel.SetActive(true);

        if (blackPanelImage != null)
            blackPanelImage.color = Color.black;

        if (endingText != null)
        {
            endingText.gameObject.SetActive(true);
            endingText.color = Color.white;
            endingText.text = "";
            endingText.transform.SetAsLastSibling();
        }

        if (roadSound != null)
            roadSound.Play();

        // Let the road sound play before the text appears.
        yield return new WaitForSeconds(2f);

        if (endingText != null)
        {
            endingText.text =
                "I think I hear the road, I'm finally free.";
        }

        // Keep the text visible.
        yield return new WaitForSeconds(4f);

        if (roadSound != null)
            roadSound.Stop();

        SceneManager.LoadScene("ForestScene");
    }
}
