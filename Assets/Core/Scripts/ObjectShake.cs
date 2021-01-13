using UnityEngine;
using System.Collections;

public class ObjectShake : MonoBehaviour
{

	private Vector3 originPosition;
	private Quaternion originRotation;
	public float shake_intensity = .3f;

	private float temp_shake_intensity = 0;

	void Update()
	{
		if(temp_shake_intensity > 0) transform.position = originPosition + Random.insideUnitSphere * temp_shake_intensity;
	}

	public void Shake()
	{
		originPosition = transform.position;
		originRotation = transform.rotation;
		temp_shake_intensity = shake_intensity;

	}

	public void StopShake()
    {
		temp_shake_intensity = 0;
	}

	public void TempShake()
    {
		StartCoroutine(TempShakeCo());
    }

	private IEnumerator TempShakeCo()
    {
		originPosition = transform.position;
		originRotation = transform.rotation;
		temp_shake_intensity = shake_intensity;
		yield return new WaitForSeconds(1f);
		temp_shake_intensity = 0;
	}
}