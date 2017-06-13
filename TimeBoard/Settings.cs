using PTLRuntime.NETScript;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.Serialization;
using TimeBoard.Properties;

namespace TimeBoard
{
    [DataContract]
    class Settings
    {
        [DataMember]
        public List<string> citiesList = new List<string>() { "Dnipro Dnipropetrovsk Oblast Ukraine", "Kyiv Ukraine" };
        [DataMember]
        public ClockTheme clockTheme = ClockTheme.Dark;
        [DataMember]
        public ClockScale clockScale = ClockScale.Middle;
        [DataMember]
        public ClockType clockType = ClockType.Analog;

        public bool isEditingMode = false;

        public Theme CurrentTheme
        {
            get { return GlobalSettings.ClocksThemes[clockTheme]; }
        }
        public ClockSize CurrentSize
        {
            get { return GlobalSettings.ClocksSizes[clockScale]; }
        }
        public ClocksType CurrentType
        {
            get { return CurrentTheme.types[clockType]; }
        }
        public Rectangle CloseCross
        {
            get { return CurrentSize.closeCrosses[clockType]; }
        }
        public ClockBitmaps CurrentClock
        {
            get { return CurrentTheme.clocks[clockScale]; }
        }
    }

    class GlobalSettings
    {
        public static bool AMMode
        {
            get { return amMode; }
        }
        static bool amMode;

        public static Dictionary<ClockTheme, Theme> ClocksThemes { get { return clocksThemes; } }
        static Dictionary<ClockTheme, Theme> clocksThemes = new Dictionary<ClockTheme, Theme>();

        public static Dictionary<ClockScale, ClockSize> ClocksSizes { get { return clocksSizes; } }
        static Dictionary<ClockScale, ClockSize> clocksSizes = new Dictionary<ClockScale, ClockSize>();

        static PrivateFontCollection customFont = new PrivateFontCollection();
        public static StringFormat StringFormat { get { return format; } }
        static StringFormat format = new StringFormat();

        static System.IntPtr fontBuffer;

        static GlobalSettings()
        {
            amMode = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.PMDesignator.Length > 0;

            byte[] font = Properties.Resources.MonospaceTypewriter;
            fontBuffer = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(font.Length);
            System.Runtime.InteropServices.Marshal.Copy(font, 0, fontBuffer, font.Length);
            customFont.AddMemoryFont(fontBuffer, font.Length);

            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            InitDictionaries();
        }

        private static void InitDictionaries()
        {

            Dictionary<ClockType, Rectangle> smallCrosses = new Dictionary<ClockType, Rectangle>();

            smallCrosses[ClockType.Analog] = new Rectangle(95, 35, 10, 10);
            smallCrosses[ClockType.Digital] = new Rectangle(90, 40, 10, 10);

            clocksSizes[ClockScale.Small] = new ClockSize()
            {
                Default = new Font("Arial", 11, FontStyle.Regular),
                Info = new Font("Arial", 8, FontStyle.Regular),
                Digits = new Font((FontFamily)customFont.Families[0], 14, FontStyle.Regular),
                Caption = new Font("Arial", 8, FontStyle.Regular),
                pivot = new Point(55, 85),
                saddleRect = new Rectangle(new Point(-Resources.hands_SL_4.Width / 2, -Resources.hands_SL_4.Height / 2), Resources.hands_SL_4.Size),
                secondsHandRect = new Rectangle(new Point(-Resources.hands_SL_1.Width / 2, -Resources.hands_SL_1.Height), Resources.hands_SL_1.Size),
                minutesHandRect = new Rectangle(new Point(-Resources.hands_SL_2.Width / 2, -Resources.hands_SL_2.Height), Resources.hands_SL_2.Size),
                hoursHandRect = new Rectangle(new Point(-Resources.hands_SL_3.Width / 2, -Resources.hands_SL_3.Height), Resources.hands_SL_3.Size),
                timeTypeCenter = new Point(55, 132),
                digitsCenter = new Point(55, 85),
                caption = new Point(55, 25),
                city = new Rectangle(0, 140, 110, 55),
                regions = new Rectangle(0, 128, 120, 25),
                panel = new Rectangle(0, 0, 110, 195),
                closeCrosses = smallCrosses
            };

            Dictionary<ClockType, Rectangle> middleCrosses = new Dictionary<ClockType, Rectangle>();

            middleCrosses[ClockType.Analog] = new Rectangle(140, 40, 10, 10);
            middleCrosses[ClockType.Digital] = new Rectangle(135, 45, 10, 10);

            clocksSizes[ClockScale.Middle] = new ClockSize()
            {
                Default = new Font("Arial", 13, FontStyle.Regular),
                Info = new Font("Arial", 11, FontStyle.Regular),
                Digits = new Font((FontFamily)customFont.Families[0], 22, FontStyle.Regular),
                Caption = new Font("Arial", 8, FontStyle.Regular),
                pivot = new Point(80, 110),
                saddleRect = new Rectangle(new Point(-Resources.hands_ML_4.Width / 2, -Resources.hands_ML_4.Height / 2), Resources.hands_ML_4.Size),
                secondsHandRect = new Rectangle(new Point(-Resources.hands_ML_1_.Width / 2, -Resources.hands_ML_1_.Height), Resources.hands_ML_1_.Size),
                minutesHandRect = new Rectangle(new Point(-Resources.hands_ML_2.Width / 2, -Resources.hands_ML_2.Height), Resources.hands_ML_2.Size),
                hoursHandRect = new Rectangle(new Point(-Resources.hands_ML_3.Width / 2, -Resources.hands_ML_3.Height), Resources.hands_ML_3.Size),
                timeTypeCenter = new Point(80, 182),
                digitsCenter = new Point(80, 110),
                caption = new Point(80, 25),
                city = new Rectangle(0, 190, 160, 60),
                regions = new Rectangle(0, 168, 120, 25),
                panel = new Rectangle(0, 0, 160, 250),
                closeCrosses = middleCrosses
            };

            Dictionary<ClockType, Rectangle> largeCrosses = new Dictionary<ClockType, Rectangle>();

            largeCrosses[ClockType.Analog] = new Rectangle(260, 40, 10, 10);
            largeCrosses[ClockType.Digital] = new Rectangle(255, 45, 10, 10);

            clocksSizes[ClockScale.Large] = new ClockSize()
            {
                Default = new Font("Arial", 15, FontStyle.Regular),
                Info = new Font("Arial", 14, FontStyle.Regular),
                Digits = new Font((FontFamily)customFont.Families[0], 36, FontStyle.Regular),
                Caption = new Font("Arial", 12, FontStyle.Regular),
                pivot = new Point(140, 170),
                saddleRect = new Rectangle(new Point(-Resources.hands_LL_4.Width / 2, -Resources.hands_LL_4.Height / 2), Resources.hands_LL_4.Size),
                secondsHandRect = new Rectangle(new Point(-Resources.hands_LL_1.Width / 2, -Resources.hands_LL_1.Height), Resources.hands_LL_1.Size),
                minutesHandRect = new Rectangle(new Point(-Resources.hands_LL_2.Width / 2, -Resources.hands_LL_2.Height), Resources.hands_LL_2.Size),
                hoursHandRect = new Rectangle(new Point(-Resources.hands_LL_3.Width / 2, -Resources.hands_LL_3.Height), Resources.hands_LL_3.Size),
                timeTypeCenter = new Point(140, 292),
                digitsCenter = new Point(140, 170),
                caption = new Point(140, 25),
                city = new Rectangle(0, 310, 280, 50),
                regions = new Rectangle(0, 288, 280, 50),
                panel = new Rectangle(0, 0, 280, 360),
                closeCrosses = largeCrosses
            };

            Dictionary<ClockScale, ClockBitmaps> darkClocks = new Dictionary<ClockScale, ClockBitmaps>();

            darkClocks[ClockScale.Small] = new ClockBitmaps()
            {
                saddle = Resources.hands_S_4,
                secondsHand = Resources.hands_S_1,
                minutesHand = Resources.hands_S_2,
                hoursHand = Resources.hands_S_3,
                analogBody = Resources.analog_dark_S,
                digitBody = Resources.digital_dark_S,
                pasSearch = Resources.search_dark_110px_1,
                actSearch = Resources.search_dark_110px_2,
                pasSearchLong = Resources.search_dark_150px_1,
                actSearchLong = Resources.search_dark_150px_2
            };

            darkClocks[ClockScale.Middle] = new ClockBitmaps()
            {
                saddle = Resources.hands_M_4,
                secondsHand = Resources.hands_M_1,
                minutesHand = Resources.hands_M_2,
                hoursHand = Resources.hands_M_3,
                analogBody = Resources.analog_dark_M,
                digitBody = Resources.digital_dark_M,
                pasSearch = Resources.search_dark_110px_1,
                actSearch = Resources.search_dark_110px_2,
                pasSearchLong = Resources.search_dark_150px_1,
                actSearchLong = Resources.search_dark_150px_2
            };

            darkClocks[ClockScale.Large] = new ClockBitmaps()
            {
                saddle = Resources.hands_L_4,
                secondsHand = Resources.hands_L_1,
                minutesHand = Resources.hands_L_2,
                hoursHand = Resources.hands_L_3,
                analogBody = Resources.analog_dark_L,
                digitBody = Resources.digital_dark_L,
                pasSearch = Resources.search_dark_110px_1,
                actSearch = Resources.search_dark_110px_2,
                pasSearchLong = Resources.search_dark_150px_1,
                actSearchLong = Resources.search_dark_150px_2
            };

            Dictionary<ClockType, ClocksType> darkTypes = new Dictionary<ClockType, ClocksType>();

            darkTypes[ClockType.Analog] = new ClocksType()
            {
                type = ClockType.Analog,
                closeBtn = Resources.cross_analog_dark
            };

            darkTypes[ClockType.Digital] = new ClocksType()
            {
                type = ClockType.Digital,
                closeBtn = Resources.cross_digital_dark
            };

            clocksThemes[ClockTheme.Dark] = new Theme()
            {
                types = darkTypes,
                city = new SolidBrush(Color.FromArgb(255, 255, 255)),
                regions = new SolidBrush(Color.FromArgb(119, 119, 119)),
                caption = new SolidBrush(Color.FromArgb(119, 119, 119)),
                digits = new SolidBrush(Color.FromArgb(51, 51, 51)),
                panelBackground = Color.FromArgb(0, 0, 0),
                backColor = Color.FromArgb(51, 51, 51),
                bodyTexture = Resources.texture_dark,
                Separator = Resources.separators_dark,
                pasBtn = new Bitmap[] { Resources.a1, Resources.b1 },
                actBtn = new Bitmap[] { Resources.c1, Resources.d1 },
                plus = Resources.icon_plus_dark_black_gray,
                edge = Resources.separator_dark,
                AddBtn = new Bitmap[] { Resources.a3, Resources.b3, Resources.c3 },
                pasTypeBtn = new Bitmap[] { Resources.icon_clock_black_yellow , Resources.icon_digital_black_yellow },
                actTypeBtn = new Bitmap[] { Resources.icon_clock_black_gray, Resources.icon_digital_black_gray },
                pasSizeBtn = new Bitmap[] { Resources.icon_S_black_yellow, Resources.icon_M_black_yellow, Resources.icon_L_black_yellow},
                actSizeBtn = new Bitmap[] { Resources.icon_S_black_gray, Resources.icon_M_black_gray, Resources.icon_L_black_gray },
                pasThemeBtn = new Bitmap[] { Resources.icon_dark_black_yellow, Resources.icon_sun_black_yellow },
                actThemeBtn = new Bitmap[] { Resources.icon_dark_black_gray, Resources.icon_sun_black_gray },
                timeType = new SolidBrush(Color.FromArgb(100, 100, 100)),
                clocks = darkClocks
            };

            Dictionary<ClockScale, ClockBitmaps> lightClocks = new Dictionary<ClockScale, ClockBitmaps>();

            lightClocks[ClockScale.Small] = new ClockBitmaps()
            {
                saddle = Resources.hands_SL_4,
                secondsHand = Resources.hands_SL_1,
                minutesHand = Resources.hands_SL_2,
                hoursHand = Resources.hands_SL_3,
                analogBody = Resources.analog_light_S,
                digitBody = Resources.digital_light_S,
                pasSearch = Resources.search_light_110px_1,
                actSearch = Resources.search_light_110px_2,
                pasSearchLong = Resources.search_light_150px_1,
                actSearchLong = Resources.search_light_150px_2
            };

            lightClocks[ClockScale.Middle] = new ClockBitmaps()
            {
                saddle = Resources.hands_ML_4,
                secondsHand = Resources.hands_ML_1_,
                minutesHand = Resources.hands_ML_2,
                hoursHand = Resources.hands_ML_3,
                analogBody = Resources.analog_light_M,
                digitBody = Resources.digital_light_M,
                pasSearch = Resources.search_light_110px_1,
                actSearch = Resources.search_light_110px_2,
                pasSearchLong = Resources.search_light_150px_1,
                actSearchLong = Resources.search_light_150px_2
            };

            lightClocks[ClockScale.Large] = new ClockBitmaps()
            {

                saddle = Resources.hands_LL_4,
                secondsHand = Resources.hands_LL_1,
                minutesHand = Resources.hands_LL_2,
                hoursHand = Resources.hands_LL_3,
                analogBody = Resources.analog_light_L,
                digitBody = Resources.digital_light_L,
                pasSearch = Resources.search_light_110px_1,
                actSearch = Resources.search_light_110px_2,
                pasSearchLong = Resources.search_light_150px_1,
                actSearchLong = Resources.search_light_150px_2
            };

            Dictionary<ClockType, ClocksType> lightTypes = new Dictionary<ClockType, ClocksType>();

            lightTypes[ClockType.Analog] = new ClocksType()
            {
                type = ClockType.Analog,
                closeBtn = Resources.cross_analog_light
            };

            lightTypes[ClockType.Digital] = new ClocksType()
            {
                type = ClockType.Digital,
                closeBtn = Resources.cross_digital_light
            };

            clocksThemes[ClockTheme.Light] = new Theme()
            {
                types = lightTypes,
                city = new SolidBrush(Color.FromArgb(0, 0, 0)),
                regions = new SolidBrush(Color.FromArgb(187, 187, 187)),
                caption = new SolidBrush(Color.FromArgb(153, 153, 153)),
                digits = new SolidBrush(Color.FromArgb(255, 255, 255)),
                panelBackground = Color.FromArgb(255, 255, 255),
                backColor = Color.FromArgb(220, 220, 220),
                bodyTexture = Resources.texture_light,
                Separator = Resources.separators_light,
                pasBtn = new Bitmap[] { Resources.a2, Resources.b2},
                actBtn = new Bitmap[] { Resources.c2, Resources.d2},
                AddBtn = new Bitmap[] { Resources.a4, Resources.b4, Resources.c4},
                plus = Resources.icon_plus_light_black_gray,
                edge = Resources.separator_light,
                pasTypeBtn = new Bitmap[] { Resources.icon_clock_white, Resources.icon_digital_white },
                actTypeBtn = new Bitmap[] { Resources.icon_clock_gray_white, Resources.icon_digital_gray_white },
                pasSizeBtn = new Bitmap[] { Resources.icon_S_white, Resources.icon_M_white, Resources.icon_L_white },
                actSizeBtn = new Bitmap[] { Resources.icon_S_gray_white, Resources.icon_M_gray_white, Resources.icon_L_gray_white },
                pasThemeBtn = new Bitmap[] {  Resources.icon_dark_white, Resources.icon_sun_white },
                actThemeBtn = new Bitmap[] {  Resources.icon_dark_gray_white, Resources.icon_sun_gray_white},
                timeType = new SolidBrush(Color.FromArgb(153, 153, 153)), 
                clocks = lightClocks
            };
        }

    }

    [DataContract]
    enum ClockType
    {
        [EnumMember]
        Analog,
        [EnumMember]
        Digital
    }

    [DataContract]
    enum ClockTheme
    {
        [EnumMember]
        Light,
        [EnumMember]
        Dark
    }

    [DataContract]
    enum ClockScale
    {
        [EnumMember]
        Small,
        [EnumMember]
        Middle,
        [EnumMember]
        Large
    }

    public struct Theme
    {
        public Bitmap[] actThemeBtn;
        public Bitmap[] pasThemeBtn;
        public Bitmap[] actSizeBtn;
        public Bitmap[] pasSizeBtn;
        public Bitmap[] actTypeBtn;
        public Bitmap[] pasTypeBtn;
        public Bitmap bodyTexture;
        public Bitmap Separator;
        public Bitmap[] actBtn;
        public Bitmap[] pasBtn;
        public Bitmap plus;
        public Bitmap[] AddBtn;
        public Bitmap edge;
        internal Dictionary<ClockScale, ClockBitmaps> clocks;
        internal Dictionary<ClockType, ClocksType> types;
        public Color backColor;
        public Color panelBackground;
        public Brush city;
        public Brush caption;
        public Brush regions;
        public Brush digits;
        public Brush timeType;
    }
    public struct ClockSize
    {
        public RectangleF hoursHandSaddle;
        public RectangleF minutesHand;
        public RectangleF minutesHandSaddle;
        public RectangleF secondsHand;
        public RectangleF secondsHandSaddle;
        public Rectangle panel;
        public Rectangle city;
        public Point caption;
        public Rectangle regions;
        internal Dictionary<ClockType, Rectangle> closeCrosses;
        public Font Default;
        public Font Info;
        public Font Digits;
        public Font Caption;
        public Point digitsCenter;
        public Point timeTypeCenter;
        public PointF pivot;
        public Rectangle saddleRect;
        public Rectangle hoursHandRect;
        public Rectangle minutesHandRect;
        public Rectangle secondsHandRect;
    }
    public struct ClockBitmaps
    {
        public Bitmap hoursHand;
        public Bitmap saddle;
        public Bitmap minutesHand;
        public Bitmap secondsHand;
        public Bitmap actSearch;
        public Bitmap pasSearch;
        public Bitmap actSearchLong;
        public Bitmap pasSearchLong;
        public Bitmap analogBody;
        public Bitmap digitBody;
    }
    public struct ClocksType
    {
        internal ClockType type;
        public Bitmap closeBtn;
    }

}
