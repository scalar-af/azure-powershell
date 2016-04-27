// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using Microsoft.Azure.Management.Compute;
using Microsoft.Azure.Management.Compute.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Azure.Commands.Compute
{
    public abstract class VirtualMachineBaseCmdlet : ComputeClientBaseCmdlet
    {
        protected const InstanceViewTypes InstanceViewExpand = InstanceViewTypes.InstanceView;

        public IVirtualMachinesOperations VirtualMachineClient
        {
            get
            {
                return ComputeClient.ComputeManagementClient.VirtualMachines;
            }
        }

        public static string FormatObject(Object obj)
        {
            var objType = obj.GetType();

            System.Reflection.PropertyInfo[] pros = objType.GetProperties();
            string result = "\n";
            var resultTuples = new List<Tuple<string, string, int>>();
            var totalTab = GetTabLength(obj, 0, 0, resultTuples) + 1;
            foreach (var t in resultTuples)
            {
                string preTab = new string(' ', t.Item3 * 2);
                string postTab = new string(' ', totalTab - t.Item3 * 2 - t.Item1.Length);

                result += preTab + t.Item1 + postTab + ": " + t.Item2 + "\n";
            }
            return result;
        }

        private static int GetTabLength(Object obj, int max, int depth, List<Tuple<string, string, int>> tupleList)
        {
            var objType = obj.GetType();
            var propertySet = new List<PropertyInfo>();
            if (objType.BaseType != null)
            {
                foreach (var property in objType.BaseType.GetProperties())
                {
                    propertySet.Add(property);
                }
            }
            foreach (var property in objType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
            {
                propertySet.Add(property);
            }

            foreach (var property in propertySet)
            {
                Object childObject = property.GetValue(obj, null);
                var elem = childObject as IList;
                if (elem != null)
                {
                    if (elem.Count != 0)
                    {
                        max = Math.Max(max, depth * 2 + property.Name.Length + 4);
                        for (int i = 0; i < elem.Count; i++)
                        {
                            Type propType = elem[i].GetType();

                            if (propType.IsSerializable)
                            {
                                tupleList.Add(MakeTuple(property.Name + "[" + i + "]", elem[i].ToString(), depth));
                            }
                            else
                            {
                                tupleList.Add(MakeTuple(property.Name + "[" + i + "]", "", depth));
                                max = Math.Max(max, GetTabLength((Object)elem[i], max, depth + 1, tupleList));
                            }
                        }
                    }
                }
                else
                {
                    if (property.PropertyType.IsSerializable)
                    {
                        if (childObject != null)
                        {
                            tupleList.Add(MakeTuple(property.Name, childObject.ToString(), depth));
                            max = Math.Max(max, depth * 2 + property.Name.Length);
                        }
                    }
                    else
                    {
                        var isDictionary = childObject as IDictionary;
                        if (isDictionary != null)
                        {
                            tupleList.Add(MakeTuple(property.Name, Newtonsoft.Json.JsonConvert.SerializeObject(childObject), depth));
                            max = Math.Max(max, depth * 2 + property.Name.Length);
                        }
                        else if (childObject != null)
                        {
                            tupleList.Add(MakeTuple(property.Name, "", depth));
                            max = Math.Max(max, GetTabLength(childObject, max, depth + 1, tupleList));
                        }
                    }
                }
            }
            return max;
        }

        private static Tuple<string, string, int> MakeTuple(string key, string value, int depth)
        {
            return new Tuple<string, string, int>(key, value, depth);
        }      
    }
}
