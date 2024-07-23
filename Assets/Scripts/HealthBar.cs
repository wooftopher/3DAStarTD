using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private Camera mycamera;
    // [SerializeField] private Transform target;
    // [SerializeField] private Vector3 offset;

    public void UpdateHealthBar(float currentValue, float maxValue){
        slider.value = currentValue / maxValue;
    }

    void Update(){
        transform.rotation = mycamera.transform.rotation;
        // transform.position = target.position + offset;
    }
}
