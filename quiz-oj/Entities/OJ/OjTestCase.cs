using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using quiz_oj.Configs.Exceptions;
using quiz_oj.IndependentNS;
using System.Runtime.InteropServices;

namespace quiz_oj.Entities.OJ
{
    [Table("OjTestCase")]
    public class OjTestCaseTable
    {
        [Key]
        [Column("ojId")]
        public string OjId { get; set; }
        
        [Column("testCaseJson")]
        public string TestCaseSetJson { get; set; }
    }

    public class OjTestCaseSet
    {

        [DllImport("TestATL", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CheckListNodeStrValidity(string s);

        [AllowNull]
        public string OjId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Difficulty { get; set; }
        public string OjCode { get; set; }
        public string MethodName { get; set; }
        public int ParamCount { get; set; }
        public List<string> ParamTypes {
            get => _paramTypes;
            set
            {
                foreach (var paramType in value)
                {
                    switch (paramType)
                    {
                        case "int": case "float": case "double": case "long": 
                        case "bool": case "char": case "string": case "ListNode":
                        case "TreeNode":
                            continue;
                        default:
                            throw new UserException("ParamType illegal!");
                    }
                }
                _paramTypes = value;
            }
        }

        private List<string> _paramTypes;
        public string[][] Params { get; set; } 
        public string[] ExpectedOutputs { get; set; }
        public string OutputType { get; set; }

        public object[][] CastParamsToObjects()
        {
            var res = new object[Params.Length][];

            for (var i = 0; i < Params.Length; i++)
            {
                res[i] = new object[ParamCount];
                var ps = Params[i];
                for (var j = 0; j < ps.Length; j++)
                {
                    var paramString = ps[j];
                    res[i][j] = CastFunc(_paramTypes[j], paramString);
                }
            }
            return res;
        }

        public object[] CastExpectedReturnValues()
        {
            return ExpectedOutputs.Select(o => CastFunc(OutputType, o)).ToArray();
        }

        public bool IsValidTestSet()
        {
            try
            {
                CastExpectedReturnValues();
                CastParamsToObjects();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        private static object CastFunc(string paramType, string paramValue)
        {
            return paramType switch
            {
                "int" => int.Parse(paramValue),
                "long" => long.Parse(paramValue),
                "float" => float.Parse(paramValue),
                "double" => double.Parse(paramValue),
                "char" => char.Parse(paramValue),
                "bool" => bool.Parse(paramValue),
                "string" => paramValue,
                "ListNode" => DataStructureParser.ParseListNode(paramValue),
                "TreeNode" => DataStructureParser.ParseTreeNode(paramValue),
                _ => throw new NonUserException("Impossible")
            };
        }
    }
}