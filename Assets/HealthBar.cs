using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Code taken from youtube video
 * "Health Bars in Unity3D - Quick, Clean & Easy"
 * Ref: https://www.youtube.com/watch?time_continue=7&v=CA2snUe7ARM&feature=emb_title
 */

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image healthImage;
    [SerializeField]
    private float updateSpeedSeconds = 0.5f;

    private void Awake()
    {
        GetComponentInParent<Health>().OnHealthPctChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct)); //add smoothing when reducing health bar
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = healthImage.fillAmount;
        float elapsed = 0f;

        while(elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            healthImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;

        }

        healthImage.fillAmount = pct;
    }

    // Update is called once per frame
    //allows the UI elements to be facing the correct direction
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
