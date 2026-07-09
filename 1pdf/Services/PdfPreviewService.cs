using System.Drawing;
using PdfiumPdfDocument = PdfiumViewer.PdfDocument;

namespace _1pdf.Services
{
    /// <summary>
    /// Result of a page render request, including page-count metadata.
    /// </summary>
    public class PreviewResult
    {
        public Image Image { get; }
        public int TotalPages { get; }
        public bool IsValidPage { get; }

        public PreviewResult(Image image, int totalPages, bool isValidPage)
        {
            Image = image;
            TotalPages = totalPages;
            IsValidPage = isValidPage;
        }
    }

    /// <summary>
    /// Handles PDF page rendering for preview using PdfiumViewer.
    /// No UI dependencies beyond returning a System.Drawing.Image.
    /// </summary>
    public class PdfPreviewService
    {
        /// <summary>
        /// Renders a single page of a PDF to an image.
        /// </summary>
        /// <param name="filePath">Path to the PDF file.</param>
        /// <param name="pageNumber">1-based page number.</param>
        /// <param name="width">Target render width in pixels.</param>
        /// <param name="height">Target render height in pixels.</param>
        /// <param name="password">Password for protected files, or null.</param>
        public PreviewResult RenderPage(string filePath, int pageNumber, int width, int height, string password = null)
        {
            using (var document = PdfiumPdfDocument.Load(filePath, password))
            {
                int totalPages = document.PageCount;

                if (pageNumber > totalPages || pageNumber < 1)
                {
                    return new PreviewResult(null, totalPages, isValidPage: false);
                }

                var image = document.Render(pageNumber - 1, width, height, true);
                return new PreviewResult(image, totalPages, isValidPage: true);
            }
        }
    }
}
