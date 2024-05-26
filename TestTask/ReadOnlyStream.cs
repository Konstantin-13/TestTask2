using System;
using System.IO;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {
        private StreamReader _localStream;
        private bool _isEof;

        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            IsEof = true;

            // TODO : Заменить на создание реального стрима для чтения файла!
            _localStream = new StreamReader(fileFullPath);
        }
                
        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof
        {
            get { return _isEof; } // TODO : Заполнять данный флаг при достижении конца файла/стрима при чтении
            private set { _isEof = value; }
        }

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {
            // TODO : Необходимо считать очередной символ из _localStream
            int nextChar = _localStream.Read();
            if (nextChar == -1)
            {
                IsEof = true;
            }
            return (char)nextChar;
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            _localStream.BaseStream.Position = 0;
            _localStream.DiscardBufferedData();
            IsEof = false;
        }

        public void Dispose()
        {
            _localStream?.Dispose();
        }
    }
}
