using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Numerics;
using TimeProductivityTracking.web.Models;


namespace TimeProductivityTracking.web.Data
{
    public class DbInitializer
    {
        public static void Initializer(IServiceProvider serviceProvider)
        {
            using (var context = new ProductivitiesContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ProductivitiesContext>>()))
            {

                // Seed Rates
                if (!context.Rates.Any())
                {
                    var rates = new Rate[]
                    {
                new Rate{RateName="L5",HourlyWage=45.00},
                new Rate{RateName="L4",HourlyWage=25.00},
                new Rate{RateName="L3",HourlyWage=20.00},
                new Rate{RateName="L2",HourlyWage=15.50},
                new Rate{RateName="L1",HourlyWage=13.00}
                    };

                    context.Rates.AddRange(rates);
                    context.SaveChanges();
                }

                /*
                foreach (Rate r in rates)
                {
                    context.Rates.Add(r);
                }*/


                // Seed SEC Contracts
                if (!context.SECContracts.Any())
                {
                    var secContract = new SECContract[]
                    {
                        new SECContract
                        {
                            SECName = "Abbeyshrule Yard Hub",
                            County = "Longford",
                            Address = "Yard Hub, Abbeyshrule, Co Longford",
                            PrimaryContract = "-",
                            Phone = "0892021000",
                            Email = "Test@gmail.com"
                        },
                        new SECContract
                        {
                            SECName = "Ardagh SEC",
                            County = "Longford",
                            Address = "Ardagh Community Centre",
                            PrimaryContract = "-",
                            Phone = "0892021000",
                            Email = "Test@gmail.com"
                        },
                        new SECContract
                        {
                            SECName = "Aughnacliffe-St Colmcille SEC",
                            County = "Longford",
                            Address = "St Colmcille Community Centre, Aughnacliffe, Co Longford",
                            PrimaryContract = "-",
                            Phone = "0892021000",
                            Email = "Test@gmail.com"
                        },
                        new SECContract
                        {
                            SECName = "Ballinalee SEC",
                            County = "Longford",
                            Address = "Ballinalee, Co Longford",
                            PrimaryContract = "-",
                            Phone = "0892021000",
                            Email = "Test@gmail.com"
                        },
                        new SECContract
                        {
                            SECName = "Drumlish - Ballinamuck Area SEC",
                            County = "Longford",
                            Address = "Drumlish Community Centre",
                            PrimaryContract = "-",
                            Phone = "0892021000",
                            Email = "Test@gmail.com"
                        },
                        new SECContract
                        {
                            SECName="Edgeworthstown SEC",
                            County="Longford",
                            Address="Edgeworthstown, Co Longford",
                            PrimaryContract="-",
                            Phone="0892021000",
                            Email="XXXXXXXXXXXXXX"
                        },
                        new SECContract
                        {
                            SECName="Granard Area SEC",
                            County="Longford",
                            Address="Granard, Co. Longford",
                            PrimaryContract="-",
                            Phone="0892021000",
                            Email="XXXXXXXXXXXXXX"
                        },
                        new SECContract
                        {
                            SECName="Lanesborough Tourism Coop SEC",
                            County="Longford",
                            Address="Lanesborough, Co Longford",
                            PrimaryContract="-",
                            Phone="0892021000",
                            Email="XXXXXXXXXXXXXX"
                        },
                        new SECContract
                        {
                            SECName="Longford Town SEC",
                            County="Longford",
                            Address="Longford Town, Co Longford",
                            PrimaryContract="-",
                            Phone="0892021000",
                            Email="XXXXXXXXXXXXXX"
                        },
                        new SECContract
                        {
                            SECName="Longford Renewables SEC",
                            County="Longford",
                            Address="Wiphawab Klinhom, Ballyleague, Lanesborough, Co Longford",
                            PrimaryContract="-",
                            Phone="0892021000",
                            Email="XXXXXXXXXXXXXX"
                        },
                        new SECContract
                        {
                            SECName="Moyne - Latin School Centre SEC",
                            County="Longford",
                            Address="Latin School, Moyne Co Longford",
                            PrimaryContract="-",
                            Phone="0892021000",
                            Email="XXXXXXXXXXXXXX"
                        },
                        new SECContract
                        {
                            SECName="Newtownforbes SEC",
                            County="Longford",
                            Address="Newtownforbes, Co Longford",
                            PrimaryContract="-",
                            Phone="0892021000",
                            Email="XXXXXXXXXXXXXX"
                        },
                        new SECContract
                        {
                            SECName="Ballymahon Area - Planned",
                            County="Longford",
                            Address="Ballymahon Town Team & GAA Club",
                            PrimaryContract="-",
                            Phone="0892021000",
                            Email="XXXXXXXXXXXXXX"
                        },
                        new SECContract
                        {
                            SECName="Kenagh - Planned",
                            County="Longford",
                            Address="Cllr Colm Murray, Aras an Chontae, Longford",
                            PrimaryContract="-",
                            Phone="0892021000",
                            Email="XXXXXXXXXXXXXX"
                        }
                    };

                


                    context.SECContracts.AddRange(secContract);
                    context.SaveChanges();


                }
            }

        }
    }
 }