using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Visualization;
using System.Windows.Forms;
using System.Drawing;

namespace Testing_openTK_again
{


    class VisualizationOperations
    {
        public static int[][] helper = new int[16][]
         {
           new  int[4]  {-1,-1,-1,-1},
           new  int[4]  {3,0,-1,-1 },
           new  int[4]  {3,2,-1,-1 },
           new  int[4]  {2,0,-1,-1 },
           new  int[4]  {2,1,-1,-1 },
           new  int[4]  {3,2  ,0 ,1 },
           new  int[4]  {1,3,-1,-1 },
           new  int[4]  {1,0,-1,-1 },
           new  int[4]  { 0,1, -1,-1 },
           new  int[4]  {1,3,-1,-1 },
           new  int[4]  {1,2,3,0 },
           new  int[4]  {1, 2,-1,-1 },
           new  int[4]  {0, 2,-1,-1 },
           new  int[4]  {2, 3,-1,-1 },
           new  int[4]  {0,3,-1,-1 },
           new  int[4]  {-1,-1,-1,-1},
        };

        private static void processCase(int[] c)
        {
            if (c[0] == -1)
                return;
        }
        private static Point3 Lerp(Point3 c1, Point3 c2, double t)
        {
            return Point3.Interpolate(t, c1, c2);

        }
        private static Vertex GetVertex(Point3 start, Point3 end, double t)
        {
            return new Vertex(Lerp(start, end, t));

        }
        private static double map(double n, double start1, double stop1, double start2, double stop2)
        {
            return ((n - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
        }
        public static List<List<Vertex>> CalculateContours(Mesh m, int contourCount, uint dataIndex,
            ref List<double[]> contourColors, Testing_openTK_again.Del valueToColor, ColorMapControl.ColorMap.Mode mode)
        {

            List<List<Vertex>> contourLines = new List<List<Vertex>>();
            double min, max;
            m.GetMinMaxValues(dataIndex, out min, out max);
            double contourStep = (max - min) / contourCount;
            double contourValue = min + contourStep * 0.5;
            contourColors.Clear();

            int idx = 0;
            for (int i = 0; i < contourCount; ++i)
            {

                foreach (Zone z in m.Zones)
                {

                    foreach (Face f in z.Faces)
                    {
                        var d1 = z.Vertices[f.Vertices[0]];
                        var d2 = z.Vertices[f.Vertices[1]];
                        var d3 = z.Vertices[f.Vertices[2]];
                        var d4 = z.Vertices[f.Vertices[3]];
                        var s = new MarchingSquare(contourValue, d1, d2, d3, d4, dataIndex);
                        if (s.squareCase == 0 || s.squareCase == 15)
                            continue;
                        contourLines.Add(new List<Vertex>());

                        System.Drawing.Color c = valueToColor(mode, (float)contourValue);
                        contourColors.Add(new double[3] { c.R / 255.0, c.G / 255.0, c.B / 255.0 });
                        var edges = helper[s.squareCase];
                        var points1 = s.GetPoints(edges[0]);
                        var points2 = s.GetPoints(edges[1]);


                        contourLines[idx].Add(GetVertex(points1.Item1, points1.Item2, s[edges[0]]));
                        contourLines[idx].Add(GetVertex(points2.Item1, points2.Item2, s[edges[1]]));
                        idx++;

                        if (edges[2] == -1)
                        {
                            //idx++;
                            continue;
                        }
                        contourLines.Add(new List<Vertex>());
                        points1 = s.GetPoints(edges[2]);
                        points2 = s.GetPoints(edges[3]);
                        contourLines[idx].Add(GetVertex(points1.Item1, points1.Item2, s[edges[2]]));
                        contourLines[idx].Add(GetVertex(points2.Item1, points2.Item2, s[edges[3]]));
                        idx++;

                    }
                }
                contourValue += contourStep;
            }
            return contourLines;
        }
        private static Point3 process(double contourValue, Point3 a, Point3 b, double d1, double d2)
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
        public static List<Shape> CalcShapes(Mesh m, int countourCount, uint dataIndex, Testing_openTK_again.Del valueToColor, ColorMapControl.ColorMap.Mode mode)
        {
            List<Shape> shapes = new List<Shape>();
            shapes.Add(new Shape());
            double min, max;
            m.GetMinMaxValues(dataIndex, out min, out max);
            double contourStep = (max - min) / countourCount;
            double contourValue = min + contourStep * 0.5;
            int idx = 0;
            double prevContourValue = 0;
            contourValue = min + contourStep * 0.5;
            prevContourValue = min;
            Point3 lower = new Point3(100000, 100000, 100000);
            for (int i = -1; i < countourCount; ++i)
            {

                foreach (Zone z in m.Zones)
                {

                    foreach (Face f in z.Faces)
                    {
                        if (z.ElementType == ElementType.Triangle)
                        {
                            (var v1, var v2, var v3) =
                           (z.Vertices[f.Vertices[0]], z.Vertices[f.Vertices[1]],
                           z.Vertices[f.Vertices[2]]);
                            (var d1, var d2, var d3) =
                                (v1.Data[dataIndex], v2.Data[dataIndex], v3.Data[dataIndex]);
                            if (i == countourCount - 1)
                                contourValue = max;
                            lower.x = Math.Min(lower.x, Math.Min(v1.Position.x, Math.Min(v2.Position.x, v3.Position.x)));
                            lower.y = Math.Min(lower.y, Math.Min(v1.Position.y, Math.Min(v2.Position.y, v3.Position.y)));

                            shapes[idx].colorAlpha = (prevContourValue + contourValue) * 0.5;
                            if (isBetween(d1, prevContourValue, contourValue))
                                shapes[idx].verts.Add(v1.Position);
                            if (isBetween(d2, prevContourValue, contourValue))
                                shapes[idx].verts.Add(v2.Position);
                            if (isBetween(d3, prevContourValue, contourValue))
                                shapes[idx].verts.Add(v3.Position);

                            if (isBetween(contourValue, d1, d2))
                                shapes[idx].verts.Add(process(contourValue, v1.Position, v2.Position, d1, d2));
                            if (isBetween(contourValue, d2, d3))
                                shapes[idx].verts.Add(process(contourValue, v2.Position, v3.Position, d2, d3));
                            if (isBetween(contourValue, d1, d3))
                                shapes[idx].verts.Add(process(contourValue, v1.Position, v3.Position, d1, d3));
                            if (isBetween(prevContourValue, d1, d2))
                                shapes[idx].verts.Add(process(prevContourValue, v1.Position, v2.Position, d1, d2));
                            if (isBetween(prevContourValue, d2, d3))
                                shapes[idx].verts.Add(process(prevContourValue, v2.Position, v3.Position, d2, d3));
                            if (isBetween(prevContourValue, d1, d3))
                                shapes[idx].verts.Add(process(prevContourValue, v1.Position, v3.Position, d1, d3));
                            if (shapes[idx].verts.Count > 2)
                            {
                                shapes.Add(new Shape());
                                idx++;
                            }
                        }
                        else
                        {


                            (var v1, var v2, var v3, var v4) =
                                (z.Vertices[f.Vertices[0]], z.Vertices[f.Vertices[1]],
                                z.Vertices[f.Vertices[2]], z.Vertices[f.Vertices[3]]);
                            (var d1, var d2, var d3, var d4) =
                                (v1.Data[dataIndex], v2.Data[dataIndex], v3.Data[dataIndex], v4.Data[dataIndex]);
                            lower.x = Math.Min(lower.x, Math.Min(v1.Position.x, Math.Min(v2.Position.x, Math.Min(v3.Position.x, v4.Position.x))));
                            lower.y = Math.Min(lower.y, Math.Min(v1.Position.y, Math.Min(v2.Position.y, Math.Min(v3.Position.y, v4.Position.y))));
                            if (i == countourCount - 1)
                                contourValue = max;

                            shapes[idx].colorAlpha = (prevContourValue + contourValue) * 0.5;
                            if (isBetween(d1, prevContourValue, contourValue))
                                shapes[idx].verts.Add(v1.Position);
                            if (isBetween(d2, prevContourValue, contourValue))
                                shapes[idx].verts.Add(v2.Position);
                            if (isBetween(d3, prevContourValue, contourValue))
                                shapes[idx].verts.Add(v3.Position);
                            if (isBetween(d4, prevContourValue, contourValue))
                                shapes[idx].verts.Add(v4.Position);
                            if (isBetween(contourValue, d1, d2))
                                shapes[idx].verts.Add(process(contourValue, v1.Position, v2.Position, d1, d2));
                            if (isBetween(contourValue, d2, d3))
                                shapes[idx].verts.Add(process(contourValue, v2.Position, v3.Position, d2, d3));
                            if (isBetween(contourValue, d3, d4))
                                shapes[idx].verts.Add(process(contourValue, v3.Position, v4.Position, d3, d4));
                            if (isBetween(contourValue, d4, d1))
                                shapes[idx].verts.Add(process(contourValue, v4.Position, v1.Position, d4, d1));
                            if (isBetween(prevContourValue, d1, d2))
                                shapes[idx].verts.Add(process(prevContourValue, v1.Position, v2.Position, d1, d2));
                            if (isBetween(prevContourValue, d2, d3))
                                shapes[idx].verts.Add(process(prevContourValue, v2.Position, v3.Position, d2, d3));
                            if (isBetween(prevContourValue, d3, d4))
                                shapes[idx].verts.Add(process(prevContourValue, v3.Position, v4.Position, d3, d4));
                            if (isBetween(prevContourValue, d4, d1))
                                shapes[idx].verts.Add(process(prevContourValue, v4.Position, v1.Position, d4, d1));
                            if (shapes[idx].verts.Count > 2)
                            {
                                shapes.Add(new Shape());
                                idx++;
                            }
                        }

                    }
                }

                prevContourValue = contourValue;
                contourValue += contourStep;

            }
            shapes.RemoveAll(s => s.verts.Count == 0);
            Point3 mostLeft = new Point3();
            Point3 mostRight = new Point3();

            foreach (Shape s in shapes)
            {
                lower = s.verts[0];
                //mostLeft = s.verts[0];
                //mostRight = s.verts[0];
                //foreach (Point3 p in s.verts)
                //{
                //    if (p.x < mostLeft.x)
                //        mostLeft = p;1
                //    if (p.x > mostRight.x)
                //        mostRight = p;
                // }
                //var upper = s.verts.Where(p => p.y <= mostLeft.y).ToList();
                //var upper = s.verts.Where(p => p.y > mostRight.y).ToList();
                foreach (Point3 p in s.verts)
                    if (p.x < lower.x)
                        lower = p;
                s.verts.Sort((Point3 a, Point3 b) =>
            {
                double angle1 = Math2.angle(a, lower);
                double angle2 = Math2.angle(b, lower);
                return angle1.CompareTo(angle2);
            });

                Color c = valueToColor(mode, (float)s.colorAlpha);
                s.color = new double[3]
                {
                            (double)c.R / 255.0,
                            (double)c.G / 255.0,
                            (double)c.B / 255.0
                };
            }

            return shapes;
        }
        private static bool isBetween(double v, double a, double b)
        {
            return (v > a && v < b) || (v < a && v > b) || (Math.Abs(a - v) <= Double.Epsilon) || (Math.Abs(b - v) <= Double.Epsilon);
        }
        public static List<IsoTri> CalcIsoSurface(Mesh m, uint dataIndex, double isoValue, Testing_openTK_again.Del valueToColor, ColorMapControl.ColorMap.Mode mode)
        {
            List<IsoTri> isoTri = new List<IsoTri>();
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
                            var p = Point3.Interpolate(alpha, startVert.Position, endVert.Position);
                            //var p = process(iso, startVert.Position, endVert.Position, startVert.Data[dataIndex], startVert.Data[dataIndex]);
                            isoTri[isoTri.Count - 1].triangle.Add(p);
                            Color c = valueToColor(mode, (float)(iso - endVert.Data[dataIndex] + startVert.Data[dataIndex]));
                            isoTri[isoTri.Count - 1].color = new double[] { c.R / 255.0, c.G / 255.0, c.B / 255.0 };
                        }
                    }

                }
            }
            return isoTri;
        }
        public static List<IsoTri> CalcIsoSurfaces(Mesh m, uint dataIndex, double isoCount, Testing_openTK_again.Del valueToColor, ColorMapControl.ColorMap.Mode mode)
        {
            List<IsoTri> isoTri = new List<IsoTri>();
            double min, max;
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
                                Color c = valueToColor(mode, (float)(iso - endVert.Data[dataIndex] + startVert.Data[dataIndex]));

                                //Color c = valueToColor(mode, (float)(iso));// (float)(iso - endVert.Data[dataIndex] + startVert.Data[dataIndex]));
                                isoTri[isoTri.Count - 1].color = new double[] { c.R / 255.0, c.G / 255.0, c.B / 255.0 };
                            }
                        }

                    }
                }
                iso += isoStep;
            }
            return isoTri;
        }

    }
    
    public delegate Color Del(ColorMapControl.ColorMap.Mode mode, float val);
    class Shape
    {
        public double colorAlpha;
        public Shape()
        {
            verts = new List<Point3>();
        }
        public List<Point3> verts;
        public double[] color;
    }
    class IsoTri
    {
        public List<Point3> triangle;
        public double[] color;
        public IsoTri()
        {
            this.triangle = new List<Point3>();
            this.color = new double[3];
        }
    }
}

