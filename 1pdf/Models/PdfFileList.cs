using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _1pdf.Models
{
    /// <summary>
    /// Maintains the ordered list of PDF files selected for merging.
    /// Encapsulates all list mutations (add, remove, reorder) so the UI
    /// only reflects state instead of owning it.
    /// </summary>
    public class PdfFileList
    {
        private readonly List<string> _files = new List<string>();

        /// <summary>
        /// Read-only view of the current files in merge order.
        /// </summary>
        public IReadOnlyList<string> Files => _files;

        public int Count => _files.Count;

        /// <summary>
        /// Adds files to the list. Only .pdf files are accepted and
        /// duplicates are skipped. Returns the files actually added.
        /// </summary>
        public IReadOnlyList<string> AddRange(IEnumerable<string> paths)
        {
            var added = new List<string>();

            foreach (var path in paths)
            {
                bool isPdf = Path.GetExtension(path)
                    .Equals(".pdf", StringComparison.OrdinalIgnoreCase);

                if (isPdf && !_files.Contains(path))
                {
                    _files.Add(path);
                    added.Add(path);
                }
            }

            return added;
        }

        /// <summary>
        /// Returns true when every path has a .pdf extension.
        /// </summary>
        public static bool AreAllPdf(IEnumerable<string> paths)
        {
            return paths.All(p => Path.GetExtension(p)
                .Equals(".pdf", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Removes a file by its path. No-op when not present.
        /// </summary>
        public void Remove(string path)
        {
            _files.Remove(path);
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < _files.Count)
            {
                _files.RemoveAt(index);
            }
        }

        /// <summary>
        /// Moves the item at the given index one position up.
        /// Returns the new index, or the original index if no move happened.
        /// </summary>
        public int MoveUp(int index)
        {
            if (index <= 0 || index >= _files.Count) return index;
            (_files[index - 1], _files[index]) = (_files[index], _files[index - 1]);
            return index - 1;
        }

        /// <summary>
        /// Moves the item at the given index one position down.
        /// Returns the new index, or the original index if no move happened.
        /// </summary>
        public int MoveDown(int index)
        {
            if (index < 0 || index >= _files.Count - 1) return index;
            (_files[index + 1], _files[index]) = (_files[index], _files[index + 1]);
            return index + 1;
        }

        /// <summary>
        /// Moves the item at <paramref name="fromIndex"/> to
        /// <paramref name="toIndex"/> (clamped to the list bounds).
        /// Returns the index where the item ended up.
        /// </summary>
        public int Move(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= _files.Count) return fromIndex;

            toIndex = Math.Max(0, Math.Min(_files.Count - 1, toIndex));
            if (toIndex == fromIndex) return fromIndex;

            string item = _files[fromIndex];
            _files.RemoveAt(fromIndex);
            _files.Insert(toIndex, item);
            return toIndex;
        }

        /// <summary>
        /// Builds a default output path based on the first file:
        /// same folder, prefixed with "_1PDF-".
        /// </summary>
        public string BuildDefaultOutputName()
        {
            if (_files.Count == 0) return string.Empty;

            string dir = Path.GetDirectoryName(_files[0]) ?? string.Empty;
            string name = Path.GetFileName(_files[0]);
            return Path.Combine(dir, "_1PDF-" + name);
        }
    }
}
