using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace VirtualGuide.Mobile.Common
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
    }
}
