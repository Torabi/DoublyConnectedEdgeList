using System;
using System.Collections;
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
    /// represent a flat graph with doubly connected edges (an edge may have a reverse edge wich belongs to the adjacent face)
    /// only manifold edges are allowed. 
    /// </summary>
    /// <typeparam name="F"></typeparam>
    /// <typeparam name="N"></typeparam>
    /// <typeparam name="E"></typeparam>
    
    public class DoublyConnectedEdgeList<F,N,E>
    {
        int _lastNodeId = -1; 
        int _lastEdgeId = -1;
        int _lastFaceId = -1;
        // storage 
        Dictionary<int, DoublyConnectedFace<F>> _faces;
        Dictionary<int, DoublyConnectedNode<N>> _nodes;
        Dictionary<int, DoublyConnectedEdge<E>> _edges;
        Dictionary<int, int> _nodePairing; 
        /// <summary>
        /// construct an instanc eof the DCEL
        /// </summary>
        public DoublyConnectedEdgeList()
        {
            _nodes = new Dictionary<int, DoublyConnectedNode<N>>();
            _edges = new Dictionary<int, DoublyConnectedEdge<E>>();
            _faces = new Dictionary<int, DoublyConnectedFace<F>>();
            _nodePairing = new Dictionary<int, int>();
        }
        /// <summary>
        /// extract the edges in the DCEL (not ordered)
        /// </summary>
        public IEnumerable<DoublyConnectedEdge<E>> Edges => _edges.Values;
        /// <summary>
        /// extract the faces in DCEL (not ordered)
        /// </summary>
        public IEnumerable<DoublyConnectedFace<F>> Facces => _faces.Values;
        /// <summary>
        /// extract the nodes in DCEL (not ordered)
        /// </summary>
        public IEnumerable<DoublyConnectedNode<N>> Nodes => _nodes.Values;
        /// <summary>
        /// add a node to the graph and assign the given value to it, it returns the node  
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public DoublyConnectedNode<N> AddNode (N val)
        {
            DoublyConnectedNode<N> node = new DoublyConnectedNode<N>(val); 
            _nodes.Add(++_lastNodeId, node);
            node._id = _lastNodeId;
            return node;
        }

        #region Edge methods
        /// <summary>
        /// retriving the edge which connects the node1 to node2
        /// return -1 if no edge found
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        private int GetEdgeByNodes(int node1, int node2)
        {
            // find the unique key for node1 and node2
            int pairing = paringFunction(node1, node2);
            
            
            if (_nodePairing.ContainsKey(pairing))
                return _nodePairing[pairing];
            else
                return -1;
        }
        /// <summary>
        /// return a unique integer for two given integer
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        int paringFunction (int i,int j)
        {
            return j + (i + j) * (i + j + 1) / 2;
        }

        private DoublyConnectedEdge<E> addEdge(E value, int node1, int node2)
        {
            


            var edge = new DoublyConnectedEdge<E>(value, node1, node2);

            _edges.Add(++_lastEdgeId, edge);
            edge._id = _lastEdgeId;
            int reverseId = GetEdgeByNodes(node2, node1);
            if (reverseId > -1)
            {
                // add the reverse edge 
                _edges[reverseId]._reverseEdgeId = _lastEdgeId;
                edge._reverseEdgeId = reverseId;
            }
            // add the edgeId to the node edgeList
            _nodes[node1]._edgeList.Add(_lastEdgeId);
            _nodes[node2]._edgeList.Add(_lastEdgeId);

            // set the node pairing 
            
            _nodePairing.Add(paringFunction(node1,node2), edge._id);
            return edge;
        }

        /// <summary>
        /// add an edge to the graph and assign a new key to the edge. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="node1"></param>
        /// <param name="node2"></param>

        /// <returns></returns>
        public DoublyConnectedEdge<E> AddEdge(E value,int node1, int node2)
        {
            if (_nodes.ContainsKey(node1) && _nodes.ContainsKey(node2))
            {
                if (GetEdgeByNodes(node1, node2) > -1)
                    throw new Exception("Edge already exist");
                return addEdge(value, node1, node2);
            }
            else
            {
                return null;
            }

        }


        /// <summary>
        /// unsafe method to delete an edge
        /// </summary>
        /// <param name="key"></param>
        public void deleteEdge(int key)
        {
            
                var edge = _edges[key];
                // remove the edge key from the nodes 

                var n1 = edge.Node1;
                var n2 = edge.Node2;
                if (_nodes.ContainsKey(n1) && _nodes.ContainsKey(n2))
                {
                    _nodes[n1]._edgeList.Remove(n1);
                    _nodes[n2]._edgeList.Remove(n2);
                }
                else
                {
                    throw new Exception($"Node key do not exist");
                }

                // remove the edge from the face

                if (_faces.ContainsKey(edge._faceId))
                {
                    // first check if dual of the edge 
                    _faces.Remove(edge._faceId);
                }


                // remove the ege from the reverse edge 
                if (_edges.ContainsKey(edge._reverseEdgeId))
                {
                    _edges[edge._reverseEdgeId]._reverseEdgeId = -1;
                }
                //remove the node from the node pairing 
                _nodePairing.Remove(paringFunction(edge.Node1, edge.Node2));

                _edges.Remove(key);


             
        }
        /// <summary>
        /// remove an edge from the graph, this will also remove the face whih contains this edge
        /// </summary>
        /// <param name="key"></param>
        public void DeleteEdge (int key)
        {
            if (_edges.ContainsKey(key))
            {
                deleteEdge(key);

            }
            else
            {
                throw new Exception($"No edge at key {key}");
            }
        }

        private DoublyConnectedFace<F> join (DoublyConnectedEdge<E> edge1, DoublyConnectedEdge<E> edge2, DoublyConnectedFace<F> face1, DoublyConnectedFace<F> face2)
        {
            face1.ResetEdgeCirculator(edge1.Key);

            var face = new DoublyConnectedFace<F>(face1.Value);
            AddFace(face);
            face1.ResetEdgeCirculator(edge1.Key);
            face1.NextEdge();
            do
            {
                
                var e = _edges[face1.CurrentEdge];
                
                
                addEdgeToFace(face, e);

            } while (face1.NextEdge());
            face2.ResetEdgeCirculator(edge2.Key);
            face2.NextEdge();
            do
            {

                var e = _edges[face2.CurrentEdge];


                addEdgeToFace(face, e);

            } while (face2.NextEdge());

            // we still need to update the nex parameter of the last edge and 
            // the previous parameter of the first page 
            int last = face.edgeList[face.edgeList.Count - 1];
            int first = face.edgeList[0];
            _edges[first].previous = last;
            _edges[last].next = first;
            return face;
        }

        public void DeleteEdge(int key, out DoublyConnectedFace<F> face)
        {
            if (_edges.ContainsKey(key))
            {                                
                
                var edge = _edges[key];
                DoublyConnectedFace<F> face1 = null, face2 = null;
                if (_faces.ContainsKey(edge._faceId))
                {
                    // first check if dual of the edge 
                    face1 = _faces[edge._faceId];
                    _faces.Remove(face1.Key);
                }
                if (_edges.ContainsKey(edge._reverseEdgeId))
                {
                    var reverseEdge = _edges[edge._reverseEdgeId];
                    if (_faces.ContainsKey(reverseEdge._faceId))
                    {
                        
                        // first check if dual of the edge 
                        face2 = _faces[reverseEdge._faceId];
                        _faces.Remove(face2.Key);
                        face = join(edge, reverseEdge, face1, face2);

                        _edges.Remove(reverseEdge._id);
                    }
                    else
                    {
                        throw new Exception($"Face {reverseEdge._faceId} does not exist");
                    }
                    



                }
                else
                {
                    throw new Exception("Edge was not a shared edge.");
                }
               
                

                // remove the edge key from the nodes 

                var n1 = edge.Node1;
                var n2 = edge.Node2;
                if (_nodes.ContainsKey(n1) && _nodes.ContainsKey(n2))
                {
                    _nodes[n1]._edgeList.Remove(n1);
                    _nodes[n2]._edgeList.Remove(n2);
                }
                else
                {
                    throw new Exception($"Node key do not exist");
                }

                // remove the edge from the face

                if (_faces.ContainsKey(face1._id))
                {
                    // first check if dual of the edge 
                    _faces.Remove(face1._id);
                }
                if (face2 != null && _faces.ContainsKey(face2._id))
                {
                    _faces.Remove(face2._id);
                }

                _edges.Remove(key);


            }
            else
            {
                throw new Exception($"No edge at key {key}");
            }
        }
        /// <summary>
        /// return the face which contains this edge , otherwise returns null
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public DoublyConnectedFace<F> GetFacesByEdge(DoublyConnectedEdge<E> edge)
        {
            if (_faces.ContainsKey(edge._faceId))
                return _faces[edge._faceId];
            else
                return null;
        }
        /// <summary>
        /// return the edge corresponding to the given key otherwise return null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DoublyConnectedEdge<E> GetEdge(int key)
        {
            if (_edges.ContainsKey(key))
                return _edges[key];
            else
                return null;
        }

        #endregion

        #region Face methods
        //public DoublyConnectedFace<F> addFace(F value, IEnumerable<int> nodes )
        //{
        //    var face = new DoublyConnectedFace<F>(value);
        //    int previous = -1;
        //    foreach(int node in nodes)
        //    {

        //    }
        //}

        private void addEdgeToFace(DoublyConnectedFace<F> face, DoublyConnectedEdge<E> edge)
        {
             
            // update the next parameter of the previous edge
            
            if (face.edgeList.Count > 0)
            {
                int previousEdgeIndex = face.edgeList[face.edgeList.Count - 1];
                _edges[previousEdgeIndex].next = edge._id; 
                // update the previous parameter of this edge
                 
                edge.previous = previousEdgeIndex;
            }

            // add the edge to the edge list
            edge._faceId = face._id;
            face.edgeList.Add(edge._id);
        }



        //public DoublyConnectedFace<F> AddFace(F value, IEnumerable<DoublyConnectedNode<N>> nodes)
        //{

        //    DoublyConnectedFace<F> face = new DoublyConnectedFace<F>(value);
        //    var enumarator = nodes.GetEnumerator();
        //    enumarator.MoveNext();
        //    DoublyConnectedNode<N> pre = enumarator.Current;
        //    DoublyConnectedNode<N> first = pre;
            
        //    while (enumarator.MoveNext())
        //    {
        //        int i = pre.Key;
        //        int j = enumarator.Current.Key;
        //        int e = getEdgeByNodes(i,j);
        //        if (e == -1)
        //            throw new Exception($"No edge found between nodes {i} and {j}");
        //        addEdgeToFace(face, e);


        //    }
        //    // close the face 
        //    int last = getEdgeByNodes(enumarator.Current.Key, first.Key);
        //    if (last == -1)
        //        throw new Exception($"No edge found between nodes {last} and {first.Key}");
        //    addEdgeToFace(face, first.Key);
        //    // add the face to the graph and update the last face index 
        //    _faces.Add(_lastFaceId++, face);
        //    face._id = _lastFaceId;
        //    return face;
        //}

        public DoublyConnectedFace<F> AddFace(F value, IEnumerable<DoublyConnectedEdge<E>> edges)
        {

            var face = new DoublyConnectedFace<F>(value);
            AddFace(face);
            var enumarator = edges.GetEnumerator();
            //enumarator.MoveNext();
            //DoublyConnectedEdge<E> pre = enumarator.Current;
            //if (!_edges.ContainsKey(pre.Key))
            //    throw new Exception($"Edge {pre.Key} do not exist");
            //DoublyConnectedEdge<E> first = pre;

            while (enumarator.MoveNext())
            {
                
                int e = enumarator.Current.Key;

                if (!_edges.ContainsKey(e))
                    throw new Exception($"Edge {e} do not exist");
                addEdgeToFace(face, _edges[ e]);


            }
            // we still need to update the nex parameter of the last edge and 
            // the previous parameter of the first page 
            int last = face.edgeList[face.edgeList.Count - 1];
            int first = face.edgeList[0];
            _edges[first].previous = last;
            _edges[last].next = first;

            // add the face to the graph and update the last face index 
            //_faces.Add(++_lastFaceId, face);
            //face._id = _lastFaceId;
            return face;
        }

        public void AddFace(DoublyConnectedFace<F> face)
        {

            // add the face to the graph and update the last face index 
            _faces.Add(++_lastFaceId, face);
            face._id = _lastFaceId;
           
        }
       
        /// <summary>
        /// join the faces if they share an edge. 
        /// the value of face1 will be assigned to the new faces. 
        /// </summary>
        /// <param name="face1"></param>
        /// <param name="face2"></param>
        /// <returns></returns> 
        //[Obsolete("this method is obsolete") ]
        //public IEnumerable< DoublyConnectedFace<F>> Join(DoublyConnectedFace<F> face1, DoublyConnectedFace<F> face2)
        //{
        //    // enumarate on one face and check if the edge has a reverese in other face 
        //    var face = new DoublyConnectedFace<F>(face1.Value);
        //    AddFace(face);

        //    //int e = face1.Current ;
        //    bool swapFaces = true;
        //    bool canContinue = true;
        //    BitArray visitedEdges1 = new BitArray(_edges.Count);
        //    BitArray visitedEdges2 = new BitArray(_edges.Count);
        //    do
        //    {
        //        // check if the edge is a shared edge 
        //        var edge = _edges[face1.CurrentEdge];
        //        if (edge._reverseEdgeId != -1)
        //        {
        //            var revereseEdge = _edges[edge._reverseEdgeId];
        //            if (revereseEdge._faceId == face2.Key)
        //            {
        //                //if (!swapFaces)
        //                //{

        //                    // switch the faces 
        //                    var temp = visitedEdges1;
        //                visitedEdges1 = visitedEdges2;
        //                visitedEdges2 = temp;
        //                    face1.ResetEdgeCirculator();
        //                    face2.ResetEdgeCirculator(edge._reverseEdgeId);
                            
        //                    var temp2 = face1;
        //                    face1 = face2;
        //                    face2 = temp2;



                            
        //                    //swapFaces = true; // keep moving on this face till first non share edge is added

        //                //}
        //                face1.NextEdge();
        //                continue; // do not add this edge!
        //            }
                    
        //        } 
        //        //swapFaces = false;
        //        addEdgeToFace(face, edge);
        //        visitedEdges1[face1.CurrentEdge] = true;
                
        //        //if (!face1.NextEdge())
        //        //{
        //        //    // we reached to the last edge of face1 we should now look for the edge that start with the end
        //        //    // node of the current edge in the face2 
        //        //    var nextEdge = face2.edgeList.First(ee => Edges[ee].Node1 == edge.Node2);

        //        //    face1.ResetEdgeCirculator();
        //        //    face2.ResetEdgeCirculator(nextEdge);

        //        //    var temp2 = face1;
        //        //    face1 = face2;
        //        //    face2 = temp2;


        //        //}
        //        if (!face1.NextEdge())
        //        {
        //            yield return face;                    
        //            face = new DoublyConnectedFace<F>(face1.Value);
        //            AddFace(face);
        //            canContinue = false;
        //            for (int i=0;i< visitedEdges1.Length;i++)
        //            {
        //                if (!visitedEdges1[face1.edgeList[i]])
        //                {
        //                    face1.ResetEdgeCirculator(face1.edgeList[i]);
        //                    canContinue = true;
        //                    break;
        //                }
        //            }

        //        }

        //    } while (canContinue);
        //}

        /// <summary>
        /// unsafe method to extract the nodes of a face in the graph
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        public IEnumerable<DoublyConnectedNode<N>> getNodesByFace(DoublyConnectedFace<F> face)
        {
            return face.edgeList.Select(i => _nodes[_edges[i].Node1]);
        }
      
       

        #endregion



    }
}
