using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Analysis.Errors;

namespace Translator.Analysis
{
    /// <summary>
    /// Тип токена
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// void
        /// </summary>
        VOID,
        /// <summary>
        /// int
        /// </summary>
        INT,
        /// <summary>
        /// long
        /// </summary>
        LONG,
        /// <summary>
        /// bool
        /// </summary>
        BOOL,
        /// <summary>
        /// main
        /// </summary>
        MAIN,
        /// <summary>
        /// switch
        /// </summary>
        SWITCH,
        /// <summary>
        /// case
        /// </summary>
        CASE,
        /// <summary>
        /// (
        /// </summary>
        LEFTBRACKET,
        /// <summary>
        /// )
        /// </summary>
        RIGHTBRACKET,
        /// <summary>
        /// {
        /// </summary>
        LEFTFIGURE,
        /// <summary>
        /// }
        /// </summary>
        RIGHTFIGURE,
        /// <summary>
        /// =
        /// </summary>
        EQUAL,
        /// <summary>
        /// +
        /// </summary>
        PLUS,
        /// <summary>
        /// -
        /// </summary>
        MINUS,
        /// <summary>
        /// *
        /// </summary>
        MUL,
        /// <summary>
        /// /
        /// </summary>
        DIV,
        /// <summary>
        /// ,
        /// </summary>
        COMMA,
        /// <summary>
        /// :
        /// </summary>
        COLON,
        /// <summary>
        /// ;
        /// </summary>
        SEMICOLON,
        /// <summary>
        /// Литерал
        /// </summary>
        LITERAL,
        /// <summary>
        /// Идентификатор
        /// </summary>
        IDENTIFIER,
        /// <summary>
        /// Разделитель
        /// </summary>
        DELIMITER
    }

    /// <summary>
    /// Токен
    /// </summary>
    public struct Token
    {
        /// <summary>
        /// Тип токена
        /// </summary>
        public TokenType Type;

        /// <summary>
        /// Значение токена
        /// </summary>
        public string Value;

        /// <summary>
        /// Создает токен с заданным типом и значением
        /// </summary>
        /// <param name="type">Тип токена</param>
        /// <param name="value">Значение токена</param>
        public Token(TokenType type, string value)
        {
            Type = type; 
            Value = value;
        }
    }

    /// <summary>
    /// Стандартный символ
    /// </summary>
    public struct StandardSymbol
    {
        /// <summary>
        /// Лексема
        /// </summary>
        public string Lexem;

        /// <summary>
        /// Номер таблицы символов
        /// 1 - таблица терминалов
        /// 2 - таблица разделителей
        /// 3 - таблица литералов
        /// 4 - таблица идентификаторов
        /// </summary>
        public int TableNumber;

        /// <summary>
        /// Индекс в таблице символов
        /// </summary>
        public int Index;

        /// <summary>
        /// Создает стандартный символ, принадлежащий заданной таблице символов
        /// </summary>
        /// <param name="lexem">Лексема</param>
        /// <param name="tableNumber">Номер таблицы символов (1 - таблица терминалов, 2 - таблица разделителей, 3 - таблица литералов, 4 - таблица идентификаторов)</param>
        /// <param name="index">Индекс в таблице символов</param>
        public StandardSymbol(string lexem, int tableNumber, int index)
        {
            Lexem = lexem;
            TableNumber = tableNumber;
            Index = index;
        }
    }

    /// <summary>
    /// Лексический анализатор
    /// </summary>
    public static class LexicalAnalyzer
    {
        /// <summary>
        /// Исходный код программы
        /// </summary>
        static string Code = "";

        /// <summary>
        /// Буфер для считывания лексемы
        /// </summary>
        static string Buffer = "";

        /// <summary>
        /// Индекс текущго символа
        /// </summary>
        static int Index;

        /// <summary>
        /// Список ограничителей
        /// </summary>
        static TokenType[] CommonDelimtters = new TokenType[]
        {
            TokenType.LEFTBRACKET, TokenType.RIGHTBRACKET, TokenType.LEFTFIGURE, TokenType.RIGHTFIGURE, TokenType.EQUAL,
            TokenType.PLUS, TokenType.MINUS, TokenType.MUL, TokenType.DIV, TokenType.COMMA, TokenType.SEMICOLON
        };

        /// <summary>
        /// Список специальных слов языка
        /// </summary>
        public static Dictionary<string, TokenType> SpecialWords = new Dictionary<string, TokenType>()
        {
            {"void", TokenType.VOID },
            {"int", TokenType.INT},
            {"long", TokenType.LONG},
            {"bool", TokenType.BOOL},
            {"main", TokenType.MAIN},
            {"switch", TokenType.SWITCH},
            {"case", TokenType.CASE }
        };

        /// <summary>
        /// Список символов-разделителей
        /// </summary>
        static Dictionary<char, TokenType> SpecialSymbols = new Dictionary<char, TokenType>()
        {
            {',', TokenType.COMMA},
            {':', TokenType.COLON},
            {';', TokenType.SEMICOLON},
            {'(', TokenType.LEFTBRACKET},
            {')', TokenType.RIGHTBRACKET},
            {'=', TokenType.EQUAL },
            {'+', TokenType.PLUS},
            {'-', TokenType.MINUS},
            {'*', TokenType.MUL},
            {'/', TokenType.DIV},
            {'{', TokenType.LEFTFIGURE },
            {'}', TokenType.RIGHTFIGURE },
        };

        /// <summary>
        /// Сформированный список лексем
        /// </summary>
        public static List<Token> Tokens = new List<Token>();

        /// <summary>
        /// Список литералов
        /// </summary>
        public static List<string> Literals = new List<string>();

        /// <summary>
        /// Список идентификаторов
        /// </summary>
        public static List<string> Identifiers = new List<string>();

        /// <summary>
        /// Список разделителей
        /// </summary>
        public static List<string> Delimiters = new List<string>();

        /// <summary>
        /// Таблица стандартных символов
        /// </summary>
        public static List<StandardSymbol> TSS = new List<StandardSymbol>();

        /// <summary>
        /// Проверяет является ли лексема разделителем
        /// </summary>
        /// <param name="token">Лексема для проверки</param>
        /// <returns>Возвращает true если лексема является разделителем и false в противном случае</returns>
        static bool IsDelimiter(Token token)
        {
            return CommonDelimtters.Contains(token.Type);
        }

        /// <summary>
        /// Проверяет является ли лексема ключевым словом
        /// </summary>
        /// <param name="word">Лексема для проверки</param>
        /// <returns>Возвращает true если лексема является ключевым словом и false в противном случае</returns>
        static bool IsSpecialWord(string word)
        {
            if (string.IsNullOrEmpty(word)) 
                return false;
            return SpecialWords.ContainsKey(word);
        }

        /// <summary>
        /// Проверяет является ли лексема символом-разделителем
        /// </summary>
        /// <param name="ch">Лексема для проверки</param>
        /// <returns>Возвращает true если лексема является символом-разделителем и false в противном случае</returns>
        static bool IsSpecialSymbol(char ch)
        { 
            return SpecialSymbols.ContainsKey(ch); 
        }

        /// <summary>
        /// Очищает входной буфер
        /// </summary>
        static void Clear()
        {
            Buffer = string.Empty;
        }

        /// <summary>
        /// Добавляет текущий символ к буферу
        /// </summary>
        static void Add()
        {
            Buffer += Code[Index];
        }

        /// <summary>
        /// Выполняет переход к следующему символу входного потока
        /// </summary>
        static void Next()
        {
            Index++;
        }

        /// <summary>
        /// Добавляет текущее содержимое буфера в выходной поток как лексему с заданым типом
        /// </summary>
        /// <param name="type">Тип текущей лексемы</param>
        static void Out(TokenType type)
        {
            Token token = new Token(type, Buffer);
            Tokens.Add(token);
        }

        /// <summary>
        /// Заполняет таблицу разделителей
        /// </summary>
        static void FillDelimitersTable()
        {
            Delimiters.Clear();
            for (int i = 0; i < SpecialSymbols.Keys.Count; i++)
                Delimiters.Add(SpecialSymbols.Keys.ToList()[i].ToString());
        }

        /// <summary>
        /// Выполняет лексический анализ кода программы
        /// </summary>
        /// <param name="text">Код программы</param>
        /// <returns>Возвращает таблицу стандартных символов</returns>
        /// <exception cref="TooLongIdentifierError"></exception>
        /// <exception cref="UnknownSymbolError"></exception>
        public static List<StandardSymbol> Analyze(string text)
        {
            Code = text;
            Index = 0;
            int len = Code.Length;
            Tokens.Clear();
            Literals.Clear();
            Identifiers.Clear();
            TSS.Clear();
            FillDelimitersTable();
            while (Index != len)
            {
                if (char.IsDigit(Code[Index])) // цифра - собираем литерал
                {
                    bool isDigit = true;
                    while (isDigit)
                    {
                        Add();
                        Next();
                        if (!char.IsDigit(Code[Index]))
                            isDigit = false;
                    }
                    Out(TokenType.LITERAL);
                    Clear();
                }
                else if (char.IsLetter(Code[Index])) // буква - собираем идентификатор 
                {
                    bool isLetter = true;
                    while (isLetter)
                    {
                        Add();
                        Next();
                        if (!char.IsLetterOrDigit(Code[Index]))
                            isLetter = false;
                    }
                    if (Buffer.Length > 8) // если длина идентификатора больше 8 символов - ошибка
                        throw new TooLongIdentifierError(Buffer, Index);
                    Out(TokenType.IDENTIFIER);
                    Clear();
                }
                else if (IsSpecialSymbol(Code[Index])) // разделитель
                {
                    Add();
                    Next();
                    Out(TokenType.DELIMITER);
                    Clear();
                }
                else if (Code[Index] == ' ' || Code[Index] == '\n') // пробельные символы
                    Next();
                else
                    throw new UnknownSymbolError(Code[Index].ToString(), Index); // недопустимый символ
            }
            for (int i = 0; i < Tokens.Count; i++)
            {
                switch (Tokens[i].Type)
                {
                    case TokenType.IDENTIFIER:
                        if (SpecialWords.ContainsKey(Tokens[i].Value))
                        {
                            int idx = SpecialWords.Keys.ToList().IndexOf(Tokens[i].Value) + 1;
                            TSS.Add(new StandardSymbol(Tokens[i].Value, 1, idx));
                        }
                        else
                        {
                            int idx = Identifiers.IndexOf(Tokens[i].Value) + 1;
                            if (idx == 0)
                            {
                                Identifiers.Add(Tokens[i].Value);
                                idx = Identifiers.Count;
                            }
                            TSS.Add(new StandardSymbol(Tokens[i].Value, 4, idx));
                        }
                        break;
                    case TokenType.DELIMITER:
                        {
                            int idx = Delimiters.IndexOf(Tokens[i].Value) + 1;
                            TSS.Add(new StandardSymbol(Tokens[i].Value, 2, idx));
                        }
                        break;
                    case TokenType.LITERAL:
                        {
                            int idx = Literals.IndexOf(Tokens[i].Value) + 1;
                            if (idx == 0)
                            {
                                Literals.Add(Tokens[i].Value);
                                idx = Literals.Count;
                            }
                            TSS.Add(new StandardSymbol(Tokens[i].Value, 3, idx));
                        }
                        break;
                }
            }
            return TSS;
        }
    }
}
 