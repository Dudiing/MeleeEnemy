using System.Collections;
using UnityEngine;

public class FuegoController : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(ActivarAnimacionPeriodica());
    }

    IEnumerator ActivarAnimacionPeriodica()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            ActivarAnimacionFuego();

            yield return new WaitForSeconds(2f);

            DesactivarAnimacionFuego();
        }
    }

    void ActivarAnimacionFuego()
    {
        anim.SetBool("Fire", true);
    }

    void DesactivarAnimacionFuego()
    {
        anim.SetBool("Fire", false);
    }
}
