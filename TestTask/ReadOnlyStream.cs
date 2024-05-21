using System;
using System.IO;

namespace TestTask
{
    internal class ReadOnlyStream : IReadOnlyStream
    {
        private readonly Stream _localStream;

        private bool _isDisposed;

        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            if (string.IsNullOrEmpty(fileFullPath))
            {
                throw new ArgumentNullException(nameof(fileFullPath));
            }
            
            _localStream = File.OpenRead(fileFullPath);
            _isDisposed = false;
        }

        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof
        {
            get
            {
                ThrowIfDisposed();
                return _localStream.Position >= _localStream.Length;
            }
        }

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {
            ThrowIfDisposed();
            int charCode = _localStream.ReadByte();
            return charCode == -1 ? '0' : (char)charCode;
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            _localStream.Position = 0;
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            
            _localStream.Close();
            _localStream.Dispose();
            _isDisposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(ReadOnlyStream));
            }
        }
    }
}
