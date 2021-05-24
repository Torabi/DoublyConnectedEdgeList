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
    /// a DoublyConnectedEdge connectes two nodes. An edge only appears one in a Fcae.
    /// Note edges in a DCEL are directional, meaining if two faces are adjacent then they share two edges that are reversed.
    /// I an edge does not have a reverse , then it happens to be on the boundary of the DCEL
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DoublyConnectedEdge<T>
    {

         
        /// <summary>
        /// edge value
        /// </summary>
        public T Value
        {
            get;set;
        }

        internal int _id = -1;
        /// <summary>
        /// public key of the edge, assigned upon addition of the edge to DCEL
        /// </summary>
        public int Key => _id;
        /// <summary>
        /// the key of the first node
        /// </summary>
        public int Node1;
        /// <summary>
        /// the key of the second node
        /// </summary>
        public int Node2;
        /// <summary>
        /// the key of the face which this edge belongs tp 
        /// </summary>
        internal int _faceId = -1 ;
        /// <summary>
        /// the id of the reverese edge
        /// </summary>
        internal int _reverseEdgeId = -1;
        /// <summary>
        /// Id of the previous edge in the face
        /// </summary>
        internal int previous = -1;
        /// <summary>
        /// Id of the next edge in the face
        /// </summary>
        internal int next = -1;
        /// <summary>
        /// the index of the reverse edge , if not exist -1
        /// </summary>
        public int ReverseEdgeId => _reverseEdgeId;

        /// <summary>
        /// The key of the next edge in the face
        /// </summary>
        public int NextEdge => next;

        /// <summary>
        /// The key of the previous edge in the face
        /// </summary>
        public int PreviousEdge => previous;

        /// <summary>
        /// construc a DoublyConnectedEdge 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="node1">Node at the start</param>
        /// <param name="node2">Node at the end</param>
        public DoublyConnectedEdge(T value, int node1, int node2)
        {
            Value = value;
            Node1 = node1;
            Node2 = node2;
           

        }
    }
}
