using Elsa;
using Elsa.Activities.Signaling.Models;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;

namespace UserTask.AddOns;

/// <summary>
/// Suspends workflow execution until the specified signal is received.
/// </summary>
[Trigger(
    Category = "UserTasks",
    Description = "Suspend workflow execution until the specified signal is received.",
    Outcomes = new[] { Previous, Next}
)]
public class UserTaskSignal : Activity
{
    private const string Previous = "Previous";
    private const string Next = "Next";
        
    [ActivityInput(Hint = "The name of the signal to wait for.",
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
    public string Signal { get; set; } = default!;

    [ActivityOutput(Hint = "The input that was received with the signal.")]
    public object SignalInput { get; set; }

    [ActivityInput(
        Hint = "The task name",
        Category = "Task"
    )]
    public string TaskName { get; set; }

    [ActivityInput(
        Hint = "The title of the task that needs to be executed.",
        Category = "Task"
    )]
    public string TaskTitle { get; set; }

    [ActivityInput(
        Hint = "The description of the task that needs to be executed.",
        UIHint = ActivityInputUIHints.MultiLine,
        Category = "Task"
    )]
    public string TaskDescription { get; set; }

    [ActivityInput(
        Hint = "The definition of the data expected to be returned",
        UIHint = ActivityInputUIHints.MultiLine,
        Category = "Task",
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
    public string UIDefinition { get; set; }

    [ActivityInput(
        Hint = "Allows user to go to previous task",
        UIHint = ActivityInputUIHints.Checkbox,
        Category = "Task",
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
    public bool AllowPrevious { get; set; }
        
    [ActivityOutput] public object Output { get; set; }

    protected override bool OnCanExecute(ActivityExecutionContext context)
    {
        if (context.Input is Signal triggeredSignal)
            return string.Equals(triggeredSignal.SignalName, Signal, StringComparison.OrdinalIgnoreCase);
        return false;
    }

    protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context) =>
        context.WorkflowExecutionContext.IsFirstPass ? OnResume(context) : Suspend();

    protected override IActivityExecutionResult OnResume(ActivityExecutionContext context)
    {
        var triggeredSignal = context.GetInput<Signal>()!;
        SignalInput = triggeredSignal.Input;
        var input = triggeredSignal.Input.ConvertTo<SignalInput>();
        Output = input.Input;
        AddOrUpdateMetadata(context, Output);

        if (AllowPrevious && input.GoToPrevious)
        {
            return Outcome(Previous);
        }
        context.LogOutputProperty(this, nameof(Output), Output);
        return Outcome(Next, Output);
    }

    private void AddOrUpdateMetadata(ActivityExecutionContext context, object data)
    {
        if (context.WorkflowInstance.Metadata.ContainsKey(context.ActivityId))
        {
            context.WorkflowInstance.Metadata[context.ActivityId] = data;
            return;
        }

        context.WorkflowInstance.Metadata.Add(context.ActivityId, data);
    }
}

public class SignalInput
{
    public bool GoToPrevious { get; set; }
    public object Input { get; set; }
}