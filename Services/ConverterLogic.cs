
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PracticalWork2Calculator.Services
{
    public static class ConverterLogic
    {
        
        public static (bool IsValid, string ErrorMessage) ValidateInput(string value, string fromBase, int? bits = null)
        {
            if (string.IsNullOrWhiteSpace(value)) return (false, "La entrada no puede estar vacía.");

            switch (fromBase)
            {
                case "Binary":
                    if (!Regex.IsMatch(value, "^[01]+$")) return (false, "Entrada binaria inválida (solo 0 y 1).");
                    if (bits.HasValue && value.Length > bits.Value) return (false, $"La entrada binaria no puede exceder los {bits.Value} bits.");
                    break;
                case "Decimal":
                    if (!Regex.IsMatch(value, @"^-?[0-9]+$")) return (false, "Entrada decimal inválida.");
                    break;
                case "Octal":
                    if (!Regex.IsMatch(value, "^[0-7]+$")) return (false, "Entrada octal inválida (solo 0-7).");
                    break;
                case "Hexadecimal":
                    if (!Regex.IsMatch(value, "^[0-9A-Fa-f]+$")) return (false, "Entrada hexadecimal inválida (solo 0-9, A-F).");
                    break;
                case "TwoComplement":
                    if (!Regex.IsMatch(value, "^[01]+$")) return (false, "Entrada de Complemento a Dos inválida (solo 0 y 1).");
                    if (bits.HasValue && value.Length != bits.Value) return (false, $"Complemento a Dos debe tener exactamente {bits.Value} bits.");
                    break;
                default:
                    return (false, "Tipo de base de origen desconocida.");
            }
            return (true, string.Empty);
        }

        public static string ConvertBinaryToDecimal(string binary) => Convert.ToInt64(binary, 2).ToString();
        public static string ConvertDecimalToBinary(string dec) => Convert.ToString(long.Parse(dec), 2);
        public static string ConvertDecimalToOctal(string dec) => Convert.ToString(long.Parse(dec), 8);
        public static string ConvertDecimalToHexadecimal(string dec) => Convert.ToString(long.Parse(dec), 16).ToUpper();
        public static string ConvertOctalToDecimal(string octal) => Convert.ToInt64(octal, 8).ToString();
        public static string ConvertHexadecimalToDecimal(string hex) => Convert.ToInt64(hex, 16).ToString();

        public static string ConvertDecimalToTwoComplement(string decStr, int bits)
        {
            if (!long.TryParse(decStr, out long dec)) throw new ArgumentException("Entrada decimal inválida.");
            
            long maxValue = (1L << (bits - 1)) - 1;
            long minValue = -(1L << (bits - 1));

            if (dec < minValue || dec > maxValue)
                throw new ArgumentOutOfRangeException($"El valor está fuera del rango para {bits} bits ({minValue} a {maxValue}).");

            if (dec >= 0)
            {
                return Convert.ToString(dec, 2).PadLeft(bits, '0');
            }
            else
            {
                
                return Convert.ToString((1L << bits) + dec, 2).PadLeft(bits, '0');
            }
        }

        public static string ConvertTwoComplementToDecimal(string binary, int bits)
        {
            if (binary.Length != bits) throw new ArgumentException($"El binario debe tener {bits} bits.");
            if (binary[0] == '0') 
            {
                return Convert.ToInt64(binary, 2).ToString();
            }
            else 
            {
                long val = Convert.ToInt64(binary, 2);
                return (val - (1L << bits)).ToString();
            }
        }
    }
}