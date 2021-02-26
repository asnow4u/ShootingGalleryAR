using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class PlacementManager : MonoBehaviour
{

    private ARRaycastManager raycastManager;
    private GameObject pointerObj;

    [SerializeField]
    private GameObject startTarget;

    [SerializeField]
    private Camera arCamera;

    [SerializeField]
    private TextMeshProUGUI infoBoard;

    [SerializeField]
    private GameObject infoCanvas;

    public bool pointerPlaced;

    // Start is called before the first frame update
    void Start()
    {
      raycastManager = FindObjectOfType<ARRaycastManager>();
      pointerObj = this.transform.GetChild(0).gameObject;
      pointerObj.SetActive(false);
      startTarget.SetActive(false);
      pointerPlaced = false;
    }

    // Update is called once per frame
    void Update()
    {
      if (!pointerPlaced) {
        List <ARRaycastHit> hit = new List<ARRaycastHit>();
        raycastManager.Raycast(new Vector2(Screen.width/2, Screen.height/2), hit, TrackableType.Planes);
        if (hit.Count > 0) {
          transform.position = hit[0].pose.position;
          transform.LookAt(arCamera.transform);
          infoCanvas.SetActive(false);

          if (!pointerObj.activeInHierarchy) {
            pointerObj.SetActive(true);
          }

          if (Input.touchCount > 0) {
            pointerPlaced = true;
            startTarget.SetActive(true);

          }
        }
      }
    }


    public void SetPointerPlaced(bool b) {
        pointerPlaced = b;
    }

    public bool GetShootingAreaPlaced() {
      return pointerPlaced;
    }
}
