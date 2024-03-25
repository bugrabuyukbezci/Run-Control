using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Camera : MonoBehaviour
{
    public Transform target;
    public Vector3 target_offset;
    public static Camera current;
    public bool SonaGeldikMi;
    public GameObject GidecegiNokta;
    private void Start()
    {
        target_offset = transform.position - target.position;
    }
    private void LateUpdate()
    {
        if(!SonaGeldikMi)
            transform.position = Vector3.Lerp(transform.position, target.position + target_offset, .125f);
        else
            transform.position = Vector3.Lerp(transform.position, GidecegiNokta.transform.position, .015f);
    }
}
