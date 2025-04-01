using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform player;
    [SerializeField] Vector3 offcet;

    void Update()
    {
        transform.position = player.position + offcet;
    }
}