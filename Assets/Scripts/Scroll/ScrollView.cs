using System;
using System.Collections.Generic;
using UnityEngine;
using FancyScrollView;
using EasingCore;

namespace ICTLauncher.Scroll
{
    class ScrollView : FancyScrollView<Description, Context>
    {
        [SerializeField] private Scroller scroller;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Ease scrollEasing;

        public Action<int> OnSelectionChanged { get; set; }

        protected override GameObject CellPrefab => cellPrefab;

        protected override void Initialize()
        {
            base.Initialize();

            Context.onCellClicked = (index) => SelectCell(index);

            scroller.OnValueChanged(UpdatePosition);
            scroller.OnSelectionChanged(UpdateSelection);
            
        }

        private void UpdateSelection(int index)
        {
            if (Context.selectedIndex == index) return;

            Context.selectedIndex = index;
            Refresh();

            OnSelectionChanged?.Invoke(index);
        }

        public new ScrollView UpdateContents(IList<Description> items) {
            base.UpdateContents(items);
            scroller.SetTotalCount(items.Count);

            return this;
        }

        public ScrollView SelectCell(int index) {
            if (index < 0 || index >= ItemsSource.Count || index == Context.selectedIndex) return this;

            UpdateSelection(index);
            scroller.ScrollTo(index, 0.35f, scrollEasing);
            OnSelectionChanged?.Invoke(index);
            return this;
        }
    }
}