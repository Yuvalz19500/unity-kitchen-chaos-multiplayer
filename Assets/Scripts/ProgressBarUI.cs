using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image progressBarImage;

    private void Start()
    {
        cuttingCounter.OnCuttingProgressChanged += CuttingCounterOnCuttingProgressChanged;
        
        progressBarImage.fillAmount = 0f;
        gameObject.SetActive(false);
    }

    private void CuttingCounterOnCuttingProgressChanged(object sender, CuttingCounter.OnCuttingProgressChangedArgs e)
    {
        progressBarImage.fillAmount = e.ProgressNormalized;

        gameObject.SetActive(e.ProgressNormalized is not (0f or 1f));
    }
}
