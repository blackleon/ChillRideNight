using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class car : MonoBehaviour
{
    private float speed;
    private float steerValue;
    private bool joystickMoved;
    private Vector3 lastDir;

    [SerializeField] private JoystickController joystick;
    [SerializeField] private List<WheelCollider> wheels;
    [SerializeField] private List<Transform> wheelMeshes;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxRotate;
    [SerializeField] private Rigidbody carRig;
    [SerializeField] private float carSpeedLimit;
    [SerializeField] private List<Light> backLights;

    private void OnEnable()
    {
        joystick.OnJoystick += OnDirection;
    }

    private void OnDisable()
    {
        joystick.OnJoystick -= OnDirection;
    }

    private void OnDirection(Vector3 dir)
    {
        lastDir = dir;
        steerValue = dir.x * maxRotate;
        speed = Mathf.Clamp(speed + dir.y, -maxSpeed * 0.5f, maxSpeed);
        joystickMoved = true;
    }


    // Start is called before the first frame update
//yeah.. let's see...
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = steerValue;

            Vector3 pos;
            Quaternion quat;
            wheels[i].GetWorldPose(out pos, out quat);
            wheelMeshes[i].rotation = quat;
        }

        for (int i = 2; i < wheels.Count; i++)
        {
            wheels[i].motorTorque = carRig.velocity.magnitude < carSpeedLimit ? speed : 0;
            Vector3 pos;
            Quaternion quat;
            wheels[i].GetWorldPose(out pos, out quat);
            wheelMeshes[i].rotation = quat;
        }

        foreach (var backLight in backLights)
        {
            backLight.range = Vector3.Angle(carRig.velocity, -transform.forward) < 90f || lastDir.y < 0f ? 1f : 0f;
        }

        if (joystickMoved)
        {
            joystickMoved = false;
        }
        else
        {
            steerValue = 0f;
            speed = 0f;
            lastDir = Vector3.zero;
        }
    }

    public void ResetCar()
    {
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene(0);
        }
    }
}