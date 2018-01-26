using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
//話題をプロじぇぃと
namespace preBots.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        string name;
        string getWorktimes;
        string leftWorktimes;
        string dayOffInfo;
        string listAttend;
        List<string> mainMenuList = new List<string>() { "出社", "退社", "休暇申請", "勤怠確認", "終了" };
        List<string> menuList = new List<string>() { "全休", "午前休", "午後休", "キャンセル"};
        public Task StartAsync(IDialogContext context)  //必ずStartAsyncから
        {
            context.Wait(ListenName); // 次の呼び出し・・StateService（情報保持）Mapみたいなもの
            // StateServiceへの保存
            // context.UserData.SetValue("<キー名>", "<保存したいデータ>")
            // 読み出し
            // string value;
            // context.UserData.TryGetValue("<キー名>", out value);

            return Task.CompletedTask;
        }

        private async Task ListenName(IDialogContext context, IAwaitable<object> result)
        {
            var text = await result as Activity;
            name = (text.Text ?? string.Empty);
            if (!name.Equals(string.Empty))
            {
                await context.PostAsync($"{name} さん、用件をお選びください。");
                //context.Wait(MessageReceivedAsync);
                MainMenu(context);
            }
        }

        private void MainMenu(IDialogContext context)
        {
            PromptDialog.Choice(context, SelectMenuDialog, mainMenuList, "用件を教えてください。");
        }

        private async Task SelectMenuDialog(IDialogContext context, IAwaitable<object> result)
        {
            var selectedMenu = await result;
            switch (selectedMenu)
            {
                case "出社":
                    context.Call(new GetWorkDialog(), GetWorkAfterDialog);
                    break;
                case "退社":
                    context.Call(new LeftWorkDialog(), LeftWorkAfterDialog);
                    break;
                case "休暇申請":
                    context.Call(new DayOffDialog(), DayOffAfterDialog);
                    break;
                case "勤怠確認":
                    //context.Call(new AttendConfirmDialog(), AttendConfirmAfterDialog);
                    break;
                case "終了":
                    await context.PostAsync("さようなら");
                    context.Wait(ListenName);
                    break;
            }
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            
            //int length = (activity.Text ?? string.Empty).Length;
            if (activity.Text.Equals($"hello"))
            {
                getWorktimes = GetNowTime();
                context.UserData.SetValue("getCompTime", getWorktimes);

                await context.PostAsync($"おはようございます。");
                await context.PostAsync($"{name} さん、{getWorktimes} に出社を確認しました。");
                context.Wait(MessageReceivedAsync);
                //context.UserData.get
            }
            else if (activity.Text.Equals($"bye"))
            {
                getWorktimes = GetNowTime();
                context.UserData.SetValue("leaveCompTime", getWorktimes);
                await context.PostAsync($"{name} さん、{getWorktimes} に退社を確認しました。");
                context.Wait(MessageReceivedAsync);
                // return our reply to the user
                //await context.PostAsync($"You sent {activity.Text} which was {length} characters");
            }
            else
            {
                PromptDialog.Choice(context, RestRegist2Async, this.menuList, $"休暇種別を選択してください。");
                //PromptDialog.Confirm(
                //    context,
                //    RestRegistAsync,
                //    $"別の幼児がありますか ?",
                //    "Didn't get that!",
                //    promptStyle: PromptStyle.Auto);
            }
           
            
        }

        private async Task RestRegistAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                //PromptDialog.Choice(
                //    context,
                //    RestRegist2Async,
                //    new[]
                //    {
                //        "全休","午前休","午後休",
                //    },
                //    "休暇の種別は何ですか ?");
            }
            context.Wait(MessageReceivedAsync);
        }

        private async Task RestRegist2Async(IDialogContext context, IAwaitable<string> result)
        {
            var rest = await result;
            
            await context.PostAsync($"了解しました。{rest}ですね。");
            context.Wait(MessageReceivedAsync);
            //await context.PostAsync($"{name} さん、本日{rest} を確認しました。");
        }

        private async Task GetWorkAfterDialog(IDialogContext context, IAwaitable<string> result)
        {
            getWorktimes = await result;
            await context.PostAsync($"{name} さん、{getWorktimes} に出社を確認しました。");
            MainMenu(context);
        }

        private async Task LeftWorkAfterDialog(IDialogContext context, IAwaitable<string> result)
        {
            leftWorktimes = await result;
            await context.PostAsync($"{name} さん、{leftWorktimes} に退社を確認しました。");
            MainMenu(context);
        }

        private async Task DayOffAfterDialog(IDialogContext context, IAwaitable<string> result)
        {
            dayOffInfo = await result;
            await context.PostAsync($"{name}さん、{dayOffInfo}");
            MainMenu(context);
        }

        private List<string> GetDaysList()
        {
            DateTime today = DateTime.Now;
            List<string> daysList = new List<string>();

            for (int i=0; i < 7;i++)
            {
                daysList.Add(today.AddDays(i).ToShortDateString());
            }
            return daysList;
        }

        public string GetNowTime()
        {
            DateTime dtNow = DateTime.Now;
            string dttime = dtNow.Hour.ToString() + ":" + dtNow.Minute.ToString();

            return dttime;
        }
    }
}