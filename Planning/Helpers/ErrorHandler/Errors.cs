using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningApi.Helpers
{
    public static class Errors
    {
        public static ModelStateDictionary AddErrorsToModelState(IdentityResult identityResult, ModelStateDictionary modelState)
        {
            foreach (var e in identityResult.Errors)
            {
                modelState.TryAddModelError(e.Code, e.Description);
            }

            return modelState;
        }

        public static ModelStateDictionary AddErrorToModelState(string code, string description, ModelStateDictionary modelState)
        {
            modelState.TryAddModelError(code, description);
            return modelState;
        }

        /// <summary>
        /// error message formated for the API with error code
        /// </summary>
        /// <param name="message">default message to display</param>
        /// <param name="code">error code tu return to the API</param>
        /// <returns>final error message with code</returns>
        public static string ErrorMessageBuilderWithCode(string code, string message)
        {
            string builder = String.Format("{0}***{1}", code, message);
            return builder;
        }

        /// <summary>
        /// Error builder from the ModelState
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static Dictionary<string, List<Dictionary<string, string>>> ErrorBuilder(ModelStateDictionary modelState)
        {
            //Json Final Result
            //var JsonErrors = new Hashtable();
            Dictionary<string, List<Dictionary<string, string>>> JsonErrors = new Dictionary<string, List<Dictionary<string, string>>>();
            //Array of Error Message for one key
            List<Dictionary<string, string>> itemListError = null;
            //Dictionnary of Error Message
            Dictionary<string, string> keyValueError = null;

            foreach (var pair in modelState)
            {
                if (pair.Value.Errors.Count > 0)
                {
                    //errors[pair.Key] = pair.Value.Errors.Select(error => error.ErrorMessage).ToList();
                    itemListError = new List<Dictionary<string, string>>();
                    foreach (var errorMessage in pair.Value.Errors)
                    {
                        keyValueError = new Dictionary<string, string>();

                        if (errorMessage.ErrorMessage.Contains("***"))
                        {
                            var code = errorMessage.ErrorMessage.Split("***")[0];
                            var message = errorMessage.ErrorMessage.Split("***")[1];
                            keyValueError.Add("Code", code);
                            keyValueError.Add("Message", message);
                        }
                        else
                        {
                            keyValueError.Add("Code", "No_Code");
                            keyValueError.Add("Message", errorMessage.ErrorMessage);
                        }

                        //Add the dictionnary of error message to the list for the current Key
                        itemListError.Add(keyValueError);
                    }
                    //register the list of error message to the current Key
                    JsonErrors.Add(pair.Key, itemListError);
                }
            }

            return JsonErrors;
        }


    }
}
