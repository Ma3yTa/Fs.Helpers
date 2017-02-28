namespace Fs.Helpers.Types
    module DateTimeTypes =
        [<Measure>] type Ms   // Milliseconds
        [<Measure>] type Sec  // Seconds
        [<Measure>] type Min  // Minutes
        [<Measure>] type Hr   // Hours
        [<Measure>] type Day  // Days
        [<Measure>] type Week // Weeks
        [<Measure>] type Mon  // Months
        [<Measure>] type Yr   // Years
        type Time = Yr | Mon | Week | Day | Hr | Min | Sec | Ms