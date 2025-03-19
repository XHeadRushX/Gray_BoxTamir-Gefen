using UnityEngine;

public class ThirdPersonThrow : MonoBehaviour
{
    [Header("References")]
    public Transform cam;               // The main camera or follow camera
    public Transform throwAttackPoint;  // The point from which the object is thrown
    public GameObject objectToThrow;    // The prefab of the object to throw

    [Header("Settings")]
    public int totalThrows = 5;         // Total number of throwable objects
    public float throwCooldown = 0.5f;  // Cooldown between throws
    public float throwForce = 20f;      // Forward force applied to the thrown object
    public float throwUpwardForce = 5f; // Upward force for a throwing arc
    public KeyCode throwKey = KeyCode.Mouse0;

    private bool readyToThrow = true;

    private void Start()
    {
        readyToThrow = true;
    }

    private void Update()
    {
        // Check for throw input and cooldown
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            ThrowObject();
        }
    }

    private void ThrowObject()
    {
        readyToThrow = false;

        // Create the throwable object at the attack point
        GameObject projectile = Instantiate(objectToThrow, throwAttackPoint.position, throwAttackPoint.rotation);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // Calculate throw direction based on camera forward and attack point
        Vector3 throwDirection = cam.forward;
        RaycastHit hit;

        int layerMask = ~LayerMask.GetMask("Player"); // Invert mask to ignore "Player" layer
        if (Physics.Raycast(cam.position, cam.forward, out hit, 100f, layerMask))
        {
            throwDirection = (hit.point - throwAttackPoint.position).normalized;
        }


        // Combine forward and upward forces for a throwing arc
        Vector3 forceToAdd = throwDirection * throwForce + Vector3.up * throwUpwardForce;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
