﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Neptuo.Windows.Controls
{
    public static class ControlHelper
    {
        public static TObject FindVisualParent<TObject>(UIElement child) where TObject : UIElement
        {
            if (child == null)
            {
                return null;
            }

            UIElement parent = VisualTreeHelper.GetParent(child) as UIElement;
            while (parent != null)
            {
                TObject found = parent as TObject;
                if (found != null)
                    return found;
                else
                    parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }
    }
}
