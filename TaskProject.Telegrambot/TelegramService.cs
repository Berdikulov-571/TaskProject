using System.Net;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace TaskProject.Telegrambot
{
    public static ITelegramBotClient _botClient;

    public static class TelegramService
    {
        public static async Task Main()
        {
            _botClient = new TelegramBotClient("6530285040:AAFOZe4HLj89L-yHVkyJLKrtqO9H2PSpxx4");

            _botClient.OnMessage += Bot_OnMessage;

            _botClient.StartReceiving();

            Console.ReadLine();

            //using (HttpClient client = new HttpClient())
            //{
            //    var response = await client.GetByteArrayAsync(@"https://localhost:44369/media//videos//FILE_1dd5bf84-d1c9-4714-a594-791c96832dd3.mp4");

            //    System.IO.File.WriteAllBytes("D:\\Teste\\test.mp4", response);
            //}
        }


    }
}
