using UnityEngine;
using UnityEngine.UI;
using FancyScrollView;

namespace ICTLauncher.Scroll
{
    class Cell : FancyCell<Description, Context>
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Image icon;

        private float currentPosition = 0f;
        
        private static class AnimatorHash
        {
            public static readonly int CellScroll = Animator.StringToHash("CellScroll");
        }

        void OnEnable() => UpdatePosition(currentPosition);

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void UpdateContent(Description content)
        {
            if (Context.selectedIndex == Index) {
                // Debug.Log(Index);
            }
            icon.sprite = content.iconData;
        }

        public override void UpdatePosition(float position)
        {
            currentPosition = position;
            if (animator.isActiveAndEnabled) animator.Play(AnimatorHash.CellScroll, -1, position);
            animator.speed = 0;
        }
    }
}