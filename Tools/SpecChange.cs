using BarcodeMainApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BarcodeMainApp.Tools
{
    internal class SpecChange
    {


        /// <summary>
        /// 计算规格组分的最终数值（用于生成规格转换码的两位数字）
        /// 规则：
        /// - 分数或混合数：直接 ×16，取整数部分
        /// - 角度（带 ° 符号）：去掉符号，直接取数字
        /// - 小数（如 1.5）：×10 取整
        /// - 纯整数：直接返回
        /// - 字母（如 R）：需要在外部转数字，此处不处理
        /// </summary>
        /// <param name="comp">规格组分字符串，如 "1/2", "1-1/2", "45°", "6", "1.5"</param>
        /// <returns>整数值（0-99）</returns>
        public static string CalculateSpecComponent(string comp, double SpecCalcBase)
        {
            if (string.IsNullOrWhiteSpace(comp))
                return "00";

            comp = comp.Trim();

            // 1. 纯字母处理（A-Z 或 a-z）：转为字母序号 (A=01, B=02, ...)
            if (Regex.IsMatch(comp, @"^[A-Za-z]$"))
            {
                int letterValue = char.ToUpper(comp[0]) - 'A' + 1;
                return letterValue.ToString("D2");   // 补零至两位
            }

            // 2. 角度处理：去掉 ° 符号，返回整数部分
            if (comp.EndsWith("°"))
            {
                string numPart = comp.TrimEnd('°');
                if (int.TryParse(numPart, out int deg))
                    return deg.ToString("D2");
                throw new FormatException($"无法解析角度值：{comp}");
            }

            // 3. 混合数：整数-分子/分母 -> 转为小数 × 基数，取整
            var mixedMatch = Regex.Match(comp, @"^(\d+)-(\d+)/(\d+)$");
            if (mixedMatch.Success)
            {
                int whole = int.Parse(mixedMatch.Groups[1].Value);
                int num = int.Parse(mixedMatch.Groups[2].Value);
                int den = int.Parse(mixedMatch.Groups[3].Value);
                if (den == 0) throw new DivideByZeroException($"分母为零：{comp}");
                double value = whole + (double)num / den;
                int result = (int)Math.Round(value * SpecCalcBase);
                return result.ToString("D2");
            }

            // 4. 纯分数：分子/分母 -> × 基数，取整
            var fracMatch = Regex.Match(comp, @"^(\d+)/(\d+)$");
            if (fracMatch.Success)
            {
                int num = int.Parse(fracMatch.Groups[1].Value);
                int den = int.Parse(fracMatch.Groups[2].Value);
                if (den == 0) throw new DivideByZeroException($"分母为零：{comp}");
                double value = (double)num / den;
                int result = (int)Math.Round(value * SpecCalcBase);
                return result.ToString("D2");
            }

            // 5. 纯数字（整数或小数）
            if (double.TryParse(comp, out double dVal))
            {
                int result;
                if (comp.Contains("."))
                {
                    // 小数：×10 取整
                    result = (int)Math.Round(dVal * 10);
                }
                else
                {
                    // 整数：直接使用
                    result = (int)dVal;
                }
                return result.ToString("D2");
            }

            throw new FormatException($"无法识别的规格组分：{comp}");
        }

        /// <summary>
        /// 新增或编辑时，自动填充规格转换码（7位数字），规则：字母转换成对应的数字
        /// </summary>
        /// <param name="displayCode"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string ConvertToCode(string displayCode)
        {
            // 去掉连字符
            string cleaned = displayCode.Replace("-", "");
            StringBuilder sb = new StringBuilder();
            foreach (char c in cleaned)
            {
                if (char.IsLetter(c))
                {
                    int val = char.ToUpper(c) - 'A' + 1;
                    sb.Append(val.ToString("D2")); // 两位
                }
                else if (char.IsDigit(c))
                {
                    sb.Append(c);
                }
            }
            string result = sb.ToString();
            // 不足7位左侧补零
            if (result.Length < 7)
                result = result.PadLeft(7, '0');
            else if (result.Length > 7)
                throw new Exception("编号转换结果超过7位，请检查规则");
            return result;
        }

        /// <summary>
        /// 分别计算规格的三个组分，并返回最终的规格转换码（2位数字）
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public static string ConvertSpec(Spec spec)
        {
            string c1 = CalculateSpecComponent(spec.Comp1, spec.CalcBase);
            string c2 = CalculateSpecComponent(spec.Comp2, spec.CalcBase);
            string c3 = CalculateSpecComponent(spec.Comp3, spec.CalcBase);
            return c1 + c2 + c3;
        }
    }
}
