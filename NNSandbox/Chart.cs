using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NNSandbox {
    public partial class Chart : UserControl {
        public ResultDataPoint FirstSuccess { get; set; }
        private readonly List<ResultDataPoint> dataPoints;

        private readonly Pen lossPen = new(Color.Red, 2);
        private readonly Pen accuracyPen = new(Color.Blue, 2);
        private readonly Pen axisPen = new(Color.Black, 1);
        private readonly Pen lightAxisPen = new(Color.LightGray, 1);
        private readonly Brush axisBrush = new SolidBrush(Color.Black);
        private readonly int leftIndent = 40;
        private readonly double verticalPadding = 0.05;

        private bool firstSuccessDrawn;

        public Chart() {
            InitializeComponent();
            dataPoints = new List<ResultDataPoint>();
            SizeChanged += Chart_SizeChanged;
        }

        private void Chart_SizeChanged(object sender, System.EventArgs e) {
            Invalidate();
            firstSuccessDrawn = false;
        }

        public void AddPoint(ResultDataPoint p) {
            using(Graphics g = CreateGraphics()) {
                if (dataPoints.Any()) {
                    DrawNextPoint(g, p);
                } else {
                    DrawFirstPoint(g, p);
                }
            }
            dataPoints.Add(p);
        }

        public void Flush() {
            dataPoints.Clear();
            firstSuccessDrawn = false;
            FirstSuccess = null;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e) {
            e.Graphics.Clear(Color.White);
            RedrawAxis(e.Graphics);
            RedrawData(e.Graphics);
        }

        private void RedrawAxis(Graphics graphics) {
            int topBoundary = (int)(ClientSize.Height * verticalPadding);
            int bottomBounday = (int)(ClientSize.Height * (1 - verticalPadding));

            graphics.DrawLine(lightAxisPen, new Point(0, topBoundary), new Point(ClientSize.Width, topBoundary));
            graphics.DrawString("100", Font, axisBrush, new PointF(1, topBoundary));

            graphics.DrawLine(axisPen, new Point(0, bottomBounday), new Point(ClientSize.Width, bottomBounday));
            graphics.DrawString("0", Font, axisBrush, new PointF(1, bottomBounday));

            graphics.DrawLine(axisPen, new Point(leftIndent, 0), new Point(leftIndent, ClientSize.Height));
        }

        private void RedrawData(Graphics graphics) {
            if (!dataPoints.Any()) return;

            DrawFirstLossMark(graphics, dataPoints.First().Loss);
            
            Point[] accuracyPoints = new Point[dataPoints.Count];
            for (int i = 0; i < dataPoints.Count; i++) {
                accuracyPoints[i] = TransformPoint(i + 1, dataPoints[i].Accuracy);
                if (!firstSuccessDrawn && FirstSuccess != null && FirstSuccess.EpochId <= dataPoints[i].EpochId)
                    DrawFirstSuccessMark(graphics, i + 1, FirstSuccess.EpochId.ToString());
            }
            graphics.DrawLines(accuracyPen, accuracyPoints);

            Point[] lossPoints = new Point[dataPoints.Count];
            for (int i = 0; i < dataPoints.Count; i++)
                lossPoints[i] = TransformPoint(i + 1, dataPoints[i].Loss);
            graphics.DrawLines(lossPen, lossPoints);
        }

        private void DrawFirstPoint(Graphics graphics, ResultDataPoint point) {
            DrawFirstLossMark(graphics, point.Loss);
            Point accuracy = TransformPoint(1, point.Accuracy);
            Point loss = TransformPoint(1, point.Loss);
            graphics.DrawLine(accuracyPen, accuracy, accuracy);
            graphics.DrawLine(lossPen, loss, loss);
        }

        private void DrawNextPoint(Graphics graphics, ResultDataPoint point) {
            ResultDataPoint previous = dataPoints.Last();
            int pointCount = dataPoints.Count;
            if (!firstSuccessDrawn && FirstSuccess != null && FirstSuccess.EpochId <= point.EpochId)
                DrawFirstSuccessMark(graphics, pointCount + 1, FirstSuccess.EpochId.ToString());

            graphics.DrawLine(accuracyPen, TransformPoint(pointCount, previous.Accuracy), TransformPoint(pointCount + 1, point.Accuracy));
            graphics.DrawLine(lossPen, TransformPoint(pointCount, previous.Loss), TransformPoint(pointCount + 1, point.Loss));
        }

        private void DrawFirstLossMark(Graphics graphics, double loss) {
            int yCoord = TransformPoint(1, loss).Y;
            graphics.DrawLine(lightAxisPen, new Point(1, yCoord), new Point(leftIndent, yCoord));
            graphics.DrawString($"{loss:0.###}", Font, axisBrush, new PointF(1, yCoord));
        }

        private void DrawFirstSuccessMark(Graphics graphics, int x, string text) {
            int xCoord = x + leftIndent;
            graphics.DrawLine(lightAxisPen, new Point(xCoord, 0), TransformPoint(x, 1));
            graphics.DrawString(text, Font, axisBrush, new PointF(xCoord, 0));
            firstSuccessDrawn = true;
        }

        private Point TransformPoint(int x, double y) {
            double verticalPosition = 1 - verticalPadding - y * (1 - 2 * verticalPadding);
            return new Point(x + leftIndent, (int)(ClientSize.Height * verticalPosition));
        }
    }
}
