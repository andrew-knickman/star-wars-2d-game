using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnakinControl : MonoBehaviour
{
    public Animator a;
    public SpriteRenderer sr;
    private int JumpFrameCount;
    private int SlashAFrameCount;
    private int SlashBFrameCount;
    private int SlashCFrameCount;


    // Start is called before the first frame update
    void Start()
    {
        JumpFrameCount = 0;
        SlashAFrameCount = 0;
        SlashBFrameCount = 0;
        SlashCFrameCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

        //movement control
        bool mov = false;
        a.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        a.SetFloat("Vertical", Input.GetAxis("Vertical"));
        if (a.GetFloat("Horizontal") < 0.5 && a.GetFloat("Horizontal") > -0.5
            && a.GetFloat("Vertical") < 0.5 && a.GetFloat("Vertical") > -0.5)
            mov = false;
        else
            mov = true;
        a.SetBool("Idle", !mov);

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
        if (Input.GetKeyDown("space") && !a.GetBool("Jump") 
            && !a.GetBool("Slash") && JumpFrameCount == 0)
        {
            Debug.Log("Entered Jump control");
            a.SetBool("Jump", true);
            a.Play("Anakin Jump");
        }
        if (a.GetBool("Jump"))
        {
            JumpFrameCount++;
            Debug.Log("Counted Jump Frame: " + JumpFrameCount);
            if (Input.GetKey("left shift"))
            {
                a.SetBool("Sprint", false);
                spr = true;
            }
        }
        if (JumpFrameCount == 40)
        {
            Debug.Log("Exited Jump control");
            a.SetBool("Jump", false);
            JumpFrameCount = 0;
        }

        //idle slash control
        //triggered
        //slash all
        bool actSlash = (Input.GetKeyDown("left ctrl") && !a.GetBool("Jump") && a.GetBool("Idle"));

        if (a.GetBool("Slash A") || a.GetBool("Slash B") || a.GetBool("Slash C"))
            a.SetBool("Slash", true);
        else
            a.SetBool("Slash", false);

        //slash A
        if (actSlash && SlashAFrameCount == 0)
        {
            Debug.Log("Entering Slash A control");
            a.Play("Anakin Slash A");
            a.SetBool("Slash A", true);
        }
        if (a.GetBool("Slash A"))
        {
            Debug.Log("Counting Slash A frames: " + SlashAFrameCount);
            mov = false;
            SlashAFrameCount++;
            transform.position = transform.position;
        }
        if(SlashAFrameCount >= 40)
        {
            SlashAFrameCount = 0;
            Debug.Log("Exiting Slash A frame count");
            a.SetBool("Slash A", false);
        }

        //slash B
        if (actSlash && SlashAFrameCount >= 10 && SlashBFrameCount == 0)
        {
            Debug.Log("Entering Slash A to Slash B control");
            a.Play("Anakin Slash B");
            SlashAFrameCount = 0;
            a.SetBool("Slash A", false);
            a.SetBool("Slash B", true);
        }
        if (a.GetBool("Slash B"))
        {
            Debug.Log("Counting Slash B frames: " + SlashBFrameCount);
            mov = false;
            SlashBFrameCount++;
            transform.position = transform.position;
        }
        if (SlashBFrameCount >= 25)
        {
            SlashAFrameCount = 0;
            SlashBFrameCount = 0;
            Debug.Log("Exiting Slash B frame count");
            a.SetBool("Slash B", false);
        }

        //slash C or kick
        if (actSlash && SlashBFrameCount >= 5 && SlashCFrameCount == 0)
        {
            Debug.Log("Entering Slash B to Slash C control");
            a.Play("Anakin Slash Kick");
            SlashBFrameCount = 0;
            SlashCFrameCount = 0;
            a.SetBool("Slash A", false);
            a.SetBool("Slash B", false);
            a.SetBool("Slash C", true);
        }
        if (a.GetBool("Slash C"))
        {
            Debug.Log("Counting Slash C/Kick frames: " + SlashCFrameCount);
            mov = false;
            SlashCFrameCount++;
            transform.position = transform.position;
        }
        if (SlashCFrameCount >= 20)
        {
            SlashAFrameCount = 0;
            SlashBFrameCount = 0;
            SlashCFrameCount = 0;
            Debug.Log("Exiting Slash C frame count");
            a.SetBool("Slash C", false);
        }

        //diagonal movement control
        if ((a.GetFloat("Vertical") != 0) && (a.GetFloat("Horizontal") != 0))
            a.SetBool("Slant", true);
        else
            a.SetBool("Slant", false);

        //direction control
        if(!a.GetBool("Slash"))
        {
            if (a.GetFloat("Horizontal") < 0)
                sr.flipX = true;
            if (a.GetFloat("Horizontal") > 0)
                sr.flipX = false;
        }

        //speed control
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
        Vector3 vertical = new Vector3(0.0f, Input.GetAxis("Vertical"), 0.0f);
        if (!spr && mov && !a.GetBool("Slash"))
            transform.position += (horizontal + vertical) * Time.deltaTime * 5;
        else if (spr && !a.GetBool("Slash"))
            transform.position += (horizontal + vertical) * Time.deltaTime * 10;

    }
}
