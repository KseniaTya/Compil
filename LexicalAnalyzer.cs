using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compil
{
    public class LexicalAnalyzer
    {
        public List<string> keywords = new List<string>() { "{", "}", "bool", "double", "else", "false", "if", "int", "read", "true", "var", "while", "write"};
        public List<string> separators = new List<string>() {"-", "!=", "&&", "(", ")", "*", "*/", ",", ".", "/", "/*", ":", ";", "||", "+", "<", "<=", "=", "==", ">", ">=" };
        public List<string> variebles = new List<string>();
        public List<string> words = new List<string>();
        public List<string> numbers = new List<string>();
        public List<string> keys = new List<string>();
        public List<string> errors = new List<string>();
        string buffer="";
        char ch; //считываемый символ
        string cs = ""; //состояние
        int number = 0;
        public string richTextBox;

        void Null()
        {
            buffer = "";
        }
        void Add()
        {
            buffer += ch;
            number++;
        }
        int SearchInKeywords()
        {
            return keywords.BinarySearch(buffer);
        }
        int SearchInSeparators()
        {
            return separators.BinarySearch(buffer);
        }
        void Symbol(/*string richTextBox*/)
        {
            ch = richTextBox[number];
        }
        string ConvertToInt()
        {
            try
            {
                return Convert.ToInt32(buffer).ToString();
            }
            catch
            {
                return "Can't convert to int!";
            }
        }
        string ConvertToDouble()
        {
            try
            {
                return Convert.ToDouble(buffer).ToString();
            }
            catch
            {
                return "Can't convert to double!";
            }
        }
        bool CheckForLatin()
        {
            if((ch>='А' && ch>='Я') || (ch>='а' && ch>='я'))
            {
                number=number+1;
                return true;
            }
            else
            {
                return false;
            }
        }
        bool Letter() //проверяем буква ли
        {
            char b = char.Parse(ch.ToString());
            if (Char.IsLetter(b))
            {
                if (Regex.IsMatch(ch.ToString(), "[a-zA-Z]"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        bool Digit() //проверяем цифра ли
        {
            int n;
            bool isNumeric=int.TryParse(ch.ToString(), out n);
            return isNumeric;
        }
        public void LexAnalyzer()
        {
            number= 0;
            cs = "H";
            if (richTextBox=="")
            {
                cs = "ER";
                errors.Add("No code to comlier");
            }
            while(cs != "ER" && cs != "V")
            {
                Symbol();
                switch(cs)
                {
                    case "H":
                        {
                            if(number==richTextBox.Length-1)
                            {
                                cs = "V"; //состояние выхода
                                break;
                            }
                            else if((ch==' ' || ch=='\n' || ch=='\t' || ch=='\0' || ch=='\r') && number<richTextBox.Length-1)
                            {
                                number++;
                                Symbol();
                                break;
                            }
                            else if(CheckForLatin())
                            {
                                cs = "ER";
                                errors.Add("Found russian letters");
                                break;
                            }
                            else if(Letter())
                            {
                                Null();
                                Add();
                                Symbol();
                                cs = "I"; //обработка слова
                                break;
                            }
                            else if(Digit())
                            {
                                Null();
                                Add();
                                Symbol();
                                cs = "N"; //обработка числа
                            }
                            else if(ch == '/') 
                            {
                                Null();
                                Add();
                                cs = "C1";  //переходим в обработу символа / - либо деление, либо комментарий
                                
                            }
                            else if(ch=='.')
                            {
                                Add();
                                Symbol();
                                cs = "P1"; //обработка чисел с плавающей точкой
                            }
                            else if(ch=='=')
                            {
                                Null();
                                Add();
                                cs = "R"; //проверка равенство или сравнение
                            }
                            else if(ch=='>')
                            {
                                Null();
                                Add();
                                cs = "B"; //больше или больше и равно
                            }
                            else if(ch=='<')
                            {
                                Null();
                                Add();
                                cs = "M"; //меньше или меньше и равно
                            }
                            else
                            {
                                Null();
                                Add();
                                Symbol();
                                cs = "OG"; //
                            }
                            break;
                        }
                    case "V":
                        {

                            break;
                        }
                    case "I": //считывает символы до пробела
                        {
                            while((Letter()||Digit()) && number!=richTextBox.Length-1)
                            {
                                Add();
                                Symbol();
                            }
                            keys.Add(buffer);
                            variebles.Add(buffer);
                            //numbers.Add(buffer);
                            words.Add(buffer);
                            
                            cs = "H";
                            break;
                        }
                    case "N":
                        {
                            while ((Letter() || Digit()) && number != richTextBox.Length-1)
                            {
                                if(ch=='E' || ch=='e')
                                {
                                    Add();
                                    Symbol();
                                    cs = "E22";
                                    break;
                                }
                                Add();
                                Symbol();
                            }
                            if(cs=="E22")
                            {
                                break;
                            }
                            if(ch=='.')
                            {
                                Add();
                                Symbol();
                                cs = "P1";
                                break;
                            }
                            else
                            {
                                keys.Add(buffer);
                                numbers.Add(buffer);
                                cs = "H";
                            }
                            break;
                        }
                    case "P1":
                        {
                            if(Digit())
                            {
                                cs = "P2";
                            }
                            else
                            {
                                errors.Add("The number is entered incorrectly");
                                cs = "ER";
                                //return "The number is entered incorrectly";
                            }
                            break;
                        }
                    case "P2":
                        {
                            while(Digit())
                            {
                                Add();
                                Symbol();
                            }
                            if(ch=='E' || ch=='e')
                            {
                                Add();
                                Symbol();
                                cs = "E21";
                            }
                            else if(Letter() || ch=='.')
                            {
                                errors.Add("Error in writing a floating point number: P2");
                                cs = "ER";
                            }
                            else
                            {
                                keys.Add(buffer);
                                numbers.Add(buffer);
                                cs = "H";
                            }
                            break;
                        }
                    case "E21": // 
                        {
                            if(ch=='+' || ch=='-')
                            {
                                Add();
                                Symbol();
                            }
                            else if(Digit())
                            {
                                cs = "E22";
                            }
                            else
                            {
                                errors.Add("Error in writing a floating point number: E21"); //ошибка в записи числа с плавающей точкой
                                cs = "ER";
                            }
                            break;
                        }
                    case "E22":
                        {
                            while (Digit())
                            {
                                Add();
                                Symbol();
                            }
                            /*if (CH == 'H' || CH == 'h')
                            {
                                keys.Add("(3, " + numbers.Count().ToString() + ")");
                                numbers.Add(convertation(16));
                                CS = "H";
                                return S;
                            }*/
                            /*else*/ if (Letter() || ch == '.')
                            {
                                
                                errors.Add("Unidentified letter after the number"); //буква после цифр, например: 123b
                                cs = "ER";
                            }
                            else
                            {
                                /* keys.Add("(3, " + numbers.Count().ToString() + ")");
                                 numbers.Add(convertatuon_to_decimal()); */
                                keys.Add(buffer);
                                numbers.Add(buffer);
                                cs = "H";
                                Null();
                            }
                            break;
                        }
                    case "R": // проверка == или =
                        {
                            Symbol();
                            if(ch=='=')
                            {
                                Add();
                                keys.Add(buffer);
                            }
                            else
                            {
                                keys.Add(buffer);
                            }
                            Null();
                            cs = "H";
                            break;
                        }
                    case "B": // проверка >= или >
                        {
                            Symbol();
                            if (ch == '=')
                            {
                                Add();
                                keys.Add(buffer);
                            }
                            else
                            {
                                keys.Add(buffer);
                            }
                            Null();
                            cs = "H";
                            break;
                        }
                    case "M": // проверка <= или <
                        {
                            Symbol();
                            if (ch == '=')
                            {
                                Add();
                                keys.Add(buffer);
                            }
                            else
                            {
                                keys.Add(buffer);
                            }
                            Null();
                            cs = "H";
                            break;
                        }
                    case "C1":
                        {
                            Symbol();
                            if(ch == '*')
                            {
                                Add();
                                cs = "OG";
                                keys.Add(buffer);
                                Null();
                            }
                            else
                            {
                                keys.Add(buffer);
                                Null();
                                cs= "H";
                            }
                            
                            break;
                        }
                    case "OG":
                        {
                            Symbol();
                            while(ch!='*' && number != richTextBox.Length - 1)
                            {
                                Symbol();
                                number= number+1;
                            }
                            if(ch=='*')
                            {
                                Add();
                                number = number-1;
                                Symbol();
                                if(ch=='/')
                                {
                                    Add();
                                    keys.Add(buffer);
                                    Null();
                                    cs= "H";
                                }
                                else
                                {
                                    cs = "OG";
                                    break;
                                }
                            }
                            else
                            {
                                Null();
                                errors.Add("Comment not ended!");
                                cs= "ER";
                            }
                            break;
                        }
                    case "ER":
                        {
                            cs= "H";
                            break;
                        }

                }
            }

        }
    }
    
}
