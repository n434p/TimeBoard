using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TimeBoard.Clocks;

namespace TimeBoard
{
    class CityClock : Panel
    {
        #region Properties

        ComboBox CityListBox = new ComboBox();
        Label deleteClockBtn = new Label();

        public event EventHandler ClockRemoved = delegate { };
        public event EventHandler ClockEdited = delegate { };

        public BaseClock Clock;

        bool hovered;

        public bool EditMode
        {
            get { return CityListBox.Visible; }
            set
            {
                CityListBox.Visible = value;
                set.isEditingMode = value;

                if(value)
                    CityListBox.Select();
            }
        }

        public City City
        {
            get
            {
                return city;
            }
            private set
            {
                if (value != null && value.name != null)
                {
                    Clock = BaseClock.CreateClock(set,set.clockType, value.offset);
                    ApplyTheme();
                    city = value;
                }
            }
        }
        City city = new City();

        Font font, regionsFont;
        Brush caption, cityb, regions;
        RectangleF  cityRect, regionsRect;
        Point captionCenter;
        StringFormat format = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center};
        Settings set;
        bool textUpdated;

        #endregion

        public CityClock(Settings settings, City city)
        {
            set = settings;
            ///avoid panel flickering
            DoubleBuffered = true;

            Clock = BaseClock.CreateClock(set, set.clockType, 0);
            City = city;

            Populate();

            CityListBox.SelectionChangeCommitted += CityListBox_SelectionChangeCommitted;
            CityListBox.TextUpdate += CityListBox_TextUpdate;
            CityListBox.LostFocus += CityListBox_LostFocus;

            ApplyTheme();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            RefreshList();
        }

        async void RefreshList()
        {
            if (City?.name != null)
            {
                EditMode = false;
                PopulateCityList(await TimeBoardPanel.timeProvider.GetCityList(City.name));
                SelectFirst();
            }
        }

        private void CityListBox_LostFocus(object sender, EventArgs e)
        {
            EditMode = false;

            if (City.name == null)
                ClockRemoved(this, e);
        }

        private void Populate()
        {
            CityListBox.AllowDrop = true;
            CityListBox.FlatStyle = FlatStyle.Flat;
            CityListBox.BackColor = System.Drawing.Color.LightGray;
            CityListBox.FormattingEnabled = true;
            CityListBox.Visible = set.isEditingMode;
            CityListBox.MaxDropDownItems = 10;

            this.Controls.Add(CityListBox);
        }

        public void SelectFirst()
        {
            if (CityListBox.Items.Count > 0 && CityListBox.SelectedItem == null)
            {
                CityListBox.SelectedIndex = 0;
            }
        }

        public void ApplyTheme()
        {
            if (set == null) return;

            Width = set.CurrentSize.panel.Width;
            Height = set.CurrentSize.panel.Height;

            CityListBox.Size = set.CurrentSize.city.Size;
            CityListBox.Location = set.CurrentSize.city.Location;

            this.BackColor = Color.Transparent;

            font = set.CurrentSize.Default;
            regionsFont = set.CurrentSize.Info;
            caption = set.CurrentTheme.caption;
            captionCenter = set.CurrentSize.caption;
            cityb = set.CurrentTheme.city;
            cityRect = set.CurrentSize.city;
            regions = set.CurrentTheme.regions;
            regionsRect = set.CurrentSize.regions;
        }

        #region Misc

        async void CityListBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            City si = CityListBox.SelectedItem as City;
            if (si != null)
            {
                si.offset = await TimeBoardPanel.timeProvider.GetCityTimeOffset(si);
                City = si;

                set.isEditingMode = false;
                EditMode = false;
            }
        }

        void PopulateCityList(List<City> list)
        {
                if (list == null)
                    return;

                var items = list.ToArray();

                if (items.Length == 0) return;

                Cursor.Current = Cursors.Default;

                var c1 = CityListBox.Items.Count;

                for (int i = 0; i < c1; i++)
                {
                    CityListBox.Items.RemoveAt(0);
                }

                for (int i = 0; i < items.Length; i++)
                {
                    CityListBox.Items.Add(items[i]);
                }

                if (!EditMode)
                    SelectFirst();
        }

        void CityListBox_TextUpdate(object sender, EventArgs e)
        {
            CityListBox.DroppedDown = CityListBox.SelectedIndex == -1 && CityListBox.Items.Count > 0 && CityListBox.Text.Length > 2;
            textUpdated = true;
        }

        private void DeleteClockBtn_Click(object sender, EventArgs e)
        {
            ClockRemoved(this, e);
        }

        public async void FillCityList()
        {
            try
            {
                if (CityListBox.Text.Length > 2 && textUpdated && CityListBox.SelectedIndex == -1)
                {
                    textUpdated = false;
                    PopulateCityList(await TimeBoardPanel.timeProvider.GetCityList(CityListBox.Text));
                }
            }
            catch
            {

            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.Bilinear;

            Clock.Draw(e.Graphics);
            DrawInfo(e.Graphics);
        }

        void DrawInfo(Graphics gfx)
        {
            gfx.ResetTransform();
            if (hovered)
            {
                gfx.DrawImage(set.CurrentType.closeBtn, set.CloseCross);
            }

            if (City != null && !CityListBox.Visible)
            {
                gfx.DrawString(City.offsetString, set.CurrentSize.Caption, caption, captionCenter, GlobalSettings.StringFormat);
                gfx.DrawString(City.name+"\n"+City.country, set.CurrentSize.Default, cityb, cityRect, format);
            }

        }

        protected override void Dispose(bool disposing)
        {
            this.Controls.Clear();
            CityListBox.Dispose();

            base.Dispose(disposing);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            hovered = true;
            Invalidate(set.CloseCross);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            hovered = false;
            Invalidate(set.CloseCross);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (city != null && set.CurrentSize.city.Contains(e.Location))
            {
                ClockEdited(this, e);
            }
            else if (set.CloseCross.Contains(e.Location))
                ClockRemoved(this, e);
            else
            {
                ClockEdited(this, e);
            }
        }

        #endregion

    }
}
