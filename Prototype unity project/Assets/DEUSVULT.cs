using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DEUSVULT : MonoBehaviour
{
    public Image DEUS;
    public AudioSource VULT;
    public GameObject CONSTANTINOPLE;
    public Button RECONQUISTA;
    public AudioSource WEWILLTAKEJERUSALEM;
    public AudioSource DEUSVULTJUMP;
    public MeshRenderer NOTCONSTANTINOPLE;
    public Material SKYCONSTANTINOPLE;
    public MeshRenderer NOTACRUSADERMESH;
    public GameObject CRUSADERHELM;

    public bool ISCRUSADING;

    // Start is called before the first frame update
    void Start()
    {
        ISCRUSADING = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (WEWILLTAKEJERUSALEM.isPlaying && WEWILLTAKEJERUSALEM.time == 6f)
        {
            WEWILLTAKEJERUSALEM.Stop();
        }

        if (FindObjectOfType<playermover>().direction == 1)
        {
            CRUSADERHELM.GetComponent<SpriteRenderer>().flipX = true;
        } else
        {
            CRUSADERHELM.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void AVEMARIA()
    {
        DEUS.gameObject.SetActive(true);
        if (!VULT.isPlaying) { VULT.Play(); }
        CONSTANTINOPLE.SetActive(true);
        RECONQUISTA.gameObject.SetActive(false);
        WEWILLTAKEJERUSALEM.time = 4f;
        WEWILLTAKEJERUSALEM.Play();
        NOTCONSTANTINOPLE.enabled = false;
        RenderSettings.skybox = SKYCONSTANTINOPLE;
        NOTACRUSADERMESH.enabled = false;
        CRUSADERHELM.SetActive(true);

        ISCRUSADING = true;
    }

    public void DEUSVAULT()
    {
        DEUSVULTJUMP.time = 0;
        DEUSVULTJUMP.Play();
    }
}
