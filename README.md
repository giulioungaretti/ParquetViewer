# Parquet Viewer

A WinUI 3 desktop app for opening and viewing Parquet files.

## Current stack

- .NET 10 LTS, with a stable SDK selected by `global.json`.
- Windows App SDK 2.5.1, including Microsoft.WindowsAppSDK.WinUI 2.3.9 (WinUI 3).
- CommunityToolkit.Mvvm 8.4.2, using observable partial properties and asynchronous relay commands.
- CommunityToolkit.WinUI.UI.Controls.DataGrid 7.1.2. This is the latest stable Toolkit DataGrid; it has no Toolkit 8 replacement and is intentionally retained.
- Windows SDK BuildTools 10.0.28000.2705.
- Parquet.Net 4.23.4, with Snappier pinned to the patched 1.3.1 release.

Windows 10 version 1809 or later is supported. Mica is used when available, with an acrylic fallback on supported systems.

## Build and run

Install Visual Studio 2026 with WinUI development/MSIX tooling and the .NET 10 SDK. Open `LOBapp.sln`, choose x64, x86, or ARM64, and launch the packaged profile. The matching Windows App SDK runtime must be installed for this framework-dependent app.

For a command-line x64 build:

```powershell
dotnet build .\LOBapp\LOBapp.csproj -p:Platform=x64
```

For an unpackaged build, add `-p:WindowsPackageType=None` and launch the resulting executable with the Windows App SDK 2.5.1 runtime installed.

## Implementation review

File loading now uses the Windows App SDK picker and a task-returning command. Parquet decoding runs off the UI thread; busy state and command availability are restored on success, failure, or picker cancellation. Closing a file updates bindings, and dismissing the InfoBar updates its view-model state. Expected file/picker errors are shown without swallowing unrelated programming errors.

The window uses the WinUI TitleBar control for dragging and caption layout. The grid is read-only, uses schema-based indexer bindings, and does not advertise unsupported sorting. Placeholder search/settings controls have been removed.

The app still materializes the entire Parquet table in memory; grid virtualization does not make file loading bounded. Very large files need row-group paging or an explicit preview policy. Nested/binary values retain Parquet.Net's existing rendering behavior (including byte-array-to-text conversion), rather than a dedicated type-aware renderer. The main view model still coordinates the window directly; a navigation/picker abstraction would improve isolated unit testing. There is no automated test suite in this repository.


![image](https://user-images.githubusercontent.com/2694199/173021898-e1e06572-58f4-4153-bc66-578fad87cb8b.png)
