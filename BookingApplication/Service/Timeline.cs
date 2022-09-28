using Entities.Models;
using Entities.Models.Enums;

namespace BookingApplication.Service
{
    public class Timeline
    {
        public static int SlotDurationMinutes = 60;

        public static int MorningShiftStarts = 9;
        public static int MorningShiftEnds = 13;

        public static int AfternoonShiftStarts = 14;
        public static int AfternoonShiftEnds = 18;

        public static List<DoctorSchedule> GenerateSlots(Guid doctorId, DateTime start, DateTime end, bool weekends)
        {
            var slots = new List<DoctorSchedule>();
            var timeline = GenerateTimeline(start, end, weekends);

            foreach (var slot in timeline)
            {
                if (start <= slot.Start && slot.End <= end)
                {
                    for (var slotStart = slot.Start; slotStart < slot.End; slotStart.AddMinutes(SlotDurationMinutes))
                    {
                        var slotEnd = slotStart.AddMinutes(SlotDurationMinutes);
                        var createdSlot = new DoctorSchedule();
                        createdSlot.DoctorId = doctorId;
                        createdSlot.ConsultationStart = slotStart;
                        createdSlot.ConsultationEnd = slotEnd;
                        createdSlot.ScheduleStatus = DoctorScheduleStatus.Active;

                        slots.Add(createdSlot);
                    }
                }
            }
            return slots;
        }

        public static List<TimeCell> GenerateTimeline(DateTime start, DateTime end, bool weekends)
        {
            var result = new List<TimeCell>();


            var incrementMorning = 1;
            var incrementAfternoon = 1;

            var days = (end.Date - start.Date).TotalDays;

            if (end > end.Date)
            {
                days += 1;
            }
            for (var i = 0; i < days; i++)
            {
                var day = start.Date.AddDays(i);
                if (!weekends)
                {
                    if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
                    {
                        continue;
                    }
                }
                for (var x = MorningShiftStarts; x < MorningShiftEnds; x += incrementMorning)
                {
                    var cell = new TimeCell();
                    cell.Start = day.AddHours(x);
                    cell.End = day.AddHours(x + incrementMorning);

                    result.Add(cell);
                }
                for (var x = AfternoonShiftStarts; x < AfternoonShiftEnds; x += incrementAfternoon)
                {
                    var cell = new TimeCell();
                    cell.Start = day.AddHours(x);
                    cell.End = day.AddHours(x + incrementAfternoon);

                    result.Add(cell);
                }
            }


            return result;
        }

    }

    public class TimeCell
    {
        public DateTime Start;
        public DateTime End;
    }
}
