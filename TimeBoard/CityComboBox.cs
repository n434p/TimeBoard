using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeBoard
{
    class CityComboBox: ComboBox
    {
        private int cornerRadius = 2;
        private Color gradientBottom = Color.Gray;
        private Color gradientTop = Color.DimGray;
        public Color HighlightColor { get; private set; }

        public CityComboBox()
        {
            SetStyle(ControlStyles.UserPaint, true);

            this.BackColor = Color.Gray;
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.HighlightColor = Color.DarkOrange;
            this.DrawItem += CityComboBox_DrawItem;
        }

        private void CityComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            //if (e.Index < 0)
            //    return;

            ComboBox combo = sender as ComboBox;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e.Graphics.FillRectangle(new SolidBrush(HighlightColor),
                                         e.Bounds);
            else
                e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                         e.Bounds);

            e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                  new SolidBrush(combo.ForeColor),
                                  new Point(e.Bounds.X, e.Bounds.Y));

            e.DrawFocusRectangle();
        }


        protected override void OnPaint(PaintEventArgs paintEvent)
        {
            Graphics graphics = paintEvent.Graphics;

            SolidBrush backgroundBrush = new SolidBrush(this.BackColor);
            graphics.FillRectangle(backgroundBrush, ClientRectangle);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            GraphicsPath graphicsPath = RoundedRectangle(rectangle, cornerRadius, 2);
            Brush brush = Brushes.Gray;
            graphics.FillPath(brush, graphicsPath);

            //rectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 100);
            //graphicsPath = RoundedRectangle(rectangle, cornerRadius, 2);
            //brush = new LinearGradientBrush(rectangle, gradientTop, gradientBottom, LinearGradientMode.Horizontal);
            //graphics.FillPath(brush, graphicsPath);
        }

        private GraphicsPath RoundedRectangle(Rectangle rectangle, int cornerRadius, int margin)
        {
            GraphicsPath roundedRectangle = new GraphicsPath();
            roundedRectangle.AddArc(rectangle.X + margin, rectangle.Y + margin, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRectangle.AddArc(rectangle.X + rectangle.Width - margin - cornerRadius * 2, rectangle.Y + margin, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRectangle.AddArc(rectangle.X + rectangle.Width - margin - cornerRadius * 2, rectangle.Y + rectangle.Height - margin - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRectangle.AddArc(rectangle.X + margin, rectangle.Y + rectangle.Height - margin - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRectangle.CloseFigure();
            return roundedRectangle;
        }

    }
}
