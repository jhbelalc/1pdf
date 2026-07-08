using System.Collections.Generic;
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
        public void Merge(IReadOnlyList<string> inputFiles, string outputFile)
        {
            using (var writer = new PdfWriter(outputFile))
            using (var document = new iTextPdfDocument(writer))
            {
                var merger = new PdfMerger(document);

                foreach (var inputFile in inputFiles)
                {
                    using (var reader = new PdfReader(inputFile))
                    using (var sourceDocument = new iTextPdfDocument(reader))
                    {
                        merger.Merge(sourceDocument, 1, sourceDocument.GetNumberOfPages());
                    }
                }
            }
        }
    }
}
