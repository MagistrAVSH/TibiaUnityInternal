﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Windows.Forms;
using DarkUI.Config;
using DarkUI.Icons;

namespace DarkUI.Controls
{
    public class DarkNumericUpDown : NumericUpDown
    {
        private bool mouseDown = false;
        private Point mousePos = new Point();
        private Rectangle scrollButtons;
        public DarkNumericUpDown()
        {
            this.ForeColor = Color.Gainsboro;
            this.BackColor = Color.FromArgb(69, 73, 74);
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                   ControlStyles.ResizeRedraw |
                   ControlStyles.UserPaint, true);
            this.Controls[0].Paint += DarkNumericUpDown_Paint;
            this.MouseMove += DarkNumericUpDown_MouseMove;
            this.MouseUp += DarkNumericUpDown_MouseUp;
            this.MouseDown += DarkNumericUpDown_MouseDown;
            try
            {
                // Prevent flickering, only if our assembly 
                // has reflection permission. 
                Type type = this.Controls[0].GetType();
                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
                MethodInfo method = type.GetMethod("SetStyle", flags);

                if (method != null)
                {
                    object[] param = { ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true };
                    method.Invoke(this.Controls[0], param);
                }
            }
            catch (SecurityException)
            {
                // Don't do anything, we are running in a trusted contex.
            }
        }

        private void DarkNumericUpDown_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
        }

        private void DarkNumericUpDown_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void DarkNumericUpDown_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos = new Point(e.Location.X - (this.Width - scrollButtons.Width),e.Location.Y);
        }

        private void DarkNumericUpDown_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Colors.DarkBackground), new Rectangle(0, 0, e.ClipRectangle.Width,e.ClipRectangle.Height));

            var upIcon = ScrollIcons.scrollbar_arrow_standard;
            var upRect = new Rectangle(e.ClipRectangle.Width/2 - upIcon.Width/2,e.ClipRectangle.Height/4 - upIcon.Height/2,upIcon.Width,upIcon.Height);
            upIcon = upRect.Contains(mousePos) ? ScrollIcons.scrollbar_arrow_hot : ScrollIcons.scrollbar_arrow_standard;

            if (mouseDown && upRect.Contains(mousePos))
                upIcon = ScrollIcons.scrollbar_arrow_clicked;

            upIcon.RotateFlip(RotateFlipType.RotateNoneFlipY);


            e.Graphics.DrawImageUnscaled(upIcon,upRect);

            // Down arrow
            var downIcon = ScrollIcons.scrollbar_arrow_standard;
            var downRect = new Rectangle(e.ClipRectangle.Width / 2 - upIcon.Width / 2, e.ClipRectangle.Height / 2 + e.ClipRectangle.Height/4 - upIcon.Height / 2, upIcon.Width, upIcon.Height);
            downIcon = downRect.Contains(mousePos) ? ScrollIcons.scrollbar_arrow_hot : ScrollIcons.scrollbar_arrow_standard;

            if (mouseDown && downRect.Contains(mousePos))
                downIcon = ScrollIcons.scrollbar_arrow_clicked;

            e.Graphics.DrawImageUnscaled(downIcon, downRect);
            scrollButtons = e.ClipRectangle;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.FromArgb(100,100,100), ButtonBorderStyle.Solid);
        }
    }
}
