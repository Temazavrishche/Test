using UnityEngine;

public class BackGroundChikenMove : MonoBehaviour
{
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Run",true);
    }
    private void Update()
    {
        transform.position = transform.position + transform.forward * 3 * Time.deltaTime;
    }
}
