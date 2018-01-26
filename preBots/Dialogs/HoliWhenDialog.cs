using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace preBots.Dialogs
{
    [Serializable]
    public class HoliWhenDialog
    {
        private DateTime today = DateTime.Now;
        List<string> menuList = new List<string>() { "全休", "午前休", "午後休", "キャンセル" };
        private List<string> dayList = new List<string>();
        private async Task SelectDays(IDialogContext context, IAwaitable<bool> confirm)
        {
            var answer = await confirm;

            if (answer)
            {
              //  PromptDialog.Choice(context, )
            }
        }
    }
}