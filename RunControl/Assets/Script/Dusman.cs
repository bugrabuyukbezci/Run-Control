using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Dusman : MonoBehaviour
{
    public GameObject Saldiri_Hedefi;
    public NavMeshAgent _NavMesh;
    public Animator _Animator;
    public GameManager _GameManager;
    bool Saldiri_Basladimi;
    public void AnimasyonuTetikle()
    {
        _Animator.SetBool("Saldir", true);
        Saldiri_Basladimi = true;
    }
    private void LateUpdate()
    {
        if (Saldiri_Basladimi)
            _NavMesh.SetDestination(Saldiri_Hedefi.transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AltKarakterler"))
        {
            Vector3 yeniPoz = new Vector3(transform.position.x, .23f, transform.position.z);
            _GameManager.YokOlmaEkektiOlustur(yeniPoz,false,true);
            gameObject.SetActive(false);
        }
    }
}
