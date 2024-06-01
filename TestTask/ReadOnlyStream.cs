using System;
using System.IO;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {
        private Stream localStream_ { get; }
        private StreamReader reader_ { get; }

        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof
        {
            get;
            private set;
        }

        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fullFilePath)
        {
            try
            {
                localStream_ = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read);
                reader_ = new StreamReader(localStream_);
                IsEof = false;
            }
            catch (IOException)
            {
                IsEof = true;
            }
        }

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar() // Метод модифицирован
        {
            if (IsEof)
            {
                throw new EndOfStreamException();
            }

            if (reader_.Peek() == -1)
            {
                IsEof = true;
            }

            return Convert.ToChar(reader_.Read());
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            localStream_.Position = 0;

            IsEof = false;
            if (localStream_ is null)
            {
                IsEof = true;
            }
        }

        /// <summary>
        /// Освобождает ресурсы для работы с файлом.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                localStream_.Dispose();
                reader_.Dispose();
            }
        }
    }
}
