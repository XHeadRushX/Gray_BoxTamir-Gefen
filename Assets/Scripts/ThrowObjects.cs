using UnityEngine;

public class ThirdPersonThrow : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform throwAttackPoint;
    public GameObject objectToThrow;
    public Animator animator;  // Reference to Animator

    [Header("Settings")]
    public int totalThrows = 5;
    public float throwCooldown = 0.5f;
    public float throwForce = 20f;
    public float throwUpwardForce = 5f;
    public KeyCode throwKey = KeyCode.Mouse0;

    private bool readyToThrow = true;

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

        // Play throwing animation with the new trigger name
        animator.SetTrigger("Frisbee Throw");

        // Create the throwable object
        GameObject projectile = Instantiate(objectToThrow, throwAttackPoint.position, throwAttackPoint.rotation);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // Calculate throw direction
        Vector3 throwDirection = cam.forward;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 100f))
        {
            throwDirection = (hit.point - throwAttackPoint.position).normalized;
        }

        // Apply force
        Vector3 forceToAdd = throwDirection * throwForce + Vector3.up * throwUpwardForce;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
        animator.ResetTrigger("Frisbee Throw");
        animator.SetTrigger("Idle");
    }

}

