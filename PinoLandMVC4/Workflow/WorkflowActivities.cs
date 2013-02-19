using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

//using System.Data.EntityClient;
//using System.Data.Objects;
//using System.Data.SqlClient;

//using MG = Fuqua.CompetativeAnalysis.MarketGame;
//using Microsoft.Samples.EntityDataReader;

namespace PinoLandMVC4.Workflow
{
    public class StartGame : CodeActivity<int>
    {
        protected override int Execute(CodeActivityContext context)
        {
            throw new NotImplementedException();
        }
    }


    public class InitializeGame : CodeActivity<int>
    {
        protected override int Execute(CodeActivityContext context)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class ProcessRoundActivity : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<int> EconomyId { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext activityContext)
        {
            int economyId = activityContext.GetValue(this.EconomyId);
            PinoLandMVC4.Controllers.ManagerController controller = new Controllers.ManagerController();
            controller.ProcessRoundInternal(economyId);
        }

    }
}
