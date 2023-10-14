using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeismoVision_Ver2
{
    public class Class_Seg2
    {
        #region Variables
        byte[] seg2Bytes = new byte[300000]; //Maximum size of your seg2 File 

        double sample_Interval = 0;     //The time interval between samples in the trace
        byte pre_Trigger = 0;           //Specifies the duration, typically in milliseconds or seconds, for which data is recorded before the actual trigger event occurs.
        int record_Length = 0;          //The total time of capturing data, typically in milliseconds or seconds
        int ch1_Hardware_Gain = 0;      //the amount of gain type applied to each channel of signals(in our case from channel1 to channel4)
        int ch2_Hardware_Gain = 0;
        int ch3_Hardware_Gain = 0;
        int ch4_Hardware_Gain = 0;
        int seg2Index = 0;
        double[] receiverLocation = new double[4];
        #endregion Variables


        #region Properties
        public byte Pre_Trigger
        {
            get
            {
                return pre_Trigger;
            }
            set
            {
                pre_Trigger = value;
            }
        }

        public double Sample_Interval
        {
            get
            {
                return sample_Interval;
            }
            set
            {
                sample_Interval = value;
            }
        }

        public int Record_Length
        {
            get
            {
                return record_Length;
            }
            set
            {
                record_Length = value;
            }
        }

        public int CH1_Hardware_Gain
        {
            get
            {
                return ch1_Hardware_Gain;
            }
            set
            {
                ch1_Hardware_Gain = value;
            }
        }

        public int CH2_Hardware_Gain
        {
            get
            {
                return ch2_Hardware_Gain;
            }
            set
            {
                ch2_Hardware_Gain = value;
            }
        }

        public int CH3_Hardware_Gain
        {
            get
            {
                return ch3_Hardware_Gain;
            }
            set
            {
                ch3_Hardware_Gain = value;
            }
        }

        public int CH4_Hardware_Gain
        {
            get
            {
                return ch4_Hardware_Gain;
            }
            set
            {
                ch4_Hardware_Gain = value;
            }
        }

        public double[] ReceiverLocation
        {
            get
            {
                return receiverLocation;
            }
            set
            {
                receiverLocation = value;
            }
        }

        #endregion Properties


        #region Methods  
        public void PrepareSeg2Format(string filePath, double[] ch1, double[] ch2, double[] ch3) //Each channel has 8000 data
        {
            string fileHeaderString = "";
            string traceHeaderString = "";
            byte[] tracePointer = new byte[12]; //12 bytes are considered for tracesize
            byte[] sampleBytes = new byte[4];   //4 bytes are considered for tracesize
            byte[] sizeBytes = new byte[4];     //4 bytes are considered for tracesize
            byte[] traceSize = new byte[2];     //2 bytes are considered for tracesize
            byte LowByte_L = 0, LowByte_H = 0, HighwByte_H = 0, HighwByte_L = 0, m = 0, n = 0;

            Int32 ch_Value = 0;
            int dataLength = 0;
            int sampleNo = 0;
            int dataBlockSize = 0;
            int traceBlockSize = 0;
            int trace1Pointer = 0;
            int trace2Pointer = 0;
            int trace3Pointer = 0;
            
            //Setting date and time
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            string date = month.ToString("00") + "/" + day.ToString("00") + "/" + year.ToString("0000");

            sampleNo = Convert.ToInt16(record_Length); //sampleNo is equal to record_Length
            dataBlockSize = sampleNo * 4;              //each sample represent in 4 bytes
            dataLength = sampleNo;

            trace1Pointer = 150;    //the first trace starts from index 150
            trace2Pointer = trace1Pointer + 32 + 273 + dataBlockSize; //Trace Pointer + DescriptorBlockSize of each trace + HeaderStringSize of each trace + Data Size
            trace3Pointer = trace2Pointer + 32 + 273 + dataBlockSize;
            
            traceBlockSize =  305; //DescriptorBlockSize of each trace  + HeaderStringSize of each trace: 32 + 273
            traceSize[0] = (byte)(traceBlockSize & 0x00FF);
            traceSize[1] = (byte)((traceBlockSize & 0xFF00) >> 8);

            tracePointer[0] = (byte)(trace1Pointer & 0x000000FF);
            tracePointer[1] = (byte)((trace1Pointer & 0x0000FF00) >> 8);
            tracePointer[2] = (byte)((trace1Pointer & 0x00FF0000) >> 16);
            tracePointer[3] = (byte)((trace1Pointer & 0xFF000000) >> 24);

            tracePointer[4] = (byte)(trace2Pointer & 0x000000FF);
            tracePointer[5] = (byte)((trace2Pointer & 0x0000FF00) >> 8);
            tracePointer[6] = (byte)((trace2Pointer & 0x00FF0000) >> 16);
            tracePointer[7] = (byte)((trace2Pointer & 0xFF000000) >> 24);

            tracePointer[8] = (byte)(trace3Pointer & 0x000000FF);
            tracePointer[9] = (byte)((trace3Pointer & 0x0000FF00) >> 8);
            tracePointer[10] = (byte)((trace3Pointer & 0x00FF0000) >> 16);
            tracePointer[11] = (byte)((trace3Pointer & 0xFF000000) >> 24);
            //-------------------

            sizeBytes[0] = (byte)(dataBlockSize & 0x000000FF);
            sizeBytes[1] = (byte)((dataBlockSize & 0x0000FF00) >> 8);
            sizeBytes[2] = (byte)((dataBlockSize & 0x00FF0000) >> 16);
            sizeBytes[3] = (byte)((dataBlockSize & 0xFF000000) >> 24);
            //-------------------

            sampleBytes[0] = (byte)(sampleNo & 0x000000FF);
            sampleBytes[1] = (byte)((sampleNo & 0x0000FF00) >> 8);
            sampleBytes[2] = (byte)((sampleNo & 0x00FF0000) >> 16);
            sampleBytes[3] = (byte)((sampleNo & 0xFF000000) >> 24);
            //-------------------

            //File Descriptor Block Bytes---
            seg2Index = 0;
            //Low byte first (The first two bytes (bytes 0 and 1) of this block contain the integer 3a55h; This integer identifies the file as a seismic file)
            seg2Bytes[0] = 85; //0x55
            seg2Bytes[1] = 58;  //0x3a
            //Revision No
            seg2Bytes[2] = 1;
            seg2Bytes[3] = 0;
            //Size of trace pointer sub-block 
            seg2Bytes[4] = 12;
            seg2Bytes[5] = 0;
            //Number of traces 
            seg2Bytes[6] = 3;
            seg2Bytes[7] = 0;
            //Size of string terminator
            seg2Bytes[8] = 1;
            //First & second string terminator character
            seg2Bytes[9] = 10; //BS:Back Space
            seg2Bytes[10] = 0;
            //Size of line terminator
            seg2Bytes[11] = 2;
            //First & second line character terminator
            seg2Bytes[12] = 13; //VL:Vertical Tab
            seg2Bytes[13] = 10; //BS:Back Space
            //Reserved
            seg2Bytes[14] = 0;
            seg2Bytes[15] = 0;
            seg2Bytes[16] = 0;
            seg2Bytes[17] = 0;
            seg2Bytes[18] = 0;
            seg2Bytes[19] = 0;
            seg2Bytes[20] = 0;
            seg2Bytes[21] = 0;
            seg2Bytes[22] = 0;
            seg2Bytes[23] = 0;
            seg2Bytes[24] = 0;
            seg2Bytes[25] = 0;
            seg2Bytes[26] = 0;
            seg2Bytes[27] = 0;
            seg2Bytes[28] = 0;
            seg2Bytes[29] = 0;
            seg2Bytes[30] = 0;
            seg2Bytes[31] = 0;
            //Pointer to trace1 descriptor 
            seg2Bytes[32] = tracePointer[0];
            seg2Bytes[33] = tracePointer[1];
            seg2Bytes[34] = tracePointer[2];
            seg2Bytes[35] = tracePointer[3];
            //Pointer to trace2 descriptor 
            seg2Bytes[36] = tracePointer[4];
            seg2Bytes[37] = tracePointer[5];
            seg2Bytes[38] = tracePointer[6];
            seg2Bytes[39] = tracePointer[7];
            //Pointer to trace3 descriptor 
            seg2Bytes[40] = tracePointer[8];
            seg2Bytes[41] = tracePointer[9];
            seg2Bytes[42] = tracePointer[10];
            seg2Bytes[43] = tracePointer[11];
            
            seg2Index += 44; //update index of array


            //file Header String             
            fileHeaderString = "ACQUISITION_DATE " + date + "\n";
            writeBlock(ConvertToSeg2Line(fileHeaderString));
            fileHeaderString = "INSTRUMENT MAE A6000S" + "\n";
            writeBlock(ConvertToSeg2Line(fileHeaderString));
            fileHeaderString = "JOB_ID " + "\n";
            writeBlock(ConvertToSeg2Line(fileHeaderString));
            fileHeaderString = "TRACE_SORT AS_ACQUIRED" + "\n";
            writeBlock(ConvertToSeg2Line(fileHeaderString));
            fileHeaderString = "UNITS METERS" + "\n";
            writeBlock(ConvertToSeg2Line(fileHeaderString));

            //Insert 2 zerobytes 
            seg2Bytes[seg2Index] = 0;
            seg2Index++;
            seg2Bytes[seg2Index] = 0;
            seg2Index++;

            //Trace1 Descriptor Block Bytes---
            //trace Bytes(The first two bytes contain the unsigned integer 4422h to identify this block as a Trace Descriptor Block)
            seg2Bytes[seg2Index + 0] = 34; //0x22
            seg2Bytes[seg2Index + 1] = 68; //0x44
            //Size of trace "TOTAL HEADEDER" in this block(305 in this project)
            seg2Bytes[seg2Index + 2] = traceSize[0];
            seg2Bytes[seg2Index + 3] = traceSize[1];
            //Size of trace pointer sub-block 
            seg2Bytes[seg2Index + 4] = sizeBytes[0];
            seg2Bytes[seg2Index + 5] = sizeBytes[1];
            seg2Bytes[seg2Index + 6] = sizeBytes[2];
            seg2Bytes[seg2Index + 7] = sizeBytes[3];
            //Number of sample in data block
            seg2Bytes[seg2Index + 8] = sampleBytes[0];
            seg2Bytes[seg2Index + 9] = sampleBytes[1];
            seg2Bytes[seg2Index + 10] = sampleBytes[2];
            seg2Bytes[seg2Index + 11] = sampleBytes[3];
            //Data Format 02:32 bit fixed
            seg2Bytes[seg2Index + 12] = 2;
            //Reserved
            seg2Bytes[seg2Index + 13] = 0;
            seg2Bytes[seg2Index + 14] = 0;
            seg2Bytes[seg2Index + 15] = 0;
            seg2Bytes[seg2Index + 16] = 0;
            seg2Bytes[seg2Index + 17] = 0;
            seg2Bytes[seg2Index + 18] = 0;
            seg2Bytes[seg2Index + 19] = 0;
            seg2Bytes[seg2Index + 20] = 0;
            seg2Bytes[seg2Index + 21] = 0;
            seg2Bytes[seg2Index + 22] = 0;
            seg2Bytes[seg2Index + 23] = 0;
            seg2Bytes[seg2Index + 24] = 0;
            seg2Bytes[seg2Index + 25] = 0;
            seg2Bytes[seg2Index + 26] = 0;
            seg2Bytes[seg2Index + 27] = 0;
            seg2Bytes[seg2Index + 28] = 0;
            seg2Bytes[seg2Index + 29] = 0;
            seg2Bytes[seg2Index + 30] = 0;
            seg2Bytes[seg2Index + 31] = 0;
            seg2Index += 32;

            //Trace1 Header String            
            traceHeaderString = "CHANNEL_NUMBER 00001" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "DELAY " + pre_Trigger.ToString("00000.00000") + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "FIXED_GAIN " + ch1_Hardware_Gain.ToString("00000") + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "HIGH_CUT_FILTER 00000" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "LOW_CUT_FILTER 00000" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "RECEIVER_LOCATION " +"0000.00000 0000.00000 0000.00000" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "SAMPLE_INTERVAL " + (Sample_Interval / 1000.0).ToString("0000.000000000") + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "SOURCE_LOCATION 0000.00000 0000.00000 0000.00000" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "TRACE_TYPE SEISMIC_DATA";
            writeBlock(ConvertToSeg2Line(traceHeaderString));

            seg2Bytes[seg2Index] = 0;
            seg2Index++;
            seg2Bytes[seg2Index] = 0;
            seg2Index++;

            //Trace1 Data
            for (int i = 0; i < dataLength; i++)
            {
                ch_Value = (Int32)(ch1[i] * Math.Pow(2, 31));
                HighwByte_H = (byte)((ch_Value & 0xFF000000) >> 24);
                HighwByte_L = (byte)((ch_Value & 0x00FF0000) >> 16);
                LowByte_H = (byte)((ch_Value & 0x0000FF00) >> 8);
                LowByte_L = (byte)(ch_Value & 0x000000FF);

                seg2Bytes[seg2Index + 0] = LowByte_L;
                seg2Bytes[seg2Index + 1] = LowByte_H;
                seg2Bytes[seg2Index + 2] = HighwByte_L;
                seg2Bytes[seg2Index + 3] = HighwByte_H;
                seg2Index += 4;
            }

            //Trace2 Descriptor Block Bytes---
            //trace2 Bytes 
            seg2Bytes[seg2Index + 0] = 34;
            seg2Bytes[seg2Index + 1] = 68;
            //Size of trace "TOTAL HEADEDER" in this block(305 in this project)
            seg2Bytes[seg2Index + 2] = traceSize[0];
            seg2Bytes[seg2Index + 3] = traceSize[1];
            //Size of trace pointer sub-block 
            seg2Bytes[seg2Index + 4] = sizeBytes[0];
            seg2Bytes[seg2Index + 5] = sizeBytes[1];
            seg2Bytes[seg2Index + 6] = sizeBytes[2];
            seg2Bytes[seg2Index + 7] = sizeBytes[3];
            //Number of sample in data block
            seg2Bytes[seg2Index + 8] = sampleBytes[0];
            seg2Bytes[seg2Index + 9] = sampleBytes[1];
            seg2Bytes[seg2Index + 10] = sampleBytes[2];
            seg2Bytes[seg2Index + 11] = sampleBytes[3];
            //Data Format 02:32 bit fixed
            seg2Bytes[seg2Index + 12] = 2;
            //Reserved
            seg2Bytes[seg2Index + 13] = 0;
            seg2Bytes[seg2Index + 14] = 0;
            seg2Bytes[seg2Index + 15] = 0;
            seg2Bytes[seg2Index + 16] = 0;
            seg2Bytes[seg2Index + 17] = 0;
            seg2Bytes[seg2Index + 18] = 0;
            seg2Bytes[seg2Index + 19] = 0;
            seg2Bytes[seg2Index + 20] = 0;
            seg2Bytes[seg2Index + 21] = 0;
            seg2Bytes[seg2Index + 22] = 0;
            seg2Bytes[seg2Index + 23] = 0;
            seg2Bytes[seg2Index + 24] = 0;
            seg2Bytes[seg2Index + 25] = 0;
            seg2Bytes[seg2Index + 26] = 0;
            seg2Bytes[seg2Index + 27] = 0;
            seg2Bytes[seg2Index + 28] = 0;
            seg2Bytes[seg2Index + 29] = 0;
            seg2Bytes[seg2Index + 30] = 0;
            seg2Bytes[seg2Index + 31] = 0;
            seg2Index += 32;

            //Trace2 Header String            
            traceHeaderString = "CHANNEL_NUMBER 00002" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "DELAY " + pre_Trigger.ToString("00000.00000") + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "FIXED_GAIN " + ch2_Hardware_Gain.ToString("00000") + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "HIGH_CUT_FILTER 00000" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "LOW_CUT_FILTER 00000" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "RECEIVER_LOCATION " + "0000.00000 0000.00000 0000.00000" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "SAMPLE_INTERVAL " + (sample_Interval / 1000.0).ToString("0000.000000000") + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "SOURCE_LOCATION 0000.00000 0000.00000 0000.00000" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "TRACE_TYPE SEISMIC_DATA";
            writeBlock(ConvertToSeg2Line(traceHeaderString));

            seg2Bytes[seg2Index] = 0;
            seg2Index++;
            seg2Bytes[seg2Index] = 0;
            seg2Index++;

            //Trace2 Data 
            for (int i = 0; i < dataLength; i++)
            {
                ch_Value = (Int32)(ch2[i] * Math.Pow(2, 31));
                HighwByte_H = (byte)((ch_Value & 0xFF000000) >> 24);
                HighwByte_L = (byte)((ch_Value & 0x00FF0000) >> 16);
                LowByte_H = (byte)((ch_Value & 0x0000FF00) >> 8);
                LowByte_L = (byte)(ch_Value & 0x000000FF);


                seg2Bytes[seg2Index + 0] = (byte)LowByte_L;
                seg2Bytes[seg2Index + 1] = (byte)LowByte_H;
                seg2Bytes[seg2Index + 2] = (byte)HighwByte_L;
                seg2Bytes[seg2Index + 3] = (byte)HighwByte_H;
                seg2Index += 4;
            }


            //Trace3 Descriptor Block Bytes---
            //trace3 Bytes         
            seg2Bytes[seg2Index + 0] = 34;
            seg2Bytes[seg2Index + 1] = 68;
            //Size of trace "TOTAL HEADEDER" in this block(305 in this project)
            seg2Bytes[seg2Index + 2] = traceSize[0];
            seg2Bytes[seg2Index + 3] = traceSize[1];
            //Size of trace pointer sub-block 
            seg2Bytes[seg2Index + 4] = sizeBytes[0];
            seg2Bytes[seg2Index + 5] = sizeBytes[1];
            seg2Bytes[seg2Index + 6] = sizeBytes[2];
            seg2Bytes[seg2Index + 7] = sizeBytes[3];
            //Number of sample in data block
            seg2Bytes[seg2Index + 8] = sampleBytes[0];
            seg2Bytes[seg2Index + 9] = sampleBytes[1];
            seg2Bytes[seg2Index + 10] = sampleBytes[2];
            seg2Bytes[seg2Index + 11] = sampleBytes[3];
            //Data Format 02:32 bit fixed
            seg2Bytes[seg2Index + 12] = 2;
            //Reserved
            seg2Bytes[seg2Index + 13] = 0;
            seg2Bytes[seg2Index + 14] = 0;
            seg2Bytes[seg2Index + 15] = 0;
            seg2Bytes[seg2Index + 16] = 0;
            seg2Bytes[seg2Index + 17] = 0;
            seg2Bytes[seg2Index + 18] = 0;
            seg2Bytes[seg2Index + 19] = 0;
            seg2Bytes[seg2Index + 20] = 0;
            seg2Bytes[seg2Index + 21] = 0;
            seg2Bytes[seg2Index + 22] = 0;
            seg2Bytes[seg2Index + 23] = 0;
            seg2Bytes[seg2Index + 24] = 0;
            seg2Bytes[seg2Index + 25] = 0;
            seg2Bytes[seg2Index + 26] = 0;
            seg2Bytes[seg2Index + 27] = 0;
            seg2Bytes[seg2Index + 28] = 0;
            seg2Bytes[seg2Index + 29] = 0;
            seg2Bytes[seg2Index + 30] = 0;
            seg2Bytes[seg2Index + 31] = 0;
            seg2Index += 32;

            //Trace3 Header String            
            traceHeaderString = "CHANNEL_NUMBER 00003" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "DELAY " + pre_Trigger.ToString("00000.00000") + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "FIXED_GAIN " + ch3_Hardware_Gain.ToString("00000") + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "HIGH_CUT_FILTER 00000" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "LOW_CUT_FILTER 00000" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "RECEIVER_LOCATION " + "0000.00000 0000.00000 0000.00000" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "SAMPLE_INTERVAL " + (sample_Interval / 1000.0).ToString("0000.000000000") + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "SOURCE_LOCATION 0000.00000 0000.00000 0000.00000" + "\n";
            writeBlock(ConvertToSeg2Line(traceHeaderString));
            traceHeaderString = "TRACE_TYPE SEISMIC_DATA";
            writeBlock(ConvertToSeg2Line(traceHeaderString));

            seg2Bytes[seg2Index] = 0;
            seg2Index++;
            seg2Bytes[seg2Index] = 0;
            seg2Index++;

            //Trace3 Data
            for (int i = 0; i < dataLength; i++)
            {
                ch_Value = (Int32)(ch3[i] * Math.Pow(2, 31));
                HighwByte_H = (byte)((ch_Value & 0xFF000000) >> 24);
                HighwByte_L = (byte)((ch_Value & 0x00FF0000) >> 16);
                LowByte_H = (byte)((ch_Value & 0x0000FF00) >> 8);
                LowByte_L = (byte)(ch_Value & 0x000000FF);


                seg2Bytes[seg2Index + 0] = (byte)LowByte_L;
                seg2Bytes[seg2Index + 1] = (byte)LowByte_H;
                seg2Bytes[seg2Index + 2] = (byte)HighwByte_L;
                seg2Bytes[seg2Index + 3] = (byte)HighwByte_H;
                seg2Index += 4;
            }

            //Save seg2 File---
            SaveSeg2File(filePath);
        }

        private int SaveSeg2File(string filePath)
        {
            filePath = filePath + ".sg2";

            Stream st = File.Create(filePath);
            BinaryWriter bw = new BinaryWriter(st);
            bw.Write(seg2Bytes, 0, seg2Index);
            bw.Close();

            Console.WriteLine("The waveform has been succesfuly saved at '" + filePath + "'", "File Saved!");

            return 1;
        }

        private byte[] ConvertToSeg2Line(string fileHeaderString)
        {
            //String to Bytes conversion
            byte[] asciiBytes = Encoding.ASCII.GetBytes(fileHeaderString);
            byte[] outputBytes = new byte[asciiBytes.Length + 2];

            int highByte = outputBytes.Length / 256;
            int lowByte = outputBytes.Length - highByte * 256;

            outputBytes[0] = (byte)lowByte;
            outputBytes[1] = (byte)highByte;
            for (int i = 0; i < asciiBytes.Length; i++)
            {
                outputBytes[i + 2] = asciiBytes[i];
            }

            return outputBytes;
        }

        private void writeBlock(byte[] string2Bytes)
        {
            for (int i = 0; i < string2Bytes.Length; i++)
            {
                seg2Bytes[seg2Index] = string2Bytes[i];
                seg2Index++;
            }
        }

        #endregion Methods

    }
}
