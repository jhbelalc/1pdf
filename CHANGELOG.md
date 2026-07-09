# Changelog

All notable changes to 1PDF will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [2.0.0] - 2026-07-09

### Added
- Full PDF viewer window: double-click a file in the list (or the preview image) to open it in a separate non-modal window with page navigation
- Zoom controls in the viewer (+/− buttons, Ctrl + mouse wheel, Ctrl +/−) with sharp re-rendering at each zoom level, like a browser
- Support for password-protected PDFs: the app asks for the password when a protected file is added and uses it for preview, viewer, and merge
- Passwords are kept only in memory for the current session — never saved to disk, and all are discarded when the program closes
- Drag & drop reordering of files inside the list (in addition to the Move Up / Move Down buttons)
- Multi-select in the file list (Ctrl/Shift/Ctrl+A) so several files can be removed at once via the "Remove selected" button
- About window with the 1PDF neon logo, version, author info, and links, styled to match the logo colors
- Application icon (`favicon.ico`) applied to the executable and all windows
- Page indicator "1 of N" / "1 de N" that follows the selected file and current preview page

### Changed
- Window title, preview prompt, and all new UI texts now switch correctly between English and Spanish
- "Remove" button renamed to "Remove selected" / "Eliminar seleccionados"
- Buttons with icons no longer overlap their captions (icon is laid out before the text)
- The browse ("...") button keeps its standard caption instead of a clipped localized text
- Project version is now tracked in `1pdf.csproj` and displayed in the About window

### Fixed
- Page label showed the literal text "LabelPages" due to a resource key mismatch
- Preview prompt stayed in Spanish after switching the language to English
- Selecting a different PDF kept the previous page number in the page selector
- Viewer window could not be opened due to a `BinaryFormatter` crash in the PdfiumViewer UI control on modern .NET (pages are now rendered directly via Pdfium)

## [1.0.4] - 2026-07-08

### Changed
- Refactored `Form1.cs` to separate concerns into dedicated `Services` and `Models` layers — the form now only handles UI wiring and state reflection
- Extracted localization logic into `Services/LocalizationService.cs`
- Extracted PDF merge logic into `Services/PdfMergeService.cs`
- Extracted PDF preview logic into `Services/PdfPreviewService.cs`
- Extracted file list state management into `Models/PdfFileList.cs`

## [1.0.3] - 2026-06-26

### Fixed
- PDF preview now works correctly in the standalone single-file executable
- Resolved `DllNotFoundException` caused by `pdfium.dll` not being available at runtime in single-file publish mode
- `pdfium.dll` is now embedded as a resource inside the executable and extracted to a temp folder on first run — no external files required

### Changed
- `Program.cs` updated to extract and load `pdfium.dll` from embedded resources before the app starts
- `1pdf.csproj` updated with a `BeforeBuild` target that automatically copies `pdfium.dll` from the NuGet package cache and embeds it as a resource

## [1.0.2] - 2026-06-26

### Fixed
- PDF merge was failing with `iText.Kernel.Exceptions.PdfException: Unknown PdfException`
- Resolved `NotSupportedException` caused by missing BouncyCastle cryptography adapter required by iText7
- Added `itext7.bouncy-castle-adapter` NuGet package which was missing from the project dependencies

## [1.0.1] - 2026-06-25

### Added
- MIT License (LICENSE file)
- CHANGELOG.md to track version history
- Build and License badges in README.md

### Changed
- GitHub Actions workflow now publishes a self-contained standalone `1pdf.exe` (no .NET installation required by the user)
- Workflow no longer generates a ZIP file — release now ships a single clean `.exe`
- Added `permissions: contents: write` to workflow to fix release creation error
- README.md updated to reference LICENSE and CHANGELOG

### Fixed
- GitHub Actions `create-release` job was failing with "Resource not accessible by integration" due to missing permissions
- Corrected `dotnet publish` output path and flags for single-file self-contained build

## [1.0.0] - 2026-06-25

### Added
- Drag & drop support for adding PDF files directly from Windows Explorer
- Multi-file selection for adding multiple PDFs at once
- File list with reorder controls (Move Up / Move Down)
- Delete individual files from the merge list
- PDF preview panel before merging
- One-click merge into a single PDF output
- Custom output file naming
- Auto-generated name based on the first file in the list
- Bilingual interface supporting Spanish and English
- GitHub Actions CI/CD pipeline for automated builds and releases

### Technologies
- C# with .NET 10.0 (Windows)
- Windows Forms UI framework
- iText7 for PDF creation and manipulation
- PdfiumViewer for PDF preview rendering

[Unreleased]: https://github.com/jhbelalc/1pdf/compare/v2.0.0...HEAD
[2.0.0]: https://github.com/jhbelalc/1pdf/compare/v1.0.4...v2.0.0
[1.0.4]: https://github.com/jhbelalc/1pdf/compare/v1.0.3...v1.0.4
[1.0.3]: https://github.com/jhbelalc/1pdf/compare/v1.0.2...v1.0.3
[1.0.2]: https://github.com/jhbelalc/1pdf/compare/v1.0.1...v1.0.2
[1.0.1]: https://github.com/jhbelalc/1pdf/compare/v1.0.0...v1.0.1
[1.0.0]: https://github.com/jhbelalc/1pdf/releases/tag/v1.0.0
