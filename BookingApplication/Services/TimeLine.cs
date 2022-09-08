using Entities.Models;
using Entities.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApplication.Services
{
    public class TimeLine

        {
            public static int SlotDurationMinutes = 60;

            public static int MorningShiftStarts = 9;
            public static int MorningShiftEnds = 13;

            public static int AfternoonShiftStarts = 14;
            public static int AfternoonShiftEnds = 18;

            public static List<Appointment> GenerateSlots(DateTime start, DateTime end, string scale)
            {
                var slots = new List<Appointment>();
                var timeline = GenerateTimeline(start, end, scale);

                foreach (var slot in timeline)
                {
                    if (start <= slot.Start && slot.End <= end)
                    {
                        for (var slotStart = slot.Start; slotStart < slot.End; slotStart.AddMinutes(SlotDurationMinutes))
                        {
                            var slotEnd = slotStart.AddMinutes(SlotDurationMinutes);
                            var createdSlot = new Appointment();
                            createdSlot.AppointmentStart = slotStart;
                            createdSlot.AppointmentEnd = slotEnd;
                            createdSlot.Status = AppointmentStatus.Open;

                            slots.Add(createdSlot);
                        }
                    }
                }
                return slots;
            }

            private static List<TimeCell> GenerateTimeline(DateTime start, DateTime end, string scale)
            {
                var result = new List<TimeCell>();


                var incrementMorning = 1;
                var incrementAfternoon = 1;

                var days = (end.Date - start.Date).TotalDays;

                if (end > end.Date)
                {
                    days += 1;
                }

                if (scale == "shifts")
                {
                    incrementMorning = MorningShiftEnds - MorningShiftStarts;
                    incrementAfternoon = AfternoonShiftEnds - AfternoonShiftStarts;
                }

                for (var i = 0; i < days; i++)
                {
                    var day = start.Date.AddDays(i);
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

