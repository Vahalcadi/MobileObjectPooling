using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private PlayerControls controls;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;

        controls = new PlayerControls();
    }

    public bool Shoot()
    {
        return controls.Player.Shoot.triggered;
    }

    public Vector3 Move()
    {
        Vector3 vector = controls.Player.Movement.ReadValue<Vector3>();
        vector = Quaternion.Euler(-90, 0, 0) * vector;

        return vector;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
