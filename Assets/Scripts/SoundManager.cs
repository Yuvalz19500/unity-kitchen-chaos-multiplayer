using System;
using Counters;
using ScriptableObjects;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    [SerializeField] AudioClipRefsSO audioClipRefsSO;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        DeliveryManager.Instance.OnOrderDelivered += DeliveryManagerOnOrderDelivered;
        DeliveryManager.Instance.OnOrderFailed += DeliveryManagerOnOrderFailed;
        CuttingCounter.OnAnyCut += CuttingCounterOnAnyCut;
        Player.Instance.OnPickSomething += PlayerOnPickSomething;
        BaseCounter.OnAnyObjectPlaced += BaseCounterOnAnyObjectPlaced;
        TrashCounter.OnAnyObjectTrashed += TrashCounterOnAnyObjectTrashed;
    }

    private void TrashCounterOnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter thrashCounter = sender as TrashCounter;
        Debug.Assert(thrashCounter != null, nameof(thrashCounter) + " != null");
        PlaySound(audioClipRefsSO.trash, thrashCounter.transform.position);
    }

    private void BaseCounterOnAnyObjectPlaced(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        Debug.Assert(baseCounter != null, nameof(baseCounter) + " != null");
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void PlayerOnPickSomething(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounterOnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        Debug.Assert(cuttingCounter != null, nameof(cuttingCounter) + " != null");
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManagerOnOrderFailed(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManagerOnOrderDelivered(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    
    private void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClips[Random.Range(0, audioClips.Length)], position, volume);
    }
    
    public void PlayFootstepSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.footstep, position);
    }
}
