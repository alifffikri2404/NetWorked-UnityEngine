//using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NullSave.GDTK
{
    public delegate void MathFunction(string value, out string result);

    public delegate void MathFunctionSolver(string fuction, string value, out string result);

    public static class MathSolver
    {

        #region Constants

        private const string BASIC_EXPRESSIONS = "=+-*/^%";

        #endregion

        #region Fields

        private static Dictionary<string, MathFunction> m_functionList;

        #endregion

        #region Fields

        public static Dictionary<string, MathFunction> functions
        {
            get
            {
                if (m_functionList == null)
                {
                    m_functionList = new Dictionary<string, MathFunction>();
                    RegisterFunctions();

                    var sorted = m_functionList.OrderByDescending(m => m.Key.Length);

                    Dictionary<string, MathFunction> tempList = new Dictionary<string, MathFunction>();
                    foreach (var sort in sorted)
                    {
                        tempList.Add(sort.Key, sort.Value);
                    }

                    m_functionList = tempList;
                }

                return m_functionList;
            }
        }

        public static MathFunctionSolver unknownFunctionHandler { get; set; }

        public static MathFunction unknownValueHandler { get; set; }

        #endregion

        #region Public Methods

        public static bool IsTrue(string condition)
        {
            if (string.IsNullOrEmpty(condition)) return true;

            List<string> parts = new List<string>();

            int andIndex, orIndex, idx;
            while (true)
            {
                andIndex = condition.IndexOf("&&");
                orIndex = condition.IndexOf("||");
                if (andIndex == -1 && orIndex == -1) break;

                if (andIndex == -1) andIndex = int.MaxValue;
                if (orIndex == -1) orIndex = int.MaxValue;
                idx = Mathf.Min(andIndex, orIndex);

                parts.Add(condition.Substring(0, idx));
                parts.Add(condition.Substring(idx, 2));
                condition = condition.Substring(idx + 2);
            }
            parts.Add(condition);

            bool res = false;
            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i] == "&&")
                {
                    res = res && EvalCondition(parts[i + 1]);
                    i += 1;
                }
                else if (parts[i] == "||")
                {
                    res = res || EvalCondition(parts[i + 1]);
                    i += 1;
                }
                else
                {
                    res = EvalCondition(parts[i]);
                }
            }

            return res;
        }

        public static double Parse(string formula)
        {
            if (string.IsNullOrEmpty(formula)) return 0;

            if (formula.IsNumeric())
            {
                return double.Parse(formula, CultureInfo.InvariantCulture);
            }

            return ParseValue(formula);
        }

        public static string[] SplitMath(string value, bool removeSymbols = false, bool removeCommands = false)
        {
            List<string> results = new List<string>();

            bool lastWasOp = false;
            for (int i = 0; i < value.Length; i++)
            {
                switch (value[i])
                {
                    case '-':
                        if (lastWasOp)
                        {
                            // Special case, need to account for negative numbers
                            lastWasOp = false;
                        }
                        else
                        {
                            if (i > 0)
                            {
                                AddComponent(results, value.Substring(0, i).Trim(), removeCommands);
                            }

                            if (!removeSymbols) results.Add("-");
                            value = value.Substring(i + 1);
                            lastWasOp = true;
                            i = -1;
                        }
                        break;
                    case '(':
                    case ')':
                    case '+':
                    case '*':
                    case '/':
                    case '=':
                    case '^':
                    case '<':
                    case '>':
                    case '&':
                    case '%':
                    case '[':
                    case ']':
                        if (i > 0)
                        {
                            AddComponent(results, value.Substring(0, i).Trim(), removeCommands);
                        }

                        if (!removeSymbols) results.Add(value.Substring(i, 1));
                        value = value.Substring(i + 1);
                        i = -1;

                        lastWasOp = true;
                        break;
                    case ' ':
                        // Break whitespace
                        if (i > 0)
                        {
                            AddComponent(results, value.Substring(0, i).Trim(), removeCommands);
                        }

                        value = value.Substring(i + 1);
                        i = -1;
                        lastWasOp = false;
                        break;
                    default:
                        lastWasOp = false;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(value))
            {
                results.Add(value);
            }

            return results.ToArray();
        }

        #endregion

        #region Private Methods

        private static void AddComponent(List<string> results, string component, bool ignoreCommands)
        {
            component = component.Trim();
            if (string.IsNullOrEmpty(component)) return;

            if (ignoreCommands)
            {
                switch (component.ToLower())
                {
                    case "abs":
                    case "roll":
                    case "pow":
                    case "floor":
                    case "ceil":
                    case "rnd_f":
                    case "rnd_i":
                    case "max":
                    case "min":
                        return;
                }
            }

            results.Add(component);
        }

        private static bool EvalCondition(string equation)
        {
            if (string.IsNullOrEmpty(equation)) return true;

            string check = "=";

            int ei = equation.IndexOf('=');
            if (ei > 0)
            {

                if (equation.Substring(ei - 1, 1) == "<")
                {
                    check = "<=";
                    ei -= 1;
                }
                else if (equation.Substring(ei - 1, 1) == ">")
                {
                    check = ">=";
                    ei -= 1;
                }
                else if (equation.Substring(ei - 1, 1) == "!")
                {
                    check = "!=";
                    ei -= 1;
                }

                double v1 = Parse(equation.Substring(0, ei));
                double v2 = Parse(equation.Substring(ei + check.Length));

                return check switch
                {
                    "<=" => v1 <= v2,
                    ">=" => v1 >= v2,
                    "!=" => v1 != v2,
                    _ => v1 == v2,
                };
            }

            check = "<";
            ei = equation.IndexOf('<');
            if (ei > 0)
            {
                double v1 = Parse(equation.Substring(0, ei));
                double v2 = Parse(equation.Substring(ei + check.Length));
                return v1 < v2;
            }

            check = ">";
            ei = equation.IndexOf('>');
            if (ei > 0)
            {
                double v1 = Parse(equation.Substring(0, ei));
                double v2 = Parse(equation.Substring(ei + check.Length));
                return v1 > v2;
            }

            return Parse(equation) > 0;
        }

        private static string HandlePEMDAS(string equation, char op)
        {
            int opLoc;
            int left, right;
            string res = string.Empty;

            while (true)
            {
                // Find sides
                opLoc = equation.IndexOf(op, 1);
                if (opLoc == -1) return equation;
                left = LeftValueStart(equation, opLoc);
                right = RightValueStart(equation, opLoc);

                try
                {
                    // Perform math
                    switch (op)
                    {
                        case '^':
                            res = Mathf.Pow(ParseToFloat(equation, left, opLoc - left), ParseToFloat(equation, opLoc + 1, right - opLoc)).ToString(CultureInfo.InvariantCulture);
                            break;
                        case '*':
                            res = (ParseToFloat(equation, left, opLoc - left) * ParseToFloat(equation, opLoc + 1, right - opLoc)).ToString(CultureInfo.InvariantCulture);
                            break;
                        case '/':
                            res = (ParseToFloat(equation, left, opLoc - left) / ParseToFloat(equation, opLoc + 1, right - opLoc)).ToString(CultureInfo.InvariantCulture);
                            break;
                        case '+':
                            res = (ParseToFloat(equation, left, opLoc - left) + ParseToFloat(equation, opLoc + 1, right - opLoc)).ToString(CultureInfo.InvariantCulture);
                            break;
                        case '-':
                            res = (ParseToFloat(equation, left, opLoc - left) - ParseToFloat(equation, opLoc + 1, right - opLoc)).ToString(CultureInfo.InvariantCulture);
                            break;
                        case '%':
                            res = (ParseToFloat(equation, left, opLoc - left) % ParseToFloat(equation, opLoc + 1, right - opLoc)).ToString(CultureInfo.InvariantCulture);
                            break;
                    }
                }
                catch
                {
                    StringExtensions.LogError("LogicExtensions.cs", "HandlePEMDAS", "Cannot parse '" + equation + "'");
                    throw;
                }

                // Replace in equation
                equation = equation.Replace(equation.Substring(left, right - left + 1), res);
            }
        }

        private static bool IsMathExpression(string value)
        {
            if (value == "+" || value == "-" || value == "=" || value == "^" || value == "/" || value == "||" ||
                value == "*" || value == "&&" || value == "<" || value == ">" || value == "<=" || value == ">=" ||
                value == "(" || value == ")" || value == "%")
            {
                return true;
            }

            return false;
        }

        private static int LeftValueStart(string equation, int opLoc)
        {
            int startFrom = opLoc - 1;

            while (startFrom > 0)
            {
                if (BASIC_EXPRESSIONS.Contains(equation.Substring(startFrom, 1)))
                {
                    startFrom += 1;
                    break;
                }
                startFrom -= 1;
            }

            return startFrom;
        }

        private static float ParseToFloat(string equation, int start, int length)
        {
            return float.Parse(equation.Substring(start, length).Replace(string.Empty + (char)1, "E-").Replace(string.Empty + (char)2, "E+"), CultureInfo.InvariantCulture);
        }

        private static double ParseValue(string formula)
        {
            StringBuilder sb = new StringBuilder();

            // Solve System Functions first
            formula = SolveFunctions(formula, functions);

            // Replace variales with values
            // Output to new formula
            string[] parts = SplitMath(formula);
            foreach (string part in parts)
            {
                if (part.IsNumeric() || IsMathExpression(part) || part.Contains("[") || part == "]")
                {
                    sb.Append(part);
                }
                else
                {
                    if (part.Contains("["))   // This is a function
                    {
                        sb.Append(part);
                    }
                    else
                    {
                        if (unknownValueHandler != null)
                        {
                            unknownValueHandler.Invoke(part, out string result);
                            sb.Append(result);
                        }
                        else
                        {

                            throw new System.Exception($"Invalid command: {part}");
                        }
                    }
                }
            }

            return SolveMath(sb.ToString());
        }

        private static void RegisterFunctions()
        {
            m_functionList.Add("abs", AbsoluteValue);
            m_functionList.Add("ceil", Ceiling);
            m_functionList.Add("cos", Cosine);
            m_functionList.Add("floor", Floor);
            m_functionList.Add("log", Logarithm);
            m_functionList.Add("max", Max);
            m_functionList.Add("min", Min);
            m_functionList.Add("rnd", RandomFloat);
            m_functionList.Add("rnd_f", RandomFloat);
            m_functionList.Add("rnd_i", RandomInt);
            m_functionList.Add("roll", RollDice);
            m_functionList.Add("sin", Sine);
            m_functionList.Add("tan", Tangent);
        }

        private static int RightValueStart(string equation, int opLoc)
        {
            int startFrom = opLoc + 1;

            while (startFrom < equation.Length - 1)
            {
                if (BASIC_EXPRESSIONS.Contains(equation.Substring(startFrom, 1)))
                {
                    if (equation[startFrom] == '-' &&
                        (equation[startFrom - 1] == '+' || equation[startFrom - 1] == '*' || equation[startFrom - 1] == '/'))
                    {
                        // Allow
                    }
                    else if (equation[startFrom] != '-' || equation[startFrom - 1] != '-')
                    {
                        startFrom -= 1;
                        break;
                    }
                }
                startFrom += 1;
            }

            return startFrom;
        }

        private static string SolveFunctions(string value, IReadOnlyDictionary<string, MathFunction> customFunctions)
        {
            if (customFunctions == null) return value;

            int start, end, i;
            string mid, fnc;

            while (true)
            {
                start = value.LastIndexOf('[');   // Start of function params
                if (start < 0) break;   // No mo functions
                end = value.IndexOf(']', start);  // End of function params

                i = start;
                while (true)
                {
                    i -= 1;
                    if (char.IsLetter(value[i]) && i > 0) continue;
                    break;
                }

                fnc = value.Substring(i, start - i).Trim();

                if (functions.ContainsKey(fnc))
                {
                    functions[fnc].Invoke(value.Substring(start + 1, end - start - 1), out mid);
                    value = value.Substring(0, start - fnc.Length) + mid + value.Substring(end + 1);
                }
                else
                {
                    if (unknownFunctionHandler != null)
                    {
                        unknownFunctionHandler.Invoke(fnc, value.Substring(start + 1, end - start - 1), out mid);
                        value = value.Substring(0, start - fnc.Length) + mid + value.Substring(end + 1);
                    }
                    else
                    {
                        value = value.Substring(0, start - fnc.Length) + "0" + value.Substring(end + 1);
                        StringExtensions.LogError("MathSolver", "SolveFunctions", "Unknown function '" + fnc + "' in " + value);
                        return value;
                    }
                }
            }

            return value;
        }

        private static double SolveMath(string value)
        {
            try
            {
                int open, close;

                // Handle parenthesis
                while (true)
                {
                    open = value.LastIndexOf('(');
                    if (open >= 0)
                    {
                        close = value.IndexOf(')', open);
                        if (close < 0)
                        {
                            StringExtensions.LogError("LogicExtensions.cs", "SolveMath", "Invalid expression: " + value);
                            return 0;
                        }
                        else
                        {
                            value = value.Substring(0, open) + SolveMath(value.Substring(open + 1, close - open - 1)) + value.Substring(close + 1);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                // Handle E-/+
                value = value.Replace("E-", string.Empty + (char)1);
                value = value.Replace("E+", string.Empty + (char)2);

                // Exponents
                value = HandlePEMDAS(value, '^');

                // Multiplication
                value = HandlePEMDAS(value, '*');

                // Division
                value = HandlePEMDAS(value, '/');

                // Modulus
                value = HandlePEMDAS(value, '%');

                // Addition
                value = HandlePEMDAS(value, '+');

                // Subtraction
                value = HandlePEMDAS(value, '-');

                return double.Parse(value, CultureInfo.InvariantCulture);
            }
            catch (System.Exception ex)
            {
                StringExtensions.LogError("LogicExtensions.cs", "SolveMath", ex.Message);
            }

            return 0;
        }

        #endregion

        #region Math Functions

        private static void AbsoluteValue(string request, out string result)
        {
            result = Mathf.Abs((float)Parse(request)).ToString();
        }

        private static void Ceiling(string request, out string result)
        {
            result = Mathf.Ceil((float)Parse(request)).ToString();
        }

        private static void Cosine(string request, out string result)
        {
            result = Mathf.Acos((float)Parse(request)).ToString();
        }

        private static void Floor(string request, out string result)
        {
            result = Mathf.Floor((float)Parse(request)).ToString();
        }

        private static void Logarithm(string request, out string result)
        {
            result = Mathf.Log((float)Parse(request)).ToString();
        }

        private static void Max(string request, out string result)
        {
            string[] parts = request.Split(';');

            result = Mathf.Max((float)Parse(parts[0]), (float)Parse(parts[1])).ToString();
        }

        private static void Min(string request, out string result)
        {
            string[] parts = request.Split(';');

            result = Mathf.Min((float)Parse(parts[0]), (float)Parse(parts[1])).ToString();
        }

        private static void RandomFloat(string request, out string result)
        {
            string[] parts = request.Split(';');
            if (parts.Length == 1)
            {
                result = Random.Range(0, (float)Parse(request)).ToString();
            }
            else
            {
                result = Random.Range((float)Parse(parts[0]), (float)Parse(parts[1])).ToString();
            }
        }

        private static void RandomInt(string request, out string result)
        {
            string[] parts = request.Split(';');
            if (parts.Length == 1)
            {
                result = Random.Range(0, (int)Parse(request)).ToString();
            }
            else
            {
                result = Random.Range((int)Parse(parts[0]), (int)Parse(parts[1])).ToString();
            }
        }

        private static void RollDice(string request, out string result)
        {
            string[] parts = request.Split('d');
            double value = 0;
            for (int i = 0; i < Parse(parts[0]); i++)
            {
                value += Random.Range(1, (int)Parse(parts[1]) + 1);
            }
            result = value.ToString();
        }

        private static void Sine(string request, out string result)
        {
            result = Mathf.Sin((float)Parse(request)).ToString();
        }

        private static void Tangent(string request, out string result)
        {
            result = Mathf.Tan((float)Parse(request)).ToString();
        }

        #endregion

    }
}