using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnakinControl : MonoBehaviour
{
    public Animator a;
    public SpriteRenderer sr;
    private int JumpFrameCount;

    // Start is called before the first frame update
    void Start()
    {
        JumpFrameCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

        //movement control
        bool mov = false;
        a.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        a.SetFloat("Vertical", Input.GetAxis("Vertical"));
        if (a.GetFloat("Horizontal") == 0 && a.GetFloat("Vertical") == 0)
            mov = false;
        else
            mov = true;
        a.SetBool("Idle", !mov);

        //slash control
        //triggered
        if (Input.GetKeyDown("left ctrl"))
        {
            a.Play("Anakin Slash A");
            a.SetBool("Slash A", true);
        }
        if (Input.GetKeyDown("left ctrl") && a.GetBool("Slash A"))
        {
            a.Play("Anakin Slash B");
            a.SetBool("Slash A", false);
        }

        //sprinting control
        //continuous
        bool spr = false;
        if (Input.GetKey("left shift") && mov && !a.GetBool("Jump"))
        {
            a.Play("Anakin Sprint");
            a.SetBool("Sprint", true);
            spr = true;
        }
        else
        {
            a.SetBool("Sprint", false);
            spr = false;
        }

        //jump control
        //triggered
        if (Input.GetKeyDown("space") && !a.GetBool("Jump") && JumpFrameCount == 0)
        {
            Debug.Log("I entered Jump control");
            a.SetBool("Jump", true);
            a.Play("Anakin Jump");
        }
        if (a.GetBool("Jump"))
        {
            JumpFrameCount++;
            if (Input.GetKey("left shift"))
            {
                a.SetBool("Sprint", false);
                spr = true;
            }
        }
        if (JumpFrameCount == 40)
        {
            Debug.Log("I exited Jump control");
            JumpFrameCount = 0;
            a.SetBool("Jump", false);
        }

        //diagonal movement control
        if ((a.GetFloat("Vertical") != 0) && (a.GetFloat("Horizontal") != 0))
            a.SetBool("Slant", true);
        else
            a.SetBool("Slant", false);

        //direction control
        if (a.GetFloat("Horizontal") < 0)
            sr.flipX = true;
        if(a.GetFloat("Horizontal") > 0)
            sr.flipX = false;

        //speed control
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
        Vector3 vertical = new Vector3(0.0f, Input.GetAxis("Vertical"), 0.0f);
        if (!spr)
            transform.position += (horizontal + vertical) * Time.deltaTime * 5;
        else
            transform.position += (horizontal + vertical) * Time.deltaTime * 10;

    }
}
