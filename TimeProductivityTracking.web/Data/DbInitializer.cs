using Microsoft.EntityFrameworkCore;
using OfficeManagement.Models;
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


                if (context.Rates.Any())
                {
                    return;//Db has been seeded
                }
                var rates = new Rate[]
                {
                new Rate{RateName="L5",HourlyWage=45.00},
                new Rate{RateName="L4",HourlyWage=25.00},
                new Rate{RateName="L3",HourlyWage=20.00},
                new Rate{RateName="L2",HourlyWage=15.50},
                new Rate{RateName="L1",HourlyWage=13.00}

                };
                foreach (Rate r in rates)
                {
                    context.Rates.Add(r);
                }

                context.SaveChanges();




                if (context.SECContracts.Any())
                {
                    return;//Db has been seeded
                }
                var secContract = new SECContract[]
                {
                    new SECContract{ SECContractId=1,SECName="Abbeyshrule Yard Hub",County="Longford",
                        Address="Yard Hub, Abbeyshrule, Co Longford",PrimaryContract=" -",Phone="0892021000",Email="Test@gmail.com" },
                    new SECContract{ SECContractId=2,SECName="Ardagh SEC",County="Longford",
                        Address="Ardagh Community Centre",PrimaryContract="-",Phone="0892021000",Email="Test@gmail.com" },

                    new SECContract{SECContractId=3, SECName="Aughnacliffe-St Colmcille SEC",County="Longford",
                        Address="St Colmcille Community Centre, Aughnacliffe, Co Longford",PrimaryContract=" - ",Phone="0892021000",Email="Test@gmail.com" },

                    new SECContract{SECContractId=4, SECName="Ballinalee SEC",County="Longford",
                        Address="Ballinalee, Co Longford",PrimaryContract="-",Phone="0892021000",Email="Test@gmail.com" }

                };
                foreach(SECContract c in  secContract)
                {
                    context.SECContracts.Add(c);
                }
                context.SaveChanges();



            }
        }

    }
}
