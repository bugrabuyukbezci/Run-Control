using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Bugra;
public class AyarlarManager : MonoBehaviour
{
    public AudioSource ButonSes;
    public Slider MenuSes;
    public Slider MenuFx;
    public Slider OyunSesi;
    BellekYonetim _BellekYonetim = new BellekYonetim();

    VeriYonetimi _VeriYonetimi = new VeriYonetimi();
    public List<DilVerileriAnaObje> _DilVerileriAnaObje = new List<DilVerileriAnaObje>();
    List<DilVerileriAnaObje> _DilOkunanVeriler = new List<DilVerileriAnaObje>();
    public TextMeshProUGUI[] TextObjeleri;
    bool DilDurumu;

    public TextMeshProUGUI DilDurumText;


    private void Start()
    {
        MenuSes.value = _BellekYonetim.VeriOku_f("MenuSes");
        MenuFx.value = _BellekYonetim.VeriOku_f("MenuFx");
        OyunSesi.value = _BellekYonetim.VeriOku_f("OyunSesi");

        _VeriYonetimi.Dil_Load();
        _DilOkunanVeriler = _VeriYonetimi.DilVerileri_ListeyiAktar();
        _DilVerileriAnaObje.Add(_DilOkunanVeriler[4]);
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
            DilDurumText.text = "TÜRKÇE";
            DilDurumu = false;

        }
        else if (_BellekYonetim.VeriOku_s("Dil") == "EN")
        {
            for (int i = 0; i < TextObjeleri.Length; i++)
            {
                TextObjeleri[i].text = _DilVerileriAnaObje[0]._DilVerileri_EN[i].Metin;
            }
            DilDurumText.text = "ENGLISH";
            DilDurumu = true;
        }
        else
        {
            for (int i = 0; i < TextObjeleri.Length; i++)
            {
                TextObjeleri[i].text = _DilVerileriAnaObje[0]._DilVerileri_TR[i].Metin;
            }
            DilDurumText.text = "TÜRKÇE";
            DilDurumu = false;
        }
    }

    public void SesAyarla(string HangiAyar)
    {
        switch (HangiAyar)
        {
            case "menuses":
                _BellekYonetim.VeriKaydet_float("MenuSes",MenuSes.value);
                break;
            case "menufx":
                _BellekYonetim.VeriKaydet_float("MenuFx", MenuFx.value);
                ButonSes.volume = PlayerPrefs.GetFloat("MenuFx");
                break;
            case "oyunsesi":
                _BellekYonetim.VeriKaydet_float("OyunSesi", OyunSesi.value);
                break;
        }
    }
    public void GeriDon()
    {
        ButonSes.Play();
        SceneManager.LoadScene(0);
    }
    public void DilDegistir()
    {      
        DilDurumu = !DilDurumu;
        ButonSes.Play();
        if (!DilDurumu)
        {
            _BellekYonetim.VeriKaydet_string("Dil", "TR");           
        }
        else
        {
            _BellekYonetim.VeriKaydet_string("Dil", "EN");
        }
        DilTercihiYonetimi();
    }
}
