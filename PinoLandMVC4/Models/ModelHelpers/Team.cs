using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PinoLandMVC4;
using MG = Fuqua.CompetativeAnalysis.MarketGame;

namespace PinoLandMVC4.Models.ModelHelpers
{
    public class Team
    {
        public static int CreateTeam(int economyId, string name, string[] members)
        {
            using (MG.GameDataObjectContext context = ContextHelper.GetObjectContext())
            {

                MG.Company company = (from c in context.Companies
                                      where c.EconomyId == economyId
                                      && string.Compare(c.Name, name) == 0
                                      select c).FirstOrDefault();
                if (company == null)
                {
                    company = new MG.Company();
                    company.EconomyId = economyId;
                    company.Name = name;
                }

                foreach (string userName in members)
                {
                    MG.IM_User user = context.IM_User.SingleOrDefault(u => u.UserName == userName);
                    if (user != null)
                    {
                        //Remove any existing company assignments for users
                        List<MG.Company> existingCompanies = user.Companies.Where(c => c.EconomyId == economyId).ToList();
                        foreach (MG.Company ec in existingCompanies)
                            user.Companies.Remove(ec);

                        //And add to the new company
                        company.Users.Add(user);
                    }
                }

                context.SaveChanges();
                return company.CompanyId;
            }
        }

        public static int CreateUser(string username, string firstname, string lastname, string email, string password)
        {

            using (MG.GameDataObjectContext context = ContextHelper.GetObjectContext())
            {
                //Find the user by username
                //If they exist, just return

                MG.IM_User user = (from u in context.IM_User
                                   where string.Compare(u.UserName, username, true) == 0
                                   select u).FirstOrDefault();
                if (user != null)
                {
                    return user.UserId;
                }

                user = new MG.IM_User()
                {
                    FirstName = firstname,
                    LastName = lastname,
                    UserName = username,
                    Email = email,
                    Password = password,
                    Active = true
                };

                context.AddToIM_User(user);
                context.SaveChanges();
                return user.UserId;
            }
        }

    }
}