using UnityEngine;

public class BoatController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;

    [Header("Turning")]
    [SerializeField] float turnSpeed;
    [SerializeField] float turnTilt;
    [SerializeField] float maxTilt;

    [Header("Waves")]
    [SerializeField] float waveForce;
    [SerializeField] float waveHeight;
    [SerializeField] float waterLevel;

    Rigidbody rb;

    void Start() => rb = GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        // Add boat forward movement with speed control
        float move = Mathf.Max(Input.GetAxis("Vertical"), 0);
        // Enable turning only when moving, with smooth torque
        float turn = move > 0 ? Input.GetAxis("Horizontal") : 0;
        rb.AddForce(transform.forward * move * speed);
        rb.AddTorque(Vector3.up * turn * turnSpeed * Time.fixedDeltaTime);

        // Implement tilt based on turning input
        float tiltFromTurn = -turn * turnTilt;

        // Add side-to-side wave sway only when stationary
        float waveTilt = move > 0 ? 0 : Mathf.Sin(Time.time) * waveForce;
        rb.AddForceAtPosition(Vector3.up * waveTilt, transform.position + transform.right);
        rb.AddForceAtPosition(Vector3.up * -waveTilt, transform.position - transform.right);

        // Clamp and smooth tilt angle for realistic leaning
        Vector3 currentRotation = transform.eulerAngles;
        float targetZ = Mathf.Clamp(tiltFromTurn + waveTilt, -maxTilt, maxTilt);
        currentRotation.z = Mathf.LerpAngle(currentRotation.z, targetZ, Time.fixedDeltaTime * 5f);

        // Simulate up-down wave motion relative to water level
        float waveUpDown = Mathf.Sin(Time.time * 1.5f) * waveHeight;
        Vector3 pos = transform.position;
        pos.y = waterLevel + waveUpDown;
        transform.position = pos;

        // Apply rotation only on Y and Z axes
        transform.eulerAngles = new Vector3(0, currentRotation.y, currentRotation.z);
    }
}