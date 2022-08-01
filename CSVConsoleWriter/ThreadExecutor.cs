using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace CSVConsoleWriter
{
    public class ThreadExecutor
    {
        public double loop_rate_ { get; set; } = 1;  // In Seconds
        private int count = 1;
        private double run_time_ = 30; // Run loop for 30 seconds by default
        private DateTime[] timeStampList_;
        private double[] timeDiff_;
        private double[] goodCtrList_;
        private string data_file_path_ = @"C:\\0-Data\\";
        private string data_file_name_ = "test_data_Csharp";

        public ThreadExecutor()
        {
            timeStampList_ = new DateTime[(int)run_time_];
            goodCtrList_ = new double[(int)run_time_];
            timeDiff_ = new double[(int)run_time_];
        }

        public ThreadExecutor(double loop_rate)
        {
            loop_rate_ = loop_rate;
            run_time_ = 60 / loop_rate_;
            if (run_time_ < 1)
            {
                run_time_ = 1;
            }
            timeStampList_ = new DateTime[(int)run_time_];
            goodCtrList_ = new double[(int)run_time_];
            timeDiff_ = new double[(int)run_time_];
        }

        public bool RunExecutor()
        {
            DateTime previous_time = DateTime.Now;
            TimeSpan time_elapsed;
            bool first_loop = true;

            while (count <= run_time_)
            {
                // Get Current time
                DateTime time_now = DateTime.Now;

                if (first_loop)
                {
                    previous_time = time_now;
                    first_loop = false;
                }

                time_elapsed = time_now - previous_time;
                previous_time = time_now;

                // Read and store values in a list
                timeStampList_[count-1] = time_now;
                goodCtrList_[count-1] = getValue();
                timeDiff_[count - 1] = time_elapsed.TotalSeconds;

                // Debug log statement
                Console.WriteLine("ThreadExecutor::RunExecutor:: Hello world "
                                  + goodCtrList_[count-1].ToString() + " " + 
                                  count.ToString() + " " +
                                  timeStampList_[count-1].ToString("HH:mm:ss.fff zzz"));
                
                // Increment counter and sleep for specified loop rate
                count++;
                Thread.Sleep((int)(1000 * loop_rate_));
            }
            
            // Write data to csv file
            writeToFile();
            return true;
        }

        public double getValue()
        {
            Random rd = new Random();
            // Get a random number betweeen 0 - 24V
            return (rd.NextDouble() * rd.Next(0, 24));
        }

        public bool writeToFile()
        {
            string data_file = data_file_path_ + data_file_name_ + "_" + DateTime.Now.ToString("HH_mm_ss") + ".txt";
            string time_diff_file = data_file_path_ + "time_diff_cSharp_" + DateTime.Now.ToString("HH_mm_ss") + ".txt";

            var str_builder = new StringBuilder();
            var time_diff_str_builder = new StringBuilder();

            for (int i = 0; i < run_time_; i++)
            {
                str_builder.AppendLine(timeStampList_[i] + " " + goodCtrList_[i]);
                // str_builder.AppendLine(timeStampList_[i].ToString() + " " + goodCtrList_[i].ToString());
                time_diff_str_builder.AppendLine(timeDiff_[i].ToString());
            }

            File.WriteAllText(data_file, str_builder.ToString());
            File.WriteAllText(time_diff_file, time_diff_str_builder.ToString());
            return true;
        }
    }
}
