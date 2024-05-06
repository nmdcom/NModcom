/*
 * ==============================================================================
 * NMODCOM: software for component-based simulation
 * 
 * MIT License
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * ================================================================================
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NModcom;

namespace SimObj.Documentation.App
{
    public class MarkDownCollector
    {
        #region Fields

        private static string cNone = "None";

        private readonly string _assemblyName;

        #endregion

        #region Properties

        public string MarkDownFile => Path.ChangeExtension(Path.GetFileName(_assemblyName), ".md");

        public IList<string> MarkDownText { get; } = new List<string>();

        #endregion

        #region Methods

        public MarkDownCollector()
        { }

        public MarkDownCollector(string assemblyName)
        {
            _assemblyName = assemblyName;
        }

        public bool CollectAttributeData()
        {
            MarkDownText.Clear();
            if (!File.Exists(_assemblyName))
                return false;

            var context = new SimObjDocumentationContextt("docContext", Path.GetDirectoryName(_assemblyName), true);
            var assembly = context.LoadFromAssemblyPath(_assemblyName);

            var simObjType = typeof(ISimObj);
            var types = GetLoadableTypes(assembly).Where(t => simObjType.IsAssignableFrom(t));

            foreach (var type in types)
            {
                var simobj = default(ISimObj);
                var ctorInfos = type.GetConstructors();

                if (ctorInfos.Length > 0)
                {
                    var paramInfos = ctorInfos[0].GetParameters();
                    var objList = new List<object>();

                    foreach (var pi in paramInfos)
                    {
                        var obj = GetDefaultValue(pi.ParameterType);
                        objList.Add(obj);
                    }

                    simobj = ctorInfos[0].Invoke(objList.ToArray()) as ISimObj;
                }

                if (simobj != default(ISimObj))
                    CollectFromInstance(simobj);
                else
                    CollectFromAttributes(type);
            }

            return MarkDownText.Any();
        }

        public bool CollectAttributeDataForSimObj(ISimObj simobj)
        {
            MarkDownText.Clear();

            CollectFromInstance(simobj);

            return MarkDownText.Any();
        }

        #region .Supporting

        private bool CheckHeader(bool hasHeader)
        {
            if (!hasHeader)
            {
                MarkDownText.Add("| Name | Description | Units | Fld/Meth | Type |");
                MarkDownText.Add("| :--- | :--- | :--- | :-: | :--- |");
            }

            return true;
        }

        private void CollectFromAttributes(Type type)
        {
            MarkDownText.Add($"# Simulationmodel <a id=\"{type.Name}\"></a>{type.Name}");

            MarkDownText.Add("## Inputs");
            if (!CollectAttributedFields<InputAttribute>(type))
                MarkDownText.Add(cNone);

            MarkDownText.Add("## Outputs");
            var hasHeader = CollectAttributedFields<OutputAttribute>(type);
            hasHeader = CollectAttributedMethods<OutputAttribute>(type, hasHeader);
            if (!hasHeader)
                MarkDownText.Add(cNone);

            MarkDownText.Add("## Params");
            if (!CollectAttributedFields<ParamAttribute>(type))
                MarkDownText.Add(cNone);

            MarkDownText.Add("## Signals");
            if (!CollectAttributedFields<SignalAttribute>(type))
                MarkDownText.Add(cNone);

            MarkDownText.Add("## States");
            if (!CollectAttributedFields<StateAttribute>(type))
                MarkDownText.Add(cNone);
        }

        private void CollectFromInstance(ISimObj simObj)
        {
            simObj.Initialize();

            MarkDownText.Add($"# Simulationmodel <a id=\"{simObj.Name}\"></a>{simObj.Name}");

            MarkDownText.Add("## Inputs");
            var hasHeader = false;

            foreach (IInput input in simObj.Inputs)
            {
                hasHeader = CheckHeader(hasHeader);
                var dataType = (input.Data != default(IData)) ? input.Data.DataType : typeof(IData);
                MarkDownText.Add($"|{input.Name} | {input.Description} | {input.Units} | Field | {dataType.Name} |");
            }

            if (!hasHeader)
                MarkDownText.Add(cNone);

            MarkDownText.Add("## Outputs");
            hasHeader = false;

            foreach (IOutput output in simObj.Outputs)
            {
                hasHeader = CheckHeader(hasHeader);
                var dataType = (output.Data != default(IData)) ? output.Data.DataType : typeof(IData);
                MarkDownText.Add($"|{output.Name} | {output.Description} | {output.Units} | Field | {dataType.Name} |");
            }

            if (!hasHeader)
                MarkDownText.Add(cNone);
        }

        private IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        private object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            return null;
        }

        private bool CollectAttributedFields<TAttribute>(Type simObjType, bool hasHeader = false)
            where TAttribute : SimObjAttribute
        {
            // examine each field
            foreach (var field in simObjType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                // find all the attributes of type TAttribute that are applied to our field
                var attributes = field.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>();

                foreach (var attribute in attributes)
                {
                    hasHeader = CheckHeader(hasHeader);
                    MarkDownText.Add($"|{attribute.Name} | {attribute.Description} | {attribute.Units} | Field | {field.FieldType.Name} |");
                }
            }

            return hasHeader;
        }

        private bool CollectAttributedMethods<TAttribute>(Type simObjType, bool hasHeader = false)
            where TAttribute : SimObjAttribute
        {
            // examine each method
            foreach (var method in simObjType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                // find all the attributes that are applied to this method
                var attributes = method.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>();

                foreach (var attribute in attributes)
                {
                    hasHeader = CheckHeader(hasHeader);
                    MarkDownText.Add($"|{attribute.Name} | {attribute.Description} | {attribute.Units} | Method | {method.ReturnType.Name} |");
                }
            }

            return hasHeader;
        }

        #endregion

        #endregion
    }
}
