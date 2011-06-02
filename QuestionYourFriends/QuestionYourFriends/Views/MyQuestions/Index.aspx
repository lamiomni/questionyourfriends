<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Mes questions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form id="form1" runat="server">
    <% for (int i = 0; i < (int)ViewData["questionCount"]; i++)
       {
           %>
            <%:((List<string>)ViewData["questions"])[i]%>
            <br/>
            <%:((List<string>)ViewData["friend"])[i]%>
            <br/>
            <asp:TextBox runat="server" ID="textBox1" />
            <br/>
            <asp:Button runat="server" ID="Cancel1" Text="annuler" />
            <asp:Button runat="server" ID="Respond1" Text="répondre" />
            <br/>
    <% } %>
    </form>

</asp:Content>
