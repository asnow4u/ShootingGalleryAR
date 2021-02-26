using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI startScreenText;

    [SerializeField]
    private TextMeshProUGUI scoreUI;

    [SerializeField]
    private TextMeshProUGUI timerUI;

    [SerializeField]
    private TextMeshProUGUI resultScore;

    [SerializeField]
    private GameObject UITarget;

    [SerializeField]
    private GameObject shootingArea;

    [SerializeField]
    private GameObject pointerController;

    [SerializeField]
    private GameObject resultCanvas;

    private GameObject[] inactiveTargets;
    private int totalScore;
    private float minutes;
    private float seconds;
    private bool gameRunning;
    private bool paused;

    public float timer;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
      totalScore = 0;
      gameRunning = false;
      paused = false;
    }

    // Update is called once per frame
    void Update()
    {

      if (pointerController.GetComponent<PlacementManager>().GetShootingAreaPlaced()) {

        if (gameRunning){
          UpdateTimer();
        }

        //Distance Check
        //To Close
        if (Vector3.Distance(shootingArea.transform.position, transform.GetChild(0).transform.position) < distance) {

          if (gameRunning) {
            gameRunning = false;
            paused = true;
            inactiveTargets = GameObject.FindGameObjectsWithTag("Target");
            foreach (GameObject target in inactiveTargets){
              target.SetActive(false);
            }

            UITarget.SetActive(true);
          }

          GetComponent<Shoot>().SetFireWhenReady(false);
          startScreenText.text = "To Close";

        //Far enough
        } else if (Vector3.Distance(shootingArea.transform.position, transform.GetChild(0).transform.position) >= distance) {

          GetComponent<Shoot>().SetFireWhenReady(true);

          startScreenText.text = "Start";

          if (GameObject.FindGameObjectsWithTag("Target").Length > 0) {
            startScreenText.text = "Resume";
          }
        }
      }
    }


    private void UpdateTimer() {

      timer -= Time.deltaTime;
      minutes = Mathf.Floor(timer / 60f);
      seconds = timer % 60f;

      if (seconds > 60){
        seconds = 59;
      }

      //Check the timer if 0 (End Game)
      if (timer < 0) {
        gameRunning = false;
        seconds = 0;
        minutes = 0;

        inactiveTargets = GameObject.FindGameObjectsWithTag("Target");
        if (inactiveTargets.Length > 0){
          foreach (GameObject target in inactiveTargets){
            Destroy(target);
          }
        }

        //HighScore
        if (PlayerPrefs.GetInt("HighScore", 0) < totalScore) {
          PlayerPrefs.SetInt("HighScore", totalScore);
        }

        resultScore.text = "Target Hits: " + totalScore.ToString() + "\nHigh Score: " + PlayerPrefs.GetInt("HighScore").ToString();
        resultCanvas.SetActive(true);
      }

      timerUI.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }


    public void IncrementScore() {

      totalScore++;
      scoreUI.text = "Targets: " + totalScore.ToString();
    }


    public void StartGame() {
      gameRunning = true;

      if (paused) {
        paused = false;
        foreach (GameObject target in inactiveTargets){
          target.SetActive(true);
        }

        return;
      }

      shootingArea.GetComponent<TargetSpawnController>().SpawnTarget();
    }

    public void ResetGame(){
      totalScore = 0;
      scoreUI.text = "Targets: " + totalScore.ToString();
      timer = 60f;
      minutes = Mathf.Floor(timer / 60f);
      seconds = timer % 60f;
      timerUI.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
