using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningSystem : MonoBehaviour
{
    public float smooth = 2.5f;
    public GameObject player;
    public Camera mainCamera;
    [SerializeField] private Transform projectile;
    [SerializeField] GameObject mainCanvasObj;
    private List<Transform> projectileTransforms = new List<Transform>();
    private Dictionary<Transform, Transform> projectileToArrowMap;
    // Start is called before the first frame update
    void Start()
    {
        projectileToArrowMap = new Dictionary<Transform, Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform targetTransform in projectileTransforms)
        {
            if (targetTransform) {
                Vector3 direction = targetTransform.position - player.transform.position;
                Vector3 backwards = mainCamera.transform.position - player.transform.position;

                direction = new Vector3(direction.x, 0, direction.z);
                backwards = new Vector3(backwards.x, 0, backwards.z);

                
                Debug.DrawRay(player.transform.position, backwards, Color.red);
                Debug.DrawRay(player.transform.position, direction, Color.green);
                float angle = Vector3.Angle(direction, backwards);
                Vector3 cross = Vector3.Cross(direction, backwards);
                if (cross.y < 0)
                {
                    angle = -angle;
                }

                projectileToArrowMap[targetTransform].rotation = Quaternion.Slerp(projectileToArrowMap[targetTransform].rotation, Quaternion.Euler(0, 0, angle),  Time.deltaTime * smooth);

                float distance = Vector3.Distance(targetTransform.position, player.transform.position);

                Image warningArrowImage = projectileToArrowMap[targetTransform].GetComponentInChildren<Image>();
                Color currentColor = warningArrowImage.color;
                currentColor.a = (20 - distance) / 20;
                if (angle > 60 || angle < -60) {
                    currentColor.a = 0;
                }
                warningArrowImage.color = currentColor;
            }
            else {
                if (projectileToArrowMap[targetTransform]) {
                    Destroy(projectileToArrowMap[targetTransform].gameObject);
                }
            }
        }
    }

    public void RemoveArrow(Transform projectileTransform)
    {
        // Check if the projectile transform is in the dictionary
        if (projectileToArrowMap.ContainsKey(projectileTransform))
        {
            // Destroy the associated warning arrow and remove the mapping
            Destroy(projectileToArrowMap[projectileTransform].gameObject);
            projectileToArrowMap.Remove(projectileTransform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!projectileTransforms.Contains(other.transform) && other.CompareTag("Enemy Projectile"))
        {
            projectileTransforms.Add(other.transform);
            Transform warningArrow = Instantiate(projectile, new Vector3(0, -218, 0), Quaternion.identity);
            projectileToArrowMap.Add(other.transform, warningArrow);
            warningArrow.transform.SetParent(mainCanvasObj.transform, false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (projectileTransforms.Contains(other.transform) && other.CompareTag("Enemy Projectile"))
        {
            projectileTransforms.Remove(other.transform);
            Destroy(projectileToArrowMap[other.transform].gameObject);
            projectileToArrowMap.Remove(other.transform);
        }
    }
}
