using System.Text.RegularExpressions;
using UnityEngine;

public class StringConverter
{
    public static string Convert(string inputText)
    {
        // ������� ����� <!DOCTYPE html>
        string result = inputText.Replace("<!DOCTYPE html>", string.Empty);

        // �������� &nbsp; �� ������� ������
        result = result.Replace("&nbsp;", "\n");

        return result;
    }
}