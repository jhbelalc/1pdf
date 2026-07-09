using System;
using System.Collections.Generic;
using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iTextPdfDocument = iText.Kernel.Pdf.PdfDocument;

namespace _1pdf.Services
{
    /// <summary>
    /// Handles PDF merging operations using iText7.
    /// No UI dependencies — fully testable in isolation.
    /// </summary>
    public class PdfMergeService
    {
        /// <summary>
        /// Merges multiple PDF files into a single output file.
        /// </summary>
        /// <param name="inputFiles">Ordered list of source PDF paths.</param>
        /// <param name="outputFile">Destination path for the merged PDF.</param>
        /// <param name="passwordLookup">
        /// Optional lookup returning the password for a given input file,
        /// or null when the file is not protected.
        /// </param>
        public void Merge(IReadOnlyList<string> inputFiles, string outputFile,
            Func<string, string> passwordLookup = null)
        {
            using (var writer = new PdfWriter(outputFile))
            using (var document = new iTextPdfDocument(writer))
            {
                var merger = new PdfMerger(document);

                foreach (var inputFile in inputFiles)
                {
                    var readerProperties = new ReaderProperties();
                    string password = passwordLookup?.Invoke(inputFile);
                    if (!string.IsNullOrEmpty(password))
                    {
                        readerProperties.SetPassword(Encoding.UTF8.GetBytes(password));
                    }

                    using (var reader = new PdfReader(inputFile, readerProperties))
                    using (var sourceDocument = new iTextPdfDocument(reader))
                    {
                        // A user (open) password grants read access but iText
                        // refuses to copy pages without the owner password
                        // unless this is set. The user already unlocked the
                        // file, so merging their own document is legitimate.
                        reader.SetUnethicalReading(true);
                        merger.Merge(sourceDocument, 1, sourceDocument.GetNumberOfPages());
                    }
                }
            }
        }
    }
}
