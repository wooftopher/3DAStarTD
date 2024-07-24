using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private Camera myCamera;
    // [SerializeField] private Transform target;
    // [SerializeField] private Vector3 offset;

    public void UpdateHealthBar(float currentValue, float maxValue){
        slider.value = currentValue / maxValue;
    }

    void Update(){
        // transform.rotation = myCamera.transform.rotation;
        transform.LookAt(transform.position + myCamera.transform.rotation * Vector3.forward,
                         myCamera.transform.rotation * Vector3.up);
        // transform.position = target.position + offset;
    }
}
