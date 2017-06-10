﻿// <auto-generated />

using System;
using System.Linq;
using WebApplication1.Web.Enums;
using WebApplication1.Web.Models;

namespace WebApplication1.Web.DatabaseContext
{
    /// <summary>
    /// Application Database Extension for seeding sample data
    /// </summary>
    public static class AppDbContextExtensions
    {
        /// <summary>
        /// Seeds sample data
        /// </summary>
        /// <param name="context"></param>
        public static void EnsureSeedData(this AppDbContext context)
        {
            if (context.Clients.Any() || context.Categories.Any())
            {
                return;
            }

            var partnersCategory = context.Categories.Add(new Category { Name = "Partners" }).Entity;
            var japaneseItCategory = context.Categories.Add(new Category { Name = "日本IT企業" }).Entity;
            var otherCategory = context.Categories.Add(new Category { Name = "Other" }).Entity;

            context.Clients.AddRange(
                new Client
                {
                    Name = "Name1",
                    Email = "name1@ignas.me",
                    Fax = "111111",
                    Phone = "222222",
                    Status = ClientStatus.Active,
                    Address = "1 Street, East Sussex, BN1 4AA, Brighton, UK",
                    CreatedOn = DateTime.Now,
                    Category = partnersCategory
                },
                new Client
                {
                    Name = "Name2",
                    Email = "name2@ignas.me",
                    Fax = "222222",
                    Phone = "333333",
                    Status = ClientStatus.Inactive,
                    Address = "2 Street, East Sussex, BN1 4AA, Brighton, UK",
                    CreatedOn = DateTime.Now,
                    Category = japaneseItCategory
                },
                new Client
                {
                    Name = "Name3",
                    Email = "name3@ignas.me",
                    Fax = "333333",
                    Phone = "444444",
                    Status = ClientStatus.Archived,
                    Address = "3 Street, East Sussex, BN1 4AA, Brighton, UK",
                    CreatedOn = DateTime.Now,
                    Category = otherCategory
                },
                new Client
                {
                    Name = "Name4",
                    Email = "name4@ignas.me",
                    Fax = "444444",
                    Phone = "555555",
                    Status = ClientStatus.Pending,
                    Address = "4 Street, East Sussex, BN1 4AA, Brighton, UK",
                    CreatedOn = DateTime.Now,
                    Category = partnersCategory
                },
                new Client
                {
                    Name = "Name5",
                    Email = "name5@ignas.me",
                    Fax = "555555",
                    Phone = "666666",
                    Status = ClientStatus.Active,
                    Address = "5 Street, East Sussex, BN1 4AA, Brighton, UK",
                    CreatedOn = DateTime.Now,
                    Category = japaneseItCategory
                }
            );

            context.SaveChanges();
        }
    }
}