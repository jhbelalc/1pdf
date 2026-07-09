using System;
using System.Collections.Generic;

namespace _1pdf.Services
{
    /// <summary>
    /// Session-only cache of passwords for protected PDFs, keyed by file path.
    /// Lives in memory for the lifetime of the process and is never persisted,
    /// so all passwords disappear when the application closes.
    /// </summary>
    public class PdfPasswordStore
    {
        private readonly Dictionary<string, string> _passwords =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Returns the stored password for the file, or null when none is known.
        /// </summary>
        public string Get(string filePath)
        {
            return _passwords.TryGetValue(filePath, out var password) ? password : null;
        }

        public void Set(string filePath, string password)
        {
            _passwords[filePath] = password;
        }
    }
}
