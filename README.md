# 1PDF - Merge Multiple PDFs into One

Convert multiple PDF files into a single PDF with a simple and intuitive Windows desktop application.

![Build](https://github.com/jhbelalc/1pdf/actions/workflows/dotnet.yml/badge.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

![Main Screen 1PDF](1pdf/Resources/1%20PDF%20Main%20Screen%20with%20Preview.png)

## ✨ Features

- **Drag & Drop Support**: Simply drag and drop PDF files from your explorer
- **Easy File Management**: Add multiple files with a user-friendly interface
- **Reorder Pages**: Move files up or down to arrange the final PDF order
- **Preview**: View PDF files before merging
- **Batch Operations**: Merge multiple PDFs in one click
- **Custom Naming**: Name your merged PDF or use an auto-generated name based on the first file
- **Bilingual Support**: Spanish and English interfaces

## 📋 Requirements

- **Operating System**: Windows 10 or later
- **RAM**: Minimum 2 GB (4 GB recommended)
- No additional dependencies — the runtime is bundled inside the executable

## 🚀 Installation

### Option 1: Download Release (Recommended)

1. Go to [Releases](https://github.com/jhbelalc/1pdf/releases)
2. Download the latest `1pdf.exe`
3. Double-click to run — no installation needed

### Option 2: Build from Source

1. Clone the repository:
   ```bash
   git clone https://github.com/jhbelalc/1pdf.git
   cd 1pdf
   ```
2. Ensure you have Visual Studio 2022 or later with .NET 10.0 SDK installed
3. Open `1pdf.sln` in Visual Studio
4. Build the solution (`Ctrl+Shift+B`)
5. Run the application from Visual Studio (`F5`)

## 📖 How to Use

1. **Add PDF Files**: Click the "Add Files" button or drag & drop PDFs directly into the list. Multiple files can be selected at once.
2. **Arrange Order**: Select a file and use "Arriba" (Up) / "Abajo" (Down) to reorder, or "Borrar" (Delete) to remove it.
3. **Set Output Name**: Enter a name in the "Name" field, or click "AQUÍ" to auto-generate one from the first file.
4. **Merge**: Click "Convertir 1 PDF", choose where to save, and wait for the operation to complete.

## 🛠️ Technologies Used

- **Language**: C# (.NET 10.0)
- **UI Framework**: Windows Forms
- **PDF Library**: [iText7](https://github.com/itext/itext7-dotnet) — PDF manipulation
- **PDF Viewer**: [PdfiumViewer](https://github.com/pvginkel/PdfiumViewer) — PDF preview

## 📦 Dependencies

- `itext7` — PDF creation and manipulation
- `PdfiumViewer` — PDF preview functionality
- `PdfiumViewer.Native.x86_64.v8-xfa` — Native PDF rendering engine

## 🐛 Troubleshooting

**Application won't start:**
- Ensure you are on Windows 10 or later
- Try right-clicking the `.exe` and selecting "Run as administrator"

**PDFs not merging:**
- Verify all PDF files are not corrupted
- Try with a single PDF first to test
- Check available disk space

**Performance issues:**
- Large PDF files may take time to process
- Close other applications to free up RAM

## 🤝 Contributing

Contributions are welcome!

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/my-feature`
3. Commit your changes: `git commit -m 'Add my feature'`
4. Push to the branch: `git push origin feature/my-feature`
5. Open a Pull Request

## 📝 Changelog

See [CHANGELOG.md](CHANGELOG.md) for the full history of changes.

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**John Harold Belalcazar Lozano** — [GitHub](https://github.com/jhbelalc)

## 📞 Support

- **Report Issues**: [GitHub Issues](https://github.com/jhbelalc/1pdf/issues)
- **Discussions**: [GitHub Discussions](https://github.com/jhbelalc/1pdf/discussions)