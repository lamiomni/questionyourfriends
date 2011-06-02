<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Mes questions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form id="form1" runat="server">
    <%:ViewData["myId"]%>
    <% for (int i = 0; i < (int)ViewData["questionCount"]; i++)
       {
           %><%:ViewData["question" + i]%>
    <% } %>
    </form>

</asp:Content>
