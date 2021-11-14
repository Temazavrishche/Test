using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class CanvasBtns : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private GameObject XMarkMusic, XMarkNotifications, LevelPanel, CharPanel;
    [SerializeField] private GameObject[] Pages;
    [SerializeField] private Characters[] CharactersInfo;
    [SerializeField] private Text Crystals;
    private int CurrentPage;
    private void Start()
    {
        PlayerPrefs.SetInt("Crystals",1000);
        if (!PlayerPrefs.HasKey("OpenCharacters"))
        {
            AddCharacterAndGiveMeRent(0);
        }
        CheckMusic();
        CheckNotifications();
        CheckShop();
        SetNumberAndUnlock();
        CheckSelected();
    }
    public void _ChangePage(int PageNumber)
    {
        CurrentPage = PageNumber;
        foreach (GameObject page in Pages)
        {
            page.SetActive(false);
        }
        Pages[PageNumber].SetActive(true);
    }
    public void ChangeSubpage(int ad)
    {
        for (int i = 0; i < Pages[CurrentPage].transform.childCount; i++)
        {
            Pages[CurrentPage].transform.GetChild(i).gameObject.SetActive(false);
        }
        Pages[CurrentPage].transform.GetChild(ad).gameObject.SetActive(true);
    }
    public void SoundOnOff()
    {
        if(PlayerPrefs.GetString("Music") == "Off")
        {
            PlayerPrefs.SetString("Music", "On");
        }
        else PlayerPrefs.SetString("Music", "Off");
        CheckMusic();
    }
    private void CheckMusic()
    {
        if(PlayerPrefs.GetString("Music") == "Off")
        {
            _audio.mute = true;
            XMarkMusic.SetActive(true);
        }
        else
        {
            _audio.mute = false;
            XMarkMusic.SetActive(false);
        }
    }
    public void NotificationsOnOff()
    {
        if (PlayerPrefs.GetString("Notifications") == "On")
        {
            PlayerPrefs.SetString("Notifications", "Off");
        }
        else PlayerPrefs.SetString("Notifications", "On");
        CheckNotifications();
    }
    private void CheckNotifications()
    {
        if (PlayerPrefs.GetString("Notifications") == "On")
        {
            ///
            XMarkNotifications.SetActive(true);
        }
        else
        {
            ///
            XMarkNotifications.SetActive(false);
        }
    }
    public void SetNumberAndUnlock()
    {
        for (int i = 0; i < LevelPanel.transform.childCount; i++)
        {
            LevelPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            if (i <= PlayerPrefs.GetInt("CountUnlockedLvls"))
                LevelPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
            else
                LevelPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }
    public void SelectCharacter(int CharNumber)
    {
        PlayerPrefs.SetInt("Characters", CharNumber);
        CheckSelected();
    }
    public void CheckSelected()
    {
        for (int i = 0; i < CharPanel.transform.childCount; i++)
        {
            CharPanel.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
        CharPanel.transform.GetChild(PlayerPrefs.GetInt("Characters")).GetChild(1).gameObject.SetActive(true);
    }
    public void CheckShop()
    {
        Crystals.text = "Кристалов: " + PlayerPrefs.GetInt("Crystals");
        for (int i = 0; i < CharPanel.transform.childCount; i++)
        {
            if (PlayerPrefs.GetString("OpenCharacters").Contains(i.ToString()))
            {
                CharPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
                CharPanel.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                CharPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
                CharPanel.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                CharPanel.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = "Купить " + CharactersInfo[i].Price;
            }
        }

    }
    public void AddCharacterAndGiveMeRent(int CharNumber)
    {
        if(CharactersInfo[CharNumber].Price <= PlayerPrefs.GetInt("Crystals"))
        {
            PlayerPrefs.SetString("OpenCharacters", PlayerPrefs.GetString("OpenCharacters") + CharNumber + " ");
            PlayerPrefs.SetInt("Crystals", PlayerPrefs.GetInt("Crystals") - CharactersInfo[CharNumber].Price);
            CheckShop();
        }
    }
}