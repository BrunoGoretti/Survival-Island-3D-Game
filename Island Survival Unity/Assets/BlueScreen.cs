using UnityEngine;
using UnityEngine.UI;

public class BlueScreen : MonoBehaviour
{
    public Canvas blueUI;
    public Image blueImage; 
    public Transform player;

    void Start()
    {
        if (blueImage != null)
        {
            blueImage.enabled = false;
        }
    }

    void Update()
    {
        if (player != null)
        {
            if (player.position.y < 10.2f)
            {
                if (blueImage != null)
                {
                    blueImage.enabled = true;
                }
            }
            else
            {
                if (blueImage != null)
                {
                    blueImage.enabled = false;
                }
            }
        }
    }
}