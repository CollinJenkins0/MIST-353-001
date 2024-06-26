﻿using GeoSnowAPI.Data;
using GeoSnowAPI.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;


namespace GeoSnowAPI.Entities
{
    public class ResortService : IResortService
    {
        private readonly DbcontextClass _dbContext;

        public ResortService(DbcontextClass dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Resort>> GetAllResorts()
        {
            return await _dbContext.RESORT.ToListAsync();
        }

        public async Task<Resort> GetResortDetails(int resortID)
        {
            return await _dbContext.RESORT.FirstOrDefaultAsync(r => r.ResortID == resortID);
        }

        public async Task<List<Resort>> SearchResortsByRadius(decimal latitude, decimal longitude, int? radius)
        {
            var latParam = new SqlParameter("@Latitude", latitude);
            var longParam = new SqlParameter("@Longitude", longitude);
            var radiusParam = new SqlParameter("@Radius", radius.HasValue ? radius.Value : (object)DBNull.Value);

            return await _dbContext.RESORT.FromSqlRaw("EXEC SearchResortsByRadius @Latitude, @Longitude, @Radius", latParam, longParam, radiusParam).ToListAsync();
        }

        public async Task<List<Resort>> SearchResortsByRadiusSingleDate(decimal latitude, decimal longitude, DateTime myDate, int? radius)
        {
            var latParam = new SqlParameter("@Latitude", latitude);
            var longParam = new SqlParameter("@Longitude", longitude);
            var dateParam = new SqlParameter("@MyDate", myDate);
            var radiusParam = new SqlParameter("@Radius", radius.HasValue ? radius.Value : (object)DBNull.Value);

            return await _dbContext.RESORT.FromSqlRaw("EXEC spResortSearchByRadiusSingleDate @Latitude, @Longitude, @MyDate, @Radius", latParam, longParam, dateParam, radiusParam).ToListAsync();
        }

        public async Task<List<Resort>> ResortSearchByRadiusDateRange(decimal latitude, decimal longitude, DateTime startDate, DateTime endDate, int? radius)
        {
            var latParam = new SqlParameter("@Latitude", latitude);
            var longParam = new SqlParameter("@Longitude", longitude);
            var startDateParam = new SqlParameter("@StartDate", startDate);
            var endDateParam = new SqlParameter("@EndDate", endDate);
            var radiusParam = new SqlParameter("@Radius", radius.HasValue ? radius.Value : (object)DBNull.Value);

            return await _dbContext.RESORT.FromSqlRaw(
                "EXEC ResortSearchByRadiusDateRange @Latitude, @Longitude, @StartDate, @EndDate, @Radius",
                latParam, longParam, startDateParam, endDateParam, radiusParam
            ).ToListAsync();

        }
        
        public async Task AddResort(Resort resort)
            {
                var parameters = new[]
                {
            new SqlParameter("@Address", resort.Address),
            new SqlParameter("@Zipcode", resort.Zipcode),
            new SqlParameter("@City", resort.City),
            new SqlParameter("@State", resort.State),
            new SqlParameter("@Country", resort.Country),
            new SqlParameter("@Name", resort.Name),
            new SqlParameter("@Phone", resort.Phone ?? (object)DBNull.Value),
            new SqlParameter("@ResortType", resort.ResortType ?? (object)DBNull.Value),
            new SqlParameter("@Email", resort.Email ?? (object)DBNull.Value),
            new SqlParameter("@Latitude", resort.Latitude ?? (object)DBNull.Value),
            new SqlParameter("@Longitude", resort.Longitude ?? (object)DBNull.Value)
        };

                await _dbContext.Database.ExecuteSqlRawAsync("EXEC AddResort @Address, @Zipcode, @City, @State, @Country, @Name, @Phone, @ResortType, @Email, @Latitude, @Longitude", parameters);
            }
        }

    }


