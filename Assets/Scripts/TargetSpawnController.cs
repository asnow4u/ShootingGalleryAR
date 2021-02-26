using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawnController : MonoBehaviour
{
    [SerializeField]
    private GameObject targetPrefab;

    private float height, width;

    public float edgeBuffer;

    // Start is called before the first frame update
    void Start()
    {
        Bounds planeBounds = GetComponent<MeshFilter>().mesh.bounds;
        height = planeBounds.size.z/2f - edgeBuffer;
        width = planeBounds.size.x/2f - edgeBuffer;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void SpawnTarget(){

      GameObject spawnedTarget = Instantiate(targetPrefab, transform.position, Quaternion.identity);
      spawnedTarget.transform.parent = gameObject.transform;
      spawnedTarget.transform.localEulerAngles = new Vector3(90f, 0f, 0f);

      float x = Random.Range(-width, width);
      float y = Random.Range(-height, height);

      spawnedTarget.transform.localScale = new Vector3(33.33f, 50f, 5f);
      spawnedTarget.transform.localPosition = new Vector3(x, 0.1f, y);
    }
}
