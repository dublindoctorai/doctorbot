#load "StopwordTool.csx"

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
        await context.PostAsync($"Are you sick? What are your symptoms?"); //
        context.Wait(MessageReceived);
    }
    [LuisIntent("Death")]
    public async Task DeathIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"Don't worry, you won't die, you're going to be fine."); //
        await context.PostAsync($"Please tell me your symptoms and how long you have had them"); //
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
        string output_symptoms = String.Join(", ", symptoms);
        var output_lengths = String.Join(", ", lengths);
        if (symptoms.Count==0) {
            await context.PostAsync($"You may be high priority, but I didn't understand your symptoms properly."); //
            await context.PostAsync($"Please tell me your symptoms and how long you have had them"); //
        } else {
            if (lengths.Count==0) {
                await context.PostAsync($"You are high priority, go to the hospital now."); //
                await context.PostAsync($"Your symptoms are : {output_symptoms}."); //
                if(getProbableCondition(output_symptoms)!= ""){
                    await context.PostAsync($"It is possible you have one of the following conditions: {getProbableCondition(output_symptoms)}");
                }
            }  else {
                await context.PostAsync($"You are high priority, go to the hospital now."); //
                await context.PostAsync($"Your symptoms are : {output_symptoms} and you have had them for {output_lengths}"); //
                if(getProbableCondition(output_symptoms)!= ""){
                    await context.PostAsync($"It is possible you have one of the following conditions: {getProbableCondition(output_symptoms)}");
                }
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
        string output_symptoms = String.Join(", ", symptoms);
        var output_lengths = String.Join(", ", lengths);
        if (symptoms.Count==0) {
            await context.PostAsync($"You may be medium priority, but I didn't understand your symptoms properly."); //
            await context.PostAsync($"Please tell me your symptoms and how long you have had them"); //
        } else {
            if (lengths.Count==0) {
                await context.PostAsync($"You are medium priority, go to the hospital when ready."); //
                await context.PostAsync($"Your symptoms are : {output_symptoms}."); //
                if(getProbableCondition(output_symptoms)!= ""){
                    await context.PostAsync($"It is possible you have one of the following conditions: {getProbableCondition(output_symptoms)}");
                }
            }  else {
                await context.PostAsync($"You are medium priority, go to the hospital when ready."); //
                await context.PostAsync($"Your symptoms are : {output_symptoms} and you have had them for {output_lengths}"); //
                if(getProbableCondition(output_symptoms)!= ""){
                    await context.PostAsync($"It is possible you have one of the following conditions: {getProbableCondition(output_symptoms)}");
                }
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
        string output_symptoms = String.Join(", ", symptoms);
        var output_lengths = String.Join(", ", lengths);
        if (symptoms.Count==0) {
            await context.PostAsync($"You may be able to get treated at a pharmacy, but I didn't understand your symptoms properly."); //
            await context.PostAsync($"Please tell me your symptoms and how long you have had them"); //
        } else {
            if (lengths.Count==0) {
                await context.PostAsync($"You may be able to get treated at a pharmacy."); //
                await context.PostAsync($"Your symptoms are : {output_symptoms}."); //
                if(getProbableCondition(output_symptoms)!= ""){
                    await context.PostAsync($"It is possible you have one of the following conditions: {getProbableCondition(output_symptoms)}");
                }
            }  else {
                await context.PostAsync($"You may be able to get treated at a pharmacy."); //
                await context.PostAsync($"Your symptoms are : {output_symptoms} and you have had them for {output_lengths}"); //
                if(getProbableCondition(output_symptoms)!= ""){
                    await context.PostAsync($"It is possible you have one of the following conditions: {getProbableCondition(output_symptoms)}");
                }
            }
        }
        
        context.Wait(MessageReceived);
    }
    
        public string getProbableCondition(string symptoms){
        Dictionary<string, List<string>> conditionLists = new Dictionary<string, List<string>>
        {
            {"sore", new List<string> {"cold","flu"}},
            {"blocked", new List<string> {"cold","flu"}},
            {"bloated", new List<string> {"constipation","dehydration","bowel obstraction", "infection"}},
            {"sneezing", new List<string> {"cold","allergies"}},
            {"cough", new List<string> {"cold","flu","asthma","infection","bronchitis","allergies"}},
            {"hoarse", new List<string> {"cold","allergies","smoking"}},
            {"weak", new List<string> {"cold"}},
            {"drowsy", new List<string> {"head injury","concussion","migraine"}},
            {"fever", new List<string> {"chicken pox","flu","Heat stroke"}},
            {"appetite", new List<string> {"chicken pox"}},
            {"headache", new List<string> {"chicken pox","flu","migraine","stress","alcohol","lack of sleep","tension headache","acute sinusitis","caffeine withdrawl"}},
            {"rash", new List<string> {"chicken pox"}},
            {"aching", new List<string> {"flu","angina"}},
            {"tired", new List<string> {"flu","angina"}},
            {"vomiting ", new List<string> {"flu","concussion","ingestion of toxins","alcohol"}},
            {"diarrhoea", new List<string> {"flu","virus","parasite","irritable bowel syndrome"}},
            {"chest", new List<string> {"angina","heart attack","heartburn","costochondritis","panic attack","anxiety"}},
            {"nausea", new List<string> {"angina","flu","food poisoning","inner-ear disease"}},
            {"breathless", new List<string> {"angina","asthma","bronchitis","pneumonia","arousal"}},
            {"sweating", new List<string> {"angina","diabetes","anxiety","malaria","Tuberculosis"}},
            {"dizzy", new List<string> {"angina","low iron levels","low suger levels","anxiety","dehydration"}},
            {"exhausted", new List<string> {"AI Hackathon"}},
            {"itchy", new List<string> {"piles","parasite","herpes"}},
            {"bleeding", new List<string> {"trauma"}},
            {"blackout", new List<string> {"medication reaction","short-acting sedative","epilepsy","intoxication","transient global amnesia"}},
            {"bruised", new List<string> {"bruise","hematoma","trauma","botox injection","thrombocytopenia"}},
            {"delusions", new List<string> {"alcohol withdrwal","cocain abuse","medical reaction"}}

        };
        
        string strippedSymptoms = StopwordTool.RemoveStopwords(symptoms);
        List<string> probabilityList = new List<string>();
        string[] splitSymptoms = strippedSymptoms.Split(' ');
        
        for(int i = 0;i<splitSymptoms.Length;i++){
            if (conditionLists.ContainsKey(splitSymptoms[i]))
            {
            foreach (string str in conditionLists[splitSymptoms[i]])
                {
                    if (!probabilityList.Contains(str))
                        probabilityList.Add(str);
                }
               //probabilityList.AddRange(conditionLists[splitSymptoms[i]]);
            }
        }
        
        string conditions = string.Join(", ", probabilityList.ToArray());
        return conditions;
    }
    
}
