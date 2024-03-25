using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bugra;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class OzellestirmeManager : MonoBehaviour
{
    public Text PuanText;
    public Text KaydedildiText;
    public GameObject[] IslemPanelleri;
    public GameObject IslemCanvasi;
    public GameObject[] GenelPanaller;
    public Button[] IslemButonlari;    
    int AktifIslemPaneliIndex;    
    [Header("----------SAPKALAR---------")]
    public GameObject[] Sapkalar;
    public Button[] SapkaButonlari;
    public Text SapkaText;
    [Header("----------SOPALAR----------")]
    public GameObject[] Sopalar;
    public Button[] SopaButonlari;
    public Text SopaText;
    [Header("----------MATERIALLER------")]
    public Material[] Mataryaller;
    public Material VarsayilanTema;
    public Button[] MataryalButonlari;
    public Text MataryalText;
    public SkinnedMeshRenderer _Renderer;

    int SapkaIndex = -1;
    int SopaIndex = -1;
    int MataryalIndex = -1;

    BellekYonetim _BellekYonetim = new BellekYonetim();
    VeriYonetimi _VeriYonetimi = new VeriYonetimi();
    [Header("----------GENEL VERÝLER-----")]
    public Animator KaydedildiAnimator;
    public AudioSource[] Sesler;
    public List<ItemBilgileri> _ItemBilgileri = new List<ItemBilgileri>();
    public List<DilVerileriAnaObje> _DilVerileriAnaObje = new List<DilVerileriAnaObje>();
    List<DilVerileriAnaObje> _DilOkunanVeriler = new List<DilVerileriAnaObje>();
    public Text[] TextObjeleri;

    string SatinAlmaText;
    string SatinAlmaBoslukluText;
    string ItemYokText;
    string ItemTemaText;
    

    private void Start()
    {        
        //_BellekYonetim.VeriKaydet_int("Puan", 2000);
        PuanText.text = _BellekYonetim.VeriOku_i("Puan").ToString();               
       _VeriYonetimi.Load();
       _ItemBilgileri = _VeriYonetimi.ListeyiAktar();
        DurumuKontrolEt(0, true);
        DurumuKontrolEt(1, true);
        DurumuKontrolEt(2, true);
        foreach (var item in Sesler)
        {
            item.volume = _BellekYonetim.VeriOku_f("MenuFx");
        }

        _VeriYonetimi.Dil_Load();
        _DilOkunanVeriler = _VeriYonetimi.DilVerileri_ListeyiAktar();
        _DilVerileriAnaObje.Add(_DilOkunanVeriler[1]);
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
            SatinAlmaText = "SATIN AL";
            SatinAlmaBoslukluText = " SATIN AL";
            ItemYokText = "ITEM YOK";
            ItemTemaText = "TEMA 1";
            KaydedildiText.text = "KAYDEDÝLDÝ";
        }
        else if (_BellekYonetim.VeriOku_s("Dil") == "EN")
        {
            for (int i = 0; i < TextObjeleri.Length; i++)
            {
                TextObjeleri[i].text = _DilVerileriAnaObje[0]._DilVerileri_EN[i].Metin;
            }
            SatinAlmaText = "BUY";
            SatinAlmaBoslukluText = " BUY";
            ItemYokText = "NO ITEM";
            ItemTemaText = "THEME 1";
            KaydedildiText.text = "WAS RECORDED";

        }
        else
        {
            for (int i = 0; i < TextObjeleri.Length; i++)
            {
                TextObjeleri[i].text = _DilVerileriAnaObje[0]._DilVerileri_TR[i].Metin;
            }
        }
    }
    public void DurumuKontrolEt(int Bolum,bool islem=false)
    {
        if (Bolum == 0)
        {
            if (_BellekYonetim.VeriOku_i("AktifSapka") == -1)
            {
                foreach (var item in Sapkalar)
                {
                    item.SetActive(false);
                }
                TextObjeleri[5].text = SatinAlmaText;                
                IslemButonlari[0].interactable = false;
                IslemButonlari[1].interactable = false;
                if (!islem)
                {
                    SapkaIndex = -1;
                    SapkaText.text = ItemYokText;
                }                
            }
            else
            {
                foreach (var item in Sapkalar)
                {
                    item.SetActive(false);
                }
                SapkaIndex = _BellekYonetim.VeriOku_i("AktifSapka");
                Sapkalar[SapkaIndex].SetActive(true);
                SapkaText.text = _ItemBilgileri[SapkaIndex].Item_Ad;
                TextObjeleri[5].text = SatinAlmaText;
                IslemButonlari[0].interactable = false;
                IslemButonlari[1].interactable = false;
            }
        }
        else if (Bolum == 1)
        {
            if (_BellekYonetim.VeriOku_i("AktifSopa") == -1)
            {
                foreach (var item in Sopalar)
                {
                    item.SetActive(false);
                }
                IslemButonlari[0].interactable = false;
                IslemButonlari[1].interactable = false;
                TextObjeleri[5].text = SatinAlmaText;
                if (!islem)
                {
                    SopaIndex = -1;
                    SopaText.text = ItemYokText;
                }
                
            }
            else
            {
                foreach (var item in Sopalar)
                {
                    item.SetActive(false);
                }
                SopaIndex = _BellekYonetim.VeriOku_i("AktifSopa");
                Sopalar[SopaIndex].SetActive(true);
                SopaText.text = _ItemBilgileri[SopaIndex+3].Item_Ad;
                TextObjeleri[5].text = SatinAlmaText;
                IslemButonlari[0].interactable = false;
                IslemButonlari[1].interactable = false;
            }
        }
        else
        {
            if (_BellekYonetim.VeriOku_i("AktifTema") == -1)
            {
                if (!islem)
                {
                    MataryalIndex = -1;
                    MataryalText.text = "Tema 1";
                    IslemButonlari[0].interactable = false;
                    IslemButonlari[1].interactable = false;
                } else
                {
                    Material[] mats = _Renderer.materials;
                    mats[0] = VarsayilanTema;
                    _Renderer.materials = mats;
                    TextObjeleri[5].text = SatinAlmaText;
                }
            }
            else
            {
                MataryalIndex = _BellekYonetim.VeriOku_i("AktifTema");
                Material[] mats = _Renderer.materials;
                mats[0] = Mataryaller[MataryalIndex];
                _Renderer.materials = mats;
                MataryalText.text = _ItemBilgileri[MataryalIndex+6].Item_Ad;
                TextObjeleri[5].text = SatinAlmaText;
                IslemButonlari[0].interactable = false;
                IslemButonlari[1].interactable = false;
            }
        }
    }    
    void SatinAlmaSonuc(int Index)
    {
        _ItemBilgileri[Index].SatinAlinmaDurumu = true;
        _BellekYonetim.VeriKaydet_int("Puan", _BellekYonetim.VeriOku_i("Puan") - _ItemBilgileri[Index].Item_Puan);
        PuanText.text = _BellekYonetim.VeriOku_i("Puan").ToString();
        IslemButonlari[0].interactable = false;
        IslemButonlari[1].interactable = true;
        TextObjeleri[5].text = SatinAlmaText;
    }
    public void SatinAl()
    {
        Sesler[1].Play();

        if (AktifIslemPaneliIndex != -1)
        {
            switch (AktifIslemPaneliIndex)
            {
                case 0:
                    //Debug.Log("Bölüm No: " + AktifIslemPaneliIndex + " Item Index:" + SapkaIndex + " Item Ad:" + _ItemBilgileri[SapkaIndex].Item_Ad);
                    SatinAlmaSonuc(SapkaIndex);
                    break;
                case 1:
                    int Index = SopaIndex + 3;
                    SatinAlmaSonuc(Index);
                    break;
                case 2:
                    int Index2 = MataryalIndex + 3;
                    SatinAlmaSonuc(Index2);
                    break;
            }
        }
    }
    public void Kaydet()
    {
        Sesler[2].Play();

        if (AktifIslemPaneliIndex != -1)
        {
            switch (AktifIslemPaneliIndex)
            {
                case 0:
                    _BellekYonetim.VeriKaydet_int("AktifSapka", SapkaIndex);                                     
                    IslemButonlari[1].interactable = false;                    
                    break;
                case 1:
                    _BellekYonetim.VeriKaydet_int("AktifSopa", SopaIndex);
                    IslemButonlari[1].interactable = false;
                    break;
                case 2:
                    _BellekYonetim.VeriKaydet_int("AktifTema", MataryalIndex);
                    IslemButonlari[1].interactable = false;
                    break;
            }
            if (!KaydedildiAnimator.GetBool("ok"))
                KaydedildiAnimator.SetBool("ok",true);
        }
        

    }
    public void Sapka_YonButonlari(string islem)
    {
        Sesler[0].Play();

        if (islem == "ileri")
        {
            if (SapkaIndex == -1)
            {
                SapkaIndex = 0;
                Sapkalar[SapkaIndex].SetActive(true);
                SapkaText.text=_ItemBilgileri[SapkaIndex].Item_Ad;
                if (!_ItemBilgileri[SapkaIndex].SatinAlinmaDurumu)
                {
                    TextObjeleri[5].text = _ItemBilgileri[SapkaIndex].Item_Puan + SatinAlmaBoslukluText;
                    IslemButonlari[1].interactable = false;
                    if (_BellekYonetim.VeriOku_i("Puan")< _ItemBilgileri[SapkaIndex].Item_Puan)
                        IslemButonlari[0].interactable = false;
                    else
                        IslemButonlari[0].interactable = true;

                }
                else
                {
                    TextObjeleri[5].text = SatinAlmaText;
                    IslemButonlari[0].interactable = false;
                    IslemButonlari[1].interactable = true;
                }
            }
            else
            {
                Sapkalar[SapkaIndex].SetActive(false);
                SapkaIndex++;
                Sapkalar[SapkaIndex].SetActive(true);
                SapkaText.text = _ItemBilgileri[SapkaIndex].Item_Ad; 
                if (!_ItemBilgileri[SapkaIndex].SatinAlinmaDurumu)
                {
                    TextObjeleri[5].text = _ItemBilgileri[SapkaIndex].Item_Puan + SatinAlmaBoslukluText;
                    IslemButonlari[1].interactable = false;
                    if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[SapkaIndex].Item_Puan)
                        IslemButonlari[0].interactable = false;
                    else
                        IslemButonlari[0].interactable = true;
                }
                else
                {
                    TextObjeleri[5].text = SatinAlmaText;
                    IslemButonlari[0].interactable = false;
                    IslemButonlari[1].interactable = true;
                }
            }
            if (SapkaIndex == Sapkalar.Length - 1)
                SapkaButonlari[1].interactable = false;
            else
                SapkaButonlari[1].interactable = true;
            if(SapkaIndex!=-1)
                SapkaButonlari[0].interactable = true;
        }
        else
        {
            if (SapkaIndex != -1)
            {
                Sapkalar[SapkaIndex].SetActive(false);
                SapkaIndex--;
                if (SapkaIndex != -1)
                {
                    Sapkalar[SapkaIndex].SetActive(true);
                    SapkaButonlari[0].interactable = true;
                    SapkaText.text = _ItemBilgileri[SapkaIndex].Item_Ad;
                    if (!_ItemBilgileri[SapkaIndex].SatinAlinmaDurumu)
                    {
                        TextObjeleri[5].text = _ItemBilgileri[SapkaIndex].Item_Puan + SatinAlmaBoslukluText;
                        IslemButonlari[1].interactable = false;
                        if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[SapkaIndex].Item_Puan)
                            IslemButonlari[0].interactable = false;
                        else
                            IslemButonlari[0].interactable = true;
                    }
                    else
                    {
                        TextObjeleri[5].text = SatinAlmaText;
                        IslemButonlari[0].interactable = false;
                        IslemButonlari[1].interactable = true;
                    }
                }
                else
                {
                    SapkaButonlari[0].interactable = false;
                    SapkaText.text = ItemYokText;
                    TextObjeleri[5].text = SatinAlmaText;
                    IslemButonlari[0].interactable = false;
                }
            }
            else
            {
                SapkaButonlari[0].interactable = false;
                SapkaText.text = ItemYokText;
                TextObjeleri[5].text = SatinAlmaText;
                IslemButonlari[0].interactable = false;
            }
            if(SapkaIndex != Sapkalar.Length - 1)
                SapkaButonlari[1].interactable = true;
        }
    }
    public void Sopa_YonButonlari(string islem)
    {
        Sesler[0].Play();

        if (islem == "ileri")
        {
            if (SopaIndex == -1)
            {
                SopaIndex = 0;
                Sopalar[SopaIndex].SetActive(true);
                SopaText.text = _ItemBilgileri[SopaIndex+3].Item_Ad;
                if (!_ItemBilgileri[SopaIndex + 3].SatinAlinmaDurumu)
                {
                    TextObjeleri[5].text = _ItemBilgileri[SopaIndex + 3].Item_Puan + SatinAlmaBoslukluText;
                    IslemButonlari[1].interactable = false;
                    if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[SopaIndex + 3].Item_Puan)
                        IslemButonlari[0].interactable = false;
                    else
                        IslemButonlari[0].interactable = true;
                }
                else
                {
                    TextObjeleri[5].text = SatinAlmaText;
                    IslemButonlari[0].interactable = false;
                    IslemButonlari[1].interactable = true;
                }
            }
            else
            {
                Sopalar[SopaIndex].SetActive(false);
                SopaIndex++;
                Sopalar[SopaIndex].SetActive(true);
                SopaText.text = _ItemBilgileri[SopaIndex+3].Item_Ad;
                if (!_ItemBilgileri[SopaIndex + 3].SatinAlinmaDurumu)
                {
                    TextObjeleri[5].text = _ItemBilgileri[SopaIndex + 3].Item_Puan + SatinAlmaBoslukluText;
                    IslemButonlari[1].interactable = false;
                    if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[SopaIndex + 3].Item_Puan)
                        IslemButonlari[0].interactable = false;
                    else
                        IslemButonlari[0].interactable = true;
                }
                else
                {
                    TextObjeleri[5].text = SatinAlmaText;
                    IslemButonlari[0].interactable = false;
                    IslemButonlari[1].interactable = true;
                }
            }
            if (SopaIndex == Sopalar.Length - 1)
                SopaButonlari[1].interactable = false;
            else
                SopaButonlari[1].interactable = true;
            if (SopaIndex != -1)
                SopaButonlari[0].interactable = true;
        }
        else
        {
            if (SopaIndex != -1)
            {
                Sopalar[SopaIndex].SetActive(false);
                SopaIndex--;
                if (SopaIndex != -1)
                {
                    Sopalar[SopaIndex].SetActive(true);
                    SopaButonlari[0].interactable = true;
                    SopaText.text = _ItemBilgileri[SopaIndex+3].Item_Ad;
                    if (!_ItemBilgileri[SopaIndex + 3].SatinAlinmaDurumu)
                    {
                        TextObjeleri[5].text = _ItemBilgileri[SopaIndex + 3].Item_Puan + SatinAlmaBoslukluText;
                        IslemButonlari[1].interactable = false;
                        if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[SopaIndex + 3].Item_Puan)
                            IslemButonlari[0].interactable = false;
                        else
                            IslemButonlari[0].interactable = true;
                    }
                    else
                    {
                        TextObjeleri[5].text = SatinAlmaText;
                        IslemButonlari[0].interactable = false;
                        IslemButonlari[1].interactable = true;
                    }
                }
                else
                {
                    SopaButonlari[0].interactable = false;
                    SopaText.text = ItemYokText;
                    TextObjeleri[5].text = SatinAlmaText;
                    IslemButonlari[0].interactable = false;
                }
            }
            else
            {
                SopaButonlari[0].interactable = false;
                SopaText.text = ItemYokText;
                TextObjeleri[5].text = SatinAlmaText;
                IslemButonlari[0].interactable = false;
            }
            if (SopaIndex != Sopalar.Length - 1)
                SopaButonlari[1].interactable = true;
        }
    }
    public void Mataryal_YonButonlari(string islem)
    {
        Sesler[0].Play();

        if (islem == "ileri")
        {
            if (MataryalIndex == -1)
            {
                MataryalIndex = 0;
                Material[] mats = _Renderer.materials;
                mats[0] = Mataryaller[MataryalIndex];
                _Renderer.materials = mats;
                
                MataryalText.text = _ItemBilgileri[MataryalIndex + 6].Item_Ad;
                if (!_ItemBilgileri[MataryalIndex + 6].SatinAlinmaDurumu)
                {
                    TextObjeleri[5].text = _ItemBilgileri[MataryalIndex + 6].Item_Puan + SatinAlmaBoslukluText;
                    IslemButonlari[1].interactable = false;
                    if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[MataryalIndex + 6].Item_Puan)
                        IslemButonlari[0].interactable = false;
                    else
                        IslemButonlari[0].interactable = true;
                }
                else
                {
                    TextObjeleri[5].text = SatinAlmaText;
                    IslemButonlari[0].interactable = false;
                    IslemButonlari[1].interactable = true;
                }
            }
            else
            {                
                MataryalIndex++;
                Material[] mats = _Renderer.materials;
                mats[0] = Mataryaller[MataryalIndex];
                _Renderer.materials = mats;
                MataryalText.text = _ItemBilgileri[MataryalIndex + 6].Item_Ad;
                if (!_ItemBilgileri[MataryalIndex + 6].SatinAlinmaDurumu)
                {
                    TextObjeleri[5].text = _ItemBilgileri[MataryalIndex + 6].Item_Puan + SatinAlmaBoslukluText;
                    IslemButonlari[1].interactable = false;
                    if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[MataryalIndex + 6].Item_Puan)
                        IslemButonlari[0].interactable = false;
                    else
                        IslemButonlari[0].interactable = true;
                }
                else
                {
                    TextObjeleri[5].text = SatinAlmaText;
                    IslemButonlari[0].interactable = false;
                    IslemButonlari[1].interactable = true;
                }
            }
            if (MataryalIndex == Mataryaller.Length - 1)
                MataryalButonlari[1].interactable = false;
            else
                MataryalButonlari[1].interactable = true;
            if (MataryalIndex != -1)
                MataryalButonlari[0].interactable = true;
        }
        else
        {
            if (MataryalIndex != -1)
            {                
                MataryalIndex--;
                if (MataryalIndex != -1)
                {
                    Material[] mats = _Renderer.materials;
                    mats[0] = Mataryaller[MataryalIndex];
                    _Renderer.materials = mats;
                    MataryalButonlari[0].interactable = true;
                    MataryalText.text = _ItemBilgileri[MataryalIndex + 6].Item_Ad;
                    if (!_ItemBilgileri[MataryalIndex + 6].SatinAlinmaDurumu)
                    {
                        TextObjeleri[5].text = _ItemBilgileri[MataryalIndex + 6].Item_Puan + SatinAlmaBoslukluText;
                            IslemButonlari[1].interactable = false;
                            if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[MataryalIndex + 6].Item_Puan)
                                IslemButonlari[0].interactable = false;
                            else
                                IslemButonlari[0].interactable = true;
                        }
                    else
                    {
                        TextObjeleri[5].text = SatinAlmaText;
                        IslemButonlari[0].interactable = false;
                        IslemButonlari[1].interactable = true;
                    }
                }
                else
                {
                    Material[] mats = _Renderer.materials;
                    mats[0] = VarsayilanTema;
                    _Renderer.materials = mats;
                    MataryalButonlari[0].interactable = false;
                    MataryalText.text = ItemTemaText;
                    TextObjeleri[5].text = SatinAlmaText;
                    IslemButonlari[0].interactable = false;
                }
            }
            else
            {
                Material[] mats = _Renderer.materials;
                mats[0] = VarsayilanTema;
                _Renderer.materials = mats;                
                MataryalButonlari[0].interactable = false;
                MataryalText.text = ItemTemaText;
                TextObjeleri[5].text = SatinAlmaText;
                IslemButonlari[0].interactable = false;
            }
            if (MataryalIndex != Mataryaller.Length - 1)
                MataryalButonlari[1].interactable = true;
        }
    }
    public void IslemPaneliCikart(int Index)
    {
        Sesler[0].Play();
        DurumuKontrolEt(Index);
        GenelPanaller[0].SetActive(true);
        IslemPanelleri[Index].SetActive(true);        
        GenelPanaller[1].SetActive(true);
        IslemCanvasi.SetActive(false);
        AktifIslemPaneliIndex = Index;
    }
    public void GeriDon()
    {
        Sesler[0].Play();
        GenelPanaller[1].SetActive(false);
        IslemCanvasi.SetActive(true);        
        GenelPanaller[0].SetActive(false);
        IslemPanelleri[AktifIslemPaneliIndex].SetActive(false);
        DurumuKontrolEt(AktifIslemPaneliIndex,true);
        AktifIslemPaneliIndex = -1;
    }
    public void AnaMenuGeriDon()
    {
        Sesler[0].Play();
        _VeriYonetimi.Save(_ItemBilgileri);
        SceneManager.LoadScene(0);
    }
}
