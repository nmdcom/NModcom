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
using System.IO;
using System.Reflection;

namespace SimObj.Documentation.App
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine($"\t{Assembly.GetExecutingAssembly().GetName().Name} p1 [p2]");
                Console.WriteLine("\t\twith\tp1 = Assembly name");
                Console.WriteLine("\t\t\tp2 = Output directory (optional)");
            }
            else if (!File.Exists(args[0]))
            {
                Console.WriteLine($"Error: Assembly {args[0]} doesn't exist");
            }
            else if (args.Length >= 2 && !Directory.Exists(args[1]))
            {
                Console.WriteLine($"Error: Output directory {args[1]} doesn't exist");
            }
            else
            {
                var assemblyName = args[0];
                var outputDirectory = (args.Length >= 2) ? args[1] : ".";

                var collector = new MarkDownCollector(assemblyName);
                if (collector.CollectAttributeData())
                {
                    File.WriteAllLines(Path.Combine(outputDirectory, collector.MarkDownFile), collector.MarkDownText);
                    Console.WriteLine($"Markdown file {collector.MarkDownFile} written");
                }
                else
                    Console.WriteLine($"No attribute data found in {assemblyName}");
            }

            Console.WriteLine();
            Console.WriteLine("Any key...");
            Console.ReadKey();
        }
    }
}
