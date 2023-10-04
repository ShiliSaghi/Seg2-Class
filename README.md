# SEG2-Class

## Introduction
This repository contains an implementation of the SEG2 file format in C#. SEG2 (Seismic Exchange Format, Revision 2) is a widely used data format in geophysics projects for the storage and analysis of seismic data. This implementation is designed to assist geophysicists and developers working with geophysical data in various applications such as Downhole 2016, Windownhole, Pickwin, and more.

## Features

- **SEG2 File Parsing**: This implementation provides a robust parser for reading SEG2 files, making it easy to extract valuable seismic data.

- **Data Extraction**: You can easily extract seismic traces, headers, and other essential information from SEG2 files, enabling you to perform in-depth analysis.

- **Compatibility**: Our SEG2-Class is designed to work seamlessly with popular geophysics software and tools. It ensures compatibility with various applications that rely on SEG2 format data.

## Getting Started

### Prerequisites

To use SEG2-Class in your C# project, make sure you have the following prerequisites:

- [Visual Studio](https://visualstudio.microsoft.com/) or any C# development environment.

### Installation

You can include SEG2-Class in your project by either:

1. **NuGet Package (Recommended)**: Install the SEG2-Class NuGet package by running the following command in your package manager console:

   ```shell
   Install-Package SEG2-Class

### Manual Installation: 
Download the source code from this repository and include it in your project.

### Usage
1. Import the SEG2-Class library in your C# code:
   using SEG2;
2. Initialize a SEG2 parser:
   SEG2Parser parser = new SEG2Parser();
3. Parse a SEG2 file:
   SEG2File seg2File = parser.Parse("your_file.seg2");
4. Access seismic data and headers:
   ```shell
   foreach (SEGYTrace trace in seg2File.Traces)
   {
       // Access trace data
       double[] data = trace.Data;
    
       // Access trace header values
       string traceID = trace.TraceHeader.TraceIdentifier;
       // ...
   }

## Contributions:
Please feel free to submit a pull request if you have ideas for improvements, bug fixes, or new features. 
