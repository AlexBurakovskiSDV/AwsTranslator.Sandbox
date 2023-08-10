using Amazon;
using Amazon.Translate;
using Amazon.Translate.Model;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

public class TranslateText
{
    public static async Task Main()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        string accessKey = configuration["AppSettings:AccessKey"];
        string secretKey = configuration["AppSettings:SecretKey"];

        if (string.IsNullOrWhiteSpace(accessKey))
        {
            WriteError("Please provide accessKey in appSettings.json");
            return;
        }
        if (string.IsNullOrWhiteSpace(secretKey))
        {
            WriteError("Please provide secretKey in appSettings.json");
            return;
        }


        var client = new AmazonTranslateClient(accessKey, secretKey, RegionEndpoint.EUCentral1);

        string srcLang = "en";
        string destLang = "ja";
        string[] srcText = GetSourceTexts();

        Console.WriteLine($"We are running translating: {DateTime.Now.ToLongTimeString()}");

        var total = await TranslatingTextAsync(client, srcLang, destLang, srcText);

        ShowText(total);

        Console.WriteLine($"We have finished translating: {DateTime.Now}");
    }

    private static void WriteError(string text)
    {
        Console.Error.WriteLine(text);
    }

    private static string[] GetSourceTexts()
    {
        return new[] {
            "Looking for true love? Come no further than me - the girl next door sweet and kind who'll bring warmth, passion, joy, and pleasure into your life. I'm both a romantic with dreams of what could be while never naïve to reality; there is always balance in my actions. With all that I have to offer you - from sweetness one day to spice another - know that when you lay down each night by my side it will fill secure knowing my loyalty lies only with you! If tired mornings alone are wearing on your soul then reach out: let's start this beautiful journey together!",
            "Of all the wise and beautiful things, the most beautiful and wisest is love. Of all faiths, the most faithful in love. Except for being a true romantic another important thing about me is that I graduated from the Faculty of Economics and I am dedicated to my profession. I have also always been interested in the social sciences. I would describe myself as a life psychologist - I can recognize all personality types and I can immediately connect and feel if we are compatible.\r\nWhen I love, I love strongly and passionately. The person who is right for me will be my best friend, my lover, my husband, and my soulmate. I imagine he will be the one who makes me laugh, my protector, and my hero! I was never picky, I always fall for the little things, the look and the touch. Searching for a partner I find myself writing this... I am modest, kind, and loving and I hope soon I will find someone who will appreciate that.",
            "There are so many stereotypes about how a \"real woman\" or a \"real man\" should look and behave. But nobody's perfect, we're humans and every each of us is wonderful. I'm a real woman looking for real man, to share life, to love and care about each other in good and bad waves of life. I am sweet, smart, open-minded, joyful, passionate and confident.\r\nIf you want to know more - just ask me! I don't bite!",
            "As a sweet and charming girl, I believe in the power of love and affection. With my hopeless romantic nature, I find joy in expressing my feelings and being open with those around me. As a passionate and affectionate woman, I strive to create meaningful connections with the people in my life.\r\nI am someone who is always open to new experiences and perspectives. My open-mindedness allows me to appreciate the uniqueness in others, as well as in myself. Whether it's trying new foods or exploring new places, I am always up for an adventure.\r\nBesides being a hopeless romantic, I also have a great sense of humor. I find that laughter is one of the best ways to connect with people and create a relaxed atmosphere. Whether it's cracking jokes or being playful, I love to make others smile and feel at ease.\r\nIf you're looking for someone who is sweet, charming, passionate, affectionate and funny, then I am the perfect girl for you. Let's get to know each other and see where this journey takes us.",
            "I am someone who radiates energy and wants to give it to others.About a warm and cozy home next to your beloved man (maybe you are reading these lines now?), about simple happiness, which is comprehended in hormones, love and understanding. I see a lot of good and bad people around me, but I believe that there is something good, warm and real in everyone.",
            "I love my life! She is bright and emotional, just like me. I work as a stewardess, this is the work of every young girl. I am constantly flying. I see a large number of countries and sights. My worries are few and I just enjoy life. I can better not sleep, but get up early and go to explore the new city for me.",
            "I have a full life, but inside I feel lonely. I came here to find peace. My friends wonder how I manage everything, because whatever I take on, I get everything done. Probably because I only do what I love. So I opened my own cosmetics store. I have many hobbies, but the main one is the pursuit of perfection in both the inner world and the physical world . I am not chasing after the material world, comfort for me creates not cars and yachts, but a cozy quiet place with my beloved man",
            "It is very difficult to describe myself, but I'll try for you!)\r\nI never save evil, never envy people and am always happy to help. My thoughts are always positive and I believe in the boomerang rule.\r\nBut sometimes, I look back on my life and I understand that it would be much more colorful if there was a self-sufficient and wise man with me.\r\nSo, I have hope that this site will help me find you, my future happiest husband in the world.",
            "I am a woman like a match, it is easy to light a flame of fire in me. I have a good heart, I am a romantic woman, a loyal, sincere woman. My personality is quiet, stable, introspective, calm, honest. I dream of having a loving family. I consider myself to be good-natured, kind, caring, loving, funny, optimistic, creative and outgoing. My life is happy, but the missing ingredient is sharing love with a real life partner.",
            "🌼 Eternal Optimist with a zest for life's wonders! 🌟 Exploring the world one smile at a time. An avid hiker who finds solace in nature's embrace, and a foodie who loves concocting culinary delights. 🍜🏞️ Let's embark on thrilling adventures and share heartfelt conversations under the stars. Passionate about music, arts, and making memories that last a lifetime. Seeking a partner to join me in crafting a love story that's as unique as we are. Are you up for the journey? 🎶✨ #SeekingAuthenticConnection",
            "Looking for true love? Come no further than me - the girl next door sweet and kind who'll bring warmth, passion, joy, and pleasure into your life. I'm both a romantic with dreams of what could be while never naïve to reality; there is always balance in my actions. With all that I have to offer you - from sweetness one day to spice another - know that when you lay down each night by my side it will fill secure knowing my loyalty lies only with you! If tired mornings alone are wearing on your soul then reach out: let's start this beautiful journey together!",
            "Of all the wise and beautiful things, the most beautiful and wisest is love. Of all faiths, the most faithful in love. Except for being a true romantic another important thing about me is that I graduated from the Faculty of Economics and I am dedicated to my profession. I have also always been interested in the social sciences. I would describe myself as a life psychologist - I can recognize all personality types and I can immediately connect and feel if we are compatible.\r\nWhen I love, I love strongly and passionately. The person who is right for me will be my best friend, my lover, my husband, and my soulmate. I imagine he will be the one who makes me laugh, my protector, and my hero! I was never picky, I always fall for the little things, the look and the touch. Searching for a partner I find myself writing this... I am modest, kind, and loving and I hope soon I will find someone who will appreciate that.",
            "There are so many stereotypes about how a \"real woman\" or a \"real man\" should look and behave. But nobody's perfect, we're humans and every each of us is wonderful. I'm a real woman looking for real man, to share life, to love and care about each other in good and bad waves of life. I am sweet, smart, open-minded, joyful, passionate and confident.\r\nIf you want to know more - just ask me! I don't bite!",
            "As a sweet and charming girl, I believe in the power of love and affection. With my hopeless romantic nature, I find joy in expressing my feelings and being open with those around me. As a passionate and affectionate woman, I strive to create meaningful connections with the people in my life.\r\nI am someone who is always open to new experiences and perspectives. My open-mindedness allows me to appreciate the uniqueness in others, as well as in myself. Whether it's trying new foods or exploring new places, I am always up for an adventure.\r\nBesides being a hopeless romantic, I also have a great sense of humor. I find that laughter is one of the best ways to connect with people and create a relaxed atmosphere. Whether it's cracking jokes or being playful, I love to make others smile and feel at ease.\r\nIf you're looking for someone who is sweet, charming, passionate, affectionate and funny, then I am the perfect girl for you. Let's get to know each other and see where this journey takes us.",
            "I am someone who radiates energy and wants to give it to others.About a warm and cozy home next to your beloved man (maybe you are reading these lines now?), about simple happiness, which is comprehended in hormones, love and understanding. I see a lot of good and bad people around me, but I believe that there is something good, warm and real in everyone.",
            "I love my life! She is bright and emotional, just like me. I work as a stewardess, this is the work of every young girl. I am constantly flying. I see a large number of countries and sights. My worries are few and I just enjoy life. I can better not sleep, but get up early and go to explore the new city for me.",
            "I have a full life, but inside I feel lonely. I came here to find peace. My friends wonder how I manage everything, because whatever I take on, I get everything done. Probably because I only do what I love. So I opened my own cosmetics store. I have many hobbies, but the main one is the pursuit of perfection in both the inner world and the physical world . I am not chasing after the material world, comfort for me creates not cars and yachts, but a cozy quiet place with my beloved man",
            "It is very difficult to describe myself, but I'll try for you!)\r\nI never save evil, never envy people and am always happy to help. My thoughts are always positive and I believe in the boomerang rule.\r\nBut sometimes, I look back on my life and I understand that it would be much more colorful if there was a self-sufficient and wise man with me.\r\nSo, I have hope that this site will help me find you, my future happiest husband in the world.",
            "I am a woman like a match, it is easy to light a flame of fire in me. I have a good heart, I am a romantic woman, a loyal, sincere woman. My personality is quiet, stable, introspective, calm, honest. I dream of having a loving family. I consider myself to be good-natured, kind, caring, loving, funny, optimistic, creative and outgoing. My life is happy, but the missing ingredient is sharing love with a real life partner.",
            "🌼 Eternal Optimist with a zest for life's wonders! 🌟 Exploring the world one smile at a time. An avid hiker who finds solace in nature's embrace, and a foodie who loves concocting culinary delights. 🍜🏞️ Let's embark on thrilling adventures and share heartfelt conversations under the stars. Passionate about music, arts, and making memories that last a lifetime. Seeking a partner to join me in crafting a love story that's as unique as we are. Are you up for the journey? 🎶✨ #SeekingAuthenticConnection"
        };
    }

    public static async Task<long> TranslatingTextAsync(AmazonTranslateClient client, string srcLang, string destLang, string[] text)
    {
        var translationTasks = new List<Task>();
        var sw = new Stopwatch();

        foreach (var item in text)
        {
            var request = new TranslateTextRequest
            {
                SourceLanguageCode = srcLang,
                TargetLanguageCode = destLang,
                Text = item,
            };

            var translationTask = client.TranslateTextAsync(request);
            translationTasks.Add(translationTask);
        }


        sw.Start();
        await Task.WhenAll(translationTasks);
        sw.Stop();

        return sw.ElapsedMilliseconds;
    }

    public static void ShowText(long totalTime)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Total time {totalTime} ms");
    }
}


