using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Bugra;

public class AnamenuManager : MonoBehaviour
{
    BellekYonetim _BellekYonetim = new BellekYonetim();
    VeriYonetimi _VeriYonetimi = new VeriYonetimi();
    public GameObject CikisPaneli;
    public List<ItemBilgileri> _Varsayilan_ItemBilgileri = new List<ItemBilgileri>();
    public List<DilVerileriAnaObje> _Varsayilan_DilVerileri = new List<DilVerileriAnaObje>();
    public AudioSource ButonSes;

    public List<DilVerileriAnaObje> _DilVerileriAnaObje = new List<DilVerileriAnaObje>();
    List<DilVerileriAnaObje> _DilOkunanVeriler = new List<DilVerileriAnaObje>();
    public Text[] TextObjeleri;
    public GameObject YuklemeEkrani;
    public Slider YukelemeSlider;
    private void Start()
    {
        _BellekYonetim.KontrolEtVeTanimla();
        _VeriYonetimi.IlkKurulumDosyaOlusturma(_Varsayilan_ItemBilgileri, _Varsayilan_DilVerileri); 
        ButonSes.volume = _BellekYonetim.VeriOku_f("MenuFx");
       // _BellekYonetim.VeriKaydet_string("Dil", "EN");
        _VeriYonetimi.Dil_Load();
        _DilOkunanVeriler = _VeriYonetimi.DilVerileri_ListeyiAktar();
        _DilVerileriAnaObje.Add(_DilOkunanVeriler[0]);
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
    public void SahneYukle(int Index)
    {
        ButonSes.Play();
        SceneManager.LoadScene(Index);
    }
    public void Oyna()
    {
        ButonSes.Play();        
        StartCoroutine(LoadASYNC(_BellekYonetim.VeriOku_i("SonLevel")));
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
    
    public void CikisButonIslem(string durum)
    {
        ButonSes.Play();
        if (durum == "cikisPanel")
            CikisPaneli.SetActive(true);
        else if (durum == "Evet")
            Application.Quit();
        else
            CikisPaneli.SetActive(false);        
    }
}
