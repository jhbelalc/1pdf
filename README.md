# 1PDF - Merge Multiple PDFs into One

Convert multiple PDF files into a single PDF with a simple and intuitive Windows desktop application.

![Main Screen 1PDF](https://github.com/jhbelalc/1pdf/blob/master/1pdf/Resources/1%20PDF%20Main%20Screen%20with%20Preview.png)

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
- **.NET Framework**: .NET 10.0 or later (Windows)
- **RAM**: Minimum 2 GB (4 GB recommended)

## 🚀 Installation

### Option 1: Download Release (Recommended)
1. Go to [Releases](https://github.com/jhbelalc/1pdf/releases)
2. Download the latest `1PDF-v*.exe` file
3. Run the installer
4. Follow the installation wizard

### Option 2: Build from Source
1. Clone the repository:
   ```bash
   git clone https://github.com/jhbelalc/1pdf.git
   cd 1pdf
Ensure you have Visual Studio 2022 or later with .NET 10.0 SDK installed
Open 1pdf.sln in Visual Studio
Build the solution (Ctrl+Shift+B)
Run the application from Visual Studio (F5)
📖 How to Use
Add PDF Files:

Click the "Add Files" button or drag & drop PDFs directly into the list
Multiple files can be selected at once
Arrange Order:

Select a file in the list
Click "Arriba" (Up) to move it higher
Click "Abajo" (Down) to move it lower
Click "Borrar" (Delete) to remove a file
Set Output Name:

Enter your desired PDF name in the "Name" field
Click "AQUÍ" to generate a default name based on the first file
Merge:

Click "Convertir 1 PDF" (Convert to 1 PDF)
Choose where to save the merged PDF
Wait for the operation to complete
🛠️ Technologies Used
Language: C# (.NET 10.0)
UI Framework: Windows Forms
PDF Library: iText7 - PDF manipulation
PDF Viewer: PdfiumViewer - PDF preview
📦 Dependencies
The project uses NuGet packages for:

itext7 - PDF creation and manipulation
PdfiumViewer - PDF preview functionality
PdfiumViewer.Native.x86_64.v8-xfa - Native PDF rendering engine
🐛 Troubleshooting
Application won't start:

Ensure .NET 10.0 is installed
On Windows 7: This app requires Windows 10 or later
PDFs not merging:

Verify all PDF files are not corrupted
Try with a single PDF first to test
Check available disk space
Performance issues:

Large PDF files may take time to process
Close other applications to free up RAM
🤝 Contributing
Contributions are welcome! Please feel free to:

Fork the repository
Create a feature branch (git checkout -b feature/my-feature)
Commit your changes (git commit -m 'Add my feature')
Push to the branch (git push origin feature/my-feature)
Open a Pull Request
📝 License
This project is currently unlicensed. See LICENSE for details, or contact the author.

👨‍💻 Author
John Harold Belalcazar Lozano - GitHub

📞 Support
Report Issues: GitHub Issues
Discussions: GitHub Discussions