using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopdownShooter.Interactions
{
    public interface IIteraction
    {
        public bool canInteraction { get; }


        public void Interaction();
    }
}


