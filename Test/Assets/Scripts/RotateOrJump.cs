using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOrJump : MonoBehaviour
{
    [SerializeField] private Vector3 Rotate;
    [SerializeField] private AnimationCurve Ycurve;
    [SerializeField] private AnimationCurve Xcurve;
    [SerializeField] private bool Jump;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform Player = other.transform;
#if UNITY_ANDROID
            if (Input.touchCount > 0 && !Jump)
            {
                other.transform.rotation = Quaternion.Euler(Player.eulerAngles + Rotate);
            }
            else if(Input.touchCount > 0)
            {
                float speed = other.GetComponent<PlayerMove>().speed;
            }
#endif
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && !Jump)
            {
                other.transform.rotation = Quaternion.Euler(Player.eulerAngles + Rotate);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                other.GetComponent<PlayerMove>().jump = true;
            }
#endif
        }
    }
}
