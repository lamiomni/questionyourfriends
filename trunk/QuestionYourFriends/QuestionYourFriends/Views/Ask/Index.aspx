<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Page d'accueil
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%:ViewData["Message"]%></h2>
    <p>
        Ask!
        
    Hello World, <%:ViewData["Firstname"]%> <%:ViewData["Lastname"]%>
    </p>
</asp:Content>
