using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bugra;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{   
    public static int AnlikKarakterSayisi = 1;
    public List<GameObject> Karakterler;
    public List<GameObject> OlusmaEfektleri;
    public List<GameObject> YokOlmaEfektleri;
    public List<GameObject> AdamLekesiEfektleri;
    [Header("----------LEVEL VERÝLERÝ-----")]
    public List<GameObject> Dusmanlar;
    public int KacDusmanOlsun;
    public GameObject _AnaKarakter;
    public bool OyunBittiMi;
    public bool SonaGeldikMi;
    [Header("----------MALZEMELER---------")]
    public GameObject[] Sapkalar;
    public GameObject[] Sopalar;
    public Material[] Mataryaller;
    public SkinnedMeshRenderer _Renderer;

    Matematiksel_Islemler _Matematiksel_Islemler = new Matematiksel_Islemler();
    BellekYonetim _BellekYonetim = new BellekYonetim();
    VeriYonetimi _VeriYonetimi = new VeriYonetimi();

    Scene _Scene;
    [Header("----------GENELVERÝLER-------")]
    public AudioSource[] Sesler;
    public GameObject[] islemPanalleri;
    public Slider OyunSesiAyar;
    public List<DilVerileriAnaObje> _DilVerileriAnaObje = new List<DilVerileriAnaObje>();
    List<DilVerileriAnaObje> _DilOkunanVeriler = new List<DilVerileriAnaObje>();
    public Text[] TextObjeleri;
    public GameObject YuklemeEkrani;
    public Slider YukelemeSlider;

    private void Awake()
    {
        Time.timeScale = 1;
        Sesler[0].volume = _BellekYonetim.VeriOku_f("OyunSesi");
        OyunSesiAyar.value = _BellekYonetim.VeriOku_f("OyunSesi");
        Sesler[1].volume = _BellekYonetim.VeriOku_f("MenuFx");
        Destroy(GameObject.FindWithTag("MenuSes"));
        ItemleriKontrolEt();
    }
    private void Start()
    {
        DusmanlariOlustur();
        _Scene = SceneManager.GetActiveScene();

        _VeriYonetimi.Dil_Load();
        _DilOkunanVeriler = _VeriYonetimi.DilVerileri_ListeyiAktar();
        _DilVerileriAnaObje.Add(_DilOkunanVeriler[5]);
        DilTercihiYonetimi();
    }
    void DilTercihiYonetimi()
    {
        if (_BellekYonetim.VeriOku_s("Dil") == "TR")
        {
            for (int i = 0; i < TextObjeleri.Length; i++)
            {
                TextObjeleri[i].text = _DilVerileriAnaObje[0]._DilVerileri_TR[i].Metin;
            }
        }
        else if (_BellekYonetim.VeriOku_s("Dil") == "EN")
        {
            for (int i = 0; i < TextObjeleri.Length; i++)
            {
                TextObjeleri[i].text = _DilVerileriAnaObje[0]._DilVerileri_EN[i].Metin;
            }
        }
        else
        {
            for (int i = 0; i < TextObjeleri.Length; i++)
            {
                TextObjeleri[i].text = _DilVerileriAnaObje[0]._DilVerileri_TR[i].Metin;
            }
        }
    }

    public void DusmanlariOlustur()
    {
        for (int i = 0; i < KacDusmanOlsun; i++)
        {
            Dusmanlar[i].SetActive(true);
        }
    }
    public void DusmanlariTetikle()
    {
        foreach (var item in Dusmanlar)
        {
            if (item.activeInHierarchy)
            {
                item.GetComponent<Dusman>().AnimasyonuTetikle();
            }
        }
        SonaGeldikMi = true;
        SavasDurumu();
    }
    void SavasDurumu()
    {
        if (SonaGeldikMi)
        {        
            if (AnlikKarakterSayisi == 1 || KacDusmanOlsun==0)
            {
            OyunBittiMi = true;
                foreach (var item in Dusmanlar)
                {
                if (item.activeInHierarchy)
                {
                    item.GetComponent<Animator>().SetBool("Saldir", false);
                }
                }
                foreach (var item in Karakterler)
                {
                if (item.activeInHierarchy)
                {
                    item.GetComponent<Animator>().SetBool("Saldir", false);
                }
                }
                _AnaKarakter.GetComponent<Animator>().SetBool("Saldir", false);
                if (AnlikKarakterSayisi < KacDusmanOlsun || AnlikKarakterSayisi == KacDusmanOlsun)
                {
                    islemPanalleri[3].SetActive(true);
                }
                else
                {
                    if (_Scene.buildIndex == _BellekYonetim.VeriOku_i("SonLevel"))
                        _BellekYonetim.VeriKaydet_int("SonLevel", _BellekYonetim.VeriOku_i("SonLevel") + 1);
                    if (AnlikKarakterSayisi > 5)
                    { 
                        _BellekYonetim.VeriKaydet_int("Puan", _BellekYonetim.VeriOku_i("Puan") + 250);  
                    }
                    else
                    {
                        _BellekYonetim.VeriKaydet_int("Puan", _BellekYonetim.VeriOku_i("Puan") + 100);
                    }
                    islemPanalleri[2].SetActive(true);
                }
            }
        }
    }
    IEnumerator LoadASYNC(int SceneIndex)
    {
        AsyncOperation Op = SceneManager.LoadSceneAsync(SceneIndex);
        YuklemeEkrani.SetActive(true);

        while (!Op.isDone)
        {
            float progress = Mathf.Clamp01(Op.progress / .9f);
            YukelemeSlider.value = progress;
            yield return null;

        }
    }
    public void Adamyonetimi(string islemturu, int GelenSayi, Transform Pozisyon)
    {
        switch (islemturu)
        {
            case "Carpma":
                _Matematiksel_Islemler.Carpma(GelenSayi,Karakterler,Pozisyon,OlusmaEfektleri);                
                break;

            case "Toplama":
                _Matematiksel_Islemler.Toplama(GelenSayi, Karakterler, Pozisyon,OlusmaEfektleri);
                break;

            case "Cikarma": 
                _Matematiksel_Islemler.Cikarma(GelenSayi, Karakterler,YokOlmaEfektleri);
                break;

            case "Bolme":
                _Matematiksel_Islemler.Bolme(GelenSayi, Karakterler,YokOlmaEfektleri);
                break;
        }
        Debug.Log(AnlikKarakterSayisi);
    }
    public void YokOlmaEkektiOlustur(Vector3 Pozisyon,bool Balyoz=false,bool Durum=false)
    {        
        foreach (var item in YokOlmaEfektleri)
        {
            if (!item.activeInHierarchy)
            {
                item.SetActive(true);
                item.transform.position = Pozisyon;
                item.GetComponent<AudioSource>().volume = _BellekYonetim.VeriOku_f("OyunSesi");
                item.GetComponent<ParticleSystem>().Play();
                item.GetComponent<AudioSource>().Play();
                if (!Durum)
                    AnlikKarakterSayisi--;
                else
                    KacDusmanOlsun--;
                Debug.Log(AnlikKarakterSayisi);
                break;
            }
        }        
        if (Balyoz)
        {
            Vector3 yeniPoz = new Vector3(Pozisyon.x, .005f, Pozisyon.z);
            foreach (var item in AdamLekesiEfektleri)
            {
                if (!item.activeInHierarchy)
                {
                    item.SetActive(true);
                    item.transform.position = yeniPoz;
                    break;
                }
            }
        }
        if(!OyunBittiMi)
            SavasDurumu();
    }
    public void ItemleriKontrolEt()
    {
        if(_BellekYonetim.VeriOku_i("AktifSapka")!=-1)
            Sapkalar[_BellekYonetim.VeriOku_i("AktifSapka")].SetActive(true);
        if (_BellekYonetim.VeriOku_i("AktifSopa") != -1)
            Sopalar[_BellekYonetim.VeriOku_i("AktifSopa")].SetActive(true);
        if (_BellekYonetim.VeriOku_i("AktifTema") != -1)
        {
            Material[] mats = _Renderer.materials;
            mats[0] = Mataryaller[_BellekYonetim.VeriOku_i("AktifTema")+1];
            _Renderer.materials = mats;
        }
        else
        {
            Material[] mats = _Renderer.materials;
            mats[0] = Mataryaller[0];
            _Renderer.materials = mats;
        }
            
    }
    public void CikisButonIslem(string durum)
    {
        Sesler[1].Play();
        
        if (durum == "durdur")
        {
            Time.timeScale = 0;
            islemPanalleri[0].SetActive(true);
        }
        else if (durum == "devamet")
        {
            islemPanalleri[0].SetActive(false);
            Time.timeScale = 1;
        }
        else if (durum == "tekrar")
        {
            Time.timeScale = 1;            
            StartCoroutine(LoadASYNC(_Scene.buildIndex));

        }
        else if (durum == "AnaMenu")
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }

    }
    public void Ayarlar(string durum)
    {
        if (durum == "ayarla")
        {
            islemPanalleri[1].SetActive(true);
            Time.timeScale = 0;
        }else
        {
            islemPanalleri[1].SetActive(false);
            Time.timeScale = 1;
        }
    }
    public void SesiAyarla()
    {
        _BellekYonetim.VeriKaydet_float("OyunSesi", OyunSesiAyar.value);
        Sesler[0].volume = OyunSesiAyar.value;
    }
    public void SonrakiLevel()
    {        
        StartCoroutine(LoadASYNC(_Scene.buildIndex + 1));
    }


}
