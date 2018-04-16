using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Visualization;

namespace Testing_openTK_again
{
    
    class MarchingSquare
    {

        
        public ControlNode topLeft, topRight, bottomLeft, bottomRight;
        public Node top, bottom, right, left;
        public int squareCase = 0;
        public double value;
        public double a1, a2, a3, a4;
        public MarchingSquare(double value, Vertex d1, Vertex d2, Vertex d3, Vertex d4, uint dataIndex)
        {
            this.value = value;
            var d1Data = a1 = d1.Data[dataIndex];
            var d2Data = a2 = d2.Data[dataIndex];
            var d3Data = a3 = d3.Data[dataIndex];
            var d4Data = a4 = d4.Data[dataIndex];
            bottomLeft = new ControlNode (d1Data < value ||Math2.NearlyEqual(d1Data,value,double.Epsilon), d1Data, d1.Position);
            topLeft = new ControlNode    (d2Data < value|| Math2.NearlyEqual(d2Data, value, double.Epsilon), d2Data, d2.Position);
            topRight = new ControlNode   (d3Data < value || Math2.NearlyEqual(d3Data, value, double.Epsilon), d3Data, d3.Position);
            bottomRight = new ControlNode(d4Data < value|| Math2.NearlyEqual(d4Data, value, double.Epsilon), d4Data, d4.Position);

            if (bottomLeft.under)
                squareCase += 1;
            if (topLeft.under)
                squareCase += 8;
            if (topRight.under)
                squareCase += 4;
            if (bottomRight.under)
                squareCase += 2;

            if(squareCase==5 || squareCase==10)
            {
                double avgData = (d1Data + d2Data + d3Data + d4Data) / 4.0;
                if (avgData >= value)
                {
                    squareCase = squareCase == 5 ? 10 : 5;
                } 
            }
            left = new Node(Math.Abs(value - Math.Min(d1Data, d2Data)) / Math.Abs(d2Data - d1Data));
            top = new Node(Math.Abs(value - Math.Min(d3Data, d2Data) )/ Math.Abs(d3Data - d2Data));
            bottom =  new Node(Math.Abs(value -Math.Min(d4Data, d1Data)) / Math.Abs(d1Data - d4Data));
            right = new Node(Math.Abs(value -Math.Min(d3Data, d4Data)) / Math.Abs(d3Data - d4Data));
        }
        public double this[int i]
        {
            get {
                if (i == 0)
                    return left.data;
                else if (i == 1)
                    return top.data;
                else if (i == 2)
                    return right.data;
                else
                    return bottom.data;
            }
        }
        public Tuple<Point3,Point3> GetPoints(int edgeIndex)
        {
            if (edgeIndex == 0)
            {
                Point3 p1;
                Point3 p2;
                if (topLeft.data <= bottomLeft.data)
                {
                    p1 = topLeft.pos;
                    p2 = bottomLeft.pos;
                }
                else
                {
                    p2 = topLeft.pos;
                    p1 = bottomLeft.pos;
                }
                return Tuple.Create(p1, p2);
            }

            else if (edgeIndex == 1)
            {
                Point3 p1;
                Point3 p2;
                if (topLeft.data <= topRight.data)
                {
                    p1 = topLeft.pos;
                    p2 = topRight.pos;
                }
                else
                {
                    p2 = topLeft.pos;
                    p1 = topRight.pos;
                }
                return Tuple.Create(p1, p2);
            }
            else if (edgeIndex == 2)
            {
                Point3 p1;
                Point3 p2;
                if (topRight.data <= bottomRight.data)
                {
                    p1 = topRight.pos;
                    p2 = bottomRight.pos;
                }
                else
                {
                    p2 = topRight.pos;
                    p1 = bottomRight.pos;
                }
                return Tuple.Create(p1, p2);
            }
            else
            {
                Point3 p1;
                Point3 p2;
                if (bottomLeft.data <= bottomRight.data)
                {
                    p1 = bottomLeft.pos;
                    p2 = bottomRight.pos;
                }
                else
                {
                    p2 = bottomLeft.pos;
                    p1 = bottomRight.pos;
                }
                return Tuple.Create(p1, p2);
            }
        }
    }

    class ControlNode
    {
        public bool under;
        public double data;
        public Point3 pos;
        public ControlNode(bool _inside,double _data,Point3 _pos)
        {
            this.under = _inside;
            this.data = _data;
            this.pos = _pos;
        }
    }
    class Node
    {
        public double data;
        public  Node(double _data)
        {
            this.data = _data;
        }
    }


}
