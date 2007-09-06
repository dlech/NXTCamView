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
            this.Name = "ZzzzRangeBar";
            this.Size = new System.Drawing.Size(344, 34);
            this.Load += new System.EventHandler(this.OnLoad);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            this.Leave += new System.EventHandler(this.OnLeave);
            this.Resize += new System.EventHandler(this.OnResize);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            this.SizeChanged += new System.EventHandler(this.OnSizeChanged);
            this.ResumeLayout(false);
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
        private Color _colorRange = SystemColors.ControlLightLight;
        private Color _colorShadowLight = SystemColors.ControlLightLight;
        private Color _colorShadowDark = SystemColors.ControlDarkDark;
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

        private Point[] _markLeft = new Point[5];
        private Point[] _markRight = new Point[5];

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

        public int RangeMaximum
        {
            set
            {
                _rangeMax = value;
                if( _rangeMax < _minimum )
                    _rangeMax = _minimum;
                else if( _rangeMax > _maximum )
                    _rangeMax = _maximum;
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
                _maximum = (double) value;
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
                _minimum = (double) value;
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

        private Color colorBackground = SystemColors.ControlLight;

        public Color ColorBackground
        {
            get { return colorBackground; }
            set
            {
                colorBackground = value;
                Refresh();
            }
        }

        /// <summary>
        /// set selected range
        /// </summary>
        /// <param name="left">Left side of range</param>
        /// <param name="right">Right side of range</param>
        public void SelectRange(int left, int right)
        {
            RangeMinimum = left;
            RangeMaximum = right;
            Range2Pos();
            Invalidate(true);
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
            int baryoff, markyoff, tickyoff1, tickyoff2;
            double dtick;
            int tickpos;
            Pen penShadowLight = new Pen(_colorShadowLight);
            Pen penShadowDark = new Pen(_colorShadowDark);
            SolidBrush brushInner;
            SolidBrush brushRange = new SolidBrush(_colorRange);
            SolidBrush brushBackground = new SolidBrush(colorBackground);

            if( Enabled )
                brushInner = new SolidBrush(_colorInner);
            else
                brushInner = new SolidBrush(Color.FromKnownColor(KnownColor.InactiveCaption));

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

            if( _barOrientation == RangeBarOrientation.Horizontal )
            {
                baryoff = (h - _barHeight)/2;
                markyoff = baryoff + (_barHeight - _markHeight)/2 - 1;

                // draw frame of slider
                //e.Graphics.FillRectangle(brushShadowDark,0,baryoff,w-1,_sizeShadow);	// Top
                //e.Graphics.FillRectangle(brushShadowDark,0,baryoff,_sizeShadow,_barHeight-1);	// Left
                //e.Graphics.FillRectangle(brushShadowLight,0,baryoff + _barHeight - 1 - _sizeShadow,w-1,_sizeShadow);	// Bottom
                //e.Graphics.FillRectangle(brushShadowLight,w-1-_sizeShadow,baryoff,_sizeShadow,_barHeight-1);	// Right

                //draw background
                e.Graphics.FillRectangle(brushBackground, 0, baryoff, w - 1, _barHeight - 1);

                // fill colored region
                e.Graphics.FillRectangle(brushInner, _posLeft, baryoff + _sizeShadow, _posRight - _posLeft,
                                         _barHeight - 1 - 2*_sizeShadow);

                // Skala
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

                if( _axisDivision > 1 )
                {
                    dtick = (double) (_xPosMax - _xPosMin)/(double) _axisDivision;
                    for( int i = 0; i < _axisDivision + 1; i++ )
                    {
                        tickpos = (int) Math.Round((double) i*dtick);
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

                // Left mark knob				
                _markLeft[0].X = _posLeft - _markWidth;
                _markLeft[0].Y = markyoff + _markHeight/3;
                _markLeft[1].X = _posLeft;
                _markLeft[1].Y = markyoff;
                _markLeft[2].X = _posLeft;
                _markLeft[2].Y = markyoff + _markHeight;
                _markLeft[3].X = _posLeft - _markWidth;
                _markLeft[3].Y = markyoff + 2*_markHeight/3;
                _markLeft[4].X = _posLeft - _markWidth;
                _markLeft[4].Y = markyoff;
                e.Graphics.FillPolygon(brushRange, _markLeft);
                e.Graphics.DrawLine(penShadowDark, _markLeft[3].X - 1, _markLeft[3].Y, _markLeft[1].X - 1,
                                    _markLeft[2].Y);
                // lower Left shadow
                e.Graphics.DrawLine(penShadowLight, _markLeft[0].X - 1, _markLeft[0].Y, _markLeft[0].X - 1,
                                    _markLeft[3].Y);
                // Left shadow				
                e.Graphics.DrawLine(penShadowLight, _markLeft[0].X - 1, _markLeft[0].Y, _markLeft[1].X - 1,
                                    _markLeft[1].Y);
                // upper shadow
                if( _posLeft < _posRight )
                    e.Graphics.DrawLine(penShadowDark, _markLeft[1].X, _markLeft[1].Y + 1, _markLeft[1].X,
                                        _markLeft[2].Y);
                // Right shadow
                //if( _activeMark == ActiveMarkType.Left )
                //{
                //    e.Graphics.DrawLine(penShadowLight, _posLeft - _markWidth/2 - 1, markyoff + _markHeight/3, _posLeft - _markWidth/2 - 1,
                //                        markyoff + 2*_markHeight/3); // active mark
                //    e.Graphics.DrawLine(penShadowDark, _posLeft - _markWidth/2, markyoff + _markHeight/3, _posLeft - _markWidth/2,
                //                        markyoff + 2*_markHeight/3); // active mark			
                //}


                // Right mark knob
                _markRight[0].X = _posRight + _markWidth;
                _markRight[0].Y = markyoff + _markHeight/3;
                _markRight[1].X = _posRight;
                _markRight[1].Y = markyoff;
                _markRight[2].X = _posRight;
                _markRight[2].Y = markyoff + _markHeight;
                _markRight[3].X = _posRight + _markWidth;
                _markRight[3].Y = markyoff + 2*_markHeight/3;
                _markRight[4].X = _posRight + _markWidth;
                _markRight[4].Y = markyoff;
                e.Graphics.FillPolygon(brushRange, _markRight);
                if( _posLeft < _posRight )
                    e.Graphics.DrawLine(penShadowLight, _markRight[1].X - 1, _markRight[1].Y + 1, _markRight[2].X - 1,
                                        _markRight[2].Y);
                // Left shadow
                e.Graphics.DrawLine(penShadowDark, _markRight[2].X, _markRight[2].Y, _markRight[3].X, _markRight[3].Y);
                // lower Right shadow
                e.Graphics.DrawLine(penShadowDark, _markRight[0].X, _markRight[0].Y, _markRight[1].X, _markRight[1].Y);
                    // upper shadow
                e.Graphics.DrawLine(penShadowDark, _markRight[0].X, _markRight[0].Y + 1, _markRight[3].X,
                                    _markRight[3].Y);
                // Right shadow
                //if( _activeMark == ActiveMarkType.Right )
                //{
                //    e.Graphics.DrawLine(penShadowLight, _posRight + _markWidth/2 - 1, markyoff + _markHeight/3, _posRight + _markWidth/2 - 1,
                //                        markyoff + 2*_markHeight/3); // active mark
                //    e.Graphics.DrawLine(penShadowDark, _posRight + _markWidth/2, markyoff + _markHeight/3, _posRight + _markWidth/2,
                //                        markyoff + 2*_markHeight/3); // active mark				
                //}

                //if (_moveLMark)
                //{
                //    Font fontMark = new Font("Arial", _markWidth);
                //    SolidBrush brushMark = new SolidBrush(_colorShadowDark);
                //    StringFormat strformat = new StringFormat();
                //    strformat.Alignment = StringAlignment.Center;
                //    strformat.LineAlignment = StringAlignment.Near;
                //    e.Graphics.DrawString(_rangeMin.ToString(), fontMark, brushMark, _posLeft, tickyoff1 + _tickHeight, strformat);
                //}

                //if (_moveRMark)
                //{
                //    Font fontMark = new Font("Arial", _markWidth);
                //    SolidBrush brushMark = new SolidBrush(_colorShadowDark);
                //    StringFormat strformat = new StringFormat();
                //    strformat.Alignment = StringAlignment.Center;
                //    strformat.LineAlignment = StringAlignment.Near;
                //    e.Graphics.DrawString(_rangeMax.ToString(), fontMark, brushMark, _posRight, tickyoff1 + _tickHeight, strformat);
                //}
                Font fontMark = new Font("Arial", _markWidth);
                //SolidBrush brushMark = new SolidBrush(_colorShadowDark);
                StringFormat strformat = new StringFormat();
                strformat.Alignment = StringAlignment.Center;
                strformat.LineAlignment = StringAlignment.Near;
                //BIG HACK  - * 16 as we can't step
                e.Graphics.DrawString((_rangeMin*16).ToString(), fontMark, SystemBrushes.ControlText, _posLeft,
                                      tickyoff1 + _tickHeight, strformat);
                e.Graphics.DrawString((_rangeMax*16).ToString(), fontMark, SystemBrushes.ControlText, _posRight,
                                      tickyoff1 + _tickHeight, strformat);
            }
        }


        // mouse down event
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if( this.Enabled )
            {
                Rectangle LMarkRect = new Rectangle(
                    Math.Min(_markLeft[0].X, _markLeft[1].X), // X
                    Math.Min(_markLeft[0].Y, _markLeft[3].Y), // Y
                    Math.Abs(_markLeft[2].X - _markLeft[0].X), // width
                    Math.Max(Math.Abs(_markLeft[0].Y - _markLeft[3].Y), Math.Abs(_markLeft[0].Y - _markLeft[1].Y)));
                    // height
                Rectangle RMarkRect = new Rectangle(
                    Math.Min(_markRight[0].X, _markRight[2].X), // X
                    Math.Min(_markRight[0].Y, _markRight[1].Y), // Y
                    Math.Abs(_markRight[0].X - _markRight[2].X), // width
                    Math.Max(Math.Abs(_markRight[2].Y - _markRight[0].Y), Math.Abs(_markRight[1].Y - _markRight[0].Y)));
                    // height

                if( LMarkRect.Contains(e.X, e.Y) )
                {
                    this.Capture = true;
                    _moveLMark = true;
                    //_activeMark = ActiveMarkType.Left;							
                    _activeMark = ActiveMarkType.None;
                    Invalidate(true);
                }

                if( RMarkRect.Contains(e.X, e.Y) )
                {
                    this.Capture = true;
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
            if( this.Enabled )
            {
                this.Capture = false;

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
                    Math.Min(_markLeft[0].X, _markLeft[1].X), // X
                    Math.Min(_markLeft[0].Y, _markLeft[3].Y), // Y
                    Math.Abs(_markLeft[2].X - _markLeft[0].X), // width
                    Math.Max(Math.Abs(_markLeft[0].Y - _markLeft[3].Y), Math.Abs(_markLeft[0].Y - _markLeft[1].Y)));
                    // height
                Rectangle RMarkRect = new Rectangle(
                    Math.Min(_markRight[0].X, _markRight[2].X), // X
                    Math.Min(_markRight[0].Y, _markRight[1].Y), // Y
                    Math.Abs(_markRight[0].X - _markRight[2].X), // width
                    Math.Max(Math.Abs(_markRight[2].Y - _markRight[0].Y), Math.Abs(_markRight[1].Y - _markRight[0].Y)));
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
                    if( this._barOrientation == RangeBarOrientation.Horizontal )
                        this.Cursor = Cursors.SizeWE;
                    else
                        this.Cursor = Cursors.SizeNS;
                    if( this._barOrientation == RangeBarOrientation.Horizontal )
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
            int w;
            int posw;

            if( _barOrientation == RangeBarOrientation.Horizontal )
                w = Width;
            else
                w = Height;
            posw = w - 2*_markWidth - 2;

            _posLeft = _xPosMin + (int) Math.Round(posw*(_rangeMin - _minimum)/(_maximum - _minimum));
            _posRight = _xPosMin + (int) Math.Round(posw*(_rangeMax - _minimum)/(_maximum - _minimum));
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
            if( this.Enabled )
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
    }
}
