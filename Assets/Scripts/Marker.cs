using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Marker : MonoBehaviour
{
    public static Marker instance;
    public bool mainTail;
    public class Marker1
    {
        public Vector3 position;
        public Quaternion rotation;

        public Marker1(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }

    }

   /* private void Update()
    {
        if (mainTail)
        {
            
            transform.position = GetComponentInParent<EnemyController>().gameObject.transform.position;
        }
    }*/

    public List<Marker1> markerList = new List<Marker1>();

    private void FixedUpdate()
    {
        UpdateMarkerList();

    }

    public void UpdateMarkerList()
    {
        markerList.Add(new Marker1(transform.position, transform.rotation));
    }
    public void ClearMarkerList()
    {
        markerList.Clear();
        markerList.Add(new Marker1(transform.position, transform.rotation));
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
