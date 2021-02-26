using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    [SerializeField]
    private GameObject gunObject;

    [SerializeField]
    private GameObject UITarget;

    [SerializeField]
    private GameObject shootingArea;

    [SerializeField]
    private GameObject resultCanvas;

    [SerializeField]
    private GameObject scoreCanvas;

    [SerializeField]
    private ParticleSystem gunFlash;

    private Animator animator;

    private AudioSource gunShot;
    private AudioSource targetBreak;

    public float timeDelay;
    private bool fired;
    public bool fireWhenReady;

    // Start is called before the first frame update
    void Start() {
      animator = gunObject.GetComponent<Animator>();
      gunShot = gunObject.GetComponent<AudioSource>();
      targetBreak = shootingArea.GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update() {

      if (fireWhenReady) {
        if (!fired) {
          if (Input.touchCount > 0 || Input.GetKeyDown("space") || Input.GetKey("space") ) {
            animator.SetTrigger("Fire");
            gunShot.Play();
            gunFlash.Play();
            TestTargetHit();
            fired = true;
            Invoke("ToggleFire", timeDelay);
          }
        }
      }
    }


    private void TestTargetHit() {

      RaycastHit hit;

      if (Physics.Raycast(gunObject.transform.position + gunObject.transform.TransformDirection(Vector3.forward) * 0.075f, gunObject.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity)){

        if (hit.transform.tag == "Target"){

          Destroy(hit.transform.gameObject);
          targetBreak.Play();
          GetComponent<GameController>().IncrementScore();
          shootingArea.GetComponent<TargetSpawnController>().SpawnTarget();
          Debug.Log("Target Hit! " + hit.distance);
        }

        else if (hit.transform.tag == "UITarget"){

          targetBreak.Play();

          // Start a new game after playing one
          if (resultCanvas.activeSelf == true){
            GetComponent<GameController>().ResetGame();
            resultCanvas.SetActive(false);
            GetComponent<GameController>().StartGame();
          }

          // Start game
          else {
            UITarget.SetActive(false);
            GetComponent<GameController>().StartGame();
          }
        }

      }
    }

    private void ToggleFire() {
      fired = false;
    }

    public void SetFireWhenReady(bool distanceCheck) {
      fireWhenReady = distanceCheck;

      if (fireWhenReady){
        animator.SetBool("Distance", true);
        // animator.ResetTrigger("Fire");

      } else {
        animator.SetBool("Distance", false);
      }
    }

    public bool GetFireWhenReady() {
      return fireWhenReady;
    }


    //Visual Representation of gun sight
    void OnDrawGizmosSelected() {
      Gizmos.color = Color.red;

      //For AK
      // Gizmos.DrawRay(gunObject.transform.position + gunObject.transform.TransformDirection(Vector3.up) * 0.485f, gunObject.transform.TransformDirection(Vector3.down) * 5f);

      //For M1 Carbine
      Gizmos.DrawRay(gunObject.transform.position + gunObject.transform.TransformDirection(Vector3.forward) * 0.075f, gunObject.transform.TransformDirection(Vector3.down) * 5f);
    }



}
