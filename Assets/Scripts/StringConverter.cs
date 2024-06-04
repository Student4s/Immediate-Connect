using System.Text.RegularExpressions;
using UnityEngine;

public class StringConverter
{
    public static string Convert(string inputText)
    {
        // Удаляем фразу <!DOCTYPE html>
        string result = inputText.Replace("<!DOCTYPE html>", string.Empty);

        // Заменяем &nbsp; на перенос строки
        result = result.Replace("&nbsp;", "\n");

        return result;
    }
}