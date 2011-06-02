<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Mes questions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

Hello World, <%:ViewData["Firstname"]%> <%:ViewData["Lastname"]%> <%:ViewData["Id"]%>
    <form id="form1" runat="server">
    <% for (int i = 0; i < (int)ViewData["questionCount"]; i++)
       {
           %><%:ViewData["question" + i]%>
    <% } %>
    </form>

</asp:Content>
