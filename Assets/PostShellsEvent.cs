using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PostShellsEvent : MonoBehaviour
{
    [SerializeField] GameObject Shells;
    [SerializeField] GameObject SpawnableShell;
    [SerializeField] GameObject ReloadPoof;

    public void DropShells()
    {
        // turn on gravity for all shells
        int maxIterations = Shells.transform.childCount;
        for (int i = 0; i < maxIterations; i++)
        {
            // instantiate physics-based shells in cylinder
            GameObject Shell = Instantiate(SpawnableShell, Shells.transform.GetChild(i).transform.position, Shells.transform.GetChild(i).transform.rotation);
            Shell.GetComponent<Rigidbody>().useGravity = true;

            // hide unmoving shells in cylinder
            Shells.transform.GetChild(i).gameObject.GetComponent<Renderer>().enabled = false;

            // destroy new shells after 10 seconds
            Destroy(Shell, 10.0f);
        }
    }

    public void LoadShells()
    {
        ReloadPoof.GetComponent<VisualEffect>().Play();

        // render shells
        for (int i = 0; i < Shells.transform.childCount; i++)
        {
            Shells.transform.GetChild(i).gameObject.GetComponent<Renderer>().enabled = true;
        }
    }
}
