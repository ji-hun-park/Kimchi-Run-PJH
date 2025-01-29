using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How fast should the texture scroll?")]
    public float ScrollSpeed;
    
    [Header("References")]
    public MeshRenderer meshRenderer;

    // Update is called once per frame
    void Update()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(ScrollSpeed * GameManager.instance.CalculateGameSpeed() / 10 * Time.deltaTime, 0);
    }
}
