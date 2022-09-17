using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Common.Helpers
{
    public static class ByteHelper
    {
        public static double ConvertBytesToMegabytes(long bytes) => (double)bytes / 1024.0 / 1024.0;

        public static long ConvertMegabytesToBytes(long megabytes) => megabytes * 1024L * 1024L;
    }
}
