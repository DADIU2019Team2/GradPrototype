using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermover : MonoBehaviour
{
    public Rigidbody rb;

    [Range(-1, 1)]
    public int direction;
    [Range(0, 50)]
    public float moveForce;
    public LineRenderer liney;
    public float timer1 = 0;
    public float timer2 = 0;

    public Transform rayObj;
    public bool grounded;
    public LayerMask layerMask;
    public bool stuckToWall = false;

    Vector3 mousePosStart, mousePosEnd;
    public bool dragStarted;

    Vector3 debugPoint0, debugPoint1;
    Vector3 dragPosEnd;

    public Transform cube, cubeTouchPos, cubePlayerDir;

    Vector3 touchPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 15f));
            touchPosition.z = 0f;
            cubeTouchPos.position = touchPosition;

        } else if (GetComponent<MeshRenderer>().material.color != Color.red)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        }

        if (stuckToWall)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
        }

        if (Input.touchCount > 0 && dragStarted)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 15f));
            touchPosition.z = 0f;
            cubeTouchPos.position = touchPosition;
            debugPoint1 = touchPosition;

            cube.position = touchPosition;
        }

        if (Input.touchCount > 0 && !dragStarted)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 15f));
            touchPosition.z = 0f;
            debugPoint0 = touchPosition;

            dragStarted = true;
            timer1 = 0;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && dragStarted)
        {
            Vector3 mouseDirXY = new Vector3((debugPoint1 - debugPoint0).x, (debugPoint1 - debugPoint0).y, 0);
            if ((debugPoint1 - debugPoint0).magnitude > 8)
            {
               
                Vector3 mouseDirXYReal = mouseDirXY.normalized;
                rb.AddForce(mouseDirXYReal * 8, ForceMode.Impulse);
            }
            if ((debugPoint1 - debugPoint0).magnitude <= 8)
            {
                mouseDirXY = new Vector3((debugPoint1 - debugPoint0).x, (debugPoint1 - debugPoint0).y, 0);
                rb.AddForce(mouseDirXY, ForceMode.Impulse);
            }

            dragStarted = false;
            timer1 = 0;

            direction = mouseDirXY.normalized.x >= 0 ? 1 : -1;
            if (FindObjectOfType<DEUSVULT>().ISCRUSADING)
            {
                FindObjectOfType<DEUSVULT>().DEUSVAULT();
            }


        } else if (timer1 > 0.5f && dragStarted && Input.touchCount > 0)
        {
            dragStarted = false;
            timer1 = 0;

            Vector3 mouseDirXY = new Vector3((debugPoint1 - debugPoint0).x, (debugPoint1 - debugPoint0).y, 0);
            if ((debugPoint1 - debugPoint0).magnitude > 10)
            {
                Vector3 mouseDirXYReal = mouseDirXY.normalized;
                rb.AddForce(mouseDirXYReal * 10, ForceMode.Impulse);
            }

            if ((debugPoint1 - debugPoint0).magnitude <= 10)
            {
                rb.AddForce(mouseDirXY, ForceMode.Impulse);
            }

            direction = mouseDirXY.normalized.x >= 0 ? 1 : -1;
            if (FindObjectOfType<DEUSVULT>().ISCRUSADING)
            {
                FindObjectOfType<DEUSVULT>().DEUSVAULT();
            }
        }


        //Move the player if not stuck to wall:
        if (!stuckToWall && grounded)
        {
            rb.AddForce(new Vector3(direction * moveForce, 0, 0), ForceMode.Force);
            cubePlayerDir.localPosition = new Vector3(direction, 0, 0);
        }

        if (stuckToWall && rb.velocity.magnitude > 1f)
        {
            stuckToWall = false;
            rb.useGravity = true;
        }

        #region groundedCheck
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, layerMask))
        {
            grounded = hit.distance > 1 ? false : true;
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.blue);
        }
        #endregion groundedCheck

        if (dragStarted)
        {
            timer1 += Time.deltaTime;
        }

        Debug.DrawRay(mousePosStart, mousePosEnd, Color.cyan);

        //Draw debuglines, hopefully
        #region linedrawing
        liney.SetPosition(0, debugPoint0);
        liney.SetPosition(1, debugPoint1);
        #endregion linedrawing
    }

    private void jumpDirection()
    {
        timer1 = 0;
        dragStarted = false;
        Ray newRay;
        RaycastHit hit;
        newRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(newRay, out hit))
        {
            //hit.collider.GetComponent<Renderer>().material.color = Color.red;
            mousePosEnd = hit.point;
            //Debug.Log(hit);
        };
        Vector3 mouseDirXY = new Vector3((mousePosEnd - mousePosStart).x, (mousePosEnd - mousePosStart).y, 0);
        Vector3 mouseDirXYReal = mouseDirXY.normalized;
        rb.AddForce(mouseDirXYReal * 10, ForceMode.Impulse);
        //Do stuff 
        Debug.Log(mouseDirXY);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("swapDirection"))
        {
            direction = direction == 1 ? -1 : 1;
        }
        if (other.CompareTag("sticky") && !grounded)
        {
            rb.velocity = Vector3.zero;
            stuckToWall = true;
            rb.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("sticky"))
        {
            stuckToWall = false;
            rb.useGravity = true;
        }
    }
}
