using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Lyra2.LyraShell
{
    public enum BorderTypes
    {
        none,
        square,
        rounded
    };

	/// <summary>
	/// Summary description for ListBoxItemBase.
	/// </summary>
	public class ExtendedListBoxItem : object
    {
        #region Properties and such

        private Icon _photo = null;
        [Browsable(false)]
        public Icon UserPhoto
        {
            set { this._photo = value; }
        }

        protected Icon _exitIcon = null;
        [Browsable(false)]
        public Icon ExitIcon
        {
            set { this._exitIcon = value; }
        }

        #region General Properties

        private bool isCollapsed = true;

        protected BorderTypes _borderType = BorderTypes.none;
        [Category("General")]
        [Description("Choose to either have no border, or a border with square or rounded edges")]
        public BorderTypes BorderType
        {
            get { return this._borderType; }
            set { this._borderType = value; }
        }

        protected int _minSize = 16;
        [Category("General")]
        [Description("Item size when collapsed...Does not take effect until item reselected")]
        public int MinSize
        {
            get { return this._minSize; }
            set { this._minSize = value; }
        }

        protected int _maxSize = 100;
        [Category("General")]
        [Description("Item size when extended...Does not take effect until item reselected")]
        public int MaxSize
        {
            get { return this._maxSize; }
            set { this._maxSize = value; }
        }

        #endregion

        #region Collapsed Properties
 
        protected string _minString = String.Empty;
        [Category("Collapsed")]
        [Description("String displayed when minimied")]
        public string MinString
        {
            get { return this._minString; }
            set { this._minString = value; }
        }

        protected Color _backColor1_C = Color.Gainsboro;
        [Category("Collapsed")]
        [Description("Linear Gradient Color one")]
        public Color BackColor1_C
        {
            get { return this._backColor1_C; }
            set { this._backColor1_C = value; }
        }

        protected Color _backColor2_C = Color.LightSlateGray;
        [Category("Collapsed")]
        [Description("Linear Gradient Color two")]
        public Color BackColor2_C
        {
            get { return this._backColor2_C; }
            set { this._backColor2_C = value; }
        }

        protected float _focusAngle_C = 90f;
        [Category("Collapsed")]
        [Description("(MSDN) \"The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line. \"")]
        public float FocusAngle_C
        {
            get { return this._focusAngle_C; }
            set { this._focusAngle_C = value; }
        }

        protected bool _useGlow = true;
        [Category("Collapsed")]
        [Description("Choose to add glow to item")]
        public bool AddGlow
        {
            get { return this._useGlow; }
            set { this._useGlow = value; }
        }

        protected int _glowBegin = 150;
        [Category("Collapsed")]
        [Description("Beginning Transparency for Glow")]
        [DefaultValue(150)]
        public int BeginGlowValue
        {
            get { return this._glowBegin; }
            set
            {
                if ((value < 0) || (value > 255))
                    throw new ArgumentException("Value must be between 0 and 255, 0 being transparent and 255 opaque");
                else
                    this._glowBegin = value;
            }
        }

        protected int _glowEnd = 0;
        [Category("Collapsed")]
        [Description("Ending Transparency for Glow")]
        [DefaultValue(0)]
        public int EndGlowValue
        {
            get { return this._glowEnd; }
            set
            {
                if ((value < 0) || (value > 255))
                    throw new ArgumentException("Value must be between 0 and 255");
                else
                    this._glowEnd = value;
            }
        }

        protected Color _transparencyColor = Color.White;
        [Category("Collapsed")]
        [Description("Transparency color for Glow")]
        [DefaultValue(typeof(Color), "White")]
        public Color TransparencyColor
        {
            get { return this._transparencyColor; }
            set { this._transparencyColor = value; }
        }

        
#endregion

        #region Expanded Properties

        protected string _maxString = String.Empty;
        [Category("Expanded")]
        [Description("String displayed when item is extended")]
        public string MaxString
        {
            get { return this._maxString; }
            set { this._maxString = value; }
        }

        protected Color _backColor1_E = Color.Gainsboro;
        [Category("Expanded")]
        [Description("Linear Gradient Color one")]
        public Color BackColor1_E
        {
            get { return this._backColor1_E; }
            set { this._backColor1_E = value; }
        }

        protected Color _backColor2_E = Color.LightSlateGray;
        [Category("Expanded")]
        [Description("Linear Gradient Color two")]
        public Color BackColor2_E
        {
            get { return this._backColor2_E; }
            set { this._backColor2_E = value; }
        }

        protected float _focusAngle_E = 65f;
        [Category("Expanded")]
        [Description("(MSDN) \"The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line. \"")]
        public float FocusAngle
        {
            get { return this._focusAngle_E; }
            set { this._focusAngle_E = value; }
        }

        #endregion

        #endregion

        public ExtendedListBoxItem()
        {
        }

		public ExtendedListBoxItem(int mns, int mxs, string ms, Color c1, Color c2)
		{
		    this._minSize = mns;
		    this._maxSize = mxs;
		    this._minString = ms;
		    this._backColor1_E = c1;
		    this._backColor2_E = c2;
		    this._backColor1_C = c1;
		    this._backColor2_C = c2;
        }

        /// <summary>
        /// Draw the item in the collapsed state
        /// </summary>
        /// <param name="e"></param>
		public virtual void DrawCollapsed(DrawItemEventArgs e)
		{
			var fnt = new Font("Arial", 8);
			var br = new SolidBrush(Color.Black);

            //Center text vertically in collapsed space
            var strSize = e.Graphics.MeasureString(this._minString, fnt);
            var textRct = new Rectangle(
                e.Bounds.X + 5,
                e.Bounds.Y + (int)(e.Bounds.Height - strSize.Height) / 2 ,
                e.Bounds.Width - 10,
                e.Bounds.Height - 4);

            //Gradient for background
            var lgb = new LinearGradientBrush(
                e.Bounds, this._backColor1_C, this._backColor2_C, this._focusAngle_C,
                true);

            var gp = this.DrawBorder(e);
            e.Graphics.FillPath(lgb, gp);
            
            //Adds a glow to the item
            if (this._useGlow) this.DrawGlow(e, gp);
	
            e.Graphics.DrawString(this._minString, fnt, br, textRct);

            gp.Dispose();
            lgb.Dispose();
            fnt.Dispose();
			br.Dispose();

		    this.isCollapsed = true;
        }

        /// <summary>
        /// Add a glow effect to the item when collapsed
        /// </summary>
        /// <param name="e"></param>
        /// <param name="gp"></param>
        private void DrawGlow(DrawItemEventArgs e, GraphicsPath gp)
        {
            var rctf = gp.GetBounds();

            var lgBrush = new LinearGradientBrush(
                rctf,
                Color.FromArgb(this._glowBegin, this._transparencyColor),
                Color.FromArgb(this._glowEnd, this._transparencyColor), this._focusAngle_C,
                true);
            e.Graphics.FillPath(lgBrush, gp); ;

            lgBrush.Dispose();
        }

        Rectangle exitRct = new Rectangle(0, 0, 16, 16);
        
        /// <summary>
        /// Draw the item in the extended state
        /// </summary>
        /// <param name="e"></param>
        public virtual void DrawExpanded(DrawItemEventArgs e)
        {
            var fnt = new Font("Palatino Linotype", 10);
            var br = new SolidBrush(Color.Black);
            var br1 = new SolidBrush(Color.Red);

            var nameRct = new Rectangle(
                e.Bounds.X + 55,
                e.Bounds.Y + 2,
                e.Bounds.Width - 10,
                20);
            var textRct = new Rectangle(
                e.Bounds.X + 55,
                e.Bounds.Y + 25,
                e.Bounds.Width - 60,
                e.Bounds.Height - 10);
            var photoRct = new Rectangle(
                e.Bounds.X + 7,
                e.Bounds.Y + 5,
                48,
                48);

            this.exitRct.X = e.Bounds.Width - 25;
            this.exitRct.Y = e.Bounds.Y + 5;

            if (this._photo == null)
            {
                textRct.X = 5;
                nameRct.X = 5;
                nameRct.Width = e.Bounds.Width - 25;
            }

            var lgb = new LinearGradientBrush(
                e.Bounds, this._backColor1_E, this._backColor2_E, this._focusAngle_E, 
                true);

            //Draw border then fill its interior
            var gp = this.DrawBorder(e);
            e.Graphics.FillPath(lgb, gp);

            //Draw photo image
            if (this._photo != null)
                e.Graphics.DrawIcon(this._photo, photoRct);

            if (this._exitIcon != null)
                e.Graphics.DrawIcon(this._exitIcon, this.exitRct);

            e.Graphics.DrawString(this._minString, fnt, br1, nameRct);
            e.Graphics.DrawString(this._maxString, fnt, br, textRct);

            gp.Dispose();
            lgb.Dispose();
            fnt.Dispose();
            br.Dispose();
            br1.Dispose();

            this.isCollapsed = false;
        }

        private const int ArcWidth = 10;
        /// <summary>
        /// Draw the Border around the item
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected GraphicsPath DrawBorder(DrawItemEventArgs e)
        {
            var rct = e.Bounds;
            var gp = new GraphicsPath();

            rct.Width -= 1;

            if (this._borderType == BorderTypes.none)
                gp.AddRectangle(rct);

            if (this._borderType == BorderTypes.square)
            {
                gp.AddRectangle(rct);
                e.Graphics.DrawRectangle(new Pen(Color.Black, 1), rct);
            }

            if (this._borderType == BorderTypes.rounded)
            {
                var arcRct = new Rectangle(rct.X, rct.Y, ArcWidth, ArcWidth);
                var pt1 = new Point(rct.X + ArcWidth, rct.Y);
                var pt2 = new Point(rct.X + rct.Width - ArcWidth, rct.Y);

                gp.AddArc(arcRct, 180, 90);
                gp.AddLine(pt1, pt2);

                arcRct.Location = pt2;
                gp.AddArc(arcRct, 270, 90);

                pt1 = new Point(rct.X + rct.Width, rct.Y + ArcWidth);
                pt2 = new Point(rct.X + rct.Width, rct.Y + rct.Height - ArcWidth);
                gp.AddLine(pt1, pt2);

                arcRct.Y = pt2.Y;
                gp.AddArc(arcRct, 0, 90);

                pt1 = new Point(rct.X + rct.Width - ArcWidth, rct.Y + rct.Height);
                pt2 = new Point(rct.X + ArcWidth, rct.Y + rct.Height);
                gp.AddLine(pt1, pt2);

                arcRct.X = rct.X;
                gp.AddArc(arcRct, 90, 90);

                gp.CloseFigure();

                e.Graphics.DrawPath(new Pen(Color.Black, 1), gp);
            }
            return gp;
        }

        public bool HitCheck(Point pt)
        {
            if (this.isCollapsed)
                return false;
            else
                return this.exitRct.Contains(pt);

        }
    }
}
