using UnityEngine;

public enum MoverType
{
    Enemy,
    Food,
    Gold
}

public class Mover : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * GameManager.instance.CalculateGameSpeed() * Time.deltaTime;
    }
}
