# SEG2-Class

## Introduction
This repository contains an implementation of the SEG2 file format in C#. SEG2 (Seismic Exchange Format, Revision 2) is a widely used data format in geophysics projects for the storage and analysis of seismic data. This class is designed to store geophysical signals in the SEG2 file format, which is commonly used for analyzing geophysical data in various applications such as Downhole 2016, Windownhole, Pickwin, and more.

## Features
   This class stores four input signals in SEG2 format and you can add or remove signals according to your needs.
   ### Required parameters for creating a SEG2 file
   - **sample_Interval**: The time interval between samples in the trace.
   - **pre_Trigger**: It determines how long data is recorded before the trigger event, typically in milliseconds or seconds.
   - **record_Length**: The total time of capturing data, typically in milliseconds or seconds
   - **chi_Hardware_Gain**: The amount of gain type applied to each channel of signals(in our case from channel1 to channel4), i is the index of channels(variables from 1 to 4)
       

  ### Methods
   - **PrepareSeg2Format**: This implementation provides a robust parser for reading SEG2 files, making it easy to extract valuable seismic data.
    
   - **SaveSeg2File**: You can easily extract seismic traces, headers, and other essential information from SEG2 files, enabling you to perform in-depth analysis.

   - **ConvertToSeg2Line**: Our SEG2-Class is designed to work seamlessly with popular geophysics software and tools. It ensures compatibility with various applications that rely on SEG2 format data.
   - - **writeBlock**:

## Getting Started
Download the source code from this repository and include it in your project.

### Usage
1. Import the SEG2-Class in your C# code.
2. Initialize a SEG2 object:
   Class_Seg2 seg2 = new Class_Seg2();
3. Assign variables to your seg2's parameters:
   seg2.Pre_Trigger = your_Pretrigger;
   seg2.Sample_Interval = your_SampleInterval;
   seg2.Record_Length = your_RecordLength;
   seg2.CH1_Hardware_Gain = your_Channel1_Hardware_Gain
   (and other channels as well)
   .
   .
   .
      
5. Call the main function:
   Call the `PrepareSeg2Format` function with its arguments as shown in the following example:
   seg2.PrepareSeg2Format(saveFileDialog1.FileName, average_CH1, average_CH2, average_CH3, average_CH4);
   
## Contributions
Please feel free to submit a pull request if you have ideas for improvements, bug fixes, or new features. 
