using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compil
{
    public class SintaxAnalyzer
    {
        public LexicalAnalyzer lAn;
        public SintaxAnalyzer(LexicalAnalyzer lexAnalyzer)
        {
            lAn= lexAnalyzer;
        }
        public List<string> errors = new List<string>();
        public string lex;
        int i = 0;
        string type;
        void GL()
        {
            lex=lAn.keys[i];
            i=i+1;
        }
        string cs = "B";
        void SintAnalyzer()
        {
            GL();
            while(cs!="ER" || cs!="V")
            {
                switch(cs)
                {
                    case "B":
                        {
                            if(lex=="{")
                            {
                                GL();
                                cs = "I"; //
                            }
                            else
                            {
                                errors.Add("Program start error");
                                cs = "ER";
                            }
                            break;
                        }
                    case "B2":
                        {
                            if (lex == "var")
                            {
                                cs = "I";
                            }
                            else if (lex == "for")
                            {
                                cs = "F";
                            }
                            else if (lex == "while")
                            {
                                cs = "WH";
                            }
                            else if (lex == "read")
                            {
                                cs = "R";
                            }
                            else if (lex == "write")
                            {
                                cs = "WR";
                            }
                            else if (lex == "if")
                            {
                                cs = "IF";
                            }
                            else
                            {
                                bool d = false;
                                foreach (string f in lAn.variebles)
                                {
                                    if (lex == f)
                                    {
                                        d = true;
                                        break;
                                    }

                                }
                                if (d == true)
                                {
                                    GL();
                                    cs = "T";
                                    break;
                                }
                                else
                                {
                                    errors.Add("Unknown lexem");
                                    cs = "ER";
                                }
                            }
                            break;
                        }
                    case "I":
                        {
                            if(lex=="var")
                            {
                                GL();
                                if(lex=="int" || lex=="double" || lex=="bool")
                                {
                                    type = lex;
                                    GL();
                                    cs= "T"; // проверяем имена переменных через запятую
                                }
                                else
                                {
                                    errors.Add("Excpected type of var");
                                }
                            }
                            else
                            {
                                cs = "O"; // состоние "оператор"
                            }
                            break;
                        }
                    case "T":
                        {
                            bool d = false; ;
                            foreach (string s in lAn.variebles) 
                            {
                                if(lex==s)
                                {
                                    d = true;
                                }
                                /*if(d==true)
                                {
                                    errors.Add("The variable name was expected, the keyword was found");
                                    break;
                                }*/
                            }
                            if (d == false)
                            {
                                errors.Add("The variable name was expected, the keyword was found");
                            }
                            else
                            {
                                GL();
                                if(lex==",")
                                {
                                    GL();
                                    break;
                                }
                                else if(lex==";")
                                {
                                    GL();
                                    
                                }
                            }
                            break;
                        }
                }
            }
        }

    }

}
