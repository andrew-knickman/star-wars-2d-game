using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnakinControl : MonoBehaviour
{
    public Animator a;
    public SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Sprint()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //movement control
        bool mov;
        a.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        a.SetFloat("Vertical", Input.GetAxis("Vertical"));
        if (a.GetFloat("Horizontal") == 0 && a.GetFloat("Vertical") == 0)
        {
            mov = false;
        }
        else
        {
            mov = true;
        }
        a.SetBool("Idle", !mov);

        //jump control
        bool jump;
        if (Input.GetKeyDown("space"))
        {
            a.Play("Anakin Jump", 0);
            a.SetBool("Jump", true);
            jump = true;
        }
        else
        {
            a.SetBool("Jump", false);
            jump = false;
        }
        //Input.GetKeyDown("space")
        //a.GetCurrentAnimatorStateInfo(0).IsName("Anakin Jump")


        //slash control
        if (Input.GetKeyDown("left ctrl"))
        {
            a.Play("Anakin Slash B");

            if (Input.GetKeyDown("left ctrl"))
                a.Play("Anakin Slash A");
        }

        //sprinting control
        bool spr;
        if (Input.GetKey("left shift"))
        {
            if (mov && !jump)
            {
                a.Play("Anakin Sprint");
                a.SetBool("Sprint", true);
                spr = true;
            }
            else if(mov && jump)
            {
                spr = true;
            }
            else
            {
                a.SetBool("Sprint", false);
                spr = false;
            }
        }
        else
        {
            a.SetBool("Sprint", false);
            spr = false;
        }

        //diagonal movement control
        if ((a.GetFloat("Vertical") != 0) && (a.GetFloat("Horizontal") != 0))
            a.SetBool("Slant", true);
        else
            a.SetBool("Slant", false);

        if (a.GetFloat("Horizontal") < 0)
            sr.flipX = true;

        //direction control
        if(a.GetFloat("Horizontal") > 0)
            sr.flipX = false;

        //speed control
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
        Vector3 vertical = new Vector3(0.0f, Input.GetAxis("Vertical"), 0.0f);
        if (!spr)
        {
            transform.position += horizontal * Time.deltaTime * 5;
            transform.position += vertical * Time.deltaTime * 5;
        }
        else
        {
            transform.position += horizontal * Time.deltaTime * 10;
            transform.position += vertical * Time.deltaTime * 10;
        }

    }
}
