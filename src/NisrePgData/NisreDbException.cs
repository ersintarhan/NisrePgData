using System;
using System.Collections.Generic;
using Npgsql;

namespace NisrePgData
{
    public class NisreDbException : Exception
    {
        
        public bool CustomException { get; set; }
        public Dictionary<string,string> ExceptionData = new Dictionary<string, string>();
        public override string Message { get;}
        
        
        public NisreDbException(NpgsqlException exception)
        {
            if (exception.Data.Count <= 0) return;
            var code = exception.Data["Code"].ToString();
            var message = exception.Data["MessageText"].ToString();
            var where = exception.Data["Where"].ToString();
            var hint = exception.Data["Hint"].ToString();
            ExceptionData.Add("code", code);
            ExceptionData.Add("messageText", message);
            ExceptionData.Add("where", @where);
            ExceptionData.Add("hint", hint);
            if (code == "P0001")
            {
                this.CustomException = true;
                this.Message = message;
            }
            else
            {
                CustomException = false;
                Message = "System Error";
            }
        }

        public NisreDbException(NpgsqlException exception, PgParam parameters)
        {
            if (exception.Data.Count <= 0) return;
            var code = exception.Data["Code"].ToString();
            var message = exception.Data["MessageText"].ToString();
            var where = exception.Data["Where"].ToString();
            var hint = exception.Data["Hint"].ToString();
            ExceptionData.Add("code", code);
            ExceptionData.Add("messageText", message);
            ExceptionData.Add("where", where);
            ExceptionData.Add("hint", hint);
            ExceptionData.Add("parameters",parameters.GetParamatersInfo().ToJson());
            if (code == "P0001")
            {
                this.CustomException = true;
                this.Message = message;
            }
            else
            {
                CustomException = false;
                Message = "System Error";
            }
        }

        public NisreDbException(Exception ex)
        {
            this.CustomException = false;
            this.Message = ex.Message;
        }
    }
}
