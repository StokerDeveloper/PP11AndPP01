namespace SF2022User_NN_LibTestProject
{
    [TestClass]
    public class UnitTest1
    {
        Calculations calculations = new Calculations();

        [TestMethod]
        public void TestMethodWithDefaultValues()
        {
            TimeSpan[] startTimes = {
                new TimeSpan(10, 0, 0),
                new TimeSpan(11, 0, 0),
                new TimeSpan(15, 0, 0),
                new TimeSpan(15, 30, 0),
                new TimeSpan(16, 50, 0)
            };
            int[] durations = {
                60,
                30,
                10,
                10,
                40
            };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[] expected = {
                "08:00-08:30",
                "08:30-09:00",
                "09:00-09:30",
                "09:30-10:00",
                "11:30-12:00",
                "12:00-12:30",
                "12:30-13:00",
                "13:00-13:30",
                "13:30-14:00",
                "14:00-14:30",
                "14:30-15:00",
                "15:40-16:10",
                "16:10-16:40",
                "17:30-18:00"
            };
            string[] actual = calculations.AvailablePeriods(
                startTimes,
                durations,
                beginWorkingTime,
                endWorkingTime,
                consultationTime
            );

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodWithMorningWorkingTime()
        {
            TimeSpan[] startTimes = {
                new TimeSpan(10, 0, 0),
                new TimeSpan(11, 0, 0),
            };
            int[] durations = {
                60,
                30,
            };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(13, 0, 0);
            int consultationTime = 30;

            string[] expected = {
                "08:00-08:30",
                "08:30-09:00",
                "09:00-09:30",
                "09:30-10:00",
                "11:30-12:00",
                "12:00-12:30",
                "12:30-13:00",
            };
            string[] actual = calculations.AvailablePeriods(
                startTimes,
                durations,
                beginWorkingTime,
                endWorkingTime,
                consultationTime
            );

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodWithAfterMorningWorkingTime()
        {
            TimeSpan[] startTimes = {
                new TimeSpan(15, 0, 0),
                new TimeSpan(15, 30, 0),
                new TimeSpan(16, 50, 0)
            };
            int[] durations = {
                10,
                10,
                40
            };
            TimeSpan beginWorkingTime = new TimeSpan(13, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[] expected = {
                "13:00-13:30",
                "13:30-14:00",
                "14:00-14:30",
                "14:30-15:00",
                "15:40-16:10",
                "16:10-16:40",
                "17:30-18:00"
            };
            string[] actual = calculations.AvailablePeriods(
                startTimes,
                durations,
                beginWorkingTime,
                endWorkingTime,
                consultationTime
            );

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodWithEmptyStartTimesAndDurations()
        {
            TimeSpan[] startTimes = { };
            int[] durations = { };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[] expected = {
                "08:00-08:30",
                "08:30-09:00",
                "09:00-09:30",
                "09:30-10:00",
                "10:00-10:30",
                "10:30-11:00",
                "11:00-11:30",
                "11:30-12:00",
                "12:00-12:30",
                "12:30-13:00",
                "13:00-13:30",
                "13:30-14:00",
                "14:00-14:30",
                "14:30-15:00",
                "15:00-15:30",
                "15:30-16:00",
                "16:00-16:30",
                "16:30-17:00",
                "17:00-17:30",
                "17:30-18:00"
            };
            string[] actual = calculations.AvailablePeriods(
                startTimes,
                durations,
                beginWorkingTime,
                endWorkingTime,
                consultationTime
            );

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodWithBigConsultationTime()
        {
            TimeSpan[] startTimes = {
                new TimeSpan(10, 0, 0),
                new TimeSpan(11, 0, 0),
                new TimeSpan(15, 0, 0),
                new TimeSpan(15, 30, 0),
                new TimeSpan(16, 50, 0)
            };
            int[] durations = {
                60,
                30,
                10,
                10,
                40
            };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 60;

            string[] expected = {
                "08:00-09:00",
                "09:00-10:00",
                "11:30-12:30",
                "12:30-13:30",
                "13:30-14:30",
                "15:40-16:40"
            };
            string[] actual = calculations.AvailablePeriods(
                startTimes,
                durations,
                beginWorkingTime,
                endWorkingTime,
                consultationTime
            );

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodWithBigDurations()
        {
            TimeSpan[] startTimes = {
                new TimeSpan(10, 0, 0),
                new TimeSpan(11, 0, 0),
                new TimeSpan(15, 0, 0),
                new TimeSpan(15, 30, 0),
                new TimeSpan(16, 50, 0)
            };
            int[] durations = {
                60,
                60,
                60,
                60,
                60
            };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[] expected = {
                "08:00-08:30",
                "08:30-09:00",
                "09:00-09:30",
                "09:30-10:00",
                "12:00-12:30",
                "12:30-13:00",
                "13:00-13:30",
                "13:30-14:00",
                "14:00-14:30",
                "14:30-15:00"
            };
            string[] actual = calculations.AvailablePeriods(
                startTimes,
                durations,
                beginWorkingTime,
                endWorkingTime,
                consultationTime
            );

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodWithManyStartTimesAndLittleDurations()
        {
            TimeSpan[] startTimes = {
                new TimeSpan(8, 0, 0),
                new TimeSpan(9, 0, 0),
                new TimeSpan(10, 0, 0),
                new TimeSpan(11, 0, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(13, 0, 0),
                new TimeSpan(14, 0, 0),
                new TimeSpan(15, 0, 0),
                new TimeSpan(16, 0, 0),
                new TimeSpan(17, 0, 0)
            };
            int[] durations = {
                10,
                10,
                10,
                10,
                10,
                10,
                10,
                10,
                10,
                10
            };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[] expected = {
                "08:10-08:40",
                "09:10-09:40",
                "10:10-10:40",
                "11:10-11:40",
                "12:10-12:40",
                "13:10-13:40",
                "14:10-14:40",
                "15:10-15:40",
                "16:10-16:40",
                "17:10-17:40"
            };
            string[] actual = calculations.AvailablePeriods(
                startTimes,
                durations,
                beginWorkingTime,
                endWorkingTime,
                consultationTime
            );

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodWithManyStartTimesAndLittleDurationsAndBigConsultationTime()
        {
            TimeSpan[] startTimes = {
                new TimeSpan(8, 0, 0),
                new TimeSpan(9, 0, 0),
                new TimeSpan(10, 0, 0),
                new TimeSpan(11, 0, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(13, 0, 0),
                new TimeSpan(14, 0, 0),
                new TimeSpan(15, 0, 0),
                new TimeSpan(16, 0, 0),
                new TimeSpan(17, 0, 0)
            };
            int[] durations = {
                10,
                10,
                10,
                10,
                10,
                10,
                10,
                10,
                10,
                10
            };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 60;

            string[] expected = { };
            string[] actual = calculations.AvailablePeriods(
                startTimes,
                durations,
                beginWorkingTime,
                endWorkingTime,
                consultationTime
            );

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodWithEmptyStartTimesAndDurationsAndLittleConsultationTime()
        {
            TimeSpan[] startTimes = { };
            int[] durations = { };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 10;

            string[] expected = {
                "08:00-08:10",
                "08:10-08:20",
                "08:20-08:30",
                "08:30-08:40",
                "08:40-08:50",
                "08:50-09:00",
                "09:00-09:10",
                "09:10-09:20",
                "09:20-09:30",
                "09:30-09:40",
                "09:40-09:50",
                "09:50-10:00",
                "10:00-10:10",
                "10:10-10:20",
                "10:20-10:30",
                "10:30-10:40",
                "10:40-10:50",
                "10:50-11:00",
                "11:00-11:10",
                "11:10-11:20",
                "11:20-11:30",
                "11:30-11:40",
                "11:40-11:50",
                "11:50-12:00",
                "12:00-12:10",
                "12:10-12:20",
                "12:20-12:30",
                "12:30-12:40",
                "12:40-12:50",
                "12:50-13:00",
                "13:00-13:10",
                "13:10-13:20",
                "13:20-13:30",
                "13:30-13:40",
                "13:40-13:50",
                "13:50-14:00",
                "14:00-14:10",
                "14:10-14:20",
                "14:20-14:30",
                "14:30-14:40",
                "14:40-14:50",
                "14:50-15:00",
                "15:00-15:10",
                "15:10-15:20",
                "15:20-15:30",
                "15:30-15:40",
                "15:40-15:50",
                "15:50-16:00",
                "16:00-16:10",
                "16:10-16:20",
                "16:20-16:30",
                "16:30-16:40",
                "16:40-16:50",
                "16:50-17:00",
                "17:00-17:10",
                "17:10-17:20",
                "17:20-17:30",
                "17:30-17:40",
                "17:40-17:50",
                "17:50-18:00"
            };
            string[] actual = calculations.AvailablePeriods(
                startTimes,
                durations,
                beginWorkingTime,
                endWorkingTime,
                consultationTime
            );

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodWithBigDurationsAndCinsultationTime()
        {
            TimeSpan[] startTimes = {
                new TimeSpan(10, 0, 0),
                new TimeSpan(11, 0, 0),
                new TimeSpan(15, 0, 0),
                new TimeSpan(15, 30, 0),
                new TimeSpan(16, 50, 0)
            };
            int[] durations = {
                60,
                60,
                60,
                60,
                60
            };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 60;

            string[] expected = {
                "08:00-09:00",
                "09:00-10:00",
                "12:00-13:00",
                "13:00-14:00",
                "14:00-15:00"
            };
            string[] actual = calculations.AvailablePeriods(
                startTimes,
                durations,
                beginWorkingTime,
                endWorkingTime,
                consultationTime
            );

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
