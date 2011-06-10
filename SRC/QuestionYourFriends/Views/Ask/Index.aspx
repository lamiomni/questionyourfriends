<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Page d'accueil
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div style="text-align:center;margin:auto;margin-top:30px">
    <fb:serverFbml  width="300px"  >
    <script type="text/fbml">
        <fb:fbml>
            <fb:request-form
            action="http://apps.facebook.com/questionyourfriends/Ask/Ask"
                    target="_top"
                    method="GET"
                    invite="true"
                    type="Demo"
                    content="Hello"
                    label='Accept' >
                <fb:friend-selector name="uid" idname="friend_sel" />
                <fb:editor-textarea name="ask" />
                <label for="annon_cost">Anonymize price</label>
                <input type="text" name="annon_cost" id="annon_cost" value="0"/>
                <label for="private_cost">Privatize price</label>
                <input type="text" name="private_cost" id="private_cost" value="0"/>
                <fb:submit >Poser</fb:submit>
            </fb:request-form>
      </fb:fbml>
    </script>
  </fb:serverFbml>
  </div>
</asp:Content>
