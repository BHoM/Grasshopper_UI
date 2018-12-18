using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System.Collections;

namespace BH.UI.Grasshopper.Base.NonComponents.Others
{
    

    public class BH_StructureIterator : IGH_DataAccess
    {
        /// <summary>
        ///  Maintains all the required information regarding where to put new data.
        ///  </summary>
        private class PathIndex
        {
            /// <summary>
            ///  Represents the current output path for new data. 
            ///  This path will be incremented during IncrementBranchIndices()
            ///  </summary>
            private GH_Path m_path;

            private GH_Path m_collision;

            private int m_offset;

            private int m_index;

            /// <summary>
            ///  If the current path has already been grafted, Grafted[i] will be True. 
            ///  If a path has been grafted, the path field of this instance will be incomplete.
            ///  </summary>
            public bool[] Grafted;

            public int Index
            {
                get
                {
                    return this.m_index;
                }
                set
                {
                    this.m_index = value;
                }
            }

            /// <summary>
            ///  Gets the current path component of this PathIndex.
            ///  </summary>
            public GH_Path Path
            {
                get
                {
                    return this.m_path;
                }
                set
                {
                    this.m_path = value;
                }
            }

            /// <summary>
            ///  Gets or sets the collision path.
            ///  </summary>
            public GH_Path Collision
            {
                get
                {
                    return this.m_collision;
                }
                set
                {
                    this.m_collision = value;
                }
            }

            public int CollisionOffset
            {
                get
                {
                    return this.m_offset;
                }
                set
                {
                    this.m_offset = value;
                }
            }

            public PathIndex()
            {
                this.m_path = new GH_Path(-1);
                this.m_offset = 0;
                this.m_index = 0;
            }

            public bool Collides(GH_Path path)
            {
                return this.m_collision != null && this.m_collision == path;
            }
        }

        private bool m_abort;

        private GH_Component m_component;

        private GH_Document m_document;

        private BH_StructureIterator.PathIndex m_target;

        private int m_iteration;

        /// <summary>
        ///  True if all input parameters have list access.
        ///  </summary>
        private bool m_allLists;

        private int m_inputCount;

        private IGH_Structure[] m_inputData;

        private GH_ParamAccess[] m_inputAccess;

        private int m_outputCount;

        private IGH_Structure[] m_outputData;

        private GH_ParamAccess[] m_outputAccess;

        private bool[] m_outputAssigned;

        private System.Collections.IList[] m_list;

        /// <summary>
        ///  Length of each current List. 
        ///  If the branch is null, the length is set to 0.
        ///  </summary>
        private int[] m_listLength;

        private int[] m_itemIndex;

        /// <summary>
        ///  Branch indices for the current iteration. Tree parameters do not have meaningful indices.
        ///  </summary>
        private int[] m_listIndex;

        private int m_principalIndex;

        public IGH_Component Component
        {
            get
            {
                return this.m_component;
            }
        }

        public GH_Document Document
        {
            get
            {
                return this.m_document;
            }
        }

        public int Iteration
        {
            get
            {
                return this.m_iteration;
            }
        }

        /// <summary>
        ///  Gets whether the solution ought to be aborted.
        ///  </summary>
        public bool AbortSolution
        {
            get
            {
                return this.m_abort;
            }
        }

        public bool Assignment
        {
            get
            {
                return true; // this.m_outputAssigned[index]; //TODO: Check what theis thing is for
            }
        }

        public BH_StructureIterator(GH_Component parent)
        {
            this.m_abort = false;
            this.m_target = new BH_StructureIterator.PathIndex();
            this.m_iteration = 0;
            this.m_allLists = true;
            this.m_inputCount = 0;
            this.m_outputCount = 0;
            this.m_principalIndex = -1;
            if (parent == null)
            {
                throw new System.ArgumentNullException("parent");
            }
            this.m_component = parent;
            this.m_document = parent.OnPingDocument();
            if (this.m_document == null)
            {
                throw new System.ArgumentException("Component is not associated with a document");
            }
            this.m_inputCount = this.m_component.Params.Input.Count;
            this.m_outputCount = this.m_component.Params.Output.Count;
            if (this.m_inputCount == 0)
            {
                this.m_list = new System.Collections.IList[0];
                this.m_listLength = new int[0];
                this.m_inputData = new IGH_Structure[0];
                this.m_inputAccess = new GH_ParamAccess[0];
                this.m_itemIndex = new int[0];
                this.m_listIndex = new int[0];
            }
            else
            {
                this.m_list = new System.Collections.IList[this.m_inputCount - 1 + 1];
                this.m_listLength = new int[this.m_inputCount - 1 + 1];
                this.m_inputData = new IGH_Structure[this.m_inputCount - 1 + 1];
                this.m_inputAccess = new GH_ParamAccess[this.m_inputCount - 1 + 1];
                this.m_itemIndex = new int[this.m_inputCount - 1 + 1];
                this.m_listIndex = new int[this.m_inputCount - 1 + 1];
            }
            if (this.m_outputCount == 0)
            {
                this.m_outputData = new IGH_Structure[0];
                this.m_outputAccess = new GH_ParamAccess[0];
                this.m_outputAssigned = new bool[0];
            }
            else
            {
                this.m_outputData = new IGH_Structure[this.m_outputCount - 1 + 1];
                this.m_outputAccess = new GH_ParamAccess[this.m_outputCount - 1 + 1];
                this.m_outputAssigned = new bool[this.m_outputCount - 1 + 1];
            }
            int arg_1EF_0 = 0;
            int num = this.m_inputCount - 1;
            for (int i = arg_1EF_0; i <= num; i++)
            {
                IGH_Param param = this.Input(i);
                this.m_inputAccess[i] = param.Access;
                this.m_inputData[i] = param.VolatileData;
                this.UpdateListPointer(i);
            }
            int arg_231_0 = 0;
            int num2 = this.m_outputCount - 1;
            for (int j = arg_231_0; j <= num2; j++)
            {
                IGH_Param param2 = this.Output(j);
                this.m_outputAccess[j] = param2.Access;
                this.m_outputData[j] = param2.VolatileData;
                this.m_outputAssigned[j] = false;
            }
            this.m_allLists = true;
            int arg_27C_0 = 0;
            int num3 = this.m_inputCount - 1;
            for (int k = arg_27C_0; k <= num3; k++)
            {
                if (this.m_inputAccess[k] == GH_ParamAccess.item)
                {
                    this.m_allLists = false;
                    break;
                }
            }
            this.ResetGraftedFlags();
            this.ResetAssignedFlags();
            this.UpdateOutputPath();
        }

        private IGH_Param Input(int index)
        {
            return this.m_component.Params.Input[index];
        }

        /// <summary>
        ///  Gets a pointer to the output parameter at the specified index.
        ///  </summary>
        /// <param name="index">Index of output parameter. Must be valid</param>
        private IGH_Param Output(int index)
        {
            return this.m_component.Params.Output[index];
        }

        private static bool CastData<T>(object @in, out T @out)
        {
            if (@in is T)
            {
                @out = (T)((object)@in);
                return true;
            }
            IGH_Goo gooData = (IGH_Goo)@in;
            if (gooData.CastTo<T>(out @out))
            {
                return true;
            }
            if (GH_TypeLib.t_gh_goo.IsAssignableFrom(typeof(T)))
            {
                if (@out == null)
                {
                    IGH_Goo temp_instance = (IGH_Goo)System.Activator.CreateInstance(typeof(T));
                    if (temp_instance.CastFrom(gooData))
                    {
                        @out = (T)((object)temp_instance);
                        return true;
                    }
                }
                else if (((IGH_Goo)((object)@out)).CastFrom(gooData))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///  Compute the new output path plus index.
        ///  If there are no input parameters, the path root is incremented.
        ///  If there is only a single input param, that path is copied.
        ///  If there are multiple connected input params, the most complex path is copied.
        ///  </summary>
        private void UpdateOutputPath()
        {
            int idx = this.PrincipalParameter();
            GH_Path path;
            if (idx < 0)
            {
                path = this.m_target.Path.Increment(-1);
            }
            else
            {
                IGH_Structure data = this.m_inputData[idx];
                int list = this.m_listIndex[idx];
                if (data.PathCount == 0)
                {
                    path = this.m_target.Path.Increment(-1);
                }
                else if (list == data.PathCount - 1)
                {
                    path = data.get_Path(list);
                    if (this.m_target.Collision == null)
                    {
                        this.m_target.Collision = path;
                        this.m_target.CollisionOffset = 0;
                    }
                    else if (this.m_target.Collides(path))
                    {
                        BH_StructureIterator.PathIndex target = this.m_target;
                        target.CollisionOffset++;
                        path = path.Increment(-1, this.m_target.CollisionOffset);
                    }
                }
                else
                {
                    path = data.get_Path(list);
                }
            }
            this.m_target.Path = path;
            this.m_target.Index = 0;
            this.ResetGraftedFlags();
        }

        /// <summary>
        ///  Attempts to find the most important input parameter that defines this operation.
        ///  </summary>
        /// <returns>The index of the master parameter or -1 if no master parameter can be identified.</returns>
        private int PrincipalParameter()
        {
            if (this.m_principalIndex >= 0)
            {
                return this.m_principalIndex;
            }
            this.m_principalIndex = this.m_component.PrincipalParameterIndex;
            if (this.m_principalIndex >= 0 && this.m_principalIndex < this.m_inputCount)
            {
                return this.m_principalIndex;
            }
            this.m_principalIndex = -1;
            if (this.m_inputCount == 0)
            {
                return -1;
            }
            if (this.m_inputCount == 1)
            {
                this.m_principalIndex = 0;
                return this.m_principalIndex;
            }
            this.m_principalIndex = this.MostComplexParameter();
            if (this.m_principalIndex >= 0)
            {
                return this.m_principalIndex;
            }
            return -1;
        }

        private int MostComplexParameter()
        {
            if (this.m_inputCount == 0)
            {
                return -1;
            }
            if (this.m_inputCount == 1)
            {
                return 0;
            }
            int[] paramComplexity = new int[this.m_inputCount - 1 + 1];
            int arg_31_0 = 0;
            int num = this.m_inputCount - 1;
            for (int i = arg_31_0; i <= num; i++)
            {
                IGH_Structure data = this.m_inputData[i];
                if (this.m_inputAccess[i] == GH_ParamAccess.tree)
                {
                    paramComplexity[i] = -1;
                }
                else
                {
                    switch (data.PathCount)
                    {
                        case 0:
                            paramComplexity[i] = -2;
                            goto IL_E8;
                        case 1:
                            if (data.get_Path(0).Length == 1 && data.get_Path(0)[0] == 0)
                            {
                                paramComplexity[i] = -2;
                                goto IL_E8;
                            }
                            break;
                    }
                    int idx = data.LongestPathIndex();
                    if (idx < 0)
                    {
                        paramComplexity[i] = -2;
                    }
                    else
                    {
                        int length = data.get_Path(idx).Length;
                        if (this.m_inputAccess[i] == GH_ParamAccess.list)
                        {
                            paramComplexity[i] = System.Math.Max(length - 1, 0);
                        }
                        else
                        {
                            paramComplexity[i] = length;
                        }
                    }
                }
                IL_E8:;
            }
            int maxIndex = 0;
            int maxComplex = -2147483648;
            int arg_10A_0 = 0;
            int num2 = this.m_inputCount - 1;
            for (int j = arg_10A_0; j <= num2; j++)
            {
                if (paramComplexity[j] > maxComplex)
                {
                    maxComplex = paramComplexity[j];
                    maxIndex = j;
                }
            }
            return maxIndex;
        }

        /// <summary>
        ///  Call this function to increment the branch indices. 
        ///  At present all branch indices are incremented whenever possible. 
        ///  This amounts to "Longest List Matching" logic. 
        ///  This function also resets all item indices.
        ///  </summary>
        /// <returns>True on success, false on failure. 
        ///  If none of the branch indices are incremented, you should stop iteration.</returns>
        public bool IncrementBranchIndices()
        {
            bool valid = false;
            int arg_0D_0 = 0;
            int num = this.m_inputCount - 1;
            for (int i = arg_0D_0; i <= num; i++)
            {
                if (this.m_inputAccess[i] != GH_ParamAccess.tree && this.m_listIndex[i] < this.m_inputData[i].PathCount - 1)
                {
                    int[] listIndex = this.m_listIndex;
                    int[] arg_4A_0 = listIndex;
                    int num2 = i;
                    arg_4A_0[num2] = listIndex[num2] + 1;
                    this.UpdateListPointer(i);
                    valid = true;
                }
            }
            this.UpdateOutputPath();
            int arg_6E_0 = 0;
            int num3 = this.m_inputCount - 1;
            for (int j = arg_6E_0; j <= num3; j++)
            {
                this.m_itemIndex[j] = 0;
            }
            return valid;
        }

        public bool IncrementItemIndices()
        {
            BH_StructureIterator.PathIndex target = this.m_target;
            target.Index++;
            switch (this.m_component.DataComparison)
            {
                case GH_DataComparison.ShortestList:
                    return this.IncrementItemIndices_ShortestList();
                case GH_DataComparison.LongestList:
                    return this.IncrementItemIndices_LongestList();
                case GH_DataComparison.CrossReference:
                    return this.IncrementItemIndices_CrossReference(0);
                default:
                    return false;
            }
        }

        /// <summary>
        ///  Increments all item indices using ShortestList Matching algorithms. 
        ///  If all parameters have been successfully incremented, the result is True.
        ///  </summary>
        /// <returns>True if all non-list parameters have been incremented successfully.</returns>
        private bool IncrementItemIndices_ShortestList()
        {
            int increment = 0;
            int overflow = 0;
            this.IncrementItemIndices_All(out increment, out overflow);
            return increment != 0 && overflow == 0;
        }

        private bool IncrementItemIndices_LongestList()
        {
            int increment = 0;
            int num = 0;
            this.IncrementItemIndices_All(out increment, out num);
            return increment > 0;
        }

        /// <summary>
        ///  Attempts to increment all item indices. Indices are not incremented beyond the end of their list. 
        ///  And list parameters are not incremented at all.
        ///  </summary>
        /// <param name="Increments">The number of successfully incremented indices.</param>
        /// <param name="Overflows">The number of overflows</param>
        private void IncrementItemIndices_All(out int Increments, out int Overflows)
        {
            Increments = 0;
            Overflows = 0;
            int arg_10_0 = 0;
            int num = this.m_inputCount - 1;
            for (int i = arg_10_0; i <= num; i++)
            {
                if (this.m_inputAccess[i] == GH_ParamAccess.item)
                {
                    int[] itemIndex = this.m_itemIndex;
                    int[] arg_2E_0 = itemIndex;
                    int num2 = i;
                    arg_2E_0[num2] = itemIndex[num2] + 1;
                    if (this.m_itemIndex[i] >= this.m_listLength[i])
                    {
                        this.m_itemIndex[i] = this.m_listLength[i] - 1;
                        Overflows++;
                    }
                    else
                    {
                        Increments++;
                    }
                }
                else
                {
                    this.m_itemIndex[i] = 0;
                }
            }
        }

        private bool IncrementItemIndices_CrossReference(int layer)
        {
            if (layer >= this.m_inputCount)
            {
                return false;
            }
            switch (this.m_inputAccess[layer])
            {
                case GH_ParamAccess.item:
                    {
                        int[] array = this.m_itemIndex;
                        array[layer]++;
                        if (this.m_itemIndex[layer] >= this.m_listLength[layer])
                        {
                            this.m_itemIndex[layer] = 0;
                            return this.IncrementItemIndices_CrossReference(layer + 1);
                        }
                        break;
                    }
                case GH_ParamAccess.list:
                    {
                        int[] array = this.m_listIndex;
                        array[layer]++;
                        this.m_itemIndex[layer] = 0;
                        if (this.m_listIndex[layer] >= this.m_inputData[layer].PathCount)
                        {
                            bool rc = this.IncrementItemIndices_CrossReference(layer + 1);
                            if (rc)
                            {
                                this.m_listIndex[layer] = 0;
                                this.UpdateListPointer(layer);
                            }
                            return rc;
                        }
                        this.UpdateListPointer(layer);
                        break;
                    }
                case GH_ParamAccess.tree:
                    this.m_itemIndex[layer] = 0;
                    this.m_listIndex[layer] = 0;
                    this.UpdateListPointer(layer);
                    return this.IncrementItemIndices_CrossReference(layer + 1);
            }
            return true;
        }

        /// <summary>
        ///  This will update the cached reference and length of branch at the given parameter index (layer). 
        ///  The m_branch_index(layer) value must be correct before calling this function.
        ///  </summary>
        private void UpdateListPointer(int index)
        {
            int listIndex = this.m_listIndex[index];
            IGH_Structure data = this.m_inputData[index];
            if (listIndex > data.PathCount - 1)
            {
                this.m_list[index] = null;
                this.m_listLength[index] = 0;
            }
            else
            {
                this.m_list[index] = this.m_inputData[index].get_Branch(this.m_listIndex[index]);
                if (this.m_list[index] == null)
                {
                    this.m_listLength[index] = 0;
                }
                else
                {
                    this.m_listLength[index] = this.m_list[index].Count;
                }
            }
        }

        /// <summary>
        ///  Increment the iteration count by 1.
        ///  </summary>
        public void IncrementIteration()
        {
            this.m_iteration++;
        }

        public void DisableGapLogic()
        {
            int arg_0F_0 = 0;
            int num = this.m_outputAssigned.Count<bool>() - 1;
            for (int i = arg_0F_0; i <= num; i++)
            {
                this.m_outputAssigned[i] = true;
            }
        }

        /// <summary>
        ///  Call this function if you want to disable the gap null logic for the current iteration. 
        ///  After you call this function, nulls will not be inserted into output parameters when the 
        ///  component fails to assign data itself.
        ///  </summary>
        /// <param name="index">Index of parameter for which to disable gap logic.</param>
        public void DisableGapLogic(int index)
        {
            this.m_outputAssigned[index] = true;
        }

        public GH_Path ParameterTargetPath(int index)
        {
            if (index < 0)
            {
                throw new System.IndexOutOfRangeException(string.Format("Output parameter Index [{0}] too low for Component {1}.", index, this.m_component.Name));
            }
            if (index >= this.m_outputCount)
            {
                throw new System.IndexOutOfRangeException(string.Format("Output parameter Index [{0}] too high for Component {1}.", index, this.m_component.Name));
            }
            if (this.m_allLists)
            {
                return new GH_Path(this.m_target.Path);
            }
            if (this.m_target.Grafted[index])
            {
                return this.m_target.Path.AppendElement(this.m_target.Index);
            }
            return new GH_Path(this.m_target.Path);
        }

        /// <summary>
        ///  Get the target index for the specified output parameter.
        ///  </summary>
        /// <param name="index">Index of output parameter.</param>
        public int ParameterTargetIndex(int index)
        {
            if (index < 0)
            {
                throw new System.IndexOutOfRangeException(string.Format("Output parameter Index [{0}] too low for Component {1}.", index, this.m_component.Name));
            }
            if (index >= this.m_outputCount)
            {
                throw new System.IndexOutOfRangeException(string.Format("Output parameter Index [{0}] too high for Component {1}.", index, this.m_component.Name));
            }
            return this.m_itemIndex[index];
        }

        public void AbortComponentSolution()
        {
            this.m_abort = true;
        }

        public bool GetData<T>(string name, ref T destination)
        {
            return this.GetData<T>(this.m_component.Params.IndexOfInputParam(name), ref destination);
        }

        public bool GetData<T>(int index, ref T destination)
        {
            if (index < 0)
            {
                throw new System.IndexOutOfRangeException(string.Format("Input parameter index [{0}] too low for Component {1}.", index, this.m_component.Name));
            }
            if (index >= this.m_inputCount)
            {
                throw new System.IndexOutOfRangeException(string.Format("Input parameter index [{0}] too high for Component {1}.", index, this.m_component.Name));
            }
            if (this.m_inputAccess[index] != GH_ParamAccess.item)
            {
                throw new System.InvalidOperationException("GetData() can only be called on a parameter with access set to GH_ParamAccess.item");
            }
            System.Collections.IList d_list = this.m_list[index];
            if (d_list == null)
            {
                return false;
            }
            if (d_list.Count == 0)
            {
                return false;
            }
            int d_index = this.m_itemIndex[index];
            if (d_index < 0 | d_index >= d_list.Count)
            {
                System.Guid assert_id = new System.Guid("{F428F6F1-56FD-4686-B110-41A6C6742900}");
                Tracing.Assert(assert_id, "Data index is out of bounds: " + d_index.ToString());
                return false;
            }
            object d_data = System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(d_list[d_index]);
            if (d_data == null)
            {
                return false;
            }
            if (BH_StructureIterator.CastData<T>(System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(d_data), out destination))
            {
                return true;
            }
            string t0 = this.MungeTypeNameForGUI(d_data.GetType().Name);
            string t = this.MungeTypeNameForGUI(typeof(T).Name);
            this.m_component.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, string.Format("Invalid cast: {0} » {1}", t0, t));
            return false;
        }

        public bool GetDataList<T>(string name, System.Collections.Generic.List<T> list)
        {
            return this.GetDataList<T>(this.m_component.Params.IndexOfInputParam(name), list);
        }

        public bool GetDataList<T>(int index, System.Collections.Generic.List<T> list)
        {
            if (index < 0)
            {
                throw new System.IndexOutOfRangeException(string.Format("Input parameter index [{0}] too low for Component {1}.", index, this.m_component.Name));
            }
            if (index >= this.m_inputCount)
            {
                throw new System.IndexOutOfRangeException(string.Format("Input parameter index [{0}] too high for Component {1}.", index, this.m_component.Name));
            }
            if (list == null)
            {
                throw new System.ArgumentNullException("[list] is a null reference, GetDataList() can only be called with a constructed List<T>.");
            }
            if (this.m_inputAccess[index] != GH_ParamAccess.list)
            {
                throw new System.InvalidOperationException("GetDataList() can only be called on a parameter with access set to GH_ParamAccess.list");
            }
            if (this.m_list[index] == null)
            {
                return false;
            }
            System.Type typ_in = this.m_component.Params.Input[index].Type;
            System.Type typ_out = typeof(T);
            if (typ_in.Equals(typ_out))
            {
                list.AddRange((System.Collections.Generic.List<T>)this.m_list[index]);
            }
            else
            {
                list.Capacity = list.Count + this.m_list[index].Count;
                System.Collections.IEnumerator enumerator = null;
                try
                {
                    enumerator = this.m_list[index].GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        object d_data = System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(enumerator.Current);
                        if (d_data != null)
                        {
                            T t_instance = default(T);
                            if (BH_StructureIterator.CastData<T>(System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(d_data), out t_instance))
                            {
                                list.Add(t_instance);
                            }
                            else
                            {
                                string t0 = this.MungeTypeNameForGUI(d_data.GetType().Name);
                                string t = this.MungeTypeNameForGUI(typeof(T).Name);
                                this.m_component.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, string.Format("Invalid cast: {0} » {1}", t0, t));
                            }
                        }
                        else
                        {
                            list.Add(default(T));
                        }
                    }
                }
                finally
                {
                    if (enumerator is System.IDisposable)
                    {
                        (enumerator as System.IDisposable).Dispose();
                    }
                }
            }
            list.TrimExcess();
            return true;
        }

        public bool GetDataTree<T>(string name, out GH_Structure<T> tree) where T : IGH_Goo
        {
            return this.GetDataTree<T>(this.m_component.Params.IndexOfInputParam(name), out tree);
        }

        public bool GetDataTree<T>(int index, out GH_Structure<T> tree) where T : IGH_Goo
        {
            if (index < 0)
            {
                throw new System.IndexOutOfRangeException(string.Format("Input parameter index [{0}] too low for Component {1}.", index, this.m_component.Name));
            }
            if (index >= this.m_inputCount)
            {
                throw new System.IndexOutOfRangeException(string.Format("Input parameter index [{0}] too high for Component {1}.", index, this.m_component.Name));
            }
            if (this.m_inputAccess[index] != GH_ParamAccess.tree)
            {
                throw new System.InvalidOperationException("GetDataTree() can only be called on a parameter with access set to GH_ParamAccess.tree");
            }
            if (this.m_inputData[index] is GH_Structure<T>)
            {
                tree = (GH_Structure<T>)this.m_inputData[index];
                return true;
            }
            System.Type type = this.m_component.Params.Input[index].Type;
            System.Type typeFromHandle = typeof(T);
            System.Guid assert_id = new System.Guid("{E3F5A5D1-1385-4183-A280-6D605C7FF2A2}");
            Tracing.Assert(assert_id, string.Concat(new string[]
            {
            "GetDataTree() must be called with a correct Type Parameter since it does not perform translations.",
            System.Environment.NewLine,
            string.Format("The parameter at index ({0}) is of type: {1}", index, type.Name),
            System.Environment.NewLine,
            string.Format("You requested type: {0}", typeFromHandle.Name)
            }));
            tree = null;
            return false;
        }

        private string MungeTypeNameForGUI(string name)
        {
            if (name.Equals("IGH_QuickCast"))
            {
                return "Primitive Data Type";
            }
            if (name.StartsWith("GH_"))
            {
                return name.Substring(3);
            }
            if (name.StartsWith("IGH_"))
            {
                return name.Substring(4);
            }
            return name;
        }

        public object GetOutputData(int index)
        {
            GH_Path localPath = this.m_target.Path;
            int localIndex = this.m_target.Index;

            IList localBranch = this.Output(index).VolatileData.get_Branch(localPath);
            if (localBranch.Count >= localIndex)
                return localBranch[localIndex];
            else
                return null;
        }

        public object GetOutputDataList(int index)
        {
            GH_Path localPath = this.m_target.Path;
            int localIndex = this.m_target.Index;

            return this.Output(index).VolatileData.get_Branch(localPath);
        }

        public bool SetData(string paramName, object data)
        {
            int index = this.m_component.Params.IndexOfOutputParam(paramName);
            return this.SetData(index, System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(data));
        }

        public bool SetData(int index, object data)
        {
            return this.SetData(index, System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(data), this.m_target.Index);
        }

        public bool SetData(int index, object data, int indexOverride)
        {
            if (index < 0 || index >= this.m_outputCount)
            {
                System.Guid assert_id = new System.Guid("{12C58BA1-ECC9-44df-98E6-15AE1D6D83B1}");
                Tracing.Assert(assert_id, string.Format("Output parameter Index [{0}] too high or too low for Component {1}.", index, this.m_component.Name));
                return false;
            }
            if (indexOverride < 0)
            {
                System.Guid assert_id = new System.Guid("{5BBD57CA-FD29-4b06-A33C-33A1FB291E20}");
                Tracing.Assert(assert_id, "item_index_override in GH_StructureIterator(Of T).SetData() cannot be negative.");
                return false;
            }
            this.m_outputAssigned[index] = true;
            GH_Path local_path = this.m_target.Path;
            int local_index = indexOverride;
            if (this.m_target.Grafted[index])
            {
                local_path = local_path.AppendElement(local_index);
                local_index = 0;
            }
            return this.Output(index).AddVolatileData(local_path, local_index, System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(data));
        }

        public bool SetDataList(string name, System.Collections.IEnumerable list)
        {
            int i = this.m_component.Params.IndexOfOutputParam(name);
            if (i < 0)
            {
                System.Guid assert_id = new System.Guid("{09911DD2-F487-4990-AB21-9E59684759B9}");
                Tracing.Assert(assert_id, string.Format("Parameter \"{0}\" does not exist.", name));
                return false;
            }
            return this.SetDataList(i, list);
        }

        public bool SetDataList(int index, System.Collections.IEnumerable list)
        {
            return this.SetDataList(index, list, this.m_target.Index);
        }

        public bool SetDataList(int index, System.Collections.IEnumerable list, int itemIndexOverride)
        {
            if (index < 0 || index >= this.m_component.Params.Output.Count)
            {
                System.Guid assert_id = new System.Guid("{7CD6516D-83D6-4c86-B8AB-EE6D7A783B7C}");
                Tracing.Assert(assert_id, string.Format("Output parameter Index [{0}] too high or too low for Component {1}.", index, this.m_component.Name));
                return false;
            }
            if (itemIndexOverride < 0)
            {
                System.Guid assert_id = new System.Guid("{C3306B8E-7FB0-4130-91DE-10248C70024F}");
                Tracing.Assert(assert_id, "item_index_override in GH_StructureIterator(Of T).SetData() cannot be negative.");
                return false;
            }
            IGH_Param param = this.Output(index);
            this.m_outputAssigned[index] = true;
            GH_Path localPath = this.m_target.Path;
            if (!this.m_allLists)
            {
                GH_Path exPath = localPath.AppendElement(itemIndexOverride);
                param.VolatileData.Graft(GH_GraftMode.GraftAll, exPath);
                if (param.VolatileData.PathExists(localPath))
                {
                    param.VolatileData.Graft(GH_GraftMode.GraftAll, localPath);
                }
                this.m_target.Grafted[index] = true;
                localPath = exPath;
            }
            return param.AddVolatileDataList(localPath, list);
        }

        public bool SetDataTree(int index, IGH_DataTree tree)
        {
            if (index < 0 || index >= this.m_outputCount)
            {
                System.Guid assert_id = new System.Guid("{E6A84E37-651D-414b-ACFF-16A1A790F5B4}");
                Tracing.Assert(assert_id, string.Format("Output parameter Index [{0}] too high or too low for Component {1}.", index, this.m_component.Name));
                return false;
            }
            IGH_Param param = this.m_component.Params.Output[index];
            if (param == null)
            {
                System.Guid assert_id = new System.Guid("{1D14E997-C203-4de5-9472-60B4118D1824}");
                Tracing.Assert(assert_id, string.Format("Output parameter Index [{0}] does not exist.", index));
                return false;
            }
            this.m_outputAssigned[index] = true;
            return tree.MergeWithParameter(param);
        }

        public bool SetDataTree(int index, IGH_Structure tree)
        {
            if (index < 0 || index >= this.m_outputCount)
            {
                System.Guid assert_id = new System.Guid("{E6A84E37-651D-414b-ACFF-16A1A790F5B4}");
                Tracing.Assert(assert_id, string.Format("Output parameter Index [{0}] too high or too low for Component {1}.", index, this.m_component.Name));
                return false;
            }
            IGH_Param param = this.m_component.Params.Output[index];
            if (param == null)
            {
                System.Guid assert_id = new System.Guid("{1D14E997-C203-4de5-9472-60B4118D1824}");
                Tracing.Assert(assert_id, string.Format("Output parameter Index [{0}] does not exist.", index));
                return false;
            }
            this.m_outputAssigned[index] = true;
            int arg_8E_0 = 0;
            int num = tree.PathCount - 1;
            for (int i = arg_8E_0; i <= num; i++)
            {
                param.AddVolatileDataList(tree.get_Path(i), tree.get_Branch(i));
            }
            return true;
        }

        public bool BlitData<Q>(int index, GH_Structure<Q> source, bool overwrite) where Q : IGH_Goo
        {
            if (index < 0 || index >= this.m_outputCount)
            {
                System.Guid assert_id = new System.Guid("{8AC7D585-F7FE-413d-AA4D-F4A3BC895815}");
                Tracing.Assert(assert_id, string.Format("Output parameter Index [{0}] too high or too low for Component {1}.", index, this.m_component.Name));
                return false;
            }
            IGH_Param param = this.m_component.Params.Output[index];
            if (param == null)
            {
                System.Guid assert_id = new System.Guid("{8AC7D585-F7FE-413d-AA4D-F4A3BC895815}");
                Tracing.Assert(assert_id, string.Format("Output parameter Index [{0}] does not exist.", index));
                return false;
            }
            IGH_Structure vol_data = param.VolatileData;
            if (vol_data == null)
            {
                System.Guid assert_id = new System.Guid("{97A554C9-26AA-4c86-8C8C-028F4793CB05}");
                Tracing.Assert(assert_id, string.Format("Volatile Data for output parameter index [{0}] hasn't been instantiated yet.", index));
                return false;
            }
            if (!(vol_data is GH_Structure<Q>))
            {
                System.Guid assert_id = new System.Guid("{A24DB009-1A97-4f70-A337-EC4A1B4BCF29}");
                Tracing.Assert(assert_id, string.Format("Output parameter index [{0}] is of a different type.", index));
                return false;
            }
            this.m_outputAssigned[index] = true;
            GH_Structure<Q> @struct = (GH_Structure<Q>)vol_data;
            if (overwrite)
            {
                @struct.Clear();
            }
            if (source != null)
            {
                @struct.MergeStructure(source);
            }
            return true;
        }

        public void ResetGraftedFlags()
        {
            if (this.m_target.Grafted == null)
            {
                if (this.m_outputCount == 0)
                {
                    this.m_target.Grafted = new bool[0];
                }
                else
                {
                    this.m_target.Grafted = new bool[this.m_outputCount - 1 + 1];
                }
            }
            int arg_4D_0 = 0;
            int num = this.m_outputCount - 1;
            for (int i = arg_4D_0; i <= num; i++)
            {
                this.m_target.Grafted[i] = false;
            }
        }

        public void ResetAssignedFlags()
        {
            int arg_0A_0 = 0;
            int num = this.m_outputCount - 1;
            for (int i = arg_0A_0; i <= num; i++)
            {
                this.m_outputAssigned[i] = false;
            }
        }

        public void EnsureAssignments()
        {
            bool addNullItems = true;
            int arg_0C_0 = 0;
            int num = this.m_inputCount - 1;
            for (int i = arg_0C_0; i <= num; i++)
            {
                if (this.m_listLength[i] <= 0 && this.m_inputAccess[i] == GH_ParamAccess.item)
                {
                    if (!this.Input(i).Optional)
                    {
                        addNullItems = false;
                        break;
                    }
                }
            }
            int arg_4C_0 = 0;
            int num2 = this.m_outputCount - 1;
            for (int j = arg_4C_0; j <= num2; j++)
            {
                if (!this.m_outputAssigned[j])
                {
                    if (this.m_outputAccess[j] == GH_ParamAccess.list || this.m_target.Grafted[j])
                    {
                        this.SetDataList(j, null);
                    }
                    else if (this.m_outputAccess[j] != GH_ParamAccess.tree)
                    {
                        if (addNullItems)
                        {
                            this.SetData(j, null);
                        }
                        else
                        {
                            this.Output(j).AddVolatileDataList(this.m_target.Path, null);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Remove all null references from a list of items.
        ///  </summary>
        /// <typeparam name="T">Type of list</typeparam>
        /// <param name="L">List to filter</param>
        /// <returns>List containing only non-null items. References are shared with L</returns>
        public System.Collections.Generic.List<T> Util_RemoveNullRefs<T>(System.Collections.Generic.List<T> L)
        {
            return GH_ListUtil.RemoveNullRefs<T>(L);
        }

        public int Util_CountNullRefs<T>(System.Collections.Generic.List<T> L)
        {
            return GH_ListUtil.CountNullRefs<T>(L);
        }

        /// <summary>
        ///  Count all object references in L
        ///  </summary>
        /// <typeparam name="T">Type of list</typeparam>
        /// <param name="L">List to parse</param>
        /// <returns>Number of proper objects in L</returns>
        public int Util_CountNonNullRefs<T>(System.Collections.Generic.List<T> L)
        {
            return GH_ListUtil.CountNonNullRefs<T>(L);
        }

        public bool Util_EnsureNonNullCount<T>(System.Collections.Generic.List<T> L, int N)
        {
            return GH_ListUtil.EnsureNonNullCount<T>(L, N);
        }

        /// <summary>
        ///  Returns the index of the first non-null item in a list.
        ///  </summary>
        /// <typeparam name="T">Type of list</typeparam>
        /// <param name="L">List to parse.</param>
        /// <returns>The index of the first non-null item or -1 if no object reference could be found.</returns>
        public int Util_FirstNonNullItem<T>(System.Collections.Generic.List<T> L)
        {
            return GH_ListUtil.FirstNonNullItem<T>(L);
        }
    }

}
