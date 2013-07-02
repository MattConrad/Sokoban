<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.hotkeys.js"></script>
    <script type="text/javascript">
        function setUpHotKeys() {
            jQuery(document).bind('keydown', 'left', function (evt) { ajaxHandleMove('w'); });
            jQuery(document).bind('keydown', 'up', function (evt) { ajaxHandleMove('n'); });
            jQuery(document).bind('keydown', 'right', function (evt) { ajaxHandleMove('e'); });
            jQuery(document).bind('keydown', 'down', function (evt) { ajaxHandleMove('s'); });
            jQuery(document).bind('keydown', 'a', function (evt) { ajaxHandleMove('w'); });
            jQuery(document).bind('keydown', 'w', function (evt) { ajaxHandleMove('n'); });
            jQuery(document).bind('keydown', 'd', function (evt) { ajaxHandleMove('e'); });
            jQuery(document).bind('keydown', 's', function (evt) { ajaxHandleMove('s'); });
            jQuery(document).bind('keydown', 'h', function (evt) { ajaxHandleMove('w'); });
            jQuery(document).bind('keydown', 'k', function (evt) { ajaxHandleMove('n'); });
            jQuery(document).bind('keydown', 'l', function (evt) { ajaxHandleMove('e'); });
            jQuery(document).bind('keydown', 'j', function (evt) { ajaxHandleMove('s'); });
            jQuery(document).bind('keydown', 'r', function (evt) { ajaxHandleMove('r'); });
            jQuery(document).bind('keydown', 'c', function (evt) { nextLevel(); });
        }

        function ajaxHandleMove(moveAction) {
            $("#gameboard").load("/Home/AjaxMoveHandler/", { GameAction: moveAction });
        }

        function nextLevel() {
            if (window.nextlevelform)
                window.nextlevelform.submit();
        }

        jQuery(document).ready(setUpHotKeys);
    </script>
    <div id="gameboard">
        <% Html.RenderPartial("Gameboard"); %>
    </div>
</asp:Content>
