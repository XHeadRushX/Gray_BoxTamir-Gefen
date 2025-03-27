using UnityEngine;

public class ThrowObjects : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public Transform player;
    public Animator animator;
    public CharacterController controller; // Reference to disable movement

    [Header("Settings")]
    public int totalThrows = 5;
    public float throwCooldown = 1f;
    public float throwForce = 20f;
    public float throwUpwardForce = 5f;
    public KeyCode throwKey = KeyCode.Mouse0;

    private bool readyToThrow = true;
    private bool isThrowing = false;

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;
        isThrowing = true;

        // Rotate player to match camera direction
        AlignWithCamera();

        // Disable movement
        if (controller != null)
            controller.enabled = false;

        // Play throw animation
        if (animator != null)
            animator.SetTrigger("Throw");

        // Instantiate and throw the object
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, Quaternion.identity);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.forward; // Default to camera forward direction

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        Vector3 forceToAdd = forceDirection * throwForce + Vector3.up * throwUpwardForce;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        // Invoke reset functions
        Invoke(nameof(ResetThrow), throwCooldown);
        Invoke(nameof(ResetMovement), throwCooldown);
    }

    private void AlignWithCamera()
    {
        Vector3 cameraForward = cam.forward;
        cameraForward.y = 0; // Keep rotation horizontal
        player.rotation = Quaternion.LookRotation(cameraForward);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
        isThrowing = false;
    }

    private void ResetMovement()
    {
        if (controller != null)
            controller.enabled = true;
    }
}
