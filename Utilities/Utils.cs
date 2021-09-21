using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ViewSample.Utilities
{
    class Utils
    {
        ///<summary>
        ///Generic method to get whatever kind of panel
        ///<param name="element">the element to find the parent or the child from</param>
        ///<param name="asChild"> true to find the panel as a child of the element false, to find it as a parent</param>
        ///</summary>
        public static T getPanel<T>(UIElement element, bool asChild) where T : Panel
        {
            T panel;
            DependencyObject dep = element;

            if (asChild)
            {
                while (!(dep == null) && !(dep is T))
                {
                    dep = VisualTreeHelper.GetChild(dep, 0);
                }
                panel = (T)dep;
            }
            else
            {
                while (!(dep == null) && !(dep is T))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }
                panel = (T)dep;
            }

            return panel;
        }


        ///<summary>
        ///This helper method returns a Visual by iterating over all the children of the parent
        ///If its no the type being looked for, recursively travel further down the visual tree
        ///</summary>
        public static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
