using Microsoft.EntityFrameworkCore;
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
                if (context.SECContracts.Any())
                {
                    return;//Db has been seeded

                }
                var secContract = new SECContract[]
                {
                    new SECContract{ SECContractID=1,SECName="Abbeyshrule Yard Hub",County="Longford",
                        Address="Yard Hub, Abbeyshrule, Co Longford",PrimaryContract=" -",Phone="0892021000",Email="Test@gmail.com" },
                    new SECContract{ SECContractID=2,SECName="Ardagh SEC",County="Longford",
                        Address="Ardagh Community Centre",PrimaryContract="-",Phone="0892021000",Email="Test@gmail.com" },

                    new SECContract{SECContractID=3, SECName="Aughnacliffe-St Colmcille SEC",County="Longford",
                        Address="St Colmcille Community Centre, Aughnacliffe, Co Longford",PrimaryContract=" - ",Phone="0892021000",Email="Test@gmail.com" },

                    new SECContract{SECContractID=4, SECName="Ballinalee SEC",County="Longford",
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
