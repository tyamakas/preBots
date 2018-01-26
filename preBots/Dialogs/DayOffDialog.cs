using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace preBots.Dialogs
{
    [Serializable]
    public class DayOffDialog : IDialog<string>
    {
        private List<string> menuList = new List<string>() { "全休", "午前休", "午後休", "キャンセル" };
        private string date;
        private string kind;
        private string reason;

        public Task StartAsync(IDialogContext context)
        {
            DateMenu(context);
            return Task.CompletedTask;
        }

        private void DateMenu(IDialogContext context)
        {
            List<string> daysList = GetDaysList();
            PromptDialog.Choice(context, SelectDate, daysList, "休暇を取得する日を選択してください。");
        }

        private async Task SelectDate(IDialogContext context, IAwaitable<object> result)
        {
            var selectedDate = await result;
            context.UserData.SetValue("dayoffDate", selectedDate);
            date = selectedDate.ToString();
            if (!date.Equals("キャンセル"))
            {
                KindMenu(context);
            }
            else
            {
                context.Done(date);
            }
        }

        private void KindMenu(IDialogContext context)
        {
            PromptDialog.Choice(context, SelectKind, menuList, "休暇種別を選択してください。");
        }

        private async Task SelectKind(IDialogContext context, IAwaitable<object> result)
        {
            var selectedKind = await result;
            context.UserData.SetValue("dayoffKind", selectedKind);
            kind = selectedKind.ToString();
            if (!kind.Equals("キャンセル"))
            {
                await context.PostAsync("休暇理由を入力してください。");
                context.Wait(DayOffReason);
            }
            else
            {
                context.Done(kind);
            }
        }

        private async Task DayOffReason(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var insertReason = await result;
            context.UserData.SetValue("dayoffReason", insertReason);
            reason = insertReason.Text;
            reason = $"{date}、{kind}の申請を受け付けました。(休暇理由：{insertReason.Text})";
            context.Done(reason);
        }

        private List<string> GetDaysList()
        {
            DateTime today = DateTime.Now;
            List<string> daysList = new List<string>();

            for (int i = 0; i < 7; i++)
            {
                daysList.Add(today.AddDays(i).ToShortDateString());
            }
            daysList.Add("キャンセル");

            return daysList;
        }
    }
}