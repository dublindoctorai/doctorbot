using System;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

// For more information about this template visit http://aka.ms/azurebots-csharp-luis
//[LuisModel("1c962f60-4db1-449e-90a6-78f6840bbfdf", "d2b50391cf184e7089d258ffe3ac9703")]
[Serializable]
public class BasicLuisDialog : LuisDialog<object>
{
    public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(Utils.GetAppSetting("LuisAppId"), Utils.GetAppSetting("LuisAPIKey"))))
    {
    }

    [LuisIntent("None")]
    public async Task NoneIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"I didn't understand that. You said: {result.Query}"); //
        await context.PostAsync($"Please tell me your symptoms and how long you have had them"); //
        context.Wait(MessageReceived);
    }
    [LuisIntent("Hello")]
    public async Task HelloIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"Hello, are you sick? What are your symptoms?"); //
        context.Wait(MessageReceived);
    }


    // Go to https://luis.ai and create a new intent, then train/publish your luis app.
    // Finally replace "MyIntent" with the name of your newly created intent in the following handler
    [LuisIntent("High priority")]
    public async Task Hp(IDialogContext context, LuisResult result)
    { 
        var symptoms = new List<string>();
        var lengths = new List<string>();
        foreach (var entity in result.Entities)
        {
            if (entity.Type=="Symptom") {
                symptoms.Add(entity.Entity);
            } else if (entity.Type=="how long") {
                lengths.Add(entity.Entity);
            }
        }
        var output_symptoms = String.Join(", ", symptoms);
        var output_lengths = String.Join(", ", lengths);
        if (symptoms.Count==0) {
            await context.PostAsync($"You may be high priority, but I didn't understand your symptoms properly."); //
            await context.PostAsync($"Please tell me your symptoms and how long you have had them"); //
        } else {
            if (lengths.Count==0) {
                await context.PostAsync($"You are high priority, go to the hospital now."); //
                await context.PostAsync($"Your symptoms are : {output_symptoms}."); //
            }  else {
                await context.PostAsync($"You are high priority, go to the hospital now."); //
                await context.PostAsync($"Your symptoms are : {output_symptoms} and you have had them for {output_lengths}"); //
            }
        }

        context.Wait(MessageReceived);
    }
    [LuisIntent("Medium priority")]
    public async Task Mp(IDialogContext context, LuisResult result)
    {
        var symptoms = new List<string>();
        var lengths = new List<string>();
        foreach (var entity in result.Entities)
        {
            if (entity.Type=="Symptom") {
                symptoms.Add(entity.Entity);
            } else if (entity.Type=="how long") {
                lengths.Add(entity.Entity);
            }
        }
        var output_symptoms = String.Join(", ", symptoms);
        var output_lengths = String.Join(", ", lengths);
        if (symptoms.Count==0) {
            await context.PostAsync($"You may be medium priority, but I didn't understand your symptoms properly."); //
            await context.PostAsync($"Please tell me your symptoms and how long you have had them"); //
        } else {
            if (lengths.Count==0) {
                await context.PostAsync($"You are medium priority, go to the hospital when ready."); //
                await context.PostAsync($"Your symptoms are : {output_symptoms}."); //
            }  else {
                await context.PostAsync($"You are medium priority, go to the hospital when ready."); //
                await context.PostAsync($"Your symptoms are : {output_symptoms} and you have had them for {output_lengths}"); //
            }
        }
       
        context.Wait(MessageReceived);
    }
    [LuisIntent("Low priority")]
    public async Task Lp(IDialogContext context, LuisResult result)
    {
        var symptoms = new List<string>();
        var lengths = new List<string>();
        foreach (var entity in result.Entities)
        {
            if (entity.Type=="Symptom") {
                symptoms.Add(entity.Entity);
            } else if (entity.Type=="how long") {
                lengths.Add(entity.Entity);
            }
        }
        var output_symptoms = String.Join(", ", symptoms);
        var output_lengths = String.Join(", ", lengths);
        if (symptoms.Count==0) {
            await context.PostAsync($"You may be able to get treated at a pharmacy, but I didn't understand your symptoms properly."); //
            await context.PostAsync($"Please tell me your symptoms and how long you have had them"); //
        } else {
            if (lengths.Count==0) {
                await context.PostAsync($"You may be able to get treated at a pharmacy."); //
                await context.PostAsync($"Your symptoms are : {output_symptoms}."); //
            }  else {
                await context.PostAsync($"You may be able to get treated at a pharmacy."); //
                await context.PostAsync($"Your symptoms are : {output_symptoms} and you have had them for {output_lengths}"); //
            }
        }
        
        await context.PostAsync($"You may be able to get treated at a pharmacy."); //
        await context.PostAsync($"Your symptoms are: {output_symptoms} and you have had them for {output_lengths}"); //
        context.Wait(MessageReceived);
    }
}
