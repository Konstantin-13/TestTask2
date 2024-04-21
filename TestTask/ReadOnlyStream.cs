using System;
using System.IO;

namespace TestTask
{
    public class ReadOnlyStream
    {
        private readonly int CountChars;

        private string _chars;
        private int _currentCharIndex;

        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(fileFullPath))
                {
                    _chars = reader.ReadToEnd();
                    CountChars = _chars.Length;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof => _currentCharIndex >= CountChars - 1;

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {
            _currentCharIndex++;
            
            if (IsEof)
            {
                throw new IndexOutOfRangeException();
            }
            
            return _chars[_currentCharIndex];
        }
        
        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            _currentCharIndex = -1;
        }
    }
}
