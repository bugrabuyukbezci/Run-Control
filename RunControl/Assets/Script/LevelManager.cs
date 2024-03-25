using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEngine.EventSystems;
using Bugra;

public class LevelManager : MonoBehaviour
{
    public Button[] Butonlar;    
    public Sprite KilitliButon;
    BellekYonetim _BellekYonetim = new BellekYonetim();
    public AudioSource ButonSes;

    VeriYonetimi _VeriYonetimi = new VeriYonetimi();
    public List<DilVerileriAnaObje> _DilVerileriAnaObje = new List<DilVerileriAnaObje>();
    List<DilVerileriAnaObje> _DilOkunanVeriler = new List<DilVerileriAnaObje>();
    public Text[] TextObjeleri;

    public GameObject YuklemeEkrani;
    public Slider YukelemeSlider;
    private void Start()
    {
        _VeriYonetimi.Dil_Load();
        _DilOkunanVeriler = _VeriYonetimi.DilVerileri_ListeyiAktar();
        _DilVerileriAnaObje.Add(_DilOkunanVeriler[2]);
        DilTercihiYonetimi();

        ButonSes.volume = _BellekYonetim.VeriOku_f("MenuFx");
        int mevcutLevel = _BellekYonetim.VeriOku_i("SonLevel") - 4;
        int Index = 1;
        for (int i = 0; i < Butonlar.Length; i++)
        {
            if (Index <= mevcutLevel)
            {
                Butonlar[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
                int SahneIndex = Index + 4;
                Butonlar[i].onClick.AddListener(delegate { SahneYukle(SahneIndex); });
            }
            else
            {
                Butonlar[i].GetComponent<Image>().sprite = KilitliButon;
                Butonlar[i].enabled = false;
            }
            Index++;
        }
       
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

    public void SahneYukle(int SahneIndex)
    {
        ButonSes.Play();        
        StartCoroutine(LoadASYNC(SahneIndex));
    }
    /*public void SahneYukle()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
        SceneManager.LoadScene(int.Parse(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text) + 4);
    }event systemle çalýþýr*/

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
    public void GeriDon()
    {
        ButonSes.Play();
        SceneManager.LoadScene(0);
    }
}
  