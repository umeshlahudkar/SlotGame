using System.Collections;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private Transform ray;
    [SerializeField] private float rayRotationSpeed;

    [SerializeField] private Transform star;
    [SerializeField] private float starScalingTime;

    private float rayCurrentAngle;

    private void OnEnable()
    {
        rayCurrentAngle = 0;
        star.localScale = Vector3.zero;
    }

    private void Start()
    {
        StartCoroutine(StarScalingAnim());
    }

    private void Update()
    {
        RotateRays();
    }

    private void RotateRays()
    {
        rayCurrentAngle += Time.deltaTime * rayRotationSpeed;
        if (rayCurrentAngle >= 360) { rayCurrentAngle = 0; }
        ray.localEulerAngles = Vector3.forward * rayCurrentAngle;
    }

    private IEnumerator StarScalingAnim()
    {
        float elapcedTime = 0;

        while (elapcedTime < starScalingTime)
        {
            elapcedTime += Time.deltaTime;
            star.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapcedTime / starScalingTime);

            yield return null;
        }
        star.localScale = Vector3.one;
    }

    public void OnScreenClick()
    {
        gameObject.SetActive(false);
        UIController.Instance.ToggleStartButton(true);
    }

}
