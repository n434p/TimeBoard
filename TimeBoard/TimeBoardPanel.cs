using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Threading;
using TimeBoard.TimeProviders;
using TimeBoard.Clocks;
using com.pfsoft.proftrading.commons.external;
using PTLRuntime.NETScript.Settings;
using System.IO;
using System.Runtime.Serialization;
using System.Linq;

namespace TimeBoard
{
    [Exportable]
    public partial class TimeBoardPanel : Form, IExternalComponent, ISettingsComponent
    {
        #region IExternalComponent
        public string ComponentName
        {
            get
            {
                return "TimeBoard";
            }
        }

        public Control Content
        {
            get
            {
                return this;
            }
        }

        public Icon IconImage
        {
            get
            {
                return Icon;
            }
        }

        public string PanelHeader
        {
            get
            {
                return "TimeBoard";
            }
        }

        public void Populate()
        {
            var p = PTLRuntime.NETScript.Application.Connection.CurrentConnection;
            var e = PTLRuntime.NETScript.Application.Connection.ProxyEnabled;

            set =  set ?? new Settings();
            SetClocks();
            KeyPreview = true;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,250);
            dispatcherTimer.Tick += t_Tick;
            dispatcherTimer.Start();
            InitButtonsLists();
        }
        #endregion

        #region Properties
        public static YandexTimeProvider timeProvider;
        enum ApplyMode { Type, Scale, Theme }

        List<Button> scalesButtons = new List<Button>();
        List<Button> typesButtons = new List<Button>();
        List<Button> themesButtons = new List<Button>();

        Dictionary<Button, bool> panelButtons = new Dictionary<Button, bool>();

        Settings set;

        List<CityClock> clocks = new List<CityClock>();

        DispatcherTimer dispatcherTimer;
        const int delayTicks = 3;
        int ticks = 0;

        #endregion

        public TimeBoardPanel()
        {
            DoubleBuffered = true;

            InitTimeProvider();

            InitializeComponent();

            mainPanel.SendToBack();
        }
        void ApplyTheme(ApplyMode kind)
        {
            if (set == null) return;

            // enable previous btns in group & disable current
            if (kind == ApplyMode.Scale || kind == ApplyMode.Theme)
                for (int i = 0; i < scalesButtons.Count; i++)
                {
                    bool equal = (ClockScale)scalesButtons[i].Tag != set.clockScale;
                    scalesButtons[i].BackgroundImage = (equal) ? set.CurrentTheme.pasBtn[0] : set.CurrentTheme.actBtn[0];
                    scalesButtons[i].Image = (equal) ? set.CurrentTheme.actSizeBtn[i] : set.CurrentTheme.pasSizeBtn[i];
                    scalesButtons[i].BackColor = set.CurrentTheme.panelBackground;
                    panelButtons[scalesButtons[i]] = equal;
                }
            // enable previous btns in group & disable current
            if (kind == ApplyMode.Type || kind == ApplyMode.Theme)
                for (int i = 0; i < typesButtons.Count; i++)
                {
                    bool equal = (ClockType)typesButtons[i].Tag != set.clockType;
                    typesButtons[i].BackgroundImage = (equal) ? set.CurrentTheme.pasBtn[0] : set.CurrentTheme.actBtn[0];
                    typesButtons[i].Image = (equal) ? set.CurrentTheme.actTypeBtn[i] : set.CurrentTheme.pasTypeBtn[i];
                    typesButtons[i].BackColor = set.CurrentTheme.panelBackground;
                    panelButtons[typesButtons[i]] = equal;
                }

            if (kind == ApplyMode.Theme)
            {
                for (int i = 0; i < themesButtons.Count; i++)
                {
                    // enable previous btns in group & disable current
                    bool equal = (ClockTheme)themesButtons[i].Tag != set.clockTheme;
                    themesButtons[i].BackgroundImage = (equal) ? set.CurrentTheme.pasBtn[0] : set.CurrentTheme.actBtn[0];
                    themesButtons[i].Image = (equal) ? set.CurrentTheme.actThemeBtn[i] : set.CurrentTheme.pasThemeBtn[i];
                    themesButtons[i].BackColor = set.CurrentTheme.panelBackground;
                    panelButtons[themesButtons[i]] = equal;
                }

                separator1.BackgroundImage = set.CurrentTheme.Separator;
                separator2.BackgroundImage = set.CurrentTheme.Separator;

                addClockBtn.BackgroundImage = set.CurrentTheme.AddBtn[0];
                addClockBtn.Image = set.CurrentTheme.plus;
                addClockBtn.BackColor = set.CurrentTheme.panelBackground;

                separator3.BackgroundImage = set.CurrentTheme.edge;
                BackColor = set.CurrentTheme.backColor;
                clocksPanel.BackgroundImage = set.CurrentTheme.bodyTexture;
                clocksPanel.BackColor = set.CurrentTheme.panelBackground;
            }
            Refresh();

        }

        void AddClock(string cityId)
        {
            CityClock clock = new CityClock(set, cityId);
            clock.ClockRemoved += OnDeleteClockBtn_Click;
            clock.ClockEdited += Clock_ClockEdited;

            clocksPanel.SuspendLayout();
            clocksPanel.Controls.Add(clock);
            clocksPanel.Controls.SetChildIndex(clock, 0);
            clocksPanel.ResumeLayout();

            clocks.Insert(0, clock);

            clock.EditMode = true;
        }

        void DeleteClock(CityClock curCityClock)
        {
            if (curCityClock == null) return;

            curCityClock.EditMode = false;
            curCityClock.ClockEdited -= Clock_ClockEdited;
            curCityClock.ClockRemoved -= OnDeleteClockBtn_Click;
            clocksPanel.Controls.Remove(curCityClock);
            clocks.Remove(curCityClock);
            curCityClock.Dispose();
            curCityClock = null;

            set.isEditingMode = false;
        }

        #region Misc
        void SetClocks()
        {
            clocksPanel.Controls.Clear();
            clocks.Clear();

            foreach (var cityName in set.citiesList)
            {
                AddClock(cityName);
            }
        }

        private void InitTimeProvider()
        {
            timeProvider = new YandexTimeProvider();
        }
        private void InitButtonsLists()
        {
            typesButtons.Add(analogTypeBtn);
            typesButtons.Add(digitalTypeBtn);

            scalesButtons.Add(sSizeBtn);
            scalesButtons.Add(mSizeBtn);
            scalesButtons.Add(lSizeBtn);

            themesButtons.Add(darkThemeBtn);
            themesButtons.Add(lightThemeBtn);

            analogTypeBtn.Tag = TimeBoard.ClockType.Analog;
            digitalTypeBtn.Tag = TimeBoard.ClockType.Digital;
            lightThemeBtn.Tag = TimeBoard.ClockTheme.Light;
            darkThemeBtn.Tag = TimeBoard.ClockTheme.Dark;
            mSizeBtn.Tag = TimeBoard.ClockScale.Middle;
            lSizeBtn.Tag = TimeBoard.ClockScale.Large;
            sSizeBtn.Tag = TimeBoard.ClockScale.Small;

            ApplyTheme(ApplyMode.Theme);

            /// tie leave/enter events to trigerred buttons
            List<Button> list = new List<Button>();

            list.AddRange(typesButtons);
            list.AddRange(scalesButtons);
            list.AddRange(themesButtons);
            foreach (Button item in list)
            {
                item.MouseEnter += B_Enter;
                item.MouseLeave += B_Leave;
                item.TabStop = false;
            }

            addClockBtn.TabStop = false;

            addClockBtn.MouseEnter += (o, e) =>
            {
                addClockBtn.BackgroundImage = set.CurrentTheme.AddBtn[1];
            };
            addClockBtn.MouseLeave += (o, e) =>
            {
                addClockBtn.BackgroundImage = set.CurrentTheme.AddBtn[0];
            };
        }

        private void B_Leave(object sender, EventArgs e)
        {
            Button but = sender as Button;
            but.BackgroundImage = (panelButtons[but]) ? set.CurrentTheme.pasBtn[0] : set.CurrentTheme.actBtn[0];
            but.TabStop = false;
        }
        private void B_Enter(object sender, EventArgs e)
        {
            Button but = sender as Button;
            but.BackgroundImage = (panelButtons[but]) ? set.CurrentTheme.pasBtn[1] : set.CurrentTheme.actBtn[1];
        }

        void t_Tick(object sender, EventArgs e)
        {
            bool clockRefresh = ticks >= delayTicks;
            ticks = (clockRefresh) ? 0 : ++ticks;

            foreach (var item in clocks)
            {
                if (item.EditMode)
                    item.FillCityList();

                if (clockRefresh)
                {
                    item.Invalidate();
                }
            }
        }
        private void OnClockScaleChanged(object sender, EventArgs e)
        {
            var curBtn = sender as Button;
            // set general clock scale

            ClockScale scale = (ClockScale)curBtn.Tag;

            if (scale != set.clockScale)
            {
                set.clockScale = scale;
                ApplyTheme(ApplyMode.Scale);

                // changing clocks scales
                for (int i = 0; i < clocks.Count; i++)
                {
                    clocks[i].ApplyTheme();
                }
                // redrawing
                this.Refresh();
            }
        }
        private void OnClockTypeChanged(object sender, EventArgs e)
        {
            var curBtn = sender as Button;
            // set general clock type

            ClockType type = (ClockType)curBtn.Tag;
            // set general clock theme
            if (type != set.clockType)
            {
                set.clockType = type;
                ApplyTheme(ApplyMode.Type);

                //reassign clocks in each CityClock
                for (int i = 0; i < clocks.Count; i++)
                {
                    var offset = clocks[i].City.offset;
                    clocks[i].Clock = BaseClock.CreateClock(set,set.clockType, offset);
                    //reassign geometry
                    clocks[i].ApplyTheme();
                }
                //redrawing
                this.Refresh();
            }

        }
        private void OnThemeBtn_Click(object sender, EventArgs e)
        {
            var curBtn = sender as Button;
            ClockTheme theme = (ClockTheme)curBtn.Tag;
            // set general clock theme
            if (theme != set.clockTheme)
            {
                set.clockTheme = (ClockTheme)curBtn.Tag;
                ApplyTheme(ApplyMode.Theme);
                // changing clocks themes
                for (int i = 0; i < clocks.Count; i++)
                {
                    clocks[i].Clock.ApplyTheme();
                    //reassign cache for colors
                    clocks[i].ApplyTheme();
                }
                Refresh();
            }

        }
        private void OnAddClockBtn_Click(object sender, EventArgs e)
        {
            if (!clocks.Any(c => c.City.id == null))
            {
                AddClock(null);
            }
        }

        private void Clock_ClockEdited(object sender, EventArgs e)
        {
            //if (set.isEditingMode) return;

            CityClock curCityClock = (sender as CityClock);
            foreach (var clock in clocks)
            {
                clock.EditMode = curCityClock == clock;
                clock.Refresh();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            CityClock clock = clocks.FirstOrDefault(c => c.EditMode);

            if (clock == null)
                return;

            if (e.KeyCode == Keys.Escape)
            {
                if (clock.City.name == null)
                    DeleteClock(clock);
                else
                    clock.EditMode = false;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                clock.SelectFirst();
            }
        }

        private void OnDeleteClockBtn_Click(object sender, EventArgs e)
        {
            DeleteClock(sender as CityClock);
        }

        public List<SettingItem> GetSettings()
        {
            var setting = new List<SettingItem>();

            var str = string.Empty;

            if (set == null)
                set = new Settings();

            if(clocks.Count > 0)
                set.citiesList = clocks.Where(cc => cc.City != null).Select(c => c.City.id).Reverse().ToList();

            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Settings));
                serializer.WriteObject(memoryStream, set);
                memoryStream.Position = 0;
                str = reader.ReadToEnd();
            }

            setting.Add(
                    new SettingItemTextBox("panel_settings", str, "options", 0) { VisibilityMode = VisibilityMode.Hidden });

            return setting;
        }

        public void ApplySettings(List<SettingItem> settings)
        {
            var item = settings.FirstOrDefault(option => option.Name == "panel_settings") as SettingItemTextBox;
            var optionString = item?.Value as string;

            if (optionString != null)
            {
                byte[] buffer = System.Text.Encoding.Default.GetBytes(optionString);

                using (Stream stream = new MemoryStream())
                {
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Position = 0;
                    DataContractSerializer deserializer = new DataContractSerializer(typeof(Settings));
                    set = deserializer.ReadObject(stream) as Settings;
                }
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED - to avoid panel flickering
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            // Detect a Click
            if (m.Msg == 0x210 && m.WParam.ToInt32() == 513)
            {
                var clock = clocks.Where(c => c.EditMode).FirstOrDefault();

                if (clock != null)
                {
                    var x = (int)(m.LParam.ToInt32() & 0xFFFF);
                    var y = (int)(m.LParam.ToInt32() >> 16);

                    var location = new Point(x, y);

                    if(this.GetChildAtPoint(location) != clocksPanel || clocksPanel.GetChildAtPoint(location) != clock)
                    {
                        clock.EditMode = false;
                        if (clock.City.id == null)
                            DeleteClock(clock);
                    }
                }
            }

            base.WndProc(ref m);
        }


        #endregion

    }
}
