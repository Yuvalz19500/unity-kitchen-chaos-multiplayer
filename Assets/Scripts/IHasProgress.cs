using System;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<OnProgressChangedArgs> OnProgressChanged;
    public class OnProgressChangedArgs : EventArgs
    {
        public float ProgressNormalized;
    }
}
