using KaitosObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ViewLayer.Extensions
{
    public static class WindowExtension
    {
        public static WindowStatsDTO GetSizeAndPosition(this Window window)
        {
            double h = window.Height;
            double w = window.Width;
            double t = window.Top;
            double l = window.Left;

            return new WindowStatsDTO() { Height = h,Width = w,Top = t,Left = l};
        }
    }
}
