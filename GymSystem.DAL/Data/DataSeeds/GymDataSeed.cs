using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.DataSeeds
{
    public class GymDataSeed
    {
        public static async Task SeedAsync(GymDbContext dbContext, string seedFilesPath, ILogger logger, CancellationToken ct = default)
        {
            try
            {
                if(!await dbContext.Plans.AnyAsync(ct))
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json", seedFilesPath);

                    if(plans.Count > 0 )
                    {
                        dbContext.Plans.AddRange(plans);
                        logger.LogInformation($"Seeded {plans.Count} plans..");
                    }
                }
                if (dbContext.ChangeTracker.HasChanges())
                {
                    await dbContext.SaveChangesAsync(ct);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gym Data Seeding Failed");
                throw;
            }
        }

        private static List<T> LoadDataFromJsonFile<T>(string fileName, string folderPath)
        {
            var filePath = Path.Combine(folderPath, fileName);
            if(!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Seed Data File not Found: {filePath}");
            }
            var Data = File.ReadAllText(filePath);
            var Options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Options.Converters.Add(new JsonStringEnumConverter());

            return JsonSerializer.Deserialize<List<T>>(Data, Options)?? [];
        }
    }
}
