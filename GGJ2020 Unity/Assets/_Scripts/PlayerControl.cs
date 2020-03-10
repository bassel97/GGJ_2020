using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    //TODO redo this class :D

    public ParticleSystem effect;
    public AudioSource audioSource;

    public Transform camAttached;

    public LayerMask attackLayer;
    public float attackCoolDown;

    void Update()
    {
        if (attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            attackCoolDown = 0.5f;

            effect.Play();
            audioSource.Play();

            if (Physics.Raycast(camAttached.position, camAttached.forward, out RaycastHit hit, 50.0f, attackLayer))
            {
                Human attacked = hit.transform.GetComponent<Human>();
                if (!attacked)
                {
                    Debug.LogWarning("Not Human ?? " + hit.transform.name);
                    return;
                }
                if (!attacked.IsPublicAttacker())
                    return;

                //Debug.Log(name + " attack " + hit.transform.name);
                attacked.KillHuman();
            }
        }
    }
}
