/*
Sample Code is provided for the purpose of illustration only and is not intended to be used in a production environment.
THIS SAMPLE CODE AND ANY RELATED INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
We grant You a nonexclusive, royalty-free right to use and modify the Sample Code and to reproduce and distribute the object code form of the Sample Code, provided that. 
You agree: 
	(i) to not use Our name, logo, or trademarks to market Your software product in which the Sample Code is embedded;
    (ii) to include a valid copyright notice on Your software product in which the Sample Code is embedded; and
	(iii) to indemnify, hold harmless, and defend Us and Our suppliers from and against any claims or lawsuits, including attorneys’ fees, that arise or result from the use or distribution of the Sample Code
**/

// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace know
{
    public static class TopWords
    {
        [FunctionName("TopWords")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("TopWords function processed a request.");       
            // code does not care (yet) for missing values, rather assume request payload has the required elements.
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // log.LogInformation($"TopWordsgot payload: {requestBody}.");       
            dynamic body = JsonConvert.DeserializeObject(requestBody);
            dynamic values = body.values;
            dynamic response = new System.Dynamic.ExpandoObject();
            List<dynamic> resValues = new List<dynamic>();
            
            // log.LogInformation($"values:{values[0].data.text}");
            foreach(dynamic item in values)
            {
                string text = item.data.mergedText;    
                word_list list = get_top_ten_words(text);
                dynamic response_item = new System.Dynamic.ExpandoObject();
                response_item.recordId = item.recordId;
                response_item.data = list;
                resValues.Add(response_item);
            }
            response.values = resValues;
            
            
            // // // prep response
            string responseMessage = $"{JsonConvert.SerializeObject(response)}";

            return new HttpResponseMessage(HttpStatusCode.OK) {
                  Content = new StringContent(responseMessage, Encoding.UTF8, "application/json")
            };
            
        }

        public static word_list get_top_ten_words(string text) 
        {

            // convert to lowercase
            text = text.ToLowerInvariant();
            List<string> words = text.Split(' ').ToList();

            //remove any non alphabet characters
            var onlyAlphabetRegEx = new Regex(@"^[A-z]+$");
            words = words.Where(f => onlyAlphabetRegEx.IsMatch(f)).ToList();

            // Array of stop words to be ignored
            string[] stopwords = { "", "i", "me", "my", "myself", "we", "our", "ours", "ourselves", "you", 
                "youre", "youve", "youll", "youd", "your", "yours", "yourself", 
                "yourselves", "he", "him", "his", "himself", "she", "shes", "her", 
                "hers", "herself", "it", "its", "itself", "they", "them", "thats",
                "their", "theirs", "themselves", "what", "which", "who", "whom", 
                "this", "that", "thatll", "these", "those", "am", "is", "are", "was",
                "were", "be", "been", "being", "have", "has", "had", "having", "do", 
                "does", "did", "doing", "a", "an", "the", "and", "but", "if", "or", 
                "because", "as", "until", "while", "of", "at", "by", "for", "with", 
                "about", "against", "between", "into", "through", "during", "before", 
                "after", "above", "below", "to", "from", "up", "down", "in", "out", 
                "on", "off", "over", "under", "again", "further", "then", "once", "here", 
                "there", "when", "where", "why", "how", "all", "any", "both", "each", 
                "few", "more", "most", "other", "some", "such", "no", "nor", "not", 
                "only", "own", "same", "so", "than", "too", "very", "s", "t", "can", 
                "will", "just", "don", "dont", "should", "shouldve", "now", "d", "ll",
                "m", "o", "re", "ve", "y", "ain", "aren", "arent", "couldn", "couldnt", 
                "didn", "didnt", "doesn", "doesnt", "hadn", "hadnt", "hasn", "hasnt", 
                "havent", "isn", "isnt", "ma", "mightn", "mightnt", "mustn", 
                "mustnt", "needn", "neednt", "shan", "shant", "shall", "shouldn", "shouldnt", "wasn", 
                "wasnt", "weren", "werent", "won", "wont", "wouldn", "wouldnt"}; 
            words = words.Where(x => !stopwords.Contains(x)).ToList();

            // Find distict keywords by key and count, and then order by count.
            var keywords = words.GroupBy(x => x).OrderByDescending(x => x.Count());
            var klist = keywords.ToList();
            var numofWords = 10;
            if(klist.Count<10)
                numofWords=klist.Count;
            
            // Return the first 10 words
            List<string> resList = new List<string>();
            for (int i = 0; i < numofWords; i++)
            {
                resList.Add(klist[i].Key);
            }

            // Construct object for results
            word_list json_result = new word_list();
            json_result.top_words = resList;

            // return the results object
            return json_result;
        }
    }

    // class for results
   public class word_list
    {
        public List<string> top_words {get; set;}
    }

    // public class SkillContent
    // {
    //     [JsonProperty("recordId")]
    //     public string recordId {get;set;}
    //     [JsonProperty("data")]
    //     public SkillData data {get;set;}
    // } 

    // public class SkillData
    // {
    //     [JsonProperty("text")]
    //     public string text {get;set;}
    // }
   
}
