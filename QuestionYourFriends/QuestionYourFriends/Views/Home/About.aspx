<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Qui sommes-nous
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>À propos de</h2>
    <p>
        <h2>
        Hello
        <%:ViewData["Firstname"]%>
        <%:ViewData["Lastname"]%>!</h2>
    </p>
</asp:Content>
