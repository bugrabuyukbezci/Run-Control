using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pervane : MonoBehaviour
{
    public Animator _Animator;
    public float beklemeSuresi;
    public BoxCollider _Ruzgar;
    private void Start()
    {
        beklemeSuresi = Random.Range(1f, 3f);

    }
    public void AnimasyonDurumu(string durum)
    {
        if (durum == "true")
        {
            _Animator.SetBool("Calistir", true);
            _Ruzgar.enabled = true;
        }           
        else
        {
            _Animator.SetBool("Calistir", false);
            StartCoroutine(AnimasyonTetik());
            _Ruzgar.enabled = false;
        }
    }
    IEnumerator AnimasyonTetik()
    {
       yield return new WaitForSeconds(beklemeSuresi);
        AnimasyonDurumu("true");
    }
}
