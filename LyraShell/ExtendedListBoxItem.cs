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
            set { _photo = value; }
        }

        protected Icon _exitIcon = null;
        [Browsable(false)]
        public Icon ExitIcon
        {
            set { _exitIcon = value; }
        }

        #region General Properties

        private bool isCollapsed = true;

        protected BorderTypes _borderType = BorderTypes.none;
        [Category("General")]
        [Description("Choose to either have no border, or a border with square or rounded edges")]
        public BorderTypes BorderType
        {
            get { return _borderType; }
            set { _borderType = value; }
        }

        protected int _minSize = 16;
        [Category("General")]
        [Description("Item size when collapsed...Does not take effect until item reselected")]
        public int MinSize
        {
            get { return _minSize; }
            set { _minSize = value; }
        }

        protected int _maxSize = 100;
        [Category("General")]
        [Description("Item size when extended...Does not take effect until item reselected")]
        public int MaxSize
        {
            get { return _maxSize; }
            set { _maxSize = value; }
        }

        #endregion

        #region Collapsed Properties
 
        protected string _minString = String.Empty;
        [Category("Collapsed")]
        [Description("String displayed when minimied")]
        public string MinString
        {
            get { return _minString; }
            set { _minString = value; }
        }

        protected Color _backColor1_C = Color.Gainsboro;
        [Category("Collapsed")]
        [Description("Linear Gradient Color one")]
        public Color BackColor1_C
        {
            get { return _backColor1_C; }
            set { _backColor1_C = value; }
        }

        protected Color _backColor2_C = Color.LightSlateGray;
        [Category("Collapsed")]
        [Description("Linear Gradient Color two")]
        public Color BackColor2_C
        {
            get { return _backColor2_C; }
            set { _backColor2_C = value; }
        }

        protected float _focusAngle_C = 90f;
        [Category("Collapsed")]
        [Description("(MSDN) \"The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line. \"")]
        public float FocusAngle_C
        {
            get { return _focusAngle_C; }
            set { _focusAngle_C = value; }
        }

        protected bool _useGlow = true;
        [Category("Collapsed")]
        [Description("Choose to add glow to item")]
        public bool AddGlow
        {
            get { return _useGlow; }
            set { _useGlow = value; }
        }

        protected int _glowBegin = 150;
        [Category("Collapsed")]
        [Description("Beginning Transparency for Glow")]
        [DefaultValue(150)]
        public int BeginGlowValue
        {
            get { return _glowBegin; }
            set
            {
                if ((value < 0) || (value > 255))
                    throw new ArgumentException("Value must be between 0 and 255, 0 being transparent and 255 opaque");
                else
                    _glowBegin = value;
            }
        }

        protected int _glowEnd = 0;
        [Category("Collapsed")]
        [Description("Ending Transparency for Glow")]
        [DefaultValue(0)]
        public int EndGlowValue
        {
            get { return _glowEnd; }
            set
            {
                if ((value < 0) || (value > 255))
                    throw new ArgumentException("Value must be between 0 and 255");
                else
                    _glowEnd = value;
            }
        }

        protected Color _transparencyColor = Color.White;
        [Category("Collapsed")]
        [Description("Transparency color for Glow")]
        [DefaultValue(typeof(Color), "White")]
        public Color TransparencyColor
        {
            get { return _transparencyColor; }
            set { _transparencyColor = value; }
        }

        
#endregion

        #region Expanded Properties

        protected string _maxString = String.Empty;
        [Category("Expanded")]
        [Description("String displayed when item is extended")]
        public string MaxString
        {
            get { return _maxString; }
            set { _maxString = value; }
        }

        protected Color _backColor1_E = Color.Gainsboro;
        [Category("Expanded")]
        [Description("Linear Gradient Color one")]
        public Color BackColor1_E
        {
            get { return _backColor1_E; }
            set { _backColor1_E = value; }
        }

        protected Color _backColor2_E = Color.LightSlateGray;
        [Category("Expanded")]
        [Description("Linear Gradient Color two")]
        public Color BackColor2_E
        {
            get { return _backColor2_E; }
            set { _backColor2_E = value; }
        }

        protected float _focusAngle_E = 65f;
        [Category("Expanded")]
        [Description("(MSDN) \"The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line. \"")]
        public float FocusAngle
        {
            get { return _focusAngle_E; }
            set { _focusAngle_E = value; }
        }

        #endregion

        #endregion

        public ExtendedListBoxItem()
        {
        }

		public ExtendedListBoxItem(int mns, int mxs, string ms, Color c1, Color c2)
		{
			_minSize = mns;
			_maxSize = mxs;
			_minString = ms;
            _backColor1_E = c1;
            _backColor2_E = c2;
            _backColor1_C = c1;
            _backColor2_C = c2;
        }

        /// <summary>
        /// Draw the item in the collapsed state
        /// </summary>
        /// <param name="e"></param>
		public virtual void DrawCollapsed(DrawItemEventArgs e)
		{
			Font fnt = new Font("Arial", 8);
			SolidBrush br = new SolidBrush(Color.Black);

            //Center text vertically in collapsed space
            SizeF strSize = e.Graphics.MeasureString(_minString, fnt);
            Rectangle textRct = new Rectangle(
                e.Bounds.X + 5,
                e.Bounds.Y + (int)(e.Bounds.Height - strSize.Height) / 2 ,
                e.Bounds.Width - 10,
                e.Bounds.Height - 4);

            //Gradient for background
            LinearGradientBrush lgb = new LinearGradientBrush(
                e.Bounds,
                _backColor1_C,
                _backColor2_C,
                _focusAngle_C,
                true);

            GraphicsPath gp = DrawBorder(e);
            e.Graphics.FillPath(lgb, gp);
            
            //Adds a glow to the item
            if (_useGlow)
                DrawGlow(e, gp);
	
            e.Graphics.DrawString(_minString, fnt, br, textRct);

            gp.Dispose();
            lgb.Dispose();
            fnt.Dispose();
			br.Dispose();

            isCollapsed = true;
        }

        /// <summary>
        /// Add a glow effect to the item when collapsed
        /// </summary>
        /// <param name="e"></param>
        /// <param name="gp"></param>
        private void DrawGlow(DrawItemEventArgs e, GraphicsPath gp)
        {
            RectangleF rctf = gp.GetBounds();

            LinearGradientBrush lgBrush = new LinearGradientBrush(
                rctf,
                Color.FromArgb(_glowBegin, _transparencyColor),
                Color.FromArgb(_glowEnd, _transparencyColor),
                _focusAngle_C,
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
            Font fnt = new Font("Palatino Linotype", 10);
            SolidBrush br = new SolidBrush(Color.Black);
            SolidBrush br1 = new SolidBrush(Color.Red);

            Rectangle nameRct = new Rectangle(
                e.Bounds.X + 55,
                e.Bounds.Y + 2,
                e.Bounds.Width - 10,
                20);
            Rectangle textRct = new Rectangle(
                e.Bounds.X + 55,
                e.Bounds.Y + 25,
                e.Bounds.Width - 60,
                e.Bounds.Height - 10);
            Rectangle photoRct = new Rectangle(
                e.Bounds.X + 7,
                e.Bounds.Y + 5,
                48,
                48);

            exitRct.X = e.Bounds.Width - 25;
            exitRct.Y = e.Bounds.Y + 5;

            if (_photo == null)
            {
                textRct.X = 5;
                nameRct.X = 5;
                nameRct.Width = e.Bounds.Width - 25;
            }

            LinearGradientBrush lgb = new LinearGradientBrush(
                e.Bounds, 
                _backColor1_E, 
                _backColor2_E, 
                _focusAngle_E, 
                true);

            //Draw border then fill its interior
            GraphicsPath gp = DrawBorder(e);
            e.Graphics.FillPath(lgb, gp);

            //Draw photo image
            if (_photo != null)
                e.Graphics.DrawIcon(_photo, photoRct);

            if (_exitIcon != null)
                e.Graphics.DrawIcon(_exitIcon, exitRct);

            e.Graphics.DrawString(_minString, fnt, br1, nameRct);
            e.Graphics.DrawString(_maxString, fnt, br, textRct);

            gp.Dispose();
            lgb.Dispose();
            fnt.Dispose();
            br.Dispose();
            br1.Dispose();

            isCollapsed = false;
        }

        private const int ArcWidth = 10;
        /// <summary>
        /// Draw the Border around the item
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected GraphicsPath DrawBorder(DrawItemEventArgs e)
        {
            Rectangle rct = e.Bounds;
            GraphicsPath gp = new GraphicsPath();

            rct.Width -= 1;

            if (_borderType == BorderTypes.none)
                gp.AddRectangle(rct);

            if (_borderType == BorderTypes.square)
            {
                gp.AddRectangle(rct);
                e.Graphics.DrawRectangle(new Pen(Color.Black, 1), rct);
            }

            if (_borderType == BorderTypes.rounded)
            {
                Rectangle arcRct = new Rectangle(rct.X, rct.Y, ArcWidth, ArcWidth);
                Point pt1 = new Point(rct.X + ArcWidth, rct.Y);
                Point pt2 = new Point(rct.X + rct.Width - ArcWidth, rct.Y);

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
            if (isCollapsed)
                return false;
            else
                return exitRct.Contains(pt);

        }
    }
}
