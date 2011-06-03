<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Page d'accueil
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%:ViewData["Message"]%></h2>
    <p>
        
    Hello World, <%:ViewData["Firstname"]%> <%:ViewData["Lastname"]%>
    <ul>
    <%
        var questions = (List<QuestionYourFriendsDataAccess.Question>) ViewData["questions"];
        var friends = (Dictionary<long, dynamic>) ViewData["friends"];
    foreach (var question in questions)
    {
      %>
      <li>
        <h2><%:question.text %></h2>

        <% if(question.answer != null) { %>
            <h3><%:question.answer %></h3>
        <% } %>

        <% if(question.private_price == 0 && friends.ContainsKey(question.Owner.fid)) { %>
            <span><%:friends[question.Owner.fid] %></span>
        <% }
           else
           {
            %>
            <a href="/FriendsQuestions/MakePublic/<%:question.id %>" alt="Découvrir l'auteur de cette question">de ???</a>
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
    </p>
</asp:Content>
