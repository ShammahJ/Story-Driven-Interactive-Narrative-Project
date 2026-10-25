
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HouseDialogue : MonoBehaviour
{
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;
    public GameObject blackPanel;
    public TMP_Text blackText;

    public GameObject doubleCharacter;
    public Light ceilingLight;
    public FirstPersonController playerController;

    private int dialogueIndex = 0;
    private bool busy = false;
    private bool effectDone = false;
    private bool finalMessageShown = false;

    private string[] lines =
    {
        "Where are we? Who are you?",
        "You really don't remember?",
        "Remember what?",
        "You keep coming back here. Every time, you forget.",
        "What are you talking…",
        "I'm you."
    };

    void Start()
    {
        // Keep the player in place for this scene.
        if (playerController != null)
            playerController.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        dialoguePanel.SetActive(true);
        blackPanel.SetActive(false);

        dialogueText.text = lines[0];
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || busy)
            return;

        // Advance through the dialogue.
        if (dialogueIndex < lines.Length - 1)
        {
            dialogueIndex++;
            dialogueText.text = lines[dialogueIndex];
            return;
        }

        // After "I'm you", flicker the light and remove the Double.
        if (!effectDone)
        {
            StartCoroutine(FlickerAndDisappear());
            return;
        }

        // Show the final message on a black screen.
        if (!finalMessageShown)
        {
            dialoguePanel.SetActive(false);
            blackPanel.SetActive(true);
            blackText.text = "See you again soon.";
            finalMessageShown = true;
            return;
        }

        // Continue to the final forest scene.
        SceneManager.LoadScene("FinalScene");
    }

    IEnumerator FlickerAndDisappear()
    {
        busy = true;

        if (ceilingLight != null)
        {
            for (int i = 0; i < 6; i++)
            {
                ceilingLight.enabled = !ceilingLight.enabled;
                yield return new WaitForSeconds(0.12f);
            }

            // Leave the light off.
            ceilingLight.enabled = false;
        }

        if (doubleCharacter != null)
            doubleCharacter.SetActive(false);

        dialogueText.text = "";
        effectDone = true;
        busy = false;
    }
}
