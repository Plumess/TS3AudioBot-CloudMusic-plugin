using System;
using TS3AudioBot.CommandSystem;
using YunPlugin.api;
using YunPlugin.api.netease;
using YunPlugin.utils;

static class Program
{
    static void Check(string input, MusicUrlType type, string id)
    {
        var parsed = NeteaseMusic.ParseInputData(Utils.RemoveBBCode(input));
        if (parsed.Type != type || parsed.Id != id)
            throw new Exception($"Unexpected result for {input}: {parsed}");
    }

    static void Reject(string input)
    {
        try { NeteaseMusic.ParseInputData(Utils.RemoveBBCode(input)); }
        catch (CommandException) { return; }
        throw new Exception($"Expected invalid link to be rejected: {input}");
    }

    static void Main()
    {
        Check("https://music.163.com/song?id=3320420574&uct2=copy", MusicUrlType.Music, "3320420574");
        Check("https://music.163.com/#/song?id=3320420574", MusicUrlType.Music, "3320420574");
        Check("http://music.163.com/song?id=3320420574", MusicUrlType.Music, "3320420574");
        Check("https://music.163.com/playlist?id=123", MusicUrlType.PlayList, "123");
        Check("https://music.163.com/#/album?id=456", MusicUrlType.Album, "456");
        Check("3320420574", MusicUrlType.Number, "3320420574");
        Check("[歌曲](https://music.163.com/song?id=42\\&uct2=copy)", MusicUrlType.Music, "42");
        Check("[url=https://music.163.com/song?id=42]歌曲[/url]", MusicUrlType.Music, "42");
        Check("https://music.163.com/song?id=42\u00a0", MusicUrlType.Music, "42");
        Reject("https://music.163.com/song?id=abc");
        Reject("https://music.163.com/song?foo=id=42");
        Reject("https://music.163.com/song?id=0");
        Reject("https://music.163.com/#/song");
        Console.WriteLine("Link parsing checks passed");
    }
}
