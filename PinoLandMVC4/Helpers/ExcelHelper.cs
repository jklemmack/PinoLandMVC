using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;


namespace PinoLandMVC4.Helpers
{
    public class ExcelHelper
    {

        public static bool UploadTeamFile(int economyId, Stream fileStream)
        {
            try
            {
                SpreadsheetDocument doc = SpreadsheetDocument.Open(fileStream, false);
                string firstSheet = doc.WorkbookPart.Workbook.Sheets.Descendants<Sheet>().First().Name;

                System.Data.DataTable teams = OpenXMLHelpers.OpenXMLDataTableHelper.GetDataTable(doc, firstSheet, "A1");

                // Create the users (if they need to be created)
                foreach (System.Data.DataRow user in teams.Rows)
                {
                    string userName = (string)user["Username"];
                    string firstName = (string)user["First Name"];
                    string lastName = (string)user["Last Name"];
                    string email = (string)user["EMail"];
                    string password = (string)user["password"];
                    Models.ModelHelpers.Team.CreateUser(userName, firstName, lastName, email, password);
                    //client.CreateUser(userName, firstName, lastName, email, password);
                }

                System.Data.DataView teamsView = new System.Data.DataView(teams);
                System.Data.DataTable distinctTeams = teamsView.ToTable(true, "Team");
                foreach (System.Data.DataRow team in distinctTeams.Rows)
                {
                    string teamName = (string)team["Team"];
                    System.Data.DataRow[] members = teams.Select(string.Format("Team = '{0}'", teamName.Replace("'", "''")));

                    List<string> memberList = members.Select(m => (string)m["Username"]).ToList();
                    Models.ModelHelpers.Team.CreateTeam(economyId, teamName, memberList.ToArray());
                    //client.CreateTeam(economyId, teamName, memberList);
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}