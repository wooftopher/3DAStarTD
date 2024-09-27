using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private Camera myCamera;

    void Start() {
        myCamera = Camera.main;
    }

    public void UpdateHealthBar(float currentValue, float maxValue) {
        slider.value = currentValue / maxValue;
    }

    void Update() {
        // Make the health bar face the camera, but align its up direction with the camera's up direction
        Vector3 direction = myCamera.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(-direction, myCamera.transform.up);
    }
}
