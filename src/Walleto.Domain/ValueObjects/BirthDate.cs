using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walleto.Domain.Abstractions;
using Walleto.Shared.Calendar;

namespace Walleto.Domain.ValueObjects
{
    public class BirthDate : ValueObject
    {
        public string Value { get; }

        public BirthDate(string shamsiDate)
        {
            if (!Calendar.IsValidShamsi(shamsiDate))
                throw new ArgumentException("تاریخ تولد معتبر نیست", nameof(shamsiDate));

            if (!Calendar.IsAgeGreatherThan18(shamsiDate))
                throw new ArgumentException("تاریخ تولد نمی‌تواند در آینده باشد", nameof(shamsiDate));
            Value = shamsiDate;
        }



        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
