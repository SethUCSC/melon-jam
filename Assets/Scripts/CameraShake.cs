using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public AnimationCurve curve;
    public PauseScript pause;
    public float duration = 1f;
    public IEnumerator Shake()
    {
        Vector3 originalPos = transform.position;

        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            originalPos = transform.position;
            elapsedTime += Time.deltaTime;
            while(pause.paused == true){ // Pause Camera Shake when game is Paused. 
                yield return new WaitForSeconds(0.1f);
            }
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = originalPos + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = originalPos;
    }

}
