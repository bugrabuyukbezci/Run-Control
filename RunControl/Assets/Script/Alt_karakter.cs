using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Alt_karakter : MonoBehaviour
{
    GameObject Target;
    NavMeshAgent _NavMesh;
    public GameManager _GameManager;
    public List<GameObject> VarisNoktasi;
    private void Start()
    {        
        _NavMesh = GetComponent<NavMeshAgent>();
        Target = VarisNoktasi[Random.Range(0, VarisNoktasi.Count)];        
    }
    private void LateUpdate()
    {
        _NavMesh.SetDestination(Target.transform.position);
    }
    Vector3 PozisyonVer()
    {
        return new Vector3(transform.position.x, .23f, transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("igneliKutu"))
        {            
            _GameManager.YokOlmaEkektiOlustur(PozisyonVer());
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Testere"))
        {
            _GameManager.YokOlmaEkektiOlustur(PozisyonVer());
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("PervaneIgneleri"))
        {
            _GameManager.YokOlmaEkektiOlustur(PozisyonVer());
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Balyoz"))
        {
            _GameManager.YokOlmaEkektiOlustur(PozisyonVer(), true);            
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Dusman"))
        {
            _GameManager.YokOlmaEkektiOlustur(PozisyonVer(), false,false);
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("BosKarakter"))
        {
            _GameManager.Karakterler.Add(other.gameObject);            
            other.gameObject.tag = "AltKarakterler";            
        }
    }
}
