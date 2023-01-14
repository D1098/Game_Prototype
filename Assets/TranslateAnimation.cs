using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class TranslateAnimation
    {
        public Animator transition;
        public TranslateAnimation()
        {
            transition = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("TransitionAnim")).GetComponentInChildren<Animator>();
        }

    }
}
