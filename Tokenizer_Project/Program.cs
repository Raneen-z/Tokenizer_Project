using System;

namespace Tokenizer_Project
{
    public class Token
    {
        public string type;
        public string value;
        public int position;
        public int lineNumber;

    }

    public class Tokenizer
    {
        public string input;
        public int currentPostion;
        public int lineNumber;

        public Tokenizer(string input)
        {
            this.input = input;
            this.currentPostion = -1;
            this.lineNumber = 1;
        }

        public bool hasMore()
        {
            return (this.currentPostion + 1) < this.input.Length;
        }
        public char peek(int numOfPositions = 1)
        {
            if (this.hasMore())
            {
                return this.input[this.currentPostion + numOfPositions];
            }
            else return '\0';
        }

        public char next()
        {
            char currentChar = this.input[++this.currentPostion];
            if (currentChar == '\n')
            {
                this.lineNumber++;
            }
            return currentChar;
        }

        public Token tokenize(Tokenizable[] handlers)
        {
            foreach (var t in handlers)
            {
                if (t.tokenizable(this))
                {
                    return t.tokenize(this);
                }

            }
            return null;
        }
    }

    public abstract class Tokenizable
    {
        public abstract bool tokenizable(Tokenizer t);
        public abstract Token tokenize(Tokenizer t);
    }
    //from class
    public class NumberTokenizer : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hasMore() && Char.IsDigit(t.peek());
        }

        public override Token tokenize(Tokenizer t)
        {
            Token token = new Token();
            token.value = "";
            token.type = "Number";
            token.position = t.currentPostion;
            token.lineNumber = t.lineNumber;

            while (t.hasMore() && Char.IsDigit(t.peek()))
            {
                token.value += t.next();
            }
            return token;
        }


    }

    //form class
    public class IdTokenizer : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hasMore() && (Char.IsLetter(t.peek()) || t.peek().Equals('_'));
        }

        public override Token tokenize(Tokenizer t)
        {
            Token token = new Token();
            token.value = "";
            token.type = "Identifrer";
            token.position = t.currentPostion;
            token.lineNumber = t.lineNumber;

            while (t.hasMore() && Char.IsLetterOrDigit(t.peek()) || t.peek().Equals('_'))
            {
                token.value += t.next();
            }
            return token;

        }


    }
    // from class
    public class SpaceTokenizer : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hasMore() && (Char.IsWhiteSpace(t.peek()));
        }

        public override Token tokenize(Tokenizer t)
        {
            Token token = new Token();
            token.value = "";
            token.type = "Whitespace";
            token.position = t.currentPostion;
            token.lineNumber = t.lineNumber;

            while (t.hasMore() && Char.IsWhiteSpace(t.peek()))
            {
                token.value += t.next();
            }
            return token;

        }


    }


    //Raneen
    public class StringTokenizer : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hasMore() && t.peek().Equals('\"');
        }

        public override Token tokenize(Tokenizer t)
        {

            Token token = new Token();
            token.value = "";
            token.type = "String";
            token.position = t.currentPostion;
            token.lineNumber = t.lineNumber;
            token.value += t.next();
            while (t.hasMore())
            {
                if (t.peek().Equals('\"'))
                {
                    token.value += t.next();
                    break;
                }
                token.value += t.next();

            }

            if (token.value[token.value.Length - 1].Equals('\"'))
            {
                return token;
            }
            else
            {

                t.currentPostion = token.position + 1;
                token.value = token.value[0].ToString();
                token.type = "**Unexpected Token**";

                return token;
            }
        }


    }


    //Raneen
    public class HexColorsTokenizer : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hasMore() && t.peek().Equals('#');
        }

        public override Token tokenize(Tokenizer t)
        {
            Token token = new Token();
            token.value = "";
            token.type = "Hex color";
            token.position = t.currentPostion;
            token.lineNumber = t.lineNumber;
            int i = 0;
            token.value += t.next();
            while (t.hasMore() && Char.IsLetterOrDigit(t.peek()) && i <= 5)
            {
                token.value += t.next();
                i++;
            }

            if (token.value.Length < 7)
            {
                while (i <= 5)
                {
                    i++;
                    token.value += '0';
                }
            }
            return token;
        }


    }
    //Maryam 
    public class CommentTokenizer : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hasMore() && t.peek().Equals('/') && t.peek(2).Equals('/');
        }
        public override Token tokenize(Tokenizer t)
        {
            Token token = new Token();
            token.value = "";
            token.type = "Comment";
            token.position = t.currentPostion;
            token.lineNumber = t.lineNumber;
            if (t.peek() == '/')
            {
                while (t.hasMore())
                {
                    if (t.peek().Equals('\n'))
                    {
                        token.value += t.next();
                        break;
                    }
                    token.value += t.next();
                }
                return token;
            }
            else
            {
                t.currentPostion = token.position + 1;
                token.value = token.value[0].ToString();
                token.type = "**Unexpected Token**";
                return token;
            }

        }
    }
    //Reema
    public class BracketsTokenizer : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hasMore() && t.peek().Equals('[');
        }
        public override Token tokenize(Tokenizer t)
        {
            Token token = new Token();
            token.value = "";
            token.type = "Square Brackets";
            token.position = t.currentPostion;
            token.lineNumber = t.lineNumber;
            token.value += t.next();
            while (t.hasMore())
            {
                if (t.peek().Equals(']'))
                {
                    token.value += t.next();
                    break;
                }
                token.value += t.next();
            }
            if (token.value[token.value.Length - 1].Equals(']'))
            {
                return token;
            }
            else
            {
                t.currentPostion = token.position + 1;
                token.value = token.value[0].ToString();
                token.type = "**Unexpected Token**";
                return token;
            }
        }
    }

    // Reema 
    public class PhoneTokenizer : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hasMore() && t.peek().Equals('+') && t.peek(2).Equals('9') && t.peek(3).Equals('6') && t.peek(4).Equals('6') && t.peek(5).Equals('5');
        }
        public override Token tokenize(Tokenizer t)
        {
            Token token = new Token();
            token.value = "";
            token.type = "Phone number";
            token.position = t.currentPostion;
            token.lineNumber = t.lineNumber;
            token.value += t.next();
            int i = 0;
            while (t.hasMore() && !t.peek().Equals(' ') && i <= 13)
            {
                token.value += t.next();
                i++;
            }
            if (token.value.Length > 13 || token.value.Length < 13)
            {
                Console.WriteLine("Phone number length should be 13 digits");
                return null;
            }
            return token;
        }
    }


    //Fatimah

    public class ATTokenizer : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hasMore() && (t.peek().Equals('@'));
        }
        public override Token tokenize(Tokenizer t)
        {
            Token token = new Token();
            token.value = "";
            token.type = "Account";
            token.position = t.currentPostion;
            token.lineNumber = t.lineNumber;
            while (t.hasMore() && !t.peek().Equals('\n') && (Char.IsLetterOrDigit(t.peek()) || t.peek() == '@'))
            {
                token.value += t.next();
            }
            return token;
        }
    }
    //fatma
    public class commintwitline : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            {
                if (t.hasMore() && t.peek() == '/')
                {
                    t.next();
                    if (t.hasMore() && (t.peek() == '/' || t.peek() == '*'))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return t.hasMore() && t.peek() == '/';
            }
        }
        public override Token tokenize(Tokenizer t)
        {

            Token token = new Token();
            token.type = "comment";
            token.value = "/";
            token.position = t.currentPostion;
            token.lineNumber = t.lineNumber;
            while (t.hasMore())
            {
                if (t.peek() == '\n')
                    break;
                if (t.peek() == '/' && token.value.EndsWith('*'))
                {
                    token.value += t.next();
                    break;
                }
                token.value += t.next();
            }
            return token;
        }
    }
    //Fatimah 

    public class SymboleTokenizer : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hasMore() && (Char.IsSymbol(t.peek()));
        }
        public override Token tokenize(Tokenizer t)
        {
            Token token = new Token();
            token.value = "";
            token.type = "Symbol";
            token.position = t.currentPostion;
            token.lineNumber = t.lineNumber;
            while (t.hasMore() && Char.IsSymbol(t.peek()))
            {
                token.value += t.next();
            }
            return token;
        }
    }
    //Batool
    public class HTMLTokenizer : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hasMore() && (t.peek() == '<');
        }
        public override Token tokenize(Tokenizer t)
        {
            Token token = new Token();
            token.value = "";
            token.type = "HTMLTag";
            token.position = t.currentPostion;
            token.lineNumber = t.lineNumber;
            while (t.hasMore() && (Char.IsLetterOrDigit(t.peek()) || "</>".Contains(t.peek())))
            {
                if (t.peek() == '>')
                {
                    token.value += t.next();
                    break;
                }
                else
                {
                    token.value += t.next();
                }
            }
            return token;
        }
    }
    //Batool
    public class PunctuationTokenizer : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hasMore() && (Char.IsPunctuation(t.peek()));
        }
        public override Token tokenize(Tokenizer t)
        {
            Token token = new Token();
            token.value = "";
            token.type = "Punctuation";
            token.position = t.currentPostion;
            token.lineNumber = t.lineNumber;
            while (t.hasMore() && Char.IsPunctuation(t.peek()))
            {
                token.value += t.next();
            }
            return token;
        }
        class Program
        {
            static void Main(string[] args)
            {
                Tokenizer test = new Tokenizer(" <>h</> 11_ . r22 _dr 33  #434334567876543\n  #ffg\"fff //df    9\n 8");
                Tokenizable[] handlers = new Tokenizable[]
                {
                    new SpaceTokenizer(), new CommentTokenizer(),
                    new NumberTokenizer(),new IdTokenizer(),
                    new HexColorsTokenizer(),new HTMLTokenizer(),
                    new StringTokenizer(),
                    new ATTokenizer(), new BracketsTokenizer (),
                    new PhoneTokenizer(),new PunctuationTokenizer(),
                    new SymboleTokenizer()
                };

                Token token = test.tokenize(handlers);

                while (token != null)
                {
                    Console.WriteLine("\nToken: {0} \nType: {1} | Position: {2} | Line: {3}\n________________", token.value, token.type, token.position, token.lineNumber);
                    token = test.tokenize(handlers);

                }


            }
        }
    }
}
