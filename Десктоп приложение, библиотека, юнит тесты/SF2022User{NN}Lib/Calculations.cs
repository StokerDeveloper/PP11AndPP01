using System;
using System.Collections.Generic;

namespace SF2022User_NN_Lib
{
    public class Calculations
    {
        public string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {
            string[] result;
            List<string> times = new List<string>(); // Список для хранения значений во время вычисления

            TimeSpan currentWorkingTime = beginWorkingTime; // Текущее время - переменная для прохождения по всему рабочему дню

            int startTimeIndex = 0; // Индекс начала перерыва для отслеживания в цикле
            while (currentWorkingTime < endWorkingTime)
            {
                if (startTimeIndex < startTimes.Length)
                {
                    if (currentWorkingTime == startTimes[startTimeIndex])  // Увеличение текущего времени на время перерыва
                    {
                        currentWorkingTime += new TimeSpan(0, durations[startTimeIndex], 0);

                        startTimeIndex++;

                        continue;
                    }

                    if (currentWorkingTime + new TimeSpan(0, consultationTime, 0) > startTimes[startTimeIndex]) // Если консультация пересекается с перерывом, то она пропускается
                    {
                        currentWorkingTime = startTimes[startTimeIndex];

                        continue;
                    }
                }

                if (currentWorkingTime + new TimeSpan(0, consultationTime, 0) > endWorkingTime) // Если консультация превышает конец рабочего дня, то выход из цикла
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
