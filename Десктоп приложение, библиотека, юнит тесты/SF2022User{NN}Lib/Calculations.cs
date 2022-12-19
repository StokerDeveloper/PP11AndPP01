using System;
using System.Collections.Generic;

namespace SF2022User_NN_Lib
{
    public class Calculations
    {
        public string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {
            string[] result;
            List<string> times = new List<string>();

            TimeSpan currentWorkingTime = beginWorkingTime;

            int startTimeIndex = 0;
            while (currentWorkingTime < endWorkingTime)
            {
                if (startTimeIndex < startTimes.Length)
                {
                    if (currentWorkingTime == startTimes[startTimeIndex])
                    {
                        currentWorkingTime += new TimeSpan(0, durations[startTimeIndex], 0);

                        startTimeIndex++;

                        continue;
                    }

                    if (currentWorkingTime + new TimeSpan(0, consultationTime, 0) > startTimes[startTimeIndex])
                    {
                        currentWorkingTime = startTimes[startTimeIndex];

                        continue;
                    }
                }

                if (currentWorkingTime + new TimeSpan(0, consultationTime, 0) > endWorkingTime)
                    break;

                string startConsultationTime = currentWorkingTime.ToString("hh\\:mm");

                currentWorkingTime += new TimeSpan(0, consultationTime, 0);

                string endConsultationTime = currentWorkingTime.ToString("hh\\:mm");

                times.Add(startConsultationTime + "-" + endConsultationTime);
            }

            result = times.ToArray();
            return result;
        }
    }
}
