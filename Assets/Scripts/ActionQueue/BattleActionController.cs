using UnityEngine;
using System.Collections.Generic;
using System;

using Match3Wrapper;
using Utils;

namespace ActionQueue
{
    [RequireComponent(typeof(Animator))]
    public class BattleActionController : MonoBehaviour
    {
        public delegate void SimpleEventDelegate();

        [SerializeField]
        private Animator animator;
        [SerializeField]
        private MovingComponent movingComponent;
        [SerializeField]
        private GemSpriteSwitcher gemSpriteSwitcher;
        [SerializeField]
        private GemPoint[] gemPoints = null;
        public GemPoint[] GemPoints
        {
            get { return gemPoints; }
            set
            {
                gemPoints = value;
                SetActionPoints(gemPoints != null ? gemPoints.Length : 0);
                gemSpriteSwitcher.SetGemType(gemPoints != null && gemPoints.Length > 0 ? gemPoints[0].type : Match3Core.GemType.None);
            }
        }

        public void SetPosition(int index)
        {
            movingComponent.MoveTo(new Vector3(index, 0.0f, 0.0f), true);
        }

        private void SetActionPoints(int count)
        {

        }

        private SimpleEventDelegate appearOverCallback;
        public void Appear(SimpleEventDelegate appearOverCallback)
        {
            this.appearOverCallback = appearOverCallback;
            animator.SetTrigger("appear");
        }

        private SimpleEventDelegate dissappearOverCallback;
        public void Disappear(SimpleEventDelegate dissappearOverCallback)
        {
            this.dissappearOverCallback = dissappearOverCallback;
            animator.SetTrigger("fadeout");
        }

        private void OnAppearOver()
        {
            if (null != appearOverCallback)
            {
                appearOverCallback();
                appearOverCallback = null;
            }
        }

        private void OnFadeoutOver()
        {
            if (null != dissappearOverCallback)
            {
                dissappearOverCallback();
                dissappearOverCallback = null;
            }
            Destroy(gameObject);
        }
    }
}