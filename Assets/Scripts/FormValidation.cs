using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormValidation : MonoBehaviour
{
    public static string ValidEmail(string email)
    {
        if(email.Contains("@") && email.Contains(".") && email.Length > 8 && !containsSpace(email))
        {
            return null;
        }
        return "Not Valid Email Address";
    }


    public static string ValidName(string name)
    {
        if (name.Length > 2 && !containsDigit(name) && !containsSpecial(name))
        {
            return null;
        }
        return "Not Valid Name";
    }


    public static string  ValidPassword(string password)
    {
        if (password.Length > 4 && !containsSpace(password))
        {
            return null;
        }
        return "Not Valid Password";
    }




    public static string ValidGamerID(string gamerID)
    {
        if(containsSpace(gamerID) || gamerID.Length<4)
        {
            return "Not Valid GamerID";
        }
        return null;
    }

    static bool containsSpace(string str)
    {
        for(int i=0;i<str.Length;i++)
        {
            if(str[i] == ' ')
            {
                return true;
            }
        }
        return false;
    }

 


    static bool containsSpecial(string str)
    {
        for(int i=0;i<str.Length;i++)
        {
            if(isSpecial(str[i]))
            {
                return true;
            }
        }
        return false;
    }

    static bool containsDigit(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            if (isDigit(str[i]))
            {
                return true;
            }
        }
        return false;
    }

    static bool isDigit(char ch)
    {
        if(ch >= '0' && ch <= '9')
        {
            return true;
        }
        return false;
    }

    static bool isChar(char ch)
    {
        if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z'))
        {
            return true;
        }
        return false;
    }

    static bool isSpecial(char ch)
    {
        switch (ch)
        {
            case '$':
            case '_':
            case '.':
            case '#':
            case '@': return true;
            default: return false;
        }
    }

}
