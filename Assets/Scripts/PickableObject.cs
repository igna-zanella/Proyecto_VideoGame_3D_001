using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public bool isPicked = false;

    private Renderer[] renderers;
    private AudioSource audioSrc;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        audioSrc = GetComponent<AudioSource>();
    }

    public void Pick()
    {
        isPicked = true;


        // Ocultar objeto
        foreach (Renderer r in renderers)
        { 
            r.enabled = false;
        }
        // Apagar sonido si tiene
        if (audioSrc != null)
        {   
            audioSrc.Stop();
         }

        GetComponent<Collider>().enabled = false;
        

    }
    public void Drop(Vector3 dropPosition)
    {
        isPicked = false;

        // Mostrar objeto
        foreach (Renderer r in renderers)
            r.enabled = true;

        // Reproducir sonido nuevamente si corresponde
        if (audioSrc != null)
            audioSrc.Play();

        transform.position = dropPosition;
        GetComponent<Collider>().enabled = true;
    }
}
