using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Crystal
{
    class DFA
    {
        public string compile(string input)
        {
            
            string temp = "";
            int line = 0;
            //List<string> output = new List<string>();
            string output1 = "";
            string temp_out = "";
            string returnStr = "";
            int opflag = 0;
            int punflag = 0;
            bool dotflag = false;
            bool linechange = false;
            bool lineendflag = false;
            bool mul_com_end = false;
            for (int i = 0; i < input.Length; i++)
            {
                if(input[i] == '#' && input[i+1] == '#')
                {
                    i = i + 2;
                    while (input[i] != '\r' && input[i + 1] != '\n')
                    {
                        i++;
                        if (i + 1 >= input.Length)
                        {
                            lineendflag = true;
                            break;
                        }
                    }
                    if(lineendflag)
                    {
                        lineendflag = false;
                        break;
                    }
                }
                //multilie comment
                if (input[i] == '|' && input[i + 1] == '#')
                {
                    i = i + 2;
                    while (input[i] != '#' && input[i + 1] != '|')
                    {
                        i++;
                        if (i + 1 >= input.Length)
                        {
                            mul_com_end = true;
                            break;
                        }
                        if (input[i] == '\r' && input[i + 1] == '\n')
                        {
                            line++;
                        }
                    }
                    if (mul_com_end)
                    {
                        mul_com_end = false;
                        break;
                    }
                    i = i + 2;
                }
           
                while (input[i] == ' ')
                    i++;

                while (i < input.Length && input[i] != ' ')
                {
                    if (input[i] == '"')
                    {
                        temp += input[i++];
                       
                        while (input[i] != '"' && input[i] != '\r' && input[i+1] != '\t')
                        {
                            if (input[i] == '\\' && input[i + 1] == '\"')
                            {
                                temp += input[i];
                                temp += input[++i];
                            }
                            else
                            {
                                temp += input[i];
                            }
                            i++;
                            if(i+1>=input.Length)
                            {
                                break;
                            }
                        }
                        if(input[i] == '"')
                            temp += input[i];
                        break;
                    }
                    if (input[i] == '\'')
                    {
                        int charlength = 0;
                        if (input[i + 1] == '\\')
                        {
                            charlength = 4;
                        }
                        else
                        {
                            charlength = 3;
                        }
                        for (int j = 0; j < charlength; j++, i++)
                        {
                            temp += input[i];
                        }
                        i--;
                        break;
                    }
                    if (checkOp(input[i].ToString()) != "")
                    {

                        if (checkOp(input[i].ToString()) != "" && opflag == 0 && checkPunc(input[i - 1].ToString()) == "")
                        {
                            opflag++;
                            i--;
                            break;
                        }
                        else if (checkOp(input[i].ToString()) != "")
                            opflag++;
                    }
                    else if (opflag == 2)
                    {
                        opflag = 0;
                        i--;
                        break;
                    }

                    if (punflag == 0)
                    {
                        if (input[i] != '.')
                            if (checkPunc(input[i].ToString()) != "")
                            {
                                punflag++;
                                i--;
                                break;
                            }
                    }
                    else if (punflag == 1)
                    {
                        punflag++;
                    }

                    if (input[i] == '.')
                    {
                        if (temp != "")
                        {
                            if (checkNum(temp) != "")
                            {
                                dotflag = true;
                                temp += input[i];
                                i++;
                            }
                            else
                            {
                                i--;
                                break;
                            }
                        }
                        if (i + 1 < input.Length && !dotflag)
                        {
                            if (checkNum(input[i + 1].ToString()) == "")
                            {
                                temp += input[i];
                                break;
                            }
                        }
                        else if (dotflag)
                        {
                            if (checkNum(input[i].ToString()) == "")
                            {
                                i--;
                                break;
                            }
                        }
                    }
                    if (input[i] == '\r' && input[i + 1] == '\n')
                    {
                        linechange = true;
                        i++;
                        break;
                    }
                    temp += input[i];

                    if (punflag > 1)
                    {
                        punflag = 0;
                        break;
                    }

                    if (opflag > 2)
                    {
                        opflag = 0;
                        break;
                    }

                    i++;
                }
                if ((returnStr = checkKW(temp)) != "")
                {
                    if (returnStr == temp)
                        temp_out = "(" + returnStr + "," + "," + line + ")";
                    else
                        temp_out = "(" + returnStr + "," + temp + "," + line + ")";

                }
                else if ((returnStr = checkId(temp)) != "")
                {
                    if (returnStr == temp)
                        temp_out = "(" + returnStr + "," + "," + line + ")";
                    else
                        temp_out = "(" + returnStr + "," + temp + "," + line + ")";

                }
                else if ((returnStr = checkOp(temp)) != "")
                {

                    if (returnStr == temp)
                        temp_out = "(" + returnStr + "," + "," + line + ")";
                    else
                        temp_out = "(" + returnStr + "," + temp + "," + line + ")";

                }
                else if ((returnStr = checkPunc(temp)) != "")
                {
                    if (returnStr == temp)
                        temp_out = "(" + returnStr + "," + "," + line + ")";
                    else
                        temp_out = "(" + returnStr + "," + temp + "," + line + ")";

                }
                else if ((returnStr = checkNum(temp)) != "")
                {
                    if (returnStr == temp)
                        temp_out = "(" + returnStr + "," + "," + line + ")";
                    else
                        temp_out = "(" + returnStr + "," + temp + "," + line + ")";

                }
                else if ((returnStr = checkPoint(temp)) != "" )
                {
                    if (returnStr == temp)
                        temp_out = "(" + returnStr + "," + "," + line + ")";
                    else
                        temp_out = "(" + returnStr + "," + temp + "," + line + ")";

                }
                else if ((returnStr = checkSent(temp)) != "")
                {
                    if (returnStr == temp)
                        temp_out = "(" + returnStr + "," + "," + line + ")";
                    else
                        temp_out = "(" + returnStr + "," + temp + "," + line + ")";

                }
                else if ((returnStr = checkSin(temp)) != "")
                {
                    if (returnStr == temp)
                        temp_out = "(" + returnStr + "," + "," + line + ")";
                    else
                        temp_out = "(" + returnStr + "," + temp + "," + line + ")";
                }
                else
                {
                    temp_out = "(" + "Lexical Error" + "," + temp + "," + line + ")";
                }
                if (linechange)
                {
                    line++;
                    linechange = false;
                }
                //output.Add(temp_out);
                if (temp != "")
                {
                    output1 += temp_out + "\n";
                }
                temp = "";
                dotflag = false;
            }
            return output1;
        }




        private int[,] TransitionTable;
        private int[] finalStates;
        private int initialState = 0;

        public string checkId(string ln)
        {
            TransitionTable = new int[,] {{1,3,2},
                                          {1,1,1},
                                          {1,1,2},
                                          {3,3,3}};
            finalStates = new int[] { 1 };
            initialState = 0;
            if (CheckIdentifier(ln))
            {
                return "ID";
            }
            return "";

        }
        private int Transition(int state, char ch)
        {
            for (int i = 0; i < 3; i++)
            {
                if (ch == '_')
                {
                    return TransitionTable[state, 2];
                }
                else if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z'))
                {
                    return TransitionTable[state, 0];
                }
                else if (ch >= '0' && ch <= '9')
                {
                    return TransitionTable[state, 1];
                }
            }
            return -1;
        }
        private bool CheckIdentifier(string input)
        {
            int curr_state = initialState;
            for (int i = 0; i < input.Length; i++)
            {
                if (curr_state != -1)
                    curr_state = Transition(curr_state, input[i]);
            }
            for (int i = 0; i < finalStates.Length; i++)
            {
                if (curr_state == finalStates[i])
                {
                    return true;
                }
            }
            return false;
        }
        //number
        public string checkNum(string ln)
        {
            TransitionTable = new int[,] {{1,2,3},
                                                    {1,4,4},
                                                    {1,4,4},
                                                    {1,4,4},
                                                    {4,4,4}};
            finalStates = new int[] { 1 };
            initialState = 0;
            if (checkNumber(ln))
            {
                return "Num_Constant";
            }
            return "";
        }
        private int TransitionNum(int state, char ch)
        {
            for (int i = 0; i < 3; i++)
            {
                if (ch == '-')
                {
                    return TransitionTable[state, 2];
                }
                else if (ch == '+')
                {
                    return TransitionTable[state, 1];
                }
                else if (ch >= '0' && ch <= '9')
                {
                    if (state != -1)
                        return TransitionTable[state, 0];
                }
            }
            return -1;
        }
        private bool checkNumber(string input)
        {
            int curr_state = initialState;
            for (int i = 0; i < input.Length; i++)
            {
                if (curr_state != -1)
                    curr_state = TransitionNum(curr_state, input[i]);
            }
            for (int i = 0; i < finalStates.Length; i++)
            {
                if (curr_state == finalStates[i])
                {
                    return true;
                }
            }
            return false;
        }
        //Point

        public string checkPoint(string ln)
        {
            TransitionTable = new int[,] {{1,1,3,4,2},
                                          {2,2,3,2,2},
                                          {2,2,2,2,2},
                                          {2,2,3,4,4},
                                          {2,2,5,2,2},
                                          {2,2,5,2,6},
                                          {2,2,7,2,2},
                                          {2,2,7,2,2}};
            finalStates = new int[] { 5, 7 };
            initialState = 0;
            if (checkPon(ln))
            {
                return "Point_Constant";
            }
            return "";
        }
        private int TransitionPon(int state, char ch)
        {
            for (int i = 0; i < 5; i++)
            {
                if (ch == '-')
                {
                    return TransitionTable[state, 1];
                }
                else if (ch == '+')
                {
                    return TransitionTable[state, 0];
                }
                else if (ch >= '0' && ch <= '9')
                {
                    if (state != -1)
                        return TransitionTable[state, 2];
                }
                else if (ch == '.')
                {
                    return TransitionTable[state, 3];
                }
                else if (ch == 'e')
                {
                    return TransitionTable[state, 4];
                }
            }
            return -1;
        }

        private bool checkPon(string input)
        {
            int curr_state = initialState;
            for (int i = 0; i < input.Length; i++)
            {
                if (curr_state != -1)
                    curr_state = TransitionPon(curr_state, input[i]);
            }
            for (int i = 0; i < finalStates.Length; i++)
            {
                if (curr_state == finalStates[i])
                {
                    return true;
                }
            }
            return false;
        }

        //char

        public string checkSin(string ln)
        {
            TransitionTable = new int[,] {{1,5,5,5},
                                          {5,3,2,3},
                                          {5,5,3,3},
                                          {4,5,5,5},
                                          {5,5,5,5},
                                          {5,5,5,5}};
            finalStates = new int[] { 4 };
            initialState = 0;
            if (isChar(ln))
            {
                return "single_constant";
            }
            return "";
        }
        private bool isChar(string word)
        {
         
            int currentState = 0;
            int finalState = 4;

            for (int i = 0; i < word.Length && currentState != 6; i++)
            {
                switch (currentState)
                {
                    case 0:
                        if (word[i] == '\'')
                        {
                            currentState = 1;
                        }
                        else
                        {
                            currentState = 5;
                        }
                        break;

                    case 1:
                        if (word[i] == '\\')
                        {
                            currentState = 2;
                        }
                        else
                        {
                            currentState = 3; //characters
                        }
                        break;

                    case 2:
                        if (Regex.IsMatch(word[i].ToString(), RegularExpression.escapeCharacters) ||
                            Regex.IsMatch(word[i].ToString(), RegularExpression.sc))
                        {
                            currentState = 3;
                        }
                        else
                        {
                            currentState = 5;
                        }
                        break;

                    case 3:
                        if (word[i] == '\'')
                        {
                            currentState = 4;
                        }
                        else
                        {
                            currentState = 5;
                        }
                        break;

                    case 4:
                        if (i <= word.Length - 1)
                        {
                            currentState = 5;
                        }
                        break;

                    case 5:
                        break;
                }

                if (currentState == finalState)
                {
                    return true;
                }
            }

            return false;
        }

        private int TransitionSin(int state, char ch)
        {
            for (int i = 0; i < 3; i++)
            {
                if (ch == '\'')
                {
                    return TransitionTable[state, 0];
                }
                else if (ch == '\\')
                {
                    return TransitionTable[state, 2];
                }
                else if (ch == '\\' || ch == '\'' || ch == '\"' || ch == '\n' || ch == '\t' || ch == '\r' || ch == '\b')
                {
                    return TransitionTable[state, 3];
                }
                else if (ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z')
                {
                    return TransitionTable[state, 1];
                }
            }
            return -1;
        }

        private bool checkSingle(string input)
        {
            int curr_state = initialState;
            for (int i = 0; i < input.Length; i++)
            {
                if (curr_state != -1)
                    curr_state = TransitionSin(curr_state, input[i]);
            }
            for (int i = 0; i < finalStates.Length; i++)
            {
                if (curr_state == finalStates[i])
                {
                    return true;
                }
            }
            return false;
        }

        //Sent

        public string checkSent(string ln)
        {
            TransitionTable = new int[,] {{1,4,4,4},
                                                    {2,1,3,1},
                                                    {4,4,4,4},
                                                    {4,4,1,1},
                                                    {4,4,4,4}};
            finalStates = new int[] { 2 };
            initialState = 0;
            if (isString(ln))
            {
                return "sent_constant";
            }
            return "";
        }
        private bool isString(string word)
        {
            int currentState = 0;
            int finalState = 3;

            for (int i = 0; i < word.Length && currentState != 6; i++)
            {
                switch (currentState)
                {
                    case 0:
                        if (word[i] == '\"')
                        {
                            currentState = 1;
                        }
                        else
                        {
                            currentState = 4;
                        }
                        break;
                    case 1:
                        if (word[i] == '\\')
                        {
                            currentState = 2;
                        }
                        else if (word[i] >= 'A' && word[i] <= 'Z' || word[i] >= 'a' && word[i] <= 'z')
                        {
                            currentState = 1;
                        }
                        else if(word[i] == '\"')
                        {
                            currentState = 3;
                        }
                        break;
                    case 2:
                        if (Regex.IsMatch(word[i].ToString(), RegularExpression.escapeCharacters) ||
                            Regex.IsMatch(word[i].ToString(), RegularExpression.sc))
                        {
                            currentState = 1;
                        }
                        else
                        {
                            currentState = 4;
                        }
                        break;
                    case 3:
                        if (i <= word.Length - 1)
                        {
                            currentState = 4;
                        }
                        break;
                    case 4:
                        //valid = false;
                        break;
                }
            }

            if (currentState == finalState)
            {
                return true;
            }

            return false;
        }
        //private int TransitionS(int state, char ch)
        //{
        //    bool flag = false;
        //    for (int i = 0; i < 3; i++)
        //    {
        //        if (!flag)
        //        {
        //            if (ch == '\"')
        //            {
        //                return TransitionTable[state, 0];
        //                flag = true;
        //            }
        //        }
        //        else if (ch == '\\')
        //        {
        //            return TransitionTable[state, 2];
        //        }
        //        else if (ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z')
        //        {
        //            return TransitionTable[state, 1];
        //        }
        //        else if (ch == '\\' || ch == '\'' || ch == '\"' || ch == 'n' || ch == 't' || ch == 'r' || ch == 'b')
        //        {
        //            return TransitionTable[state, 3];
        //        }
        //    }
        //    return -1;
        //}

        //private bool checkS(string input)
        //{
        //    int curr_state = initialState;
        //    for (int i = 0; i < input.Length; i++)
        //    {
        //        if (curr_state != -1)
        //            curr_state = TransitionS(curr_state, input[i]);
        //    }
        //    for (int i = 0; i < finalStates.Length; i++)
        //    {
        //        if (curr_state == finalStates[i])
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //Operators
        public string checkOp(string input)
        {
            string[][] Op = new string[9][];
            Op[0] = new string[] { "!", "!" };//not
            Op[1] = new string[] { "INC_DEC", "++", "--" };//INC_DEC
            Op[2] = new string[] { "D_M", "/", "%" };//D_M
            Op[3] = new string[] { "*", "*" };//M
            Op[4] = new string[]{"P_M","+",
                                 "-"};//P_M
            Op[5] = new string[] { "Rel_Op", "<", ">", "<=", ">=", "!=", "==" };//Rel_Op
            Op[6] = new string[] { "&&", "&&" };//&&
            Op[7] = new string[] { "||", "||" };//||
            Op[8] = new string[] { "AssignOp", "=", "+=", "-=", "*=", "/=", "%=" };//Assign_Op

            for (int i = 0; i < Op.GetLength(0); i++)
            {
                for (int j = 1; j < Op[i].Length; j++)
                {
                    if (input == Op[i][j])
                    {
                        return Op[i][0];
                    }
                }
            }
            return "";
        }
        //punctuators
        public string checkPunc(string input)
        {
            string[][] Op = new string[8][];
            Op[0] = new string[] { ".", "." };
            Op[1] = new string[] { ",", "," };
            Op[2] = new string[] { ";", ";" };
            Op[3] = new string[] { "{", "{" };
            Op[4] = new string[] { "}", "}" };
            Op[5] = new string[] { "[", "]" };
            Op[6] = new string[] { "(", "(" };
            Op[7] = new string[] { ")", ")" };

            for (int i = 0; i < Op.GetLength(0); i++)
            {
                for (int j = 1; j < Op[i].Length; j++)
                {
                    if (input == Op[i][j])
                    {
                        return Op[i][0];
                    }
                }
            }
            return "";
        }
        //Keywords
        public string checkKW(string input)
        {
            string[][] Op = new string[10][];
            Op[0] = new string[] { "DT", "number", "point", "sent", "single", "bool" };
            Op[1] = new string[] { "##", "##" };
            Op[2] = new string[] { "|#", "|#" };
            Op[3] = new string[] { "#|", "#|" };
            Op[4] = new string[] { "barbar", "barbar" };
            Op[5] = new string[] { "while", "while" };
            Op[6] = new string[] { "agar", "agar" };
            Op[7] = new string[] { "yaphir", "yaphir" };
            Op[8] = new string[] { "warna", "warna" };
            Op[9] = new string[] { "class", "class" };

            for (int i = 0; i < Op.GetLength(0); i++)
            {
                for (int j = 1; j < Op[i].Length; j++)
                {
                    if (input == Op[i][j])
                    {
                        return Op[i][0];
                    }
                }
            }
            return "";
        }
    }
    public static class RegularExpression
    {
        public static string escapeCharacters = @"[nrtfbv]";
        public static string sc = "[\\\\\"\']";
    }
}
