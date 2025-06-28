using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtils
{
    public class ValidationFunctions
    {
        // Testing can be performed on this function
        public bool isValidEmail(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input) || input.Length > maxLength)
                return false;

            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        }

        public string isValidPassword(string input, int maxLength)
        {
            string cleanedInput = System.Text.RegularExpressions.Regex.Replace(input, @"[^a-zA-Z0-9]", "");

            if (cleanedInput.Length > maxLength)
            {
                cleanedInput = cleanedInput.Substring(0, maxLength);    // extracting the allowed number of characters from 0 index to maxlength index
            }

            return cleanedInput;
        }

        public string isValidUsername(string input, int maxLength)
        {
            string cleanedInput = System.Text.RegularExpressions.Regex.Replace(input, @"[^a-zA-Z0-9_-]", "");

            if (cleanedInput.Length > maxLength)
            {
                cleanedInput = cleanedInput.Substring(0, maxLength);    // extracting the allowed number of characters from 0 index to maxlength index
            }

            return cleanedInput;
        }
    }
}
