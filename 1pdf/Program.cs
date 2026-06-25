using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace _1pdf
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ExtractPdfium();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void ExtractPdfium()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith("pdfium.dll"));

            if (resourceName == null) return;

            var tempDir = Path.Combine(Path.GetTempPath(), "1pdf");
            Directory.CreateDirectory(tempDir);
            var dllPath = Path.Combine(tempDir, "pdfium.dll");

            if (!File.Exists(dllPath))
            {
                using var stream = assembly.GetManifestResourceStream(resourceName)!;
                using var file = File.Create(dllPath);
                stream.CopyTo(file);
            }

            NativeLibrary.Load(dllPath);
        }
    }
}