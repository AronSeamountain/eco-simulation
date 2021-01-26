using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private int saturation;

    public int Saturation => saturation;

    public void Consume()
    {
        Destroy(gameObject);
    }
}
