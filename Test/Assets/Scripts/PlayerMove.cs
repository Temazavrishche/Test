using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    private GameManager gm;
    private AudioSource PickUpSound;

    private void Start()
    {
        PickUpSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    private void FixedUpdate()
    {
        if (GameManager.GameStarted)
            rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndPos"))
        {
            if (gm.CurrentLvlNumber == PlayerPrefs.GetInt("CountUnlockedLvls"))
                PlayerPrefs.SetInt("CountUnlockedLvls", PlayerPrefs.GetInt("CountUnlockedLvls") + 1);
            gm.nextLvl();
        }
        else if (other.CompareTag("Crystal"))
        {
            if(PlayerPrefs.GetString("Music") != "Off")
                PickUpSound.Play();
            PlayerPrefs.SetInt("Crystals", PlayerPrefs.GetInt("Crystals") + 1);
            Destroy(other.gameObject);
        }
        gm.UpdateGameCanvas();
    }
}
