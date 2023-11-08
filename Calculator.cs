using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compil
{
   
    public class Calculator
    {
        List<string> znaki = new List<string>() {"+", "-", "=", "/", "*" };
        List<string> type = new List<string>() {"int", "float" };

        public string Calculating(string str)
        {
            List<string> words = new List<string>();
            int i = 0;
            
            string a="";
            for(i=0; i<str.Length; i=i+1)
            {
                
                if (str[i]!=' ')
                {
                    a += str[i];
                }
                else
                {

                    words.Add(a);
                    a = "";
                }

            }
            for(int j=0; j<words.Count(); j=j+1) 
            {
                if(j==0 && !type.Contains(words[0]))
                {
                    return "Wrong syntaxis: Error 1";
                }
                if(j==1 && (type.Contains(words[1]) || znaki.Contains(words[1])))
                {
                    return "Error 2";
                }
            }

            return "Good!";
        }
    }
}
