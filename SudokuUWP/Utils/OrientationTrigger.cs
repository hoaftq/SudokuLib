// UWP Sodoku Game
// Write by Trac Quang Hoa, 03/2019

using System.Collections.Generic;
using Windows.Graphics.Display;
using Windows.UI.Xaml;

namespace SudokuUWP.Utils
{
    public class OrientationTrigger : StateTriggerBase
    {
        private DisplayInformation displayInfo;

        public List<string> Orientations
        {
            get { return (List<string>)GetValue(OrientationsProperty); }
            set { SetValue(OrientationsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientations.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationsProperty =
            DependencyProperty.Register("Orientations", typeof(List<string>), typeof(OrientationTrigger), new PropertyMetadata(new List<string>() { DisplayOrientations.Portrait.ToString() }));

        public OrientationTrigger()
        {
            displayInfo = DisplayInformation.GetForCurrentView();
            displayInfo.OrientationChanged += DisplayInfo_OrientationChanged;
            UpdateTrigger(displayInfo);
        }

        private void DisplayInfo_OrientationChanged(DisplayInformation sender, object args)
        {
            UpdateTrigger(sender);
        }

        private void UpdateTrigger(DisplayInformation sender)
        {
            SetActive(Orientations.Contains(sender.CurrentOrientation.ToString()));
        }
    }
}
