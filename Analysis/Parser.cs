using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Analysis.Errors;

namespace Translator.Analysis
{
    /// <summary>
    /// Синтаксический анализатор (парсер)
    /// </summary>
    public static class Parser
    {
        /// <summary>
        /// Стек состояний
        /// </summary>
        static Stack<int> StateStack = new Stack<int>();

        /// <summary>
        /// Стек разбора
        /// </summary>
        static Stack<StandardSymbol> ParseStack = new Stack<StandardSymbol>();

        /// <summary>
        /// Таблица стандартных символов
        /// </summary>
        static List<StandardSymbol> TSS = new List<StandardSymbol>();

        /// <summary>
        /// Индекс анализируемого символа
        /// </summary>
        static int Index;

        /// <summary>
        /// Выполняет операцию "Сдвиг"
        /// </summary>
        static void Shift()
        {
            ParseStack.Push(TSS[Index]);
            Index++;
        }

        /// <summary>
        /// Выполняет операцию "Переход"
        /// </summary>
        /// <param name="state"></param>
        static void Pass(int state)
        { 
            StateStack.Push(state); 
        }

        /// <summary>
        /// Выполняет операцию "Приведение"
        /// </summary>
        /// <param name="count">Количество удаляемых символов</param>
        /// <param name="symbol">Символ, помещаемый на вершину стека разбора</param>
        static void Reduce(int count, StandardSymbol symbol)
        {
            for (int i = 0; i < count; i++)
            {
                StateStack.Pop();
                ParseStack.Pop();
            }
            ParseStack.Push(symbol);
        }

        /// <summary>
        /// Выполняет синтаксический разбор программы
        /// </summary>
        public static void Parse()
        {
            StateStack.Clear();
            ParseStack.Clear();
            TSS = LexicalAnalyzer.TSS;
            Index = 0;
            StateStack.Push(0);
            while (true)
            {
                switch (StateStack.Peek())
                {
                    case 0:
                        if (ParseStack.Count == 0) Shift();
                        else if (ParseStack.Peek().Lexem == "prog") return;
                        else if (ParseStack.Peek().Lexem == "void") Pass(1);
                        else throw new UnexpectedLexemError("void", ParseStack.Peek().Lexem);
                        break;

                    case 1:
                        if (ParseStack.Peek().Lexem == "void") Shift();
                        else if (ParseStack.Peek().Lexem == "main") Pass(2);
                        else throw new UnexpectedLexemError("main", ParseStack.Peek().Lexem);
                        break;

                    case 2:
                        if (ParseStack.Peek().Lexem == "main") Shift();
                        else if (ParseStack.Peek().Lexem == "(") Pass(3);
                        else throw new UnexpectedLexemError("(", ParseStack.Peek().Lexem);
                        break;

                    case 3:
                        if (ParseStack.Peek().Lexem == "(") Shift();
                        else if (ParseStack.Peek().Lexem == ")") Pass(4);
                        else throw new UnexpectedLexemError(")", ParseStack.Peek().Lexem);
                        break;

                    case 4:
                        if (ParseStack.Peek().Lexem == ")") Shift();   
                        else if (ParseStack.Peek().Lexem == "{") Pass(5);
                        else throw new UnexpectedLexemError("{", ParseStack.Peek().Lexem);
                        break;

                    case 5:
                        if (ParseStack.Peek().Lexem == "{") Shift();
                        else if (ParseStack.Peek().Lexem == "opis") Pass(6);
                        else if (ParseStack.Peek().Lexem == "spis_opis") Pass(15);
                        else if (ParseStack.Peek().Lexem == "tip") Pass(7);
                        else if (ParseStack.Peek().Lexem == "int") Pass(8);
                        else if (ParseStack.Peek().Lexem == "long") Pass(9);
                        else if (ParseStack.Peek().Lexem == "bool") Pass(10);
                        else throw new WrongNonTerminalError("описание", ParseStack.Peek().Lexem);
                        break;

                    case 6:
                        if (ParseStack.Peek().Lexem == "opis") Reduce(1, new StandardSymbol("spis_opis", 0, -1));
                        else throw new WrongNonTerminalError("описание", ParseStack.Peek().Lexem);
                        break;

                    case 7:
                        if (ParseStack.Peek().Lexem == "tip") Shift();
                        else if (ParseStack.Peek().Lexem == "spis_perem") Pass(11);
                        else if (ParseStack.Peek().TableNumber == 4) Pass(12);
                        else throw new WrongNonTerminalError("объяление переменных", ParseStack.Peek().Lexem);
                        break;

                    case 8:
                        if (ParseStack.Peek().Lexem == "int") Reduce(1, new StandardSymbol("tip", 0, -1));
                        else throw new WrongNonTerminalError("тип", ParseStack.Peek().Lexem);
                        break;

                    case 9:
                        if (ParseStack.Peek().Lexem == "long") Reduce(1, new StandardSymbol("tip", 0, -1));
                        else throw new WrongNonTerminalError("тип", ParseStack.Peek().Lexem);
                        break;

                    case 10:
                        if (ParseStack.Peek().Lexem == "bool") Reduce(1, new StandardSymbol("tip", 0, -1));
                        else throw new WrongNonTerminalError("тип", ParseStack.Peek().Lexem);
                        break;

                    case 11:
                        if (ParseStack.Peek().Lexem == "spis_perem")
                        {
                            if (TSS[Index].Lexem == ",") Shift();
                            else Reduce(2, new StandardSymbol("opis", 0, -1));
                        }
                        else if (ParseStack.Peek().Lexem == ",") Pass(13);
                        else throw new WrongNonTerminalError("переменная", ParseStack.Peek().Lexem);
                        break;

                    case 12:
                        if (ParseStack.Peek().TableNumber == 4) Reduce(1, new StandardSymbol("spis_perem", 0, -1));
                        else throw new WrongNonTerminalError("идентификатор", ParseStack.Peek().Lexem);
                        break;

                    case 13:
                        if (ParseStack.Peek().Lexem == ",") Shift();
                        else if (ParseStack.Peek().TableNumber == 4) Pass(14);
                        else throw new UnexpectedLexemError(",", ParseStack.Peek().Lexem);
                        break;

                    case 14:
                        if (ParseStack.Peek().TableNumber == 4) Reduce(3, new StandardSymbol("spis_perem", 0, -1));
                        else throw new WrongNonTerminalError("идентификатор", ParseStack.Peek().Lexem);
                        break;

                    case 15:
                        if (ParseStack.Peek().Lexem == "spis_opis") Shift();
                        else if (ParseStack.Peek().Lexem == ";") Pass(16);
                        else throw new UnexpectedLexemError(";", ParseStack.Peek().Lexem);
                        break;

                    case 16:
                        if (ParseStack.Peek().Lexem == ";") Shift();
                        else if (ParseStack.Peek().Lexem == "opis") Pass(17);
                        else if (ParseStack.Peek().Lexem == "spis_opis") Pass(15);
                        else if (ParseStack.Peek().Lexem == "tip") Pass(7);
                        else if (ParseStack.Peek().Lexem == "int") Pass(8);
                        else if (ParseStack.Peek().Lexem == "long") Pass(9);
                        else if (ParseStack.Peek().Lexem == "bool") Pass(10);
                        else if (ParseStack.Peek().Lexem == "oper") Pass(18);
                        else if (ParseStack.Peek().Lexem == "spis_oper") Pass(42);
                        else if (ParseStack.Peek().Lexem == "prisv") Pass(19);
                        else if (ParseStack.Peek().Lexem == "vybor") Pass(20);
                        else if (ParseStack.Peek().TableNumber == 4) Pass(21);
                        else if (ParseStack.Peek().Lexem == "switch") Pass(33);
                        else throw new WrongNonTerminalError("описание или оператор", ParseStack.Peek().Lexem);
                        break;

                    case 17:
                        if (ParseStack.Peek().Lexem == "opis") Reduce(3, new StandardSymbol("spis_opis", 0, -1));
                        else throw new WrongNonTerminalError("описание", ParseStack.Peek().Lexem);
                        break;

                    case 18:
                        if (ParseStack.Peek().Lexem == "oper") Reduce(1, new StandardSymbol("spis_oper", 0, -1));
                        else throw new WrongNonTerminalError("оператор", ParseStack.Peek().Lexem);
                        break;

                    case 19:
                        if (ParseStack.Peek().Lexem == "prisv") Reduce(1, new StandardSymbol("oper", 0, -1));
                        else throw new WrongNonTerminalError("присваивание", ParseStack.Peek().Lexem);
                        break;

                    case 20:
                        if (ParseStack.Peek().Lexem == "vybor") Reduce(1, new StandardSymbol("oper", 0, -1));
                        else throw new WrongNonTerminalError("выбор", ParseStack.Peek().Lexem);
                        break;

                    case 21:
                        if (ParseStack.Peek().TableNumber == 4) Shift();
                        else if (ParseStack.Peek().Lexem == "=") Pass(22);
                        else throw new WrongNonTerminalError("идентификатор", ParseStack.Peek().Lexem);
                        break;

                    case 22:
                        if (ParseStack.Peek().Lexem == "=") Shift();
                        else if (ParseStack.Peek().Lexem == "arifm") Pass(32);
                        else if (ParseStack.Peek().Lexem == "operand") Pass(25);
                        else if (ParseStack.Peek().TableNumber == 4) Pass(23);
                        else if (ParseStack.Peek().TableNumber == 3) Pass(24);
                        else throw new UnexpectedLexemError("=", ParseStack.Peek().Lexem);
                        break;

                    case 23:
                        if (ParseStack.Peek().TableNumber == 4) Reduce(1, new StandardSymbol("operand", 0, -1));
                        else throw new WrongNonTerminalError("значение", ParseStack.Peek().Lexem);
                        break;

                    case 24:
                        if (ParseStack.Peek().TableNumber == 3) Reduce(1, new StandardSymbol("operand", 0, -1));
                        else throw new WrongNonTerminalError("значение", ParseStack.Peek().Lexem);
                        break;

                    case 25:
                        if (ParseStack.Peek().Lexem == "operand") Shift();
                        else if (ParseStack.Peek().Lexem == "operation") Pass(30);
                        else if (ParseStack.Peek().Lexem == "+") Pass(26);
                        else if (ParseStack.Peek().Lexem == "-") Pass(27);
                        else if (ParseStack.Peek().Lexem == "*") Pass(28);
                        else if (ParseStack.Peek().Lexem == "/") Pass(29);
                        else throw new WrongNonTerminalError("операция", ParseStack.Peek().Lexem);
                        break;

                    case 26:
                        if (ParseStack.Peek().Lexem == "+") Reduce(1, new StandardSymbol("operation", 0, -1));
                        else throw new UnknownArithmeticOperationError(ParseStack.Peek().Lexem);
                        break;

                    case 27:
                        if (ParseStack.Peek().Lexem == "-") Reduce(1, new StandardSymbol("operation", 0, -1));
                        else throw new UnknownArithmeticOperationError(ParseStack.Peek().Lexem);
                        break;

                    case 28:
                        if (ParseStack.Peek().Lexem == "*") Reduce(1, new StandardSymbol("operation", 0, -1));
                        else throw new UnknownArithmeticOperationError(ParseStack.Peek().Lexem);
                        break;

                    case 29:
                        if (ParseStack.Peek().Lexem == "/") Reduce(1, new StandardSymbol("operation", 0, -1));
                        else throw new UnknownArithmeticOperationError(ParseStack.Peek().Lexem);
                        break;

                    case 30:
                        if (ParseStack.Peek().Lexem == "operation") Shift();
                        else if (ParseStack.Peek().Lexem == "operand") Pass(31);
                        else if (ParseStack.Peek().TableNumber == 4) Pass(23);
                        else if (ParseStack.Peek().TableNumber == 3) Pass(24);
                        else throw new WrongNonTerminalError("значение", ParseStack.Peek().Lexem);
                        break;

                    case 31:
                        if (ParseStack.Peek().Lexem == "operand") Reduce(3, new StandardSymbol("arifm", 0, -1));
                        else throw new WrongNonTerminalError("значение", ParseStack.Peek().Lexem);
                        break;

                    case 32:
                        if (ParseStack.Peek().Lexem == "arifm") Reduce(3, new StandardSymbol("prisv", 0, -1));
                        else throw new WrongNonTerminalError("арифметическое выражение", ParseStack.Peek().Lexem);
                        break;

                    case 33:
                        if (ParseStack.Peek().Lexem == "switch") Shift();
                        else if (ParseStack.Peek().Lexem == "(") Pass(34);
                        else throw new UnexpectedLexemError("(", ParseStack.Peek().Lexem);
                        break;

                    case 34:
                        if (ParseStack.Peek().Lexem == "(") Shift();
                        else if (ParseStack.Peek().TableNumber == 4) Pass(35);
                        else throw new WrongNonTerminalError("идентификатор", ParseStack.Peek().Lexem);
                        break;

                    case 35:
                        if (ParseStack.Peek().TableNumber == 4) Shift();
                        else if (ParseStack.Peek().Lexem == ")") Pass(36);
                        else throw new UnexpectedLexemError(")", ParseStack.Peek().Lexem);
                        break;

                    case 36:
                        if (ParseStack.Peek().Lexem == ")") Shift();
                        else if (ParseStack.Peek().Lexem == "{") Pass(37);
                        else throw new UnexpectedLexemError("{", ParseStack.Peek().Lexem);
                        break;

                    case 37:
                        if (ParseStack.Peek().Lexem == "{") Shift();
                        else if (ParseStack.Peek().Lexem == "spis_var") Pass(43);
                        else if (ParseStack.Peek().Lexem == "var") Pass(38);
                        else if (ParseStack.Peek().Lexem == "case") Pass(39);
                        else throw new UnexpectedLexemError("case", ParseStack.Peek().Lexem);
                        break;

                    case 38:
                        if (ParseStack.Peek().Lexem == "var") Reduce(1, new StandardSymbol("spis_var", 0, -1)); 
                        else throw new WrongNonTerminalError("вариант", ParseStack.Peek().Lexem);
                        break;

                    case 39:
                        if (ParseStack.Peek().Lexem == "case") Shift();
                        else if (ParseStack.Peek().TableNumber == 3) Pass(40);
                        else throw new WrongNonTerminalError("литерал", ParseStack.Peek().Lexem);
                        break;

                    case 40:
                        if (ParseStack.Peek().TableNumber == 3) Shift();
                        else if (ParseStack.Peek().Lexem == ":") Pass(41);
                        else throw new UnexpectedLexemError(":", ParseStack.Peek().Lexem);
                        break;

                    case 41:
                        if (ParseStack.Peek().Lexem == ":") Shift();
                        else if (ParseStack.Peek().Lexem == "oper") Pass(18);
                        else if (ParseStack.Peek().Lexem == "spis_oper") Pass(42);
                        else if (ParseStack.Peek().Lexem == "prisv") Pass(19);
                        else if (ParseStack.Peek().Lexem == "vybor") Pass(20);
                        else if (ParseStack.Peek().TableNumber == 4) Pass(21);
                        else if (ParseStack.Peek().Lexem == "switch") Pass(33);
                        else throw new WrongNonTerminalError("оператор", ParseStack.Peek().Lexem);
                        break;

                    case 42:
                        if (ParseStack.Peek().Lexem == "spis_oper")
                        {
                            if (TSS[Index].TableNumber == 4 || TSS[Index].Lexem == "switch" || (Index == TSS.Count - 1 && TSS[Index].Lexem == "}")) Shift();
                            else Reduce(4, new StandardSymbol("var", 0, -1));
                        }
                        else if (ParseStack.Peek().Lexem == "}") Pass(47);
                        else if (ParseStack.Peek().Lexem == "oper") Pass(46);
                        else if (ParseStack.Peek().Lexem == "prisv") Pass(19);
                        else if (ParseStack.Peek().Lexem == "vybor") Pass(20);
                        else if (ParseStack.Peek().TableNumber == 4) Pass(21);
                        else if (ParseStack.Peek().Lexem == "switch") Pass(33);
                        else throw new WrongNonTerminalError("оператор", ParseStack.Peek().Lexem);
                        break;

                    case 43:
                        if (ParseStack.Peek().Lexem == "spis_var") Shift();
                        else if (ParseStack.Peek().Lexem == "}") Pass(44);
                        else if (ParseStack.Peek().Lexem == "var") Pass(45);
                        else if (ParseStack.Peek().Lexem == "case") Pass(39);
                        else throw new UnexpectedLexemError("case или }", ParseStack.Peek().Lexem);
                        break;

                    case 44:
                        if (ParseStack.Peek().Lexem == "}") Reduce(7, new StandardSymbol("vybor", 0, -1));
                        else throw new UnexpectedLexemError("}", ParseStack.Peek().Lexem);
                        break;

                    case 45:
                        if (ParseStack.Peek().Lexem == "var") Reduce(2, new StandardSymbol("spis_var", 0, -1));
                        else throw new WrongNonTerminalError("вариант", ParseStack.Peek().Lexem);
                        break;

                    case 46:
                        if (ParseStack.Peek().Lexem == "oper") Reduce(2, new StandardSymbol("spis_oper", 0, -1));
                        else throw new WrongNonTerminalError("оператор", ParseStack.Peek().Lexem);
                        break;

                    case 47:
                        if (ParseStack.Peek().Lexem == "}") Reduce(9, new StandardSymbol("prog", 0, -1));
                        else throw new UnexpectedLexemError("}", ParseStack.Peek().Lexem);
                        break;
                }
            }
        }
     }
}
