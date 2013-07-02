<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    About ASP.NET MVC Sokoban
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>About ASP.NET MVC Sokoban</h2>
    <p>
        This was a little project that I wrote for fun. I got the idea from <a href="http://www.rubyquiz.com/quiz5.html">RubyQuiz #5</a>
        (also where I got the levels from--I didn't write those myself). I thought it would be an amusing way to
        get a little more experience with .NET MVC while taking a break from <a href="http://www.aspnetmvcblog.com">www.aspnetmvcblog.com/blog/</a>.
    </p>
    <p>
        There's lots of possible improvements here--not having to start on the first screen every time or being able to 
        go backwards to earlier screens are two that leap out at me. There's also a couple of bugs that I'm aware of:
        <ul>
            <li>
                If you hold down the arrow keys to move fast, your worker will sometime "flicker" backward a step or two.
                I suspect a cheap fix would be binding keyboard movement on keyup instead of keydown, so you would only move one step at
                a time, but I decided it's fun to race around, even if you sometimes flicker.
            </li>
            <li>
                Normally, the game will fit within a browser window without requiring scrollbars. If you're playing on a small screen
                (perhaps a mobile unit) such that the browser has scrollbars, the arrow keys will navigate the browser
                window within the scrollbars at the same time you're moving your worker. Not very playable. A workaround 
                for this problem would be using WASD or vim keybindings instead of the arrow keys.
            </li>
        </ul>
    </p>
    <p>
        If you've got questions, or suggestions, or bug reports, I'd be happy to hear them.
        Drop me a note at matt@wichitaprogrammer.com.
    </p>
</asp:Content>
