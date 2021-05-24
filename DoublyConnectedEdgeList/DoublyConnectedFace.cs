using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
    Copyright 2021 Ali Torabi (ParametricZoo)

    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
    to deal in the Software without restriction, including without limitation the rights to 
    use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
    and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
    WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
namespace GraphData
{
    /// <summary>
    /// a DoublyConnectedFace is collection of DoublyConnectedEdges which are in order  
    /// </summary>
    public class DoublyConnectedFace<T>
    {
        /// <summary>
        /// the value assigned to this face
        /// </summary>
        public T Value { get; set; }

        internal int _id;

        public int Key => _id;
        /// <summary>
        /// holds the edge keys 
        /// </summary>
        internal List<int> edgeList;
        ///// <summary>
        ///// holds the node keys  
        ///// </summary>
        //internal List<int> nodeList;
       

        /// <summary>
        /// the circulator stops when it reaches this index (start index)
        /// </summary>
        int edgeCirculatorStart = 0;
        
        /// <summary>
        ///  the position of the circulator 
        /// </summary>
        int edgeCirculator = 0;

        /// <summary>
        /// construc a doubly connected face, 
        /// this method should not be used to create faces, use <c>AddFace()</c> instead 
        /// </summary>
        /// <param name="value"></param>

        internal DoublyConnectedFace(T value)
        {
            Value = value;
            edgeList = new List<int>();
            //nodeList = new List<int>();
        }
    
        
        public void ResetEdgeCirculator(int edgeIndex)
        {
            edgeCirculatorStart = edgeCirculator = edgeList.IndexOf(edgeIndex);
           
        }

        public void ResetEdgeCirculator()
        {
            edgeCirculatorStart = edgeCirculator;

        }
        /// <summary>
        /// moves the circulator one edge forward.
        /// </summary>
        /// <returns></returns>
        public bool NextEdge()
        {
            edgeCirculator++;
            int c = edgeList.Count-1;
            if (edgeCirculator > c)
                edgeCirculator = edgeCirculator-c-1;

            return (edgeCirculator != edgeCirculatorStart);
                
        }
        /// <summary>
        /// current edge at circulator position, use NextEdge() to walk around the face 
        /// </summary>
        public int CurrentEdge => edgeList[edgeCirculator];

        
    }
}
