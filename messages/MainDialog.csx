#r "Newtonsoft.Json"
#load "BasicForm.csx"
#load "StopwordTool.csx"

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

public class Symptom
{
    public string type { get; set; }
}

public class Condition
{
    public string descriptor {get; set;}
    public string duration { get; set; }
    public string priority { get; set; }
    public List<Symptom> symptoms { get; set; }
}

[Serializable]
public class MainDialog : IDialog<BasicForm>
{
    public MainDialog()
    {
    }

    public Task StartAsync(IDialogContext context)
    {
        context.Wait(MessageReceivedAsync);
        return Task.CompletedTask;
    }

    public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var message = await argument;
        context.Call(BasicForm.BuildFormDialog(FormOptions.PromptInStart), FormComplete);
    }

    private async Task FormComplete(IDialogContext context, IAwaitable<BasicForm> result)
    {
        try
        {
            var form = await result;
            if (form != null)
            {
             //   Condition condition = JsonConvert.DeserializeObject<Condition>(File.ReadAllText(@"Sexyfrombot\DocJson.json"));
                await context.PostAsync("Thanks for completing the form! Just type anything to restart it.");
                form.Symptoms = StopwordTool.RemoveStopwords(form.Symptoms);
                await context.PostAsync($"{form.Symptoms}");
            //    await context.PostAsync($"{condition.descriptor}");
            }
            else
            {
                await context.PostAsync("Form returned empty response! Type anything to restart it.");
            }
        }
        catch (OperationCanceledException)
        {
            await context.PostAsync("You canceled the form! Type anything to restart it.");
        }

        context.Wait(MessageReceivedAsync);
    }
}