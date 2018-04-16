using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using Visualization;
using Tao.OpenGl;

namespace Testing_openTK_again
{

    public partial class Form2 : Form
    {
        public class MouseData
        {
            public static int x;
            public double mouseX { set; get; }
            public double mouseY { set; get; }
            public bool leftClicked { set; get; }
            public bool rightClicked { set; get; }
        }
        public class TransformationData
        {
            public double translateX { set; get; }
            public double translateY { set; get; }
            public double translateZ { set; get; }
            public double scaleX { set; get; }
            public double rotationAngle { set; get; }
            public double rotateX { get; internal set; }
            public double translationSpeed = 150.0;
        }
        List<List<Vertex>> contourLines = new List<List<Vertex>>();
        List<double[]> contourColors = new List<double[]>();
        List<Shape> shapes = new List<Shape>();
        bool loaded = false;
        Mesh m;
        double min, max;
        uint dataIndex = 0;
        bool edgeColorEnabled = true;

        MouseData mouseData = new MouseData();
        TransformationData tranformationData = new TransformationData();
        Del valueToColorHandler;
        public Form2()
        {
            InitializeComponent();
            valueToColorHandler = colorMap1.ValueToColor;
            comboBox2.Items.Add("Edge");
            comboBox2.Items.Add("Face");
            comboBox2.SelectedItem = comboBox2.Items[0];
            colorMap1.IndexChanged += OnModeChanged;
            numericUpDown1.Value = 10;
            floodBox.Checked = true;
        }
        private void OnModeChanged(object sender, EventArgs e)
        {
            if (m != null)
            {
                CalculateColors(!edgeColorEnabled);
                glControl1.Invalidate();
            }
        }
        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (!loaded)
                return;
        }
        private void glControl1_Load(object sender, EventArgs e)
        {
            //m = new Mesh("C:\\Users\\runmd\\Desktop\\College 4th yera\\Visualisation\\Labs\\Lab 2 Material\\Visualization Objects\\big.dat");

            loaded = true;

            SetupViewport();
        }
        private Point3 Lerp(Point3 c1, Point3 c2, double t)
        {
            return Point3.Interpolate(t, c1, c2);
        }
        private  Point3 process(double contourValue, Point3 a, Point3 b, double d1, double d2)
        {
            double alpha = (contourValue - Math.Min(d1, d2)) / Math.Abs(d1 - d2);
            Point3 p1;
            Point3 p2;
            if (d1 <= d2)
            {
                p1 = a;
                p2 = b;
            }
            else
            {
                p2 = a;
                p1 = b;
            }
            return Lerp(p1, p2, alpha);
        }


        void drawShapes()
        {

            if (firstTimeDrawingContours)
            {
                contoursMatrix = m.Transformation.Clone();
            }
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPushMatrix();
            Gl.glMultMatrixd(contoursMatrix.Data);
            Random rand = new Random();
            foreach (Shape s in shapes)
            {
                Gl.glColor3d(s.color[0], s.color[1], s.color[2]);

                ////Gl.glColor3d(rand.NextDouble(), rand.NextDouble(), rand.NextDouble());
                //var x = map(mouseData.mouseX, 0, glControl1.Width, 0, 1);
                //var y = map(mouseData.mouseY, 0, glControl1.Height, 0, 1);
                //Gl.glColor3d(x, y, s.color[2]);
                Gl.glBegin(Gl.GL_POLYGON);

                foreach (Point3 p in s.verts)
                {

                    p.glTell();

                }
                Gl.glEnd();

            }
            Gl.glPopMatrix();
        }
        private void SetupViewport()
        {

            int w = glControl1.Width;
            int h = glControl1.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            //GL.Ortho(-1, 1, -1, 1, -1, 1);
            Glu.gluPerspective(60.0, glControl1.Width / glControl1.Height, 0.01, 1000000);
            // GL.Viewport(0, 0, w, h);
            GL.ClearColor(0.9f, 0.9f, 0.9f, 1);
            Gl.glClearColor(1f, 1f, 1f, 1);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
        }

        
	
       
    private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            if (m != null)
            {
                m.Transformation.Translate(tranformationData.translateX * tranformationData.translationSpeed, tranformationData.translateY * tranformationData.translationSpeed, 0);
                m.Transformation = Matrix.Translation(tranformationData.translateX * tranformationData.translationSpeed,
                    tranformationData.translateY * tranformationData.translationSpeed,
                    tranformationData.translateZ) * m.Transformation;
                tranformationData.translateX = 0;
                tranformationData.translateY = 0;
                tranformationData.translateZ = 0;

                var mat = m.Transformation.Data;

                m.Transformation *=
                    //Matrix.Translation(-mat[3], -mat[(1 << 2) + 3], -mat[(2 << 2) + 3])
                    Matrix.RotationX(tranformationData.rotationAngle);
                //* Matrix.RotationZ(tranformationData.rotationAngle)
                //* Matrix.Translation(mat[3], mat[(1 << 2) + 3], mat[(2 << 2) + 3]);

                //* m.Transformation;


                if (edgeColorEnabled)
                {
                    // m.glDrawColoredEdges();
                    //?m.glDraw();
                }

                else
                {
                    //m.glDraw();
                    //m.glDrawColoredFaces();
                }
                drawIsoSurface();
                tranformationData.rotationAngle = 0;
            }

            //if (m != null)
            //{
            //    m.Transformation.Translate(tranformationData.translateX * tranformationData.translationSpeed, tranformationData.translateY * tranformationData.translationSpeed, 0);
            //    m.glDraw();
            //    //if (edgeColorEnabled)
            //    //{
            //    //    m.glDrawColoredEdges();
            //    //    if(floodBox.Checked)
            //    //      drawShapes();
            //    //    DrawContours();

            //    //}
            //    //else
            //    //{
            //    //    m.glDrawColoredFaces();
            //    //    if (floodBox.Checked)
            //    //        drawShapes();

            //    //    DrawContours();

            //    //}
            //}
            glControl1.SwapBuffers();

        }
        public void AddItem(double item)
        {
            this.comboBox3.Items.Add(item);
        }
       
        List<IsoTri> isoTri = new List<IsoTri>();

        public void MarchingCube(double isoValue)
        {
            isoTri = new List<IsoTri>();
            //double iso = double.Parse( isoValueText.Text);
            double iso = isoValue;

            foreach (Zone z in m.Zones)
            {

                foreach (Element e in z.Elements)
                {

                    List<double> data = new List<double>();
                    for (int i = 0; i < e.vertInOrder.Length; ++i)
                    {
                        data.Add(z.Vertices[e.vertInOrder[i]].Data[dataIndex]);
                    }

                    var elementCase = ISOSurface.GetElementCase(data.ToArray(),iso );
                    var edges = ISOSurface.GetCaseEdges(elementCase);
                    for (int i = 0; i < edges.Length; i += 3)
                    {
                        if (edges[i] <0 || edges[i] > 16)
                            break;
                        isoTri.Add(new IsoTri());
                        //triangles.Add(new List<Point3>());
                        for (int j = 0; j < 3; ++j)
                        {
                            var edge = ISOSurface.GetEdgePoints(edges[i+j]);
                            var startVert = z.Vertices[e.vertInOrder[edge.Start]];
                            var endVert = z.Vertices[e.vertInOrder[edge.End]];
                            var alpha = (iso - startVert.Data[dataIndex]) / (endVert.Data[dataIndex] - startVert.Data[dataIndex]);
                            var p = Lerp(startVert.Position, endVert.Position, alpha);
                            //var p = process(iso, startVert.Position, endVert.Position, startVert.Data[dataIndex], startVert.Data[dataIndex]);
                            isoTri[isoTri.Count-1].triangle.Add(p);
                            Color c = colorMap1.ValueToColor(colorMap1.GetMode(), (float)(iso- endVert.Data[dataIndex] + startVert.Data[dataIndex]));
                            isoTri[isoTri.Count-1].color = new double[] { c.R / 255.0, c.G / 255.0, c.B / 255.0 };
                        }
                    }

                }
            }
            //MessageBox.Show(isoTri.Count.ToString());
        }
        public void MarchingCubeCount()
        {
            isoTri = new List<IsoTri>();
            int isoCount = (int)numericUpDown1.Value;
            m.GetMinMaxValues(dataIndex, out min, out max);
            double isoStep = (max - min) / isoCount;
            double iso = min + isoStep * 0.5;
            for (int ic = 0; ic < isoCount; ++ic)
            {
                foreach (Zone z in m.Zones)
                {

                    foreach (Element e in z.Elements)
                    {

                        List<double> data = new List<double>();
                        for (int i = 0; i < e.vertInOrder.Length; ++i)
                        {
                            data.Add(z.Vertices[e.vertInOrder[i]].Data[dataIndex]);
                        }

                        var elementCase = ISOSurface.GetElementCase(data.ToArray(), iso);
                        var edges = ISOSurface.GetCaseEdges(elementCase);
                        for (int i = 0; i < edges.Length; i += 3)
                        {
                            if (edges[i] < 0 || edges[i] > 16)
                                break;
                            isoTri.Add(new IsoTri());
                            //triangles.Add(new List<Point3>());
                            for (int j = 0; j < 3; ++j)
                            {
                                var edge = ISOSurface.GetEdgePoints(edges[i + j]);
                                var startVert = z.Vertices[e.vertInOrder[edge.Start]];
                                var endVert = z.Vertices[e.vertInOrder[edge.End]];
                                var alpha = (iso - startVert.Data[dataIndex]) / (endVert.Data[dataIndex] - startVert.Data[dataIndex]);
                                var p = Lerp(startVert.Position, endVert.Position, alpha);
                                //var p = process(iso, startVert.Position, endVert.Position, startVert.Data[dataIndex], startVert.Data[dataIndex]);
                                isoTri[isoTri.Count - 1].triangle.Add(p);
                                Color c = colorMap1.ValueToColor(colorMap1.GetMode(), (float)(iso));// (float)(iso - endVert.Data[dataIndex] + startVert.Data[dataIndex]));
                                isoTri[isoTri.Count - 1].color = new double[] { c.R / 255.0, c.G / 255.0, c.B / 255.0 };
                            }
                        }

                    }
                }
                iso += isoStep;
            }
            //MessageBox.Show(isoTri.Count.ToString());
        }
        private void drawIsoSurface()
        {
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPushMatrix();
            Gl.glMultMatrixd(m.Transformation.Data);
            foreach (var ts in isoTri)
            {
                Gl.glBegin(Gl.GL_TRIANGLES);
              
                Gl.glColor3d(ts.color[0],ts.color[1],ts.color[2]);
 
                foreach (var t in ts.triangle)
                {
                    t.glTell();
                }
                Gl.glEnd();

            }
            Gl.glPopMatrix();
            //MessageBox.Show(isoTri.Count.ToString());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = textBox1.Text = GetCorrectFileName(openFileDialog1.FileName);
                m = new Mesh(fileName);
                m.Transformation.Translate(0, 0, -200);
                AddItemsToComboBox(m);
                dataIndex = (uint)m.VarToIndex[comboBox1.SelectedItem.ToString()];
                m.GetMinMaxValues(dataIndex, out min, out max);

                colorMap1.setMax((float)max);
                colorMap1.setMin((float)min);

               //contourLines = VisualizationOperations.CalculateContours(m, (int)numericUpDown1.Value, dataIndex, ref contourColors, valueToColorHandler, colorMap1.GetMode());
                //CalcControusOnEdges();
                firstTimeDrawingContours = true;
                // shapes = VisualizationOperations.CalcShapes(m, (int)numericUpDown1.Value, dataIndex, valueToColorHandler, colorMap1.GetMode());
                isoTri = VisualizationOperations.CalcIsoSurface(m, dataIndex, double.Parse(isoValueText.Text), valueToColorHandler, colorMap1.GetMode());
               // MarchingCube(double.Parse(isoValueText.Text));
                glControl1.Invalidate();
            }
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (m == null)
                return;

            // contourLines = VisualizationOperations.CalculateContours(m, (int)numericUpDown1.Value, dataIndex, ref contourColors, valueToColorHandler, colorMap1.GetMode());

            //  CalcControusOnEdges();
            //shapes = VisualizationOperations.CalcShapes(m, (int)numericUpDown1.Value, dataIndex, valueToColorHandler, colorMap1.GetMode());
            //MarchingCube(double.Parse(isoValueText.Text));
            //MarchingCubeCount();
            isoTri = VisualizationOperations.CalcIsoSurfaces(m, dataIndex, (int)numericUpDown1.Value, valueToColorHandler, colorMap1.GetMode());
            glControl1.Invalidate();
        }
        private void AddItemsToComboBox(Mesh m)
        {
            comboBox1.Items.Clear();
            foreach (string s in m.VarToIndex.Keys)
                comboBox1.Items.Add(s);
            comboBox1.SelectedItem = comboBox1.Items[1];
        }
        private string GetCorrectFileName(string fileName)
        {
            for (int i = 0; i < fileName.Length; ++i)
                if (fileName[i] == '\\')
                    fileName = fileName.Insert(i++, "\\");
            return fileName;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataIndex = (uint)m.VarToIndex[comboBox1.SelectedItem.ToString()];
            m.GetMinMaxValues(dataIndex, out min, out max);
            colorMap1.setMax((float)max);
            colorMap1.setMin((float)min);
            CalculateColors(!edgeColorEnabled);
            glControl1.Invalidate();
        }
        private void CalculateColors(bool filled)
        {
            if (m == null)
                return;
            if (filled)
            {
                foreach (Zone z in m.Zones)
                {
                   
                    foreach (Face f in z.Faces)
                    {
                        double d1 = z.Vertices[f.Vertices[0]].Data[dataIndex];
                        double d2 = z.Vertices[f.Vertices[1]].Data[dataIndex];
                        double d3 = z.Vertices[f.Vertices[2]].Data[dataIndex];
                        double avgData = (d1 + d2 + d3) / 3.0;
                        Color c = colorMap1.ValueToColor(colorMap1.GetMode(), (float)avgData);
                        f.Color = new double[] { c.R / 255.0, c.G / 255.0, c.B / 255.0 };
                    }

                }
            }
            else
            {
                foreach (Zone z in m.Zones)
                {
                    //Gl.glBegin(Gl.GL_TRIANGLES);d
                    foreach (Face f in z.Faces)
                    {

                        foreach (uint ee in f.Edges)
                        {
                            Edge e = z.Edges[ee];
                            double avgData = (z.Vertices[e.Start].Data[dataIndex] + z.Vertices[e.End].Data[dataIndex]) * 0.5;
                            Color c = colorMap1.ValueToColor(colorMap1.GetMode(), (float)avgData);
                            e.Color = new double[] { c.R / 255.0, c.G / 255.0, c.B / 255.0 };
                            //Gl.glColor3d(c.R / 255.0, c.G / 255.0, c.B / 255.0);

                        }

                    }
                }
            }
        }

        bool firstTimeDrawingContours = true;
        Matrix contoursMatrix;
        void DrawContours()
        {
            if (m == null)
                return;

            if (firstTimeDrawingContours)
            {
                contoursMatrix = m.Transformation.Clone();
            }

            int c = 0;
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPushMatrix();
            Gl.glMultMatrixd(contoursMatrix.Data);

            foreach (List<Vertex> points in contourLines)
            {

                if (c < contourColors.Count)
                {
                    Gl.glColor3d(contourColors[c][0], contourColors[c][1], contourColors[c][2]);
                }
                c++;
              //  Gl.glColor3d(0, 0, 0);
                Gl.glBegin(Gl.GL_LINES);
                foreach (Vertex vert in points)
                {
                    vert.Position.glTell();
                }
                Gl.glEnd();
            }
            Gl.glPopMatrix();
            firstTimeDrawingContours = false;
        }

        void CalcControusOnEdges()
        {
            if (m == null)
                return;
            double min, max;
            int contourCount = (int)numericUpDown1.Value;
            m.GetMinMaxValues(dataIndex, out min, out max);
            double contourStep = (max - min) / contourCount;
            double contourValue = min + contourStep * 0.5;
            int j = 0;
            int foundContours = 0;
            //   MessageBox.Show(min.ToString() +" " + max.ToString());
            contourLines.Clear();
            contourColors.Clear();
            contourLines.Add(new List<Vertex>());

            for (int i = 0; i < contourCount; ++i)
            {
                // MessageBox.Show(contourValue.ToString());
                foreach (Zone z in m.Zones)
                {

                    foreach (Face f in z.Faces)
                    {

                        foreach (uint ee in f.Edges)
                        {
                            Edge e = z.Edges[ee];
                            var d1 = z.Vertices[e.Start].Data[dataIndex];
                            var d2 = z.Vertices[e.End].Data[dataIndex];
                            if (isBetween(contourValue, d1, d2))
                            {
                                double alpha = Math.Abs(contourValue - d1) / Math.Abs(d2 - d1);

                                var vert = new Vertex();
                                var point = Lerp(z.Vertices[e.Start].Position, z.Vertices[e.End].Position, alpha);
                                // var point = z.Vertices[e.End].Position;

                                vert.Position.Set(point);
                                contourLines[j].Add(vert);
                                // comboBox4.Items.Add(alpha);
                                foundContours++;
                            }
                        }
                        if (foundContours > 1)
                        {
                            Color c = colorMap1.ValueToColor(colorMap1.GetMode(), (float)contourValue);

                            contourColors.Add(new double[3]
                            {
                            (double)c.R / 255.0,
                            (double)c.G / 255.0,
                            (double)c.B / 255.0
                            });

                            // comboBox4.Items.Add(c);
                            foundContours = 0;
                            j++;
                            contourLines.Add(new List<Vertex>());

                        }
                    }
                }
                contourValue += contourStep;
            }
        }
        

        private bool isBetween(double v, double a, double b)
        {
            return (v > a && v < b) || (v < a && v > b) || (Math.Abs(a - v) <= Double.Epsilon) || (Math.Abs(b - v) <= Double.Epsilon);
        }


        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseData.leftClicked == true)
            {
                tranformationData.translateX = e.X - mouseData.mouseX;
                tranformationData.translateY = e.Y - mouseData.mouseY; ;
                tranformationData.translateX = map(tranformationData.translateX, 0, glControl1.Width, 0, 1);
                tranformationData.translateY = map(tranformationData.translateY, 0, glControl1.Height, 0, 1) * -1;
                glControl1.Invalidate();
                mouseData.mouseX = e.X;
                mouseData.mouseY = e.Y;
            }
            if (mouseData.rightClicked == true)
            {

                double deltaY = e.Y - mouseData.mouseY;
                tranformationData.rotationAngle -= deltaY;

                glControl1.Invalidate();
                mouseData.mouseX = e.X;
                mouseData.mouseY = e.Y;
            }
        }
        double map(double n, double start1, double stop1, double start2, double stop2)
        {
            return ((n - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
        }
        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseData.leftClicked = true;
                mouseData.mouseX = e.X;
                mouseData.mouseY = e.Y;
            }
            if (e.Button == MouseButtons.Right)
            {
                mouseData.rightClicked = true;
                mouseData.mouseX = e.X;
                mouseData.mouseY = e.Y;
            }
            
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseData.leftClicked = false;
            }
            if (e.Button == MouseButtons.Right)
            {
                mouseData.rightClicked = false;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            isoValueText.Text = "0.1";
        }

        private void floodBox_CheckedChanged(object sender, EventArgs e)
        {
            glControl1.Invalidate();
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                MessageBox.Show("hello");
                tranformationData.rotateX = 1.5;
            }
            else if (e.KeyCode == Keys.Right)
            {
                tranformationData.rotateX = -1.5;
            }
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show(e.KeyCode.ToString());
            if (e.KeyCode == Keys.A)
            {
                MessageBox.Show("hello");
                tranformationData.rotateX = 1.5;
            }
            else if (e.KeyCode == Keys.D)
            {
                tranformationData.rotateX = -1.5;
            }
            glControl1.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //for (double i =min; i<=max;i+=0.001)
            //{
            //    MarchingCube(i);
            //    //drawTriangles();
            //    glControl1.Invalidate();
            //    System.Threading.Thread.Sleep(500);
            //}
            MarchingCube(double.Parse(isoValueText.Text));
            glControl1.Invalidate();
        }

        private void zoomUpbtn_Click(object sender, EventArgs e)
        {
            tranformationData.translateZ += 4;
            glControl1.Invalidate();
        }

        private void zoomDownBtn_Click(object sender, EventArgs e)
        {
            tranformationData.translateZ -= 4;
            glControl1.Invalidate();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            edgeColorEnabled = comboBox2.SelectedItem.ToString() == "Edge" ? true : false;
            CalculateColors(!edgeColorEnabled);
            glControl1.Invalidate();
        }
    }


}
