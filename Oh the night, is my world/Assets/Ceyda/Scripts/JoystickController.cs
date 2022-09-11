using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour
{
    [SerializeField] private Image outerCircle;
    [SerializeField] private Image innerCircle;
    
    public UnityAction<Vector3> OnJoystick;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            outerCircle.gameObject.SetActive(true);
            outerCircle.transform.position = Input.mousePosition;
        }
        
        if (Input.GetMouseButton(0))
        {
            innerCircle.transform.position = Input.mousePosition;
            Vector3 difference = innerCircle.transform.position - outerCircle.transform.position;
            difference.x /= Screen.width * 0.5f;
            difference.x = Mathf.Clamp(difference.x, -1f, 1f);
            if(difference.y >= 0f)
                difference.y = 1f;
            
            OnJoystick?.Invoke(difference);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            outerCircle.gameObject.SetActive(false);
        }
    }
}
