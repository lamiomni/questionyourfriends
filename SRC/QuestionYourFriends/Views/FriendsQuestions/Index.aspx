<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Page d'accueil
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%:ViewData["Message"]%></h2>
    <ul>
    <%
        var questions = (List<QuestionYourFriendsDataAccess.Question>) ViewData["questions"];
        var friends = (Dictionary<long, dynamic>) ViewData["friends"];
        foreach (var question in questions)
        {
            if(!friends.ContainsKey(question.Owner.fid))
                continue;
%>
      <li>
        <h2><%:question.text%></h2>

        <%
            if (question.answer != null)
            {%>
            <h3><%:question.answer%></h3>
        <%
            }%>
        <%
                if (question.anom_price == 0)
                {%>
            <span><%:friends[question.Owner.fid].name%></span>
        <%
                }
                else
                {
%>
            <form method="post" action="FriendsQuestions/Reveal">
                <input type="hidden" name="qid" value="<%:question.id%>" />
                <button>de ???</button>
            </form>
            <%
                }
%>

        <% if(question.Receiver != null) { %>
            <span><%:friends[question.Receiver.fid].name %></span>
        <% } %>
    </li>
    <%
    }
    %>
    </ul>
</asp:Content>
