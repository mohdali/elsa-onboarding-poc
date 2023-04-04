using Elsa.Activities.Console;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Signaling;
using Elsa.Builders;
using UserTask.AddOns;

namespace ElsaEngine.Workflows
{
    internal class OnBoardingWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            builder
                .StartWith<WriteLine>(s=> s.WithText("In Progress"))
                .WriteLine($"In progress")
                .Then<UserTaskSignal>(a =>
                {
                    a.Set(x => x.Signal, "userDetails");
                    a.Set(x => x.TaskName, "Demo Sample3");
                    a.Set(x => x.TaskTitle, "Dynamic forms");
                    a.Set(x => x.UIDefinition,
                        "Elsa.OnBoardingProcess.PoC.Pages.OnBoarding.UserDetails");
                    a.Set(x => x.AllowPrevious,
                        false);
                    a.Set(x => x.TaskName, "The task will suspend the execution until the button is pressed");
                }, userDetails =>
                {
                    userDetails.When("Next")
                        .Then<UserTaskSignal>(a =>
                        {
                            a.Set(x => x.Signal, "userOnBoard");
                            a.Set(x => x.TaskName, "Demo Sample3");
                            a.Set(x => x.TaskTitle, "Dynamic forms");
                            a.Set(x => x.UIDefinition,
                                "Elsa.OnBoardingProcess.PoC.Pages.OnBoarding.UserOnBoard");
                            a.Set(x => x.AllowPrevious,
                                true);
                            a.Set(x => x.TaskName, "The task will suspend the execution until the button is pressed");
                            a.Set(x => x.Name, "userOnBoard");
                        }, userOnBoard =>
                        {
                            userOnBoard.When("Previous")
                                .ThenNamed("userDetails");

                            userOnBoard.When("Next")
                                .WriteLine("Workflow completed! On Boarding Process Done!");
                        });
                }).WithName("userDetails");
        }
    }
}