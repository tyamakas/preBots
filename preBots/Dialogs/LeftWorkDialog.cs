using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using preBots.Models;

namespace preBots.Dialogs
{
    [Serializable]
    public class LeftWorkDialog : IDialog<string>
    {
        public Task StartAsync(IDialogContext context)
        {
            string times = GetNowTime();
            context.UserData.SetValue("leftCompTime", times);

            //context.Done($"{times} に退社を確認しました。");
            context.Done(times);
            return Task.CompletedTask;
        }

        public string GetNowTime()
        {
            DateTime dtNow = DateTime.Now;
            string dttime = dtNow.Hour.ToString() + ":" + dtNow.Minute.ToString();

            return dttime;

        }
    }
}