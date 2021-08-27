using UnityEngine;

public class Diamond : MonoBehaviour
{
    void Update()
    {
        // Rotate the diamond
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * 30f);
    }

    // Make the diamond visible, disable collider and play audio
    public void DiamondFound()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = true;
    }
}
