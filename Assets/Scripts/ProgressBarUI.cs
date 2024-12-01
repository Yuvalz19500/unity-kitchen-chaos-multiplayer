using System;
using Counters;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image progressBarImage;
    
    private IHasProgress _hasProgress;

    private void Start()
    {
        _hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        Debug.Assert(_hasProgress != null);
        
        _hasProgress.OnProgressChanged += HasProgressOnProgressChanged;
        
        progressBarImage.fillAmount = 0f;
        gameObject.SetActive(false);
    }

    private void HasProgressOnProgressChanged(object sender, IHasProgress.OnProgressChangedArgs e)
    {
        progressBarImage.fillAmount = e.ProgressNormalized;

        gameObject.SetActive(e.ProgressNormalized is not (0f or 1f));
    }
}
