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
    /// represent a node on the graph, each node may correspond to more than 1 edge.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DoublyConnectedNode<T>
    {
        public T Value { get; set; }
        internal int _id= -1;
        /// <summary>
        /// public Key for the node. The key is assign upon adding the node to the DCEL
        /// </summary>
        public int Key => _id;
        /// <summary>
        /// connected edges of the node
        /// </summary>
        internal List<int> _edgeList;
        /// <summary>
        /// Construct a node
        /// </summary>
        /// <param name="value"></param>
        public DoublyConnectedNode(T value)
        {
            Value = value;
            _edgeList = new List<int>();
        }

    }
}
