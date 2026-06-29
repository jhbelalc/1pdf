# Changelog

All notable changes to 1PDF will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

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

[Unreleased]: https://github.com/jhbelalc/1pdf/compare/v1.0.3...HEAD
[1.0.3]: https://github.com/jhbelalc/1pdf/compare/v1.0.2...v1.0.3
[1.0.2]: https://github.com/jhbelalc/1pdf/compare/v1.0.1...v1.0.2
[1.0.1]: https://github.com/jhbelalc/1pdf/compare/v1.0.0...v1.0.1
[1.0.0]: https://github.com/jhbelalc/1pdf/releases/tag/v1.0.0
