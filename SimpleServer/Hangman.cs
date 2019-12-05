using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer
{
    class Hangman
    {
        string _word; // Chosen word to guess
        StringBuilder _internalObscuredWord; // Underscore hidden word equivalement of the word 
        string _externalObscuredWord; // Underscore whitespace separated obscured word
        int _countOfTries;
        string[] _hangmanStages = new string[]
        {
            "",
            @"
              |
              |
              |",
            @"
              |
              |
              |
              |
              |
              |",
            @"
               ___
              |
              |
              |
              |
              |
              |",
             @"
               ______
              |
              |
              |
              |
              |
              |",
             @"
               _________
              |
              |
              |
              |
              |
              |",
             @"
               _________
              |                  |
              |                 0
              |
              |
              |
              |",
             @"
               _________
              |                  |
              |                 0
              |                /|\
              |                / \
              |
              |",
        }; // Contains stages of hangman drawing

        public Hangman()
        {
            _countOfTries = 0;
            _word = "corbin";
            _internalObscuredWord = new StringBuilder();
            _internalObscuredWord.Insert(0, "_", _word.Length);

            // Init external obscureword for chat message representation
            UpdateObscuredWord(_internalObscuredWord);
        }

        public int Update(string message)
        {
            // Remove '!' prefix from the message
            string clientMessage = message.Remove(0, 1);

            // Message is the word
            if (_word == clientMessage)
            {
                return 2; // Won
            }

            // Message contains letter sent by client
            else if (_word.Contains(clientMessage))
            {
                //StringBuilder sb = new StringBuilder(_internalObscuredWord);
                char[] charMessage = clientMessage.ToCharArray();
                int index = _word.IndexOf(clientMessage);
                _internalObscuredWord[index] = charMessage[0];

                // last correct letter sent, obscured word is revealed
                if (_internalObscuredWord.ToString() == _word)
                {
                    return 2;
                }
                // Split obscured word to have whitespaces between letters
                else
                {
                    UpdateObscuredWord(_internalObscuredWord);
                }
            }

            // Message doesn't contain letter sent by client
            else
            {
                _countOfTries++;
                // Maximum amount of tries reached
                if (_countOfTries == 7)
                {
                    return 1;
                }
            }
            return 0;
        }
        
        private void UpdateObscuredWord(StringBuilder sb)
        {
            int length = sb.Length * 2;
            StringBuilder stringBuilder = new StringBuilder(sb.ToString());
            for (int i = 1; i < length; i += 2)
            {
                stringBuilder.Insert(i, ' ');
            }
            _externalObscuredWord = stringBuilder.ToString();
        }

        public string GetObscuredWord()
        {
            return _externalObscuredWord;
        }
        public string GetHangmanASCIIPicture()
        {
            return _hangmanStages[_countOfTries];
        }
        public string GetHangmanWord()
        {
            return _word;
        }
    }
}
