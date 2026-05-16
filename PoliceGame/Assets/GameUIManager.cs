using UnityEngine;
using TMPro;
using System.Collections;

public class GameUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startCard;
    public GameObject rulesCard;
    public GameObject timerCard;
    public GameObject winCard;
    public GameObject endCard;
    public GameObject bloodRoomCard;

    [Header("Timer")]
    public TextMeshProUGUI timerText;

    private float timeRemaining = 60f;
    private bool isTimerRunning = false;

    private bool gameEnded = false;
    private bool isWinning = false; // 🔥 NEW SAFETY FLAG

    void Start()
    {
        ShowStart();
    }

    void Update()
    {
        // 🔥 HARD BLOCK EVERYTHING IF GAME ENDED OR WINNING
        if (gameEnded || isWinning)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (startCard.activeSelf)
                OpenRules();
            else if (rulesCard.activeSelf)
                StartGame();
        }

        // 🔥 TIMER ONLY RUNS IF ALLOWED
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                GameOver();
            }
        }
    }

    // ================= UI =================

    void ShowStart()
    {
        startCard.SetActive(true);
        rulesCard.SetActive(false);
        timerCard.SetActive(false);
        winCard.SetActive(false);
        endCard.SetActive(false);
        bloodRoomCard.SetActive(false);
    }

    public void OpenRules()
    {
        startCard.SetActive(false);
        rulesCard.SetActive(true);
    }

    public void StartGame()
    {
        gameEnded = false;
        isWinning = false;

        startCard.SetActive(false);
        rulesCard.SetActive(false);
        winCard.SetActive(false);
        endCard.SetActive(false);
        bloodRoomCard.SetActive(false);

        timerCard.SetActive(true);

        timeRemaining = 60f;
        isTimerRunning = true;
    }

    // ================= WIN =================

    public void WinGameWithDelay()
    {
        StartCoroutine(WinSequence());
    }

    IEnumerator WinSequence()
    {
        // 🔥 IMMEDIATELY STOP TIMER
        isTimerRunning = false;
        isWinning = true;

        yield return new WaitForSeconds(2f);

        ShowWin();
    }

    void ShowWin()
    {
        gameEnded = true;
        isTimerRunning = false;

        startCard.SetActive(false);
        rulesCard.SetActive(false);
        timerCard.SetActive(false);
        endCard.SetActive(false);
        bloodRoomCard.SetActive(false);

        winCard.SetActive(true);
    }

    // ================= GAME OVER =================

    void GameOver()
    {
        gameEnded = true;
        isTimerRunning = false;

        startCard.SetActive(false);
        rulesCard.SetActive(false);
        timerCard.SetActive(false);
        winCard.SetActive(false);
        bloodRoomCard.SetActive(false);

        endCard.SetActive(true);
    }

    // ================= TIMER =================

    void UpdateTimerDisplay()
    {
        if (timerText == null) return;

        float m = Mathf.FloorToInt(timeRemaining / 60);
        float s = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = $"{m:00}:{s:00}";
    }

    // ================= BLOOD ROOM =================

    public void ShowBloodRoomCard()
    {
        if (gameEnded) return;

        startCard.SetActive(false);
        rulesCard.SetActive(false);
        timerCard.SetActive(false);
        winCard.SetActive(false);
        endCard.SetActive(false);

        bloodRoomCard.SetActive(true);
    }

    public void HideBloodRoomCard()
    {
        if (gameEnded) return;

        bloodRoomCard.SetActive(false);

        if (isTimerRunning)
            timerCard.SetActive(true);
    }
}