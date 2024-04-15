using System;
using System.IO;
using System.Text;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {
        private StreamReader _reader;

        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            // TODO : Заменить на создание реального стрима для чтения файла!
            IsEof = false;

            try
            {
                _reader = new StreamReader(fileFullPath, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while opening file: {ex.Message}");
                IsEof = true;
            }
        }
                
        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof { get; private set; } // TODO : Заполнять данный флаг при достижении конца файла/стрима при чтении

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {
            // TODO : Необходимо считать очередной символ из _localStream
            if (_reader == null || _reader.EndOfStream)
            {
                IsEof = true;
                throw new EndOfStreamException("End of stream reached.");
            }

            var nextChar = _reader.Read();
            
            if (nextChar != -1) return (char)nextChar;
            
            IsEof = true;
            throw new EndOfStreamException("End of stream reached.");
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            if (_reader == null)
            {
                IsEof = true;
                return;
            }

            _reader.BaseStream.Position = 0;
            IsEof = false;
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }
    }
}
