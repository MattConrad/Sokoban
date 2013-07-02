<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<!-- don't escape the GameHTML, it's got HTML in it.  :) -->
<div id="playarea">
    <%= ViewData["GameHTML"] %>
</div>
<p>
    Moves: <%= ViewData["Moves"] %>
</p>

<% if ((int)ViewData["FinishedInMoves"] != 0)
   { %>
        <form id="nextlevelform" action="/Home/NextLevel/" method="post">
            <p>Finished!</p>
            <p>Click <input type="submit" name="btnContinue" value="Continue" /> or press 'c' to move to the next level.</p>
        </form>
<% }
   else
   { %>
       <p>Instructions:</p>
       <p>Use your worker (@) to push all the crates (o) onto the storage spaces (.) without getting any
       crates stuck.</p>
       <p>Use the arrow keys (or WASD) to move your worker.</p>
       <p>If you get a crate stuck, press 'r' to restart the level.</p>
<% } %>
