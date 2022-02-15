using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringUtility
{
    public static string ConvertToString(this float number)
    {
        int digit = GetNumberOfDigit((int)number);
        switch (digit)
        {
            case 1:
            case 2:
                return number.ToString("0.##");
            case 3:
                return number.ToString("0.#");
            default:
                return number.ToString("0");
        }
    }

    //TODO: use Mathf.Floor(log(number) + 1) to calcualte digit count (Performance check required)
    //TODO: OR use multipication instead of division as it is faster (ex multiply by 0.1f instead of dividing by 10) but in this case since its integer, test is required.  
    private static int GetNumberOfDigit(int number)
    {
        
        int digit = 0;

        while (number >= 1)
        {
            // sayi = sayi / 10; 'a göre daha iyi bir yöntem
            number /= 10;
            digit++;
        }

        return digit;
    }
}
