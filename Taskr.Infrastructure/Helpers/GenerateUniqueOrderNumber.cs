using System;

namespace Taskr.Infrastructure.Helpers
{
    public static class GenerateUniqueOrderNumber
    {
        
        public static string GenerateNumber()
        {
            var rand = new Random();
            string orderNumber = rand.Next(100, 999) + "-" + rand.Next(100, 999) + "-" + rand.Next(100000, 999999);
            return orderNumber;
        }
    }
}