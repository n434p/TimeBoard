using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace TimeBoard.Clocks
{
    class DigitalClock: BaseClock
    {
        StringFormat format = GlobalSettings.StringFormat;
        public DigitalClock(Settings settings, int offset):base(settings,offset)
        {
            this.offset = offset;
            ApplyTheme();
        }

        public override void ApplyTheme()
        {
            base.ApplyTheme();
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);

            //digits
            graphics.ResetTransform();

            graphics.DrawImage(set.CurrentClock.digitBody, new Point(0, 30));

            string time, type;
            if (!GlobalSettings.AMMode)
            {
                time = GetLocalTime().ToString("H:mm:ss",System.Globalization.CultureInfo.CurrentCulture);
                type = "24H";
            }
            else
            {
                time = GetLocalTime().ToString("h:mm:ss",System.Globalization.CultureInfo.CurrentCulture);
                type = GetLocalTime().ToString("tt", System.Globalization.CultureInfo.InvariantCulture);
            }

            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            graphics.DrawString(time, set.CurrentSize.Digits, set.CurrentTheme.digits, set.CurrentSize.digitsCenter, GlobalSettings.StringFormat);
            graphics.DrawString(type, set.CurrentSize.Info, set.CurrentTheme.timeType, set.CurrentSize.timeTypeCenter, GlobalSettings.StringFormat);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

        }
    }
}
