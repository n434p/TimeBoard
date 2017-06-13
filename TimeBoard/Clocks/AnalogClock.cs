using System.Drawing;
using System.Drawing.Drawing2D;

namespace TimeBoard.Clocks
{
    class AnalogClock: BaseClock
    {
        public AnalogClock(Settings settings, int offset) : base(settings, offset)
        {
            this.offset = offset;
            ApplyTheme();
        }

        float GetClockHandAngle(ClockHandType type)
        {
            var time = GetLocalTime();
            switch (type)
            {
                case ClockHandType.Hour:
                    return time.Hour * 30 + 0.5f * time.Minute;
                case ClockHandType.Minute:
                    return time.Minute * 6;
                case ClockHandType.Second:
                    return time.Second * 6;
            }
            return 0;
        }

        public override void ApplyTheme()
        {
            base.ApplyTheme();
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);

            //body
            graphics.ResetTransform();
            graphics.DrawImage(set.CurrentClock.analogBody, new Point(0, 30));

            //hours

            graphics.ResetTransform();
            graphics.TranslateTransform(set.CurrentSize.pivot.X, set.CurrentSize.pivot.Y);
            graphics.DrawImage(set.CurrentClock.saddle, set.CurrentSize.saddleRect);
            graphics.RotateTransform(GetClockHandAngle(ClockHandType.Hour));
            graphics.DrawImage(set.CurrentClock.hoursHand, set.CurrentSize.hoursHandRect);


            ////minutes
            graphics.ResetTransform();
            graphics.TranslateTransform(set.CurrentSize.pivot.X, set.CurrentSize.pivot.Y);
            graphics.RotateTransform(GetClockHandAngle(ClockHandType.Minute));
            graphics.DrawImage(set.CurrentClock.minutesHand, set.CurrentSize.minutesHandRect);

            ////seconds
            graphics.ResetTransform();
            graphics.TranslateTransform(set.CurrentSize.pivot.X, set.CurrentSize.pivot.Y);
            graphics.RotateTransform(GetClockHandAngle(ClockHandType.Second));
            graphics.DrawImage(set.CurrentClock.secondsHand, set.CurrentSize.secondsHandRect);

        }
    }

    #region Utils

    enum ClockHandType { Hour, Minute, Second };
    
    #endregion
}
