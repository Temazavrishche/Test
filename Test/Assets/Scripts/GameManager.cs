using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] Lvls, Characters,BackGroundAnim;
    [SerializeField] private Levels[] AllLevelsScenario;
    private GameObject CurrentLvl, NextLvl, PrevLvl,Player;
    public int CurrentLvlNumber;
    public static bool GameStarted;
    [SerializeField] private GameObject MenuCamera,Canvas,LoseCanvas,GameCanvas,JumpImpactEffect;
    private int CurrentAction = 0;
    private Levels CurrentScenario;
    [SerializeField] private CinemachineFreeLook GameCamera;
    private AudioSource PlayerFall;
    [SerializeField] private Text CanvasLvl, CanvasCrystal;

    private void Start()
    {
        PlayerFall = GameCamera.GetComponent<AudioSource>();
        StartCoroutine(BackGroundActions());
    }
    private void Update()
    {
        if(GameStarted && Player.transform.position.y < 0)
        {
            if (PlayerPrefs.GetString("Music") != "Off")
                PlayerFall.Play();
            GameStarted = false;
            LoseCanvas.SetActive(true);
            GameCamera.Follow = null;
            GameCamera.LookAt = null;
            Player = null;
            CurrentAction = 0;
            Destroy(Player, 2);
            GameCanvas.SetActive(false);
        }
        if (GameStarted)
        {
#if UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Began)
                {
                    Action();
                }
            }
#endif
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
               //Action();
            }
#endif
        }
    }
    public void StartGame(int LvlNumber)
    {
        CurrentLvlNumber = LvlNumber;
        Canvas.SetActive(false);
        CurrentLvl = Instantiate(Lvls[CurrentLvlNumber],Vector3.zero,Quaternion.identity);
        Transform nextPos = CurrentLvl.transform.Find("EndPos");
        if(LvlNumber + 1 < Lvls.Length)
        NextLvl = Instantiate(Lvls[LvlNumber + 1], nextPos.position, Quaternion.Euler(nextPos.eulerAngles));
        CurrentScenario = AllLevelsScenario[CurrentLvlNumber];
        MenuCamera.SetActive(false);
        GameCanvas.SetActive(true);
        CameraAndPlayer();
        StartCoroutine(ChangeBool());
        UpdateGameCanvas();
    }
    public void nextLvl()
    {
        CurrentLvlNumber++;
        CurrentAction = 0;
        PrevLvl = CurrentLvl;
        CurrentLvl = NextLvl;
        Transform _nextPos = CurrentLvl.transform.Find("EndPos");
        if (CurrentLvlNumber  < Lvls.Length - 1)
            NextLvl = Instantiate(Lvls[CurrentLvlNumber + 1], _nextPos.position, Quaternion.Euler(_nextPos.eulerAngles));
        CurrentScenario = AllLevelsScenario[CurrentLvlNumber];
        Destroy(PrevLvl,1.5f);
        
    }
    IEnumerator ChangeBool()
    {
        yield return new WaitForSeconds(1.5f);
        Player.GetComponentInChildren<Rigidbody>().useGravity = true;
        GameStarted = true;
        GameCamera.m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
        Player.GetComponent<Animator>().SetBool("Run",true);
    }
    private void RotatePlayer(float Angle)
    {
        Player.transform.rotation = Quaternion.Euler(Player.transform.eulerAngles + new Vector3(0,Angle,0));
    }
    private void Action()
    {
        if(CurrentAction <= CurrentScenario.NextAction.Length - 1)
        {
            switch (CurrentScenario.NextAction[CurrentAction])
            {
                case "Right":
                    RotatePlayer(90);
                    break;
                case "Left":
                    RotatePlayer(-90);
                    break;
                case "Jump":
                    StartCoroutine(JumpImpact());
                    Player.GetComponent<Rigidbody>().AddForce(transform.up * 3, ForceMode.Impulse);
                    Player.GetComponent<Animator>().SetTrigger("Jump");
                    break;
            }
            CurrentAction++;
        }
    }
    IEnumerator JumpImpact()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject obj =  Instantiate(JumpImpactEffect,Player.transform.position,Quaternion.identity);
        Destroy(obj, 1);
    }
    IEnumerator BackGroundActions()
    {
        while (!GameStarted)
        {
            GameObject obj = Instantiate(BackGroundAnim[Random.Range(0, BackGroundAnim.Length)],new Vector3( 0,10,-18),Quaternion.Euler(Random.Range(-25,25), Random.Range(60, 120), 0));
            float progress = 0;
            Vector3 newPos = new Vector3(0, Random.Range(-5, 5), -18);
            while (progress < 1)
            {
                progress += Time.deltaTime;
                obj.transform.position = Vector3.Lerp(obj.transform.position, newPos, 5 * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(5);
            float _progress = 0;
            while (_progress < 1)
            {
                _progress += Time.deltaTime;
                obj.transform.position = Vector3.Lerp(obj.transform.position, new Vector3(0,-20,-10), 5 * Time.deltaTime);
                yield return null;
            }
            Destroy(obj,2);
        }
        yield break;
    }
    public void RestartGame()
    {
        GameCanvas.SetActive(true);
        CameraAndPlayer();
        LoseCanvas.SetActive(false);
        StartCoroutine(ChangeBool());
    }
    public void Map()
    {
        MenuCamera.SetActive(true);
        LoseCanvas.SetActive(false);
        GameCanvas.SetActive(false);
        Canvas.SetActive(true);
        Destroy(CurrentLvl);
        Destroy(NextLvl);
        StartCoroutine(BackGroundActions());
    }
    private void CameraAndPlayer()
    {
        GameCamera.m_BindingMode = CinemachineTransposer.BindingMode.LockToTarget;
        Player = Instantiate(Characters[PlayerPrefs.GetInt("Characters")], CurrentLvl.transform.position + new Vector3(0, 0.5f, 0), CurrentLvl.transform.rotation);
        GameCamera.Follow = Player.transform;
        GameCamera.LookAt = Player.transform;
    }
    public void UpdateGameCanvas()
    {
        CanvasLvl.text = "Уровень: " + (CurrentLvlNumber + 1);
        CanvasCrystal.text = "Кристалов: " + PlayerPrefs.GetInt("Crystals");
    }
}
