using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace VirtualGuide.Mobile.Helper
{
    public static class UIHelper
    {
        public static T FindChild<T>(this DependencyObject parent, string childName = null) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null)
                return null;

            T foundChild = null;

            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; foundChild == null && i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                // If the child is not of the request child type child
                var childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                    }
                    else
                    {
                        // Need this in case the element we want is nested
                        // in another element of the same type
                        foundChild = FindChild<T>(child, childName);
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                }
            }

            return foundChild;
        }

        public static void ScollHubToSection(HubSection section, ref Hub MainHub)
        {
            var visual = section.TransformToVisual(MainHub);
            var point = visual.TransformPoint(new Point(0, 0));
            var viewer = UIHelper.FindChild<ScrollViewer>(MainHub, "ScrollViewer");

            //sin(pi/(1080*2) * x) * 1080
            var xfactor = Math.PI / (point.X * 2);
            double move = 0;

            for (int i = 0; i < point.X; i += 5)
            {
                move = Math.Sin(xfactor * i) * point.X;
                viewer.ChangeView(move, null, null, false);
            }
            viewer.ChangeView(point.X, null, null, false);
        }
    }
}
