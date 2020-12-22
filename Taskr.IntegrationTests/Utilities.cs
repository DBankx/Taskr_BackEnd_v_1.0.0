using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Taskr.Domain;
using Taskr.Persistance;

namespace Taskr.IntegrationTests
{
    public static class Utilities
    {
        public static void InitializeDbForTests(DataContext db)
        {
            db.Jobs.AddRange(GetSeedingMessages());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(DataContext db)
        {
            db.Jobs.RemoveRange(db.Jobs);
            InitializeDbForTests(db);
        }

        public static List<Job> GetSeedingMessages()
        {
            return new List<Job>()
            {
                new Job()
                {
                    Description = "test job 1 desc",
                    Title = "test job 1",
                    InitialPrice = 200
                },
                new Job()
                {
                    Description = "test job 2 desc",
                    Title = "test job 2",
                    InitialPrice = 300
                }
            };
        } 
    }
}