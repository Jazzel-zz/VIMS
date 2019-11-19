using System;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VIMS.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(VIMS.Startup))]
namespace VIMS
{
    public partial class Startup
    {
        List<string> policyTypes = new List<string>();
        List<string> policyDescriptions = new List<string>();
        List<double> policyRates = new List<double>();
        List<double> trackerRates = new List<double>();
        List<string> policyDuration = new List<string>();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            PopulateLists();
            AddUserAndRoles();
        }

        private void PopulateLists()
        {
            policyTypes.Add("Liability coverage");
            policyTypes.Add("Collision insurance");
            policyTypes.Add("Comprehensive insurance");
            policyTypes.Add("Underinsured motorist insurance");
            policyTypes.Add("Medical payments coverage");
            policyTypes.Add("Personal injury protection insurance");
            policyTypes.Add("Gap insurance");
            policyTypes.Add("Towing and labor insurance");
            policyTypes.Add("Rental reimbursement insurance");
            policyTypes.Add("Classic car insurance");
            policyDescriptions.Add("Liability coverage is required in most of the countries as a legal requirement to drive a car. Liability insurance may help cover damages for injuries and property damage to others for which you become legally responsible resulting from a covered accident.");
            policyDescriptions.Add("Collision insurance may cover damage to your car after an accident involving another vehicle and may help to repair or replace a covered vehicle.");
            policyDescriptions.Add("Comprehensive insurance can provide an extra level of coverage in the instance of an accident involving another vehicle. It may help pay for damage to your car due to incidents besides collisions, including vandalism, certain weather events and accidents with animals.");
            policyDescriptions.Add("Many drivers choose to carry the minimum in liability coverage to save money, but this might not provide enough coverage. Underinsured motorist insurance can protect you in the event of an accident with a driver whose insurance is not enough to cover the costs.");
            policyDescriptions.Add("Medical costs following an accident can be very expensive. Medical payments coverage can help pay medical costs related to a covered accident, regardless of who is at fault.");
            policyDescriptions.Add("Personal injury protection insurance may cover certain medical expenses and loss of income resulting from a covered accident. Depending on the limits of a policy, personal injury protection could cover as much as 80% of medical and other expenses stemming from a covered accident.");
            policyDescriptions.Add("Car value can depreciate quickly, so an auto insurance settlement might not be enough to cover the cost of a loan. Gap insurance may help certain drivers cover the amount owed on a car loan after a total loss or theft.");
            policyDescriptions.Add("Available if you already have comprehensive car insurance, towing and labor insurance may reimburse you for a tow and for the labor costs to repair your vehicle.");
            policyDescriptions.Add("Figuring out how to get around after an accident can be expensive. Rental reimbursement insurance helps pay for a rental car if your vehicle cannot be driven after an accident.");
            policyDescriptions.Add("Classic car insurance provides specialized coverage designed for the unique needs of vintage and classic car collectors. Find out if classic car insurance is right for you.");
            policyRates.Add(1.25);
            policyRates.Add(1.50);
            policyRates.Add(1.75);
            policyRates.Add(2.00);
            policyRates.Add(2.25);
            policyRates.Add(2.50);
            policyRates.Add(2.75);
            policyRates.Add(3.00);
            policyRates.Add(3.25);
            policyRates.Add(3.50);
            trackerRates.Add(3.50);
            trackerRates.Add(3.25);
            trackerRates.Add(3.00);
            trackerRates.Add(2.75);
            trackerRates.Add(2.50);
            trackerRates.Add(2.25);
            trackerRates.Add(2.00);
            trackerRates.Add(1.75);
            trackerRates.Add(1.50);
            trackerRates.Add(1.25);
            policyDuration.Add("2 years");
            policyDuration.Add("5 years");
            policyDuration.Add("8 years");
            policyDuration.Add("10 years");
            policyDuration.Add("15 years");
            policyDuration.Add("24 years");
            policyDuration.Add("30 years");
            policyDuration.Add("35 years");
            policyDuration.Add("lifetime");
            policyDuration.Add("lifetime");

        }

        private void AddPolicies(string _id, string _type, string _description, double _rate, double _tracker, string _duration)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                PolicyType type = new PolicyType()
                {
                    Type = _type,
                    Description = _description,
                    ApplicationUserId = _id,
                    Rate = _rate,
                    Tracker = _tracker,
                    Duration = _duration,
                };
                db.PolicyTypes.Add(type);
                db.SaveChanges();

            }
        }

        private void AddUserAndRoles()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (!roleManger.RoleExists("Employee"))
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                roleManger.Create(role);
                var user = new ApplicationUser()
                {
                    UserName = "JazzEmployee",
                    Email = "jazzelmehmood4@gmail.com",
                    EmailConfirmed = true,
                    DateOfJoining = DateTime.Now,
                    Name = "Muhammad Jazzel Mehmood",
                    PhoneNumber = "+92-348-2453559",
                };
                var result = userManager.Create(user, "Jazz@123");
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Employee");
                    if (policyTypes.Count == policyDescriptions.Count)
                    {
                        for (int i = 0; i < policyTypes.Count; i++)
                        {
                            AddPolicies(user.Id, policyTypes[i], policyDescriptions[i], policyRates[i], trackerRates[i],policyDuration[i]);
                        }
                    }
                }
            }
            if (!roleManger.RoleExists("Customer"))
            {
                var role = new IdentityRole();
                role.Name = "Customer";
                roleManger.Create(role);

                var user = new ApplicationUser()
                {
                    UserName = "JazzCustomer",
                    Email = "jazzelmehmood2013@gmail.com",
                    EmailConfirmed = true,
                    DateOfJoining = DateTime.Now,
                    Name = "Muhammad Jazzel Mehmood",
                    PhoneNumber = "+92-348-2453559",
                };
                var result = userManager.Create(user, "Jazz@123");
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Customer");
                }
            }
        }
    }
}