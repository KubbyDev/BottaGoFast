using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public LayerMask layerMask;
    public float maxRayLenght;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startPosition_frontRay = transform.position + transform.forward * transform.localScale.z / 2;
        Vector3 startPosition_frontRayRight = startPosition_frontRay + transform.right*transform.localScale.x/2;
        Vector3 startPosition_frontRayLeft = startPosition_frontRay - transform.right * transform.localScale.x / 2;
        Vector3 startPosition_SideRayRight = transform.position + transform.right *transform.localScale.x / 2;
        Vector3 startPosition_SideRayLeft = transform.position - transform.right * transform.localScale.x / 2;

        List<Ray> rays = new List<Ray>();
        rays.Add(new Ray(startPosition_frontRay, transform.forward));
        rays.Add(new Ray(startPosition_frontRayRight,transform.forward+transform.right));
        rays.Add(new Ray(startPosition_frontRayLeft, transform.forward-transform.right));
        rays.Add(new Ray(startPosition_SideRayRight, transform.right));
        rays.Add(new Ray(startPosition_SideRayLeft, -transform.right));

        List<float> distances = new List<float>();
        string str = "";
        foreach (Ray ray in rays)
        {
            
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, layerMask))
            {
                Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction*maxRayLenght , Color.green);
            }
            distances.Add(hitInfo.distance);
            str += System.Math.Round(hitInfo.distance,2) + " ";
        }
        Debug.Log(str);
    }
}
