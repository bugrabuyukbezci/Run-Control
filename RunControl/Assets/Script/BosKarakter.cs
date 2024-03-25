using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BosKarakter : MonoBehaviour
{
    public SkinnedMeshRenderer _Renderer;
    public Material AtanacakMatarial;
    public NavMeshAgent _NavMesh;
    public Animator _Animator;
    private GameObject Target;
    public List<GameObject> VarisNoktasi;
    bool TemasVar;
    public GameManager _GameManager;
    private void LateUpdate()
    {
        if(TemasVar)
        _NavMesh.SetDestination(Target.transform.position);
    }
    void Start()
    {
        Target = VarisNoktasi[Random.Range(0, VarisNoktasi.Count)];        
    }
    Vector3 PozisyonVer()
    {
        return new Vector3(transform.position.x, .23f, transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!TemasVar)
        {
            if (other.CompareTag("AltKarakterler") || other.CompareTag("Player"))
            {
                MaterialDegistirVeAnimasyonTetikle();
                GameManager.AnlikKarakterSayisi++;
                Debug.Log(GameManager.AnlikKarakterSayisi);
                TemasVar = true;
                GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("OyunSesi");
                GetComponent<AudioSource>().Play();
            }
        }
        if (other.CompareTag("Dusman"))
        {
            _GameManager.YokOlmaEkektiOlustur(PozisyonVer(), false, false);
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("igneliKutu"))
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
    }
    void MaterialDegistirVeAnimasyonTetikle()
    {
        Material[] mats = _Renderer.materials;
        mats[0] = AtanacakMatarial;
        _Renderer.materials = mats;
        _Animator.SetBool("Saldir", true);
    }
}
