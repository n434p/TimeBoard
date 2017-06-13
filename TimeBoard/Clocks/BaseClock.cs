using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace TimeBoard.Clocks
{
    public abstract class BaseClock
    {
        protected int offset = 0;
        internal Settings set;

        internal BaseClock(Settings settings, int offset)
        {
            this.offset = offset;
            set = settings;
            ApplyTheme();
        }

        protected DateTime GetLocalTime()
        {
            return DateTime.UtcNow.AddMilliseconds(offset);
        }

        public virtual void Draw(Graphics graphics)
        {
            graphics.ResetTransform();
        }

        public virtual void ApplyTheme()
        {

        }

        internal static BaseClock CreateClock(Settings set, ClockType type, int offset)
        {
            switch (type)
            {
                case ClockType.Analog:
                    return new AnalogClock(set,offset);
                case ClockType.Digital:
                    return new DigitalClock(set,offset);
            }
            
            return null;
        }
    }


}
