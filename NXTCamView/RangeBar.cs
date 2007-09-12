// The RangeBar code is based on the codeproject project by Detlef Neunherz
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NXTCamView
{
    public class RangeBar : UserControl
    {
        public delegate void RangeChangedEventHandler(object sender, EventArgs e);
        public delegate void RangeChangingEventHandler(object sender, EventArgs e);

        private Container components = null;

        public RangeBar()
        {
            InitializeComponent();
            _fontMark = new Font("Arial", _markWidth);
            _strformat = new StringFormat();
            _strformat.Alignment = StringAlignment.Center;
            _strformat.LineAlignment = StringAlignment.Near;
        }

        protected override void Dispose(bool disposing)
        {
            if( disposing )
            {
                if( components != null )
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // ZzzzRangeBar
            // 
            Name = "ZzzzRangeBar";
            Size = new System.Drawing.Size(344, 34);
            Load += new System.EventHandler(OnLoad);
            MouseDown += new System.Windows.Forms.MouseEventHandler(OnMouseDown);
            MouseMove += new System.Windows.Forms.MouseEventHandler(OnMouseMove);
            Leave += new System.EventHandler(OnLeave);
            Resize += new System.EventHandler(OnResize);
            KeyPress += new System.Windows.Forms.KeyPressEventHandler(OnKeyPress);
            Paint += new System.Windows.Forms.PaintEventHandler(OnPaint);
            MouseUp += new System.Windows.Forms.MouseEventHandler(OnMouseUp);
            SizeChanged += new System.EventHandler(OnSizeChanged);
            ResumeLayout(false);
        }

        #endregion

        public enum ActiveMarkType
        {
            None,
            Left,
            Right
        } ;

        public enum RangeBarOrientation
        {
            Horizontal,
            Vertical
        } ;

        public enum TopBottomOrientation
        {
            Top,
            Bottom,
            Both
        } ;

        private Color _colorInner = Color.LightGreen;
        private SolidBrush _brushFill = new SolidBrush(SystemColors.ControlLightLight);
        private SolidBrush _brushFillLight = new SolidBrush(Color.FromArgb(100,SystemColors.ControlLightLight));
        private Color _colorShadowLight = SystemColors.ControlLightLight;
        private Color _colorShadowDark = SystemColors.ControlDarkDark;
        private SolidBrush _brushInner;
        private SolidBrush _brushYellow;
        private SolidBrush _brushInnerLight;
        private Font _fontMark; 
        private StringFormat _strformat;

        private int _sizeShadow = 1;
        private double _minimum = 0;
        private double _maximum = 10;
        private double _rangeMin = 3;
        private double _rangeMax = 5;
        private ActiveMarkType _activeMark = ActiveMarkType.None;


        private RangeBarOrientation _barOrientation = RangeBarOrientation.Horizontal; // orientation of range bar
        private TopBottomOrientation _scaleOrientation = TopBottomOrientation.Bottom;
        private int _barHeight = 8;
        private int _markWidth = 8;
        private int _markHeight = 24;
        private int _tickHeight = 6;
        private int _axisDivision = 10;

        private int _posLeft = 0;
        private int _posRight = 0;
        private int _xPosMin;
        private int _xPosMax;

        private int _posValue = 0;

        private Point[] _pointsLeft = new Point[5];
        private Point[] _pointsRight = new Point[5];
        private Point[] _pointValue = new Point[5];


        private bool _moveLMark = false;
        private bool _moveRMark = false;

        public int TickHeight
        {
            set
            {
                _tickHeight = Math.Min(Math.Max(1, value), _barHeight);
                Invalidate();
                Update();
            }
            get { return _tickHeight; }
        }

        public int MarkHeight
        {
            set
            {
                _markHeight = Math.Max(_barHeight + 2, value);
                Invalidate();
                Update();
            }
            get { return _markHeight; }
        }

        public int BarHeight
        {
            set
            {
                _barHeight = Math.Min(value, _markHeight - 2);
                Invalidate();
                Update();
            }
            get { return _barHeight; }
        }

        public RangeBarOrientation Orientation
        {
            set
            {
                _barOrientation = value;
                Invalidate();
                Update();
            }
            get { return _barOrientation; }
        }

        public TopBottomOrientation ScaleOrientation
        {
            set
            {
                _scaleOrientation = value;
                Invalidate();
                Update();
            }
            get { return _scaleOrientation; }
        }


        public void SetRangeMinMax( double min, double max )
        {
            _rangeMin = min;
            _rangeMax = max;
            if( _rangeMin > _rangeMax )
            {
                double temp = _rangeMax;
                _rangeMax = _rangeMin;
                _rangeMin = temp;
                Range2Pos();
                Invalidate(true);
            }


        }
        public int RangeMaximum
        {
            set
            {
                _rangeMax = value;
                //PT - fix - change the other side if my new value doesn't fit
                if( _rangeMax < _rangeMin )
                    _rangeMin = _rangeMax;
                Range2Pos();
                Invalidate(true);
            }
            get { return (int) _rangeMax; }
        }


        public int RangeMinimum
        {
            set
            {
                _rangeMin = value;
                if( _rangeMin < _minimum )
                    _rangeMin = _minimum;
                else if( _rangeMin > _maximum )
                    _rangeMin = _maximum;
                //PT - fix - change the other side if my new value doesn't fit
                if( _rangeMin > _rangeMax )
                    _rangeMax = _rangeMin;
                Range2Pos();
                Invalidate(true);
            }
            get { return (int) _rangeMin; }
        }


        public int TotalMaximum
        {
            set
            {
                _maximum = value;
                if( _rangeMax > _maximum )
                    _rangeMax = _maximum;
                Range2Pos();
                Invalidate(true);
            }
            get { return (int) _maximum; }
        }


        public int TotalMinimum
        {
            set
            {
                _minimum = value;
                if( _rangeMin < _minimum )
                    _rangeMin = _minimum;
                Range2Pos();
                Invalidate(true);
            }
            get { return (int) _minimum; }
        }


        public int DivisionCount
        {
            set
            {
                _axisDivision = value;
                Refresh();
            }
            get { return _axisDivision; }
        }


        public Color InnerColor
        {
            set
            {
                _colorInner = value;
                Refresh();
            }
            get { return _colorInner; }
        }

        private Color _colorBackground = SystemColors.ControlLight;

        public Color ColorBackground
        {
            get { return _colorBackground; }
            set
            {
                _colorBackground = value;
                Refresh();
            }
        }

        private double _value;

        public double Value
        {
            get { return _value; }
            set
            {
                _value = Math.Max(_minimum, Math.Min( _maximum, value ));
                Value2Pos();
                Invalidate();
            }
        }

        private bool _valueActive = false;
        public bool ValueActive 
        { 
            get { return _valueActive; } 
            set
            {
                 _valueActive = value;
                Invalidate();
            } 
        }

        /// <summary>
        /// set selected range
        /// </summary>
        /// <param name="left">Left side of range</param>
        /// <param name="right">Right side of range</param>
        public void SelectRange(int left, int right)
        {
            SetRangeMinMax(left,right);
        }


        /// <summary>
        /// set range limits
        /// </summary>
        /// <param name="left">Left side of range limit</param>
        /// <param name="right">Right side of range limit</param>
        public void SetRangeLimit(double left, double right)
        {
            _minimum = left;
            _maximum = right;
            Range2Pos();
            Invalidate(true);
        }


        // paint event reaction
        private void OnPaint(object sender, PaintEventArgs e)
        {
            int h = Height;
            int w = Width;
            Pen penShadowLight = new Pen(_colorShadowLight);
            Pen penShadowDark = new Pen(_colorShadowDark);
            Pen penHighlight = new Pen( Color.DarkBlue, 3 );
            SolidBrush brushBackground = new SolidBrush(_colorBackground);

            _brushInner = Enabled ? new SolidBrush(_colorInner) : new SolidBrush(Color.FromKnownColor(KnownColor.InactiveCaption));
            _brushInnerLight = Enabled ? new SolidBrush(Color.FromArgb(100,_colorInner)) : new SolidBrush(Color.FromKnownColor(KnownColor.InactiveCaption));
            _brushYellow = new SolidBrush(Color.FromArgb(255, 255,192));

            // range
            _xPosMin = _markWidth + 1;
            if( _barOrientation == RangeBarOrientation.Horizontal )
                _xPosMax = w - _markWidth - 1;
            else
                _xPosMax = h - _markWidth - 1;

            // range check
            if( _posLeft < _xPosMin ) _posLeft = _xPosMin;
            if( _posLeft > _xPosMax ) _posLeft = _xPosMax;
            if( _posRight > _xPosMax ) _posRight = _xPosMax;
            if( _posRight < _xPosMin ) _posRight = _xPosMin;

            Range2Pos();
            Value2Pos();

            if( _barOrientation == RangeBarOrientation.Horizontal )
            {
                int baryoff = (h - _barHeight)/2;
                int markyoff;
                markyoff = baryoff + (_barHeight - _markHeight)/2 - 1;

                //draw background
                e.Graphics.FillRectangle(brushBackground, 0, baryoff-1, w - 1, _barHeight+1);

                drawColoredRegion(baryoff, e.Graphics);


                // Scale
                int tickyoff1;
                int tickyoff2;
                if( _scaleOrientation == TopBottomOrientation.Bottom )
                {
                    tickyoff1 = tickyoff2 = baryoff + _barHeight + 2;
                }
                else if( _scaleOrientation == TopBottomOrientation.Top )
                {
                    tickyoff1 = tickyoff2 = baryoff - _tickHeight - 4;
                }
                else
                {
                    tickyoff1 = baryoff + _barHeight + 2;
                    tickyoff2 = baryoff - _tickHeight - 4;
                }

                //draw ticks
                if( _axisDivision > 1 )
                {
                    double dtick;
                    dtick = (double) (_xPosMax - _xPosMin)/(double) _axisDivision;
                    for( int i = 0; i < _axisDivision + 1; i++ )
                    {
                        int tickpos;
                        tickpos = (int) Math.Round(i*dtick);
                        if( _scaleOrientation == TopBottomOrientation.Bottom
                            || _scaleOrientation == TopBottomOrientation.Both )
                        {
                            e.Graphics.DrawLine(penShadowDark, _markWidth + 1 + tickpos,
                                                tickyoff1,
                                                _markWidth + 1 + tickpos,
                                                tickyoff1 + _tickHeight);
                        }
                        if( _scaleOrientation == TopBottomOrientation.Top
                            || _scaleOrientation == TopBottomOrientation.Both )
                        {
                            e.Graphics.DrawLine(penShadowDark, _markWidth + 1 + tickpos,
                                                tickyoff2,
                                                _markWidth + 1 + tickpos,
                                                tickyoff2 + _tickHeight);
                        }
                    }
                }

                drawRangeText(e, tickyoff1);

                drawKnobs(e.Graphics, markyoff, penHighlight, penShadowDark, penShadowLight);
            }
        }

        private void drawColoredRegion(int baryoff, Graphics g)
        {
            int left = _posLeft;
            int right = _posRight;
            int leftLight = _posLeft;
            int rightLight = _posLeft;
            switch (_colorFunction)
            {
                case ColorFunction.Setting:
                    break;
                case ColorFunction.Adding:
                    if( _posValue < _posLeft )
                    {
                        leftLight = _posValue;
                        rightLight = _posLeft;
                    }
                    else if( _posValue > _posRight )
                    {
                        leftLight = _posRight;
                        rightLight = _posValue;
                    }
                    break;
                case ColorFunction.Removing:
                    if (!(_posValue < _posLeft || _posValue > _posRight))
                    {
                        if (_posRight - _posValue > _posValue - _posLeft)
                        {
                            left = _posValue;
                        }
                        else
                        {
                            right = _posValue;
                        }
                    }
                    break;
            }
            // fill colored region
            g.FillRectangle(_brushInner, left, baryoff + _sizeShadow, right - left,
                                     _barHeight - 1 - 2 * _sizeShadow);
            if (leftLight != rightLight)
            {
                g.FillRectangle(_brushInnerLight, leftLight, baryoff + _sizeShadow, rightLight - leftLight,
                                _barHeight - 1 - 2*_sizeShadow);
            }
        }

        private void drawKnobs(Graphics g, int markyoff, Pen penHighlight, Pen penShadowDark, Pen penShadowLight )
        {
            bool isLeftEnabled = true;
            bool isRightEnabled = true;
            switch( _colorFunction )
            {
                case ColorFunction.Setting:
                    drawValue(g, penHighlight);
                    break;
                case ColorFunction.Adding:
                    if( _posValue < _posLeft )
                    {
                        drawLeftKnob(g, _posValue, false, _pointValue, markyoff, penShadowDark, penShadowLight);
                    }
                    else if( _posValue > _posRight )
                    {
                        drawRightKnob(g, _posValue, false, _pointValue, markyoff, penShadowDark );    
                    }
                    else
                    {
                        drawValue(g, penHighlight);
                    }
                    break;
                case ColorFunction.Removing:
                    if (_posValue < _posLeft || _posValue > _posRight)
                    {
                        g.DrawLine(penHighlight, _posValue, markyoff, _posValue, markyoff + _markHeight);
                    }
                    else
                    {
                        if( _posRight - _posValue > _posValue - _posLeft )
                        {
                            drawLeftKnob(g, _posValue, false, _pointValue, markyoff, penShadowDark, penShadowLight);
                        }
                        else
                        {
                            drawRightKnob(g, _posValue, false, _pointValue, markyoff, penShadowDark);
                        }
                    }
                    break;
            }
            drawLeftKnob(g, _posLeft, isLeftEnabled, _pointsLeft, markyoff, penShadowDark, penShadowLight);
            drawRightKnob(g, _posRight, isRightEnabled, _pointsRight, markyoff, penShadowDark);
        }

        private void drawValue(Graphics g, Pen penHighlight)
        {
            int baryoff = (Height - _barHeight) / 2;
            g.DrawLine(penHighlight, _posValue, baryoff-1, _posValue, baryoff + _barHeight+1);
        }

        private void drawRightKnob(Graphics g, int position, bool isSolid, Point[] points, int markyoff, Pen penShadowDark )
        {
            points[0].X = position + _markWidth;
            points[0].Y = markyoff + _markHeight / 3;
            points[1].X = position;
            points[1].Y = markyoff;
            points[2].X = position;
            points[2].Y = markyoff + _markHeight;
            points[3].X = position + _markWidth;
            points[3].Y = markyoff + 2 * _markHeight / 3;
            points[4].X = position + _markWidth;
            points[4].Y = markyoff;
            g.FillPolygon(isSolid ? _brushFill : _brushFillLight, points);

            // Left shadow
            g.DrawLine(penShadowDark, points[2].X, points[2].Y, points[3].X, points[3].Y); 
            // lower Right shadow
            g.DrawLine(penShadowDark, points[0].X, points[0].Y, points[1].X, points[1].Y);
            // upper shadow
            g.DrawLine(penShadowDark, points[0].X, points[0].Y + 1, points[3].X, points[3].Y);

            if (!isSolid) addText(g, markyoff, _markWidth/2);
        }

        private void drawLeftKnob(Graphics g, int position, bool isSolid, Point[] points, int markyoff, Pen penShadowDark, Pen penShadowLight)
        {
            points[0].X = position - _markWidth;
            points[0].Y = markyoff + _markHeight/3;
            points[1].X = position;
            points[1].Y = markyoff;
            points[2].X = position;
            points[2].Y = markyoff + _markHeight;
            points[3].X = position - _markWidth;
            points[3].Y = markyoff + 2*_markHeight/3;
            points[4].X = position - _markWidth;
            points[4].Y = markyoff;
            g.FillPolygon(isSolid ? _brushFill : _brushFillLight, points);

            g.DrawLine(penShadowDark, points[3].X - 1, points[3].Y, points[1].X - 1, points[2].Y);
            // lower Left shadow
            g.DrawLine(penShadowLight, points[0].X - 1, points[0].Y, points[0].X - 1, points[3].Y);
            // Left shadow				
            g.DrawLine(penShadowLight, points[0].X - 1, points[0].Y, points[1].X - 1, points[1].Y);

            if( !isSolid ) addText(g, markyoff, -_markWidth/2);
        }

        private void addText(Graphics g, int markyoff, int offset)
        {
            g.FillPie( _brushYellow, _posValue + offset - 6, markyoff-3, 10, 10, 0, 360 );
            string plusMinus = _colorFunction == ColorFunction.Adding ? "+" : "-";
            g.DrawString(plusMinus, _fontMark, SystemBrushes.ControlText, _posValue + offset, markyoff-6, _strformat);
        }

        private void drawRangeText(PaintEventArgs e, int tickyoff1)
        {
            //BIG HACK  - * 16 as we can't step
            e.Graphics.DrawString((_rangeMin * 16).ToString(), _fontMark, SystemBrushes.ControlText, _posLeft,
                                  tickyoff1 + _tickHeight, _strformat);
            e.Graphics.DrawString((_rangeMax * 16).ToString(), _fontMark, SystemBrushes.ControlText, _posRight,
                                  tickyoff1 + _tickHeight, _strformat);
        }

        // mouse down event
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if( Enabled )
            {
                Rectangle LMarkRect = new Rectangle(
                    Math.Min(_pointsLeft[0].X, _pointsLeft[1].X), // X
                    Math.Min(_pointsLeft[0].Y, _pointsLeft[3].Y), // Y
                    Math.Abs(_pointsLeft[2].X - _pointsLeft[0].X), // width
                    Math.Max(Math.Abs(_pointsLeft[0].Y - _pointsLeft[3].Y), Math.Abs(_pointsLeft[0].Y - _pointsLeft[1].Y)));
                    // height
                Rectangle RMarkRect = new Rectangle(
                    Math.Min(_pointsRight[0].X, _pointsRight[2].X), // X
                    Math.Min(_pointsRight[0].Y, _pointsRight[1].Y), // Y
                    Math.Abs(_pointsRight[0].X - _pointsRight[2].X), // width
                    Math.Max(Math.Abs(_pointsRight[2].Y - _pointsRight[0].Y), Math.Abs(_pointsRight[1].Y - _pointsRight[0].Y)));
                    // height

                if( LMarkRect.Contains(e.X, e.Y) )
                {
                    Capture = true;
                    _moveLMark = true;
                    //_activeMark = ActiveMarkType.Left;							
                    _activeMark = ActiveMarkType.None;
                    Invalidate(true);
                }

                if( RMarkRect.Contains(e.X, e.Y) )
                {
                    Capture = true;
                    _moveRMark = true;
                    //_activeMark = ActiveMarkType.Right;							
                    _activeMark = ActiveMarkType.None;
                    Invalidate(true);
                }
            }
        }


        // mouse up event
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if( Enabled )
            {
                Capture = false;

                _moveLMark = false;
                _moveRMark = false;

                Invalidate();

                OnRangeChanged(EventArgs.Empty);
            }
        }


        // mouse move event
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if( Enabled )
            {
                Rectangle LMarkRect = new Rectangle(
                    Math.Min(_pointsLeft[0].X, _pointsLeft[1].X), // X
                    Math.Min(_pointsLeft[0].Y, _pointsLeft[3].Y), // Y
                    Math.Abs(_pointsLeft[2].X - _pointsLeft[0].X), // width
                    Math.Max(Math.Abs(_pointsLeft[0].Y - _pointsLeft[3].Y), Math.Abs(_pointsLeft[0].Y - _pointsLeft[1].Y)));
                    // height
                Rectangle RMarkRect = new Rectangle(
                    Math.Min(_pointsRight[0].X, _pointsRight[2].X), // X
                    Math.Min(_pointsRight[0].Y, _pointsRight[1].Y), // Y
                    Math.Abs(_pointsRight[0].X - _pointsRight[2].X), // width
                    Math.Max(Math.Abs(_pointsRight[2].Y - _pointsRight[0].Y), Math.Abs(_pointsRight[1].Y - _pointsRight[0].Y)));
                    // height

                if( LMarkRect.Contains(e.X, e.Y) || RMarkRect.Contains(e.X, e.Y) )
                {
                    if( _barOrientation == RangeBarOrientation.Horizontal )
                        Cursor = Cursors.SizeWE;
                    else
                        Cursor = Cursors.SizeNS;
                }
                else Cursor = Cursors.Arrow;

                if( _moveLMark )
                {
                    if( _barOrientation == RangeBarOrientation.Horizontal )
                        Cursor = Cursors.SizeWE;
                    else
                        Cursor = Cursors.SizeNS;
                    if( _barOrientation == RangeBarOrientation.Horizontal )
                        _posLeft = e.X;
                    else
                        _posLeft = e.Y;
                    if( _posLeft < _xPosMin )
                        _posLeft = _xPosMin;
                    if( _posLeft > _xPosMax )
                        _posLeft = _xPosMax;
                    if( _posRight < _posLeft )
                        _posRight = _posLeft;
                    if( pos2Range() )
                    {
                        _activeMark = ActiveMarkType.Left;
                        Invalidate(true);
                        OnRangeChanging(EventArgs.Empty);
                    }
                }
                else if( _moveRMark )
                {
                    if( _barOrientation == RangeBarOrientation.Horizontal )
                        Cursor = Cursors.SizeWE;
                    else
                        Cursor = Cursors.SizeNS;
                    if( _barOrientation == RangeBarOrientation.Horizontal )
                        _posRight = e.X;
                    else
                        _posRight = e.Y;
                    if( _posRight > _xPosMax )
                        _posRight = _xPosMax;
                    if( _posRight < _xPosMin )
                        _posRight = _xPosMin;
                    if( _posLeft > _posRight )
                        _posLeft = _posRight;
                    if( pos2Range() )
                    {
                        _activeMark = ActiveMarkType.Right;
                        Invalidate(true);
                        OnRangeChanging(EventArgs.Empty);
                    }
                }
            }
        }


        /// <summary>
        ///  transform pixel position to range position
        /// </summary>
        private bool pos2Range()
        {
            double oldRangeMin = _rangeMin;
            double oldRangeMax = _rangeMax;
            int w;
            int posw;

            if( _barOrientation == RangeBarOrientation.Horizontal )
                w = Width;
            else
                w = Height;
            posw = w - 2*_markWidth - 2;

            _rangeMin = _minimum + (int) Math.Round((_maximum - _minimum)*(_posLeft - _xPosMin)/posw);
            _rangeMax = _minimum + (int) Math.Round((_maximum - _minimum)*(_posRight - _xPosMin)/posw);
            return oldRangeMin != _rangeMin || oldRangeMax != _rangeMax;
        }


        /// <summary>
        ///  transform range position to pixel position
        /// </summary>
        private void Range2Pos()
        {
            int width = _barOrientation == RangeBarOrientation.Horizontal ? Width : Height;
            int totalPosWidth = width - 2*_markWidth - 2;
            _posLeft = _xPosMin + (int) Math.Round(totalPosWidth*(_rangeMin - _minimum)/(_maximum - _minimum));
            _posRight = _xPosMin + (int) Math.Round(totalPosWidth*(_rangeMax - _minimum)/(_maximum - _minimum));
        }

        private void Value2Pos()
        {            
            int width = _barOrientation == RangeBarOrientation.Horizontal ? Width : Height;
            int totalPosWidth = width - 2 * _markWidth - 2;
            _posValue = _xPosMin + (int)Math.Round(totalPosWidth * (_value - _minimum) / (_maximum - _minimum));
        }


        /// <summary>
        /// method to handle resize event
        /// </summary>
        /// <param name="sender">object that sends event to resize</param>
        /// <param name="e">event parameter</param>
        private void OnResize(object sender, EventArgs e)
        {
            //Range2Pos();
            Invalidate(true);
        }


        /// <summary>
        /// method to handle key pressed event
        /// </summary>
        /// <param name="sender">object that sends key pressed event</param>
        /// <param name="e">event parameter</param>
        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if( Enabled )
            {
                if( _activeMark == ActiveMarkType.Left )
                {
                    if( e.KeyChar == '+' )
                    {
                        _rangeMin++;
                        if( _rangeMin > _maximum )
                            _rangeMin = _maximum;
                        if( _rangeMax < _rangeMin )
                            _rangeMax = _rangeMin;
                        OnRangeChanged(EventArgs.Empty);
                    }
                    else if( e.KeyChar == '-' )
                    {
                        _rangeMin--;
                        if( _rangeMin < _minimum )
                            _rangeMin = _minimum;
                        OnRangeChanged(EventArgs.Empty);
                    }
                }
                else if( _activeMark == ActiveMarkType.Right )
                {
                    if( e.KeyChar == '+' )
                    {
                        _rangeMax++;
                        if( _rangeMax > _maximum )
                            _rangeMax = _maximum;
                        OnRangeChanged(EventArgs.Empty);
                    }
                    else if( e.KeyChar == '-' )
                    {
                        _rangeMax--;
                        if( _rangeMax < _minimum )
                            _rangeMax = _minimum;
                        if( _rangeMax < _rangeMin )
                            _rangeMin = _rangeMax;
                        OnRangeChanged(EventArgs.Empty);
                    }
                }
                Invalidate(true);
            }
        }


        private void OnLoad(object sender, EventArgs e)
        {
            // use double buffering
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            Invalidate(true);
            Update();
        }

        private void OnLeave(object sender, EventArgs e)
        {
            _activeMark = ActiveMarkType.None;
        }


        public event RangeChangedEventHandler RangeChanged; // event handler for range changed
        public event RangeChangedEventHandler RangeChanging; // event handler for range is changing

        public virtual void OnRangeChanged(EventArgs e)
        {
            if( RangeChanged != null )
                RangeChanged(this, e);
        }

        public virtual void OnRangeChanging(EventArgs e)
        {
            if( RangeChanging != null )
                RangeChanging(this, e);
        }

        private ColorFunction _colorFunction;
        public ColorFunction ColorFunction
        {
            get { return _colorFunction; }
            set
            {
                _colorFunction = value;
                Invalidate();
            }
        }
    }
}
